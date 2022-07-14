using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticObject : MonoBehaviour
{
    public static StaticObject instance;
    public LevelManager levelManager;

    public AIPlaces aiPlaces;
    public Transform targetForEmoji;
    public SkinnedMeshRenderer doorLift;
    [HideInInspector] public BoxCollider doorLiftCollider;
    [HideInInspector] public bool canOpenDoor;
    [HideInInspector] public float valueOpen;
    public List<GameObject> botsGetRes;
    public float maxPopular;
    [HideInInspector]public bool onesBording;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        doorLiftCollider = doorLift.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if(canOpenDoor)
        {
            OpenCloseDoor(100);
            if(doorLiftCollider!=null && valueOpen >= 90) doorLiftCollider.enabled = false;
        }
    }

    public void OpenCloseDoor(float number)
    {
        valueOpen = Mathf.Lerp(valueOpen, number, 0.05f);
        doorLift.SetBlendShapeWeight(0, valueOpen);
    }

    public void Refresh(Level level)
    {
        doorLift = level.doorLift;
        valueOpen = 0;
        canOpenDoor = false;
        doorLiftCollider = level.doorLift.GetComponent<BoxCollider>();
        targetForEmoji.GetChild(0).GetComponent<Image>().fillAmount = 0;
        instance.doorLiftCollider.enabled = true;
    }
}
