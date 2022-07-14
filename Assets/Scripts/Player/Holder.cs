using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YsoCorp.GameUtils;

public class Holder : MonoBehaviour
{
    public Image popular;
    public Animator anim;
    ObjectPooling objectPooling;

    public List<SmoothLerp> objects = new List<SmoothLerp>();
    //GameObject lastObject;
    [SerializeField] Transform holderPoint;
    [SerializeField] Transform[] allHolderPoint;

    [SerializeField] Transform startPosCoin;

    public int maxCount;
    public int currentCount;

    public int lvlPopular;
    public float countPopular;
    public float[] numberPopular = { 2000, 2500, 2750, 2750, 3000, 3000, 3250, 3250, 3500, 3500 };

    [SerializeField] int followSpeed = 50;

    [Space]
    [Space]
    [SerializeField] TextMeshProUGUI coinsText;
    public int coinsCount;
    [SerializeField] TextMeshProUGUI redPills;
    int redCount;
    [SerializeField] TextMeshProUGUI bluePills;
    int blueCount;
    [SerializeField] TextMeshProUGUI greenPills;
    int greenCount;
    public Transform startPosHero;
    List<CoinController> startAllCoins;

    bool ones, twoes;
    //Ads
    public float time;
    float timer = 50;
    public bool canOnAds;

    private void Awake()
    {
        startAllCoins = new List<CoinController>();
        coinsText.text = coinsCount.ToString();
        objectPooling = ObjectPooling.Instance;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Lvl"))
            return;
        StartCoins();
    }

    public int FreeSpaceCount()
    {
        int x = maxCount - currentCount;
        return x;
    }

    void UpdateObjects()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].parentObject = (i == 0) ? holderPoint : objects[i - 1].transform;
            objects[i].lerpTime = followSpeed;
        }

        redPills.text = redCount.ToString();
        bluePills.text = blueCount.ToString();
        greenPills.text = greenCount.ToString();
    }

    public void RecieveObject(GameObject obj)
    {
        SmoothLerp objectMoveController = obj.GetComponent<SmoothLerp>();

        objectMoveController.activeMove = true;

        objects.Add(objectMoveController);

        switch (obj.GetComponent<PillController>().pillType)
        {
            case PillController.typeOfPill.RED:
                redCount++;
                if (LevelManager.lvl == 0 && !ones)
                {
                    OnBoarding.onBoarding.Boarding(3);
                    ones = true;
                }
                else if (LevelManager.lvl != 0) StaticObject.instance.levelManager.currentLevel.GetComponent<Level>().Activated(1);
                break;
            case PillController.typeOfPill.GREEN:
                greenCount++;
                StaticObject.instance.levelManager.currentLevel.GetComponent<Level>().Activated(1);
                break;
            case PillController.typeOfPill.BLUE:
                blueCount++;
                StaticObject.instance.levelManager.currentLevel.GetComponent<Level>().Activated(1);
                break;
            case PillController.typeOfPill.RESOURCE:
                if (LevelManager.lvl == 0 && !twoes)
                {
                    twoes = true;
                    OnBoarding.onBoarding.Boarding(1);
                }
                else if (LevelManager.lvl != 0) StaticObject.instance.levelManager.currentLevel.GetComponent<Level>().Activated(0);
                break;
            default:
                break;
        }

        currentCount++;

        if (objects[0].GetComponent<PillController>().pillType == PillController.typeOfPill.RESOURCE)
            holderPoint = allHolderPoint[0];
        else holderPoint = allHolderPoint[1];
        UpdateObjects();

        anim.SetBool("isEmpty", false);

        if (canOnAds)
        {
            YCManager.instance.adsManager.ShowInterstitial(() => {
                canOnAds = false;
                time = 0;
            });
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > timer)
            canOnAds = true;
    }

    public void SellObject(string tag, AIHolder holder)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            string pillType = objects[i].GetComponent<PillController>().pillType.ToString();

            if (pillType == tag)
            {
                holder.RecieveObject(objects[i],pillType,this);
                
                switch (objects[i].GetComponent<PillController>().pillType.ToString())
                {
                    case "RED":
                        redCount--;
                        break;
                    case "GREEN":
                        greenCount--;
                        break;
                    case "BLUE":
                        blueCount--;
                        break;
                    case "RESOURCE":
                        break;
                    default:
                        break;
                }

                objects.Remove(objects[i]);
                currentCount--;

                UpdateObjects();

                if (currentCount == 0) anim.SetBool("isEmpty", true);

                break;
            }
        }

        if (canOnAds)
        {
            YCManager.instance.adsManager.ShowInterstitial(() => {
                canOnAds = false;
                time = 0;
            });
        }
    }

    public void AddPopular(float number)
    {
        countPopular += number;
        popular.fillAmount += countPopular / numberPopular[lvlPopular];
        if (popular.fillAmount >= 0.99f)
        {
            if(LevelManager.lvl==0)
                OnBoarding.onBoarding.Boarding(7);
            StaticObject.instance.canOpenDoor = true;
        }
        if (canOnAds)
        {
            YCManager.instance.adsManager.ShowInterstitial(() => {
                canOnAds = false;
                time = 0;
            });
        }
    }
    public void PlaceResource(ConveerHandler conveer)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            string pillType = objects[i].GetComponent<PillController>().pillType.ToString();

            if (pillType == "RESOURCE")
            {
                conveer.GetResource(objects[i]);

                objects.Remove(objects[i]);
                currentCount--;

                UpdateObjects();

                if (currentCount == 0) anim.SetBool("isEmpty", true);

                break;
            }
        }
    }

    public void GetCoin()
    {
        coinsCount++;
        coinsText.text = coinsCount.ToString();
        if (canOnAds)
        {
            YCManager.instance.adsManager.ShowInterstitial(() => {
                canOnAds = false;
                time = 0;
            });
        }
    }

    public void GiveCoins(Transform target, Action action)
    {
        if(coinsCount > 0)
        {
            coinsCount--;
            coinsText.text = coinsCount.ToString();

            action.Invoke();

            GameObject coin = objectPooling.SpawnFromPool("coin", new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), transform.rotation);
            CoinController coinController = coin.GetComponent<CoinController>();
            coinController.followedTarget = target;
            coinController.fastFollow = true;
        }
    }

    public void StartCoins()
    {
        //TinySauce.OnGameStarted($"{LevelManager.lvl}");
        transform.position = startPosHero.position;
        coinsCount = 80;
        while (coinsCount > 0)
        {
            coinsCount--;
            coinsText.text = coinsCount.ToString();

            GameObject coin = objectPooling.SpawnFromPool("coin", new Vector3(startPosCoin.transform.position.x, startPosCoin.transform.position.y, startPosCoin.transform.position.z), startPosCoin.transform.rotation);
            CoinController coinController = coin.GetComponent<CoinController>();
            startAllCoins.Add(coinController);
        }
        StartCoroutine(DelayMoveCoin(startAllCoins));
    }

    IEnumerator DelayMoveCoin(List<CoinController> coinControllers)
    {
        yield return new WaitForSeconds(2);
        for(int i=0; i< coinControllers.Count;i++)
        {
            coinControllers[i].followedTarget = transform;
            coinControllers[i].fastFollow = true;
            coinControllers[i].GetComponent<SmoothLerp>().parentObject = transform;
        }
        //startPosCoin.position = new Vector3(startPosCoin.position.x,startPosCoin.position.y+10, startPosCoin.position.z);
    }

    public void DropObject(Transform receivePoint)
    {
        redCount = 0;
        blueCount = 0;
        greenCount = 0;

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].activeMove = true;
            objects[i].parentObject = receivePoint;
            objects[i].lerpTime = 5;
            objects[i].gameObject.SetActive(false);
            objects.Remove(objects[i]);

            currentCount--;

            anim.SetBool("isEmpty", true);
        }

        if (canOnAds)
        {
            YCManager.instance.adsManager.ShowInterstitial(() => {
                canOnAds = false;
                time = 0;
            });
        }
        UpdateObjects();
    }

    public void Refresh()
    {
        for (int i = 0; i < objects.Count; i++)
            objects[i].gameObject.SetActive(false);
        objects.Clear();

        transform.position = startPosHero.position;
        anim.SetBool("isEmpty", true);
        currentCount = 0;
        countPopular = 0;
        if (lvlPopular < numberPopular.Length - 1)
            lvlPopular++;

        foreach (var i in startAllCoins)
            i.gameObject.SetActive(false);

        if(startAllCoins!=null)
        startAllCoins.Clear();
        coinsText.text=$"0";
        coinsCount = 0;
        redPills.text = $"0";
        redCount = 0;
        bluePills.text = $"0";
        blueCount=0;
        greenPills.text = $"0";
        greenCount=0;

        switch(LevelManager.lvl)
        {
            case 0:
                bluePills.transform.parent.gameObject.SetActive(false);
                greenPills.transform.parent.gameObject.SetActive(false);
                break;
            case 1:
                bluePills.transform.parent.gameObject.SetActive(true);
                greenPills.transform.parent.gameObject.SetActive(false);
                break;
            case 2:
                bluePills.transform.parent.gameObject.SetActive(true);
                greenPills.transform.parent.gameObject.SetActive(true);
                break;
            case 3:
                bluePills.transform.parent.gameObject.SetActive(true);
                greenPills.transform.parent.gameObject.SetActive(true);
                break;
        }
}
}
