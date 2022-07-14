using UnityEngine;

public class ResourcesController : MonoBehaviour
{
    SmoothLerp smoothLerp;

    private void Awake()
    {
        smoothLerp = GetComponent<SmoothLerp>();
    }

    private void OnDisable()
    {
        smoothLerp.parentObject = null;
    }
}
