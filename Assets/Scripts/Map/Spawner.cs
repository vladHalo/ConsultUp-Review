using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public BossMovement boss;
    [SerializeField] string objecTag;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform firstSpawn;

    [SerializeField] int maxCount;
    [SerializeField] float delayToSpawn;

    int currentCount;

    [SerializeField] ConveerHandler conveerHandler;
    [SerializeField] PillsGenerator pillGenerator;
    ObjectPooling objectPooling;
    [HideInInspector]public List<SmoothLerp> activeObjects = new List<SmoothLerp>();
    GameObject lastObject;
    bool onesBoarding;

    private void Start()
    {
        objectPooling = ObjectPooling.Instance;
    }

    private void Update()
    {
        if (pillGenerator.resorceCount <= 0)
        {
            pillGenerator.resorceCount = 0;
            pillGenerator.UpdDisplay();
            return;
        }
    }

    //void UpdateObjects()
    //{
    //    for (int i = 0; i < activeObjects.Count; i++)
    //    {
    //        activeObjects[activeObjects.Count - 1 - i].activeMove = true;
    //        activeObjects[activeObjects.Count - 1 - i].parentObject = (i == 0) ? spawnPoint: activeObjects[activeObjects.Count - i].transform;
    //        activeObjects[activeObjects.Count - 1 - i].lerpTime = 5;
    //    }
    //}

    public void SpawnObject()
    {
        GameObject go = objectPooling.SpawnFromPool(objecTag, firstSpawn.position, firstSpawn.rotation);
        SmoothLerp objectMoveController = go.GetComponent<SmoothLerp>();

        objectMoveController.parentObject = (lastObject != null) ? lastObject.transform : spawnPoint;

        boss.poolObjects.Add(objectMoveController.transform);
        activeObjects.Add(objectMoveController);
        currentCount++;

        pillGenerator.resorceCount--;
        pillGenerator.UpdDisplay();

        lastObject = go;
        if (LevelManager.lvl == 0 && !onesBoarding) 
        { 
            OnBoarding.onBoarding.Boarding(2);
            onesBoarding = true;
        } 
    }

    //void SpawnObjectOnBot()
    //{
    //    GameObject go = objectPooling.SpawnFromPool(objecTag, spawnPoint.position, spawnPoint.rotation);
    //    SmoothLerp objectMoveController = go.GetComponent<SmoothLerp>();

    //    objectMoveController.parentObject = spawnPoint;

    //    activeObjects.Add(objectMoveController);
    //    currentCount++;
        
    //    UpdateObjects();
    //}

    void GiveObject(Holder holder)
    {
        if (activeObjects.Count - 1 >= 0)
        {
            lastObject = spawnPoint.gameObject;
            currentCount--;

            holder.RecieveObject(activeObjects[activeObjects.Count - 1].gameObject);
            activeObjects.Remove(activeObjects[activeObjects.Count - 1]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Holder>(out Holder holder))
        {
            if (holder.currentCount == 10 && currentCount == 10) return;
            int freeSpaceCount = holder.FreeSpaceCount();

            var startValue = activeObjects.Count;
            
            for (int i=0; i< startValue;i++)
                if (freeSpaceCount > 0) GiveObject(holder);

            if (currentCount == 0)
            {
                activeObjects.Clear();
                boss.poolObjects.Clear();
                boss.GoBackInTable();
            }

        }
    }
}
