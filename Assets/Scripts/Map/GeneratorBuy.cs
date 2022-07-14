
using System.Collections;
using TMPro;
using UnityEngine;

public class GeneratorBuy : MonoBehaviour
{
    //public string tagID;

    Holder holder;
    Coroutine recieveCoroutine;
    AISpawner spawner;

    [SerializeField] string addedTag;
    [SerializeField] Transform recievePoint;
    [SerializeField] GameObject generatorToActivate;

    [SerializeField] TextMeshPro priceText;

    [SerializeField] int priceCount = 50;
    public int coinCount = 0;
    [SerializeField] float delay = 0.1f;

    private void Awake()
    {
        spawner = FindObjectOfType<AISpawner>();

    //    if(PlayerPrefs.GetInt(tagID) == 1)
    //    {
    //        Builded();
    //    }
    //    else
    //    {
    //        coinCount = PlayerPrefs.GetInt(tagID + "num");
    //        priceText.text = coinCount.ToString() + "/" + priceCount.ToString();
    //    }
    }

    private void OnTriggerStay(Collider other)
    {
        if (coinCount >= priceCount) return;
        if (other.TryGetComponent<Holder>(out Holder holderInTrigger))
        {
            holder = holderInTrigger;
            if (recieveCoroutine == null) recieveCoroutine = StartCoroutine(RecieveCoin());
        }
    }

    public void GetCoin()
    {
        coinCount++;

        //PlayerPrefs.SetInt(tagID + "num", coinCount);

        priceText.text = coinCount.ToString() + "/" + priceCount.ToString();

        if (coinCount >= priceCount)
        {
            priceText.gameObject.SetActive(false);

            Destroy(gameObject, 0.5f);

            if (!string.IsNullOrEmpty(addedTag) && StaticObject.instance.levelManager.aiSpawner.CheckTags(addedTag))
                spawner.tags.Add(addedTag);
            generatorToActivate.SetActive(true);

            //PlayerPrefs.SetInt(tagID, 1);
        }
        else
        {
            //PlayerPrefs.SetInt(tagID, 0);
        }
    }

    public void Builded()
    {
        priceText.gameObject.SetActive(false);
        if (!string.IsNullOrEmpty(addedTag) && StaticObject.instance.levelManager.aiSpawner.CheckTags(addedTag))
            spawner.tags.Add(addedTag);
        generatorToActivate.SetActive(true);

        Destroy(gameObject);
    }

    IEnumerator RecieveCoin()
    {
        yield return new WaitForSeconds(delay);

        holder.GiveCoins(recievePoint, GetCoin);

        recieveCoroutine = null;
    }
}
