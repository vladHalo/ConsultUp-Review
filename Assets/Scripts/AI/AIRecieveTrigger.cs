//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRecieveTrigger : MonoBehaviour
{
    [HideInInspector]public AISpawner aiSpawner;
    [HideInInspector]
    public string currentTag;

    public AIHolder aiHolder;

    [SerializeField] Transform receivePoint;

    public int reciveCountMin;
    public int reciveCountMax;
    [HideInInspector]
    public int count;
    public int price;

    private void Awake()
    {
        aiSpawner = FindObjectOfType<AISpawner>();

        int rand = Random.Range(0, aiSpawner.tags.Count);

        currentTag = aiSpawner.tags[rand];

        count = Random.Range(reciveCountMin, reciveCountMax);

        price = count * (rand + 1);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent<Holder>(out Holder holder))
        {
            holder.SellObject(currentTag, aiHolder);
        }
    }
}
