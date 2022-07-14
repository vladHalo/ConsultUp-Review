using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    //public AIPlace[] mainPlaces;
    public SkinnedMeshRenderer doorLift;
    //public AIPlace[] aIPlaces;

    public SpriteRenderer[] neededImage;
    public SpriteRenderer[] backgroundImg;
    public TextMeshPro[] neededText;

    //Boss
    public BossMovement [] bossMovement;
    public ConveerHandler[] conveerHandlers;
    public ResourceBuyer resourceBuyers;

    [Space]
    [Header("ActivatedObjects")]
    public ObjectsBoarding[] activatedObjects;

    public void Activated(int next)
    {
        for (int i = 0; i < activatedObjects[next].objects.Length; i++)
        {
            if (activatedObjects[next].objects[i] != null)
                activatedObjects[next].objects[i].gameObject.SetActive(true);
        }
    }
}
