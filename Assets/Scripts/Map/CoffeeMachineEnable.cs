using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMachineEnable : MonoBehaviour
{
    public int number;
    private void OnEnable()
    {
        if (LevelManager.lvl == 0)
            OnBoarding.onBoarding.Boarding(number);
    }
}
