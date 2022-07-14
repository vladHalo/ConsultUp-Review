using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    LevelManager levelManager;

    private void Start()
    {
        levelManager = StaticObject.instance.levelManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
            levelManager.FinishLevel();
    }
}
