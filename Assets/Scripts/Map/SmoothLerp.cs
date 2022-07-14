using UnityEngine;

public class SmoothLerp : MonoBehaviour
{
    public bool activeMove = false;

    public float lerpTime;
    public Transform parentObject;
    public float rotateSpeed=20;

    Vector3 myPos;
    [SerializeField] public Vector3 offset;

    float t = 0f;

    private void Update()
    {
        if(parentObject != null) FollowObject();
    }

    public Vector3 PillOffset()
    {
        return offset;
    }

    void FollowObject()
    {
        myPos = parentObject.position;

        transform.position = Vector3.MoveTowards(transform.position, myPos + offset, lerpTime * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, parentObject.rotation, rotateSpeed * rotateSpeed * Time.deltaTime);
        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);

        if (t > 0.99f)
        {
            if(!activeMove) parentObject = null;
            t = 0f;
        }
    }
}
