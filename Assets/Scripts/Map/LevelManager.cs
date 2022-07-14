using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YsoCorp.GameUtils;

public class LevelManager : MonoBehaviour
{
    public static int mainLvl=1;
    public static int lvl;
    //public int lvlUI;
    public AIPlaces aiPlaces;
    public AISpawner aiSpawner;
    public List<GameObject>levelPrefabs;
    public GameObject currentLevel;
    public Transform destination;
    public Transform hero;
    //public SkinnedMeshRenderer doorLift;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        if (PlayerPrefs.HasKey("Lvl"))
        {
            lvl = PlayerPrefs.GetInt("Lvl");
            RefreshLevel();
        }

        if (PlayerPrefs.HasKey("mainLvl"))
            mainLvl = PlayerPrefs.GetInt("mainLvl");
        YsoCorp.GameUtils.YCManager.instance.OnGameStarted(mainLvl);
    }

    public void FinishLevel()
    {
        //TinySauce.OnGameFinished(lvl);
        //lvlUI++;
        YsoCorp.GameUtils.YCManager.instance.OnGameFinished(true);
        lvl++;
        if (lvl == levelPrefabs.Count)
            lvl = 1;
        mainLvl++;
        YsoCorp.GameUtils.YCManager.instance.OnGameStarted(mainLvl);
        PlayerPrefs.SetInt("Lvl",lvl);
        PlayerPrefs.Save();
        RefreshLevel();
    }

    void RefreshLevel()
    {
        var levelScript = currentLevel.GetComponent<Level>();
        //objects boss table
        foreach (var i in levelScript.bossMovement)
            i.Refresh();
        //resourse boss table
        foreach (var i in levelScript.conveerHandlers)
            i.Refresh();
        //resourse in coffee machine
        levelScript.resourceBuyers.Refresh();
        //resourse on hero
        var heroHolder = hero.GetComponent<Holder>();
        heroHolder.Refresh();
        heroHolder.coinsCount = 0;
        heroHolder.StartCoins();

        Destroy(currentLevel);
        currentLevel = Instantiate(levelPrefabs[lvl]);
        
        levelScript = currentLevel.GetComponent<Level>();
        //Clear bots        
        for (int i = 0; i < aiPlaces.bots.Count; i++)
            Destroy(aiPlaces.bots[i].gameObject);
        aiPlaces.bots.Clear();
        aiSpawner.countBots = 0;
        aiSpawner.RefreshTags();
        //Clear bots get res
        var botsGetRes = StaticObject.instance.botsGetRes;
        foreach (var i in botsGetRes)
            Destroy(i);
        botsGetRes.Clear();
        //Clear all chairs
        aiPlaces.allPlace.Clear();
        //foreach (var i in levelScript.aIPlaces)
        //    aiPlaces.allPlace.Add(i);
        aiPlaces.mainPlace.Clear();
        //aiPlaces.mainPlace.Add(levelScript.mainPlaces[0]);

        //Off UI
        foreach (Transform i in destination)
            i.gameObject.SetActive(false);
        
        //lift
        StaticObject.instance.Refresh(levelScript);

        for (int i = 0; i < levelScript.backgroundImg.Length; i++)
        {
            aiSpawner.destinationPoint[0].backgroundImg[i] = levelScript.backgroundImg[i];
            levelScript.backgroundImg[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < levelScript.neededImage.Length; i++)
            aiSpawner.destinationPoint[0].neededImage[i] = levelScript.neededImage[i];

        for (int i = 0; i < levelScript.neededText.Length; i++)
            aiSpawner.destinationPoint[0].neededText[i] = levelScript.neededText[i];
    }

    private void Update()
    {
        if (aiSpawner.destinationPoint.Count == 0) return;

        for (int i = 0; i < aiSpawner.destinationPoint.Count; i++)
        {
            var euler = aiSpawner.destinationPoint[0].backgroundImg[i].transform;
            euler.localEulerAngles = new Vector3(euler.localEulerAngles.x,-euler.parent.transform.eulerAngles.y, euler.localEulerAngles.z);
        }
    }
}
