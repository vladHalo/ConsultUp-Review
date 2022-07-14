using UnityEngine;

public class EnableSit : MonoBehaviour
{
    public bool onBoarding;
    
    private void OnEnable()
    {
        foreach (Transform i in transform)
            StaticObject.instance.aiPlaces.allPlace.Add(i.GetComponent<AIPlace>());
        if (LevelManager.lvl == 0 && onBoarding)
            OnBoarding.onBoarding.Boarding(6);
        else if(LevelManager.lvl != 0)
            StaticObject.instance.levelManager.currentLevel.GetComponent<Level>().Activated(3);
    }
}
