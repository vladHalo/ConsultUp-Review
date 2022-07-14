using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableEnable : MonoBehaviour
{
    public int number;
    public bool active;
    private void OnEnable()
    {
        if (LevelManager.lvl == 0 && active)
            OnBoarding.onBoarding.Boarding(number);
        else if (LevelManager.lvl != 0)
            StaticObject.instance.levelManager.currentLevel.GetComponent<Level>().Activated(2);
        StaticObject.instance.aiPlaces.mainPlace.Add(transform.GetChild(1).GetComponent<AIPlace>());
    }
}
