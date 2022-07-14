using System.Collections.Generic;
using UnityEngine;

public class AIPlaces : MonoBehaviour
{
    public List<AIPlace> mainPlace;
    public List<AIPlace> allPlace;

    public List<AIController> bots;

    private void Awake()
    {
        bots = new List<AIController>();
    }

    public AIPlace FreeMainPlace()
    {
        for (int i = 0; i < mainPlace.Count; i++)
        {
            if (!mainPlace[i].busy)
                return mainPlace[i];
        }

        return null;
    }
    public Transform FindFreePlace(Transform positionBot)
    {
        foreach (var i in allPlace)
            if (!i.busy) return i.transform;
        return positionBot;
    }

    public bool FindFreePlace()
    {
        foreach (var i in allPlace)
            if (i.busy) return true;
        return false;
    }

    //public void FreeUpSpace()
    //{
    //    for(int i=0;i<allPlace.Length;i++)
    //    {
    //        if(allPlace[i].busy)
    //        {
    //            allPlace[i].aiController.isSit = 0;
    //            return;
    //        }
    //    }
    //}
}
