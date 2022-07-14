using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBuyer : MonoBehaviour
{
    [SerializeField] string objecTag;
    [SerializeField] Transform deliveryPoint;

    [SerializeField] int indexCoffee;
    [SerializeField] int maxCoffee;
    [SerializeField] float delay;

    [SerializeField] List<SmoothLerp> activeObjects = new List<SmoothLerp>();

    float timerSpawn;
    bool canGive,canSpawn;
    ObjectPooling objectPooling;
    GameObject lastObject;
    Holder holder;

    private void Start()
    {
        objectPooling = ObjectPooling.Instance;
    }

    private void Update()
    {
        if (holder == null) return;
        if (!canSpawn || holder.currentCount>=holder.maxCount) return;
        timerSpawn += Time.deltaTime;
        if (timerSpawn >= delay && indexCoffee > 0 && indexCoffee>0)
        {
            timerSpawn = 0;
            SpawnObject();
        }
    }

    public void SpawnObject()
    {
        GameObject go = objectPooling.SpawnFromPool(objecTag, deliveryPoint.position, deliveryPoint.rotation);
        SmoothLerp objectMoveController = go.GetComponent<SmoothLerp>();

        objectMoveController.parentObject = (lastObject != null) ? lastObject.transform : deliveryPoint;
        objectMoveController.lerpTime = 40;
        objectMoveController.activeMove = true;
        activeObjects.Add(objectMoveController);
        indexCoffee--;

        lastObject = go;
        if (activeObjects.Count == maxCoffee)
        {
            StartCoroutine(GiveCoffee());
        }
    }

    void GiveObject(Holder holder)
    {
        lastObject = deliveryPoint.gameObject;
        holder.RecieveObject(activeObjects[activeObjects.Count - 1].gameObject);
        activeObjects.Remove(activeObjects[activeObjects.Count - 1]);
        if (activeObjects.Count == 0)
        {
            canGive = false;
            indexCoffee = maxCoffee;
        }
    }

    IEnumerator GiveCoffee()
    {
        yield return new WaitForSeconds(1f);
        canGive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        canSpawn = true;
    }

    private void OnTriggerExit(Collider other)
    {
        canSpawn = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Holder>(out Holder holder) && other.gameObject.layer==3)
        {
            if (canGive && activeObjects.Count>0 && holder.currentCount<=holder.maxCount)
            {
                GiveObject(holder);
                //if (LevelManager.lvl == 0) OnBoarding.onBoarding.Boarding(0);
            }
            else if(!canGive)
            this.holder = holder;
        }
    }

    public void Refresh()
    {
        foreach (var i in activeObjects)
            i.gameObject.SetActive(false);
        activeObjects.Clear();
    }
}
