using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveerHandler : MonoBehaviour
{
    public BossMovement boss;
    [SerializeField] Spawner spawner;
    public Transform objectSetter;
    public Transform bossPoint;
    public PillsGenerator pillsGenerator;

    public List<SmoothLerp> allResources = new List<SmoothLerp>();
    Coroutine giveResCoroutine, moveToBoss,moveBoss;
    int resCountReward = 0;
    int resCountBoss;

    private void FixedUpdate()
    {
        if (giveResCoroutine == null && resCountReward > 0) giveResCoroutine = StartCoroutine(GiveResource());
        if (boss.colliderActive.enabled == false && allResources.Count > 0)
            if (moveToBoss == null && spawner.activeObjects.Count == 0) moveToBoss = StartCoroutine(MoveToBoss());
    }

    void UpdateObjects(bool type,int number)
    {
        if (type)
        {
            for (int i = number; i < allResources.Count; i++)
            {
                allResources[i].parentObject = (i == number) ? objectSetter : allResources[i - 1].transform;
                allResources[i].lerpTime = 50;
            }
        }
        else
        {
            allResources[number].parentObject = bossPoint;
            allResources[number].lerpTime = 50;
        }
    }
    
    public void GetResource(SmoothLerp obj)
    {
        obj.activeMove = true;

        allResources.Add(obj);
        UpdateObjects(true, 0);
        
        pillsGenerator.resorceCount = allResources.Count;
        pillsGenerator.UpdDisplay();

        if (moveToBoss == null && spawner.activeObjects.Count==0) moveToBoss = StartCoroutine(MoveToBoss());
    }

    IEnumerator MoveToBoss()
    {
        yield return new WaitForSeconds(2f);
        var valueRes = allResources.Count;
        if (valueRes > 10) valueRes = 10;
        while (resCountBoss < valueRes)
        {
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < allResources.Count; i++)
                allResources[i].parentObject = null;
            yield return new WaitForSeconds(0.2f);
            UpdateObjects(false, resCountBoss);
            spawner.SpawnObject();
            yield return new WaitForSeconds(0.1f);
            resCountBoss++;
            UpdateObjects(true, resCountBoss);
        }
        moveToBoss = null;
        if (moveBoss == null) moveBoss = StartCoroutine(MoveBoss());
    }

    IEnumerator MoveBoss()
    {
        yield return new WaitForSeconds(0.5f);
        
        resCountReward = allResources.Count;

        yield return new WaitForSeconds(1f);
        moveBoss = null;
        boss.canMove = true;
        boss.colliderActive.enabled = true;

        if (allResources.Count < 10)
        {
            for (int i = 0; i < allResources.Count; i++)
            {
                allResources[i].gameObject.SetActive(false);
                allResources.RemoveAt(0);
            }
            allResources.Clear();
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                allResources[0].gameObject.SetActive(false);
                allResources.RemoveAt(0);
            }
        }
        pillsGenerator.resorceCount = allResources.Count;
        pillsGenerator.UpdDisplay();
    }

    IEnumerator GiveResource()
    {
        resCountReward--;
        resCountBoss = 0;
        yield return new WaitForSeconds(0);
        giveResCoroutine = null;
        //pillsGenerator.resorceCount = allResources.Count;
        //pillsGenerator.UpdDisplay();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Holder>(out Holder holder))
        {
            if (allResources.Count > 19) return;
            holder.PlaceResource(this);
        }
    }

    public void Refresh()
    {
        foreach (var i in allResources)
            i.gameObject.SetActive(false);
        allResources.Clear();
    }
}
