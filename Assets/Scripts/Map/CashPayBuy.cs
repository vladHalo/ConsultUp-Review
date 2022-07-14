using System.Collections;
using TMPro;
using UnityEngine;

public class CashPayBuy : MonoBehaviour
{
    public string tagID;
    public string addedTag;

    Animator anim;
    Holder holder;
    Coroutine recieveCoroutine;
    AISpawner spawner;

    [SerializeField] TextMeshPro priceText;
    [SerializeField] GameObject table;

    [SerializeField] int priceCount = 50;
    public int coinCount = 0;
    [SerializeField] float delay = 0.1f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spawner = FindObjectOfType<AISpawner>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (coinCount >= priceCount) return;
        if(other.TryGetComponent<Holder>(out Holder holderInTrigger))
        {
            holder = holderInTrigger;
            if (recieveCoroutine == null) recieveCoroutine = StartCoroutine(RecieveCoin());
        }
    }

    public void GetCoin()
    {
        coinCount++;
        priceText.text = coinCount.ToString() + "/" + priceCount.ToString();

        if(coinCount >= priceCount)
        {
            anim.SetBool("Open", true);
            foreach (Transform i in transform)
                i.gameObject.SetActive(false);
            
            if (!string.IsNullOrEmpty(addedTag) && StaticObject.instance.levelManager.aiSpawner.CheckTags(addedTag))
                spawner.tags.Add(addedTag);
            table.SetActive(true);
            Destroy(gameObject, 3);
        }
    }

    public void Builded()
    {
        priceText.gameObject.SetActive(false);

        table.SetActive(true);
        Destroy(gameObject);
    }

    IEnumerator RecieveCoin()
    {
        yield return new WaitForSeconds(delay);

        holder.GiveCoins(transform, GetCoin);

        recieveCoroutine = null;
    }
}
