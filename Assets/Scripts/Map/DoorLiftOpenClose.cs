using UnityEngine;

public class DoorLiftOpenClose : MonoBehaviour
{
    public bool isEmpty;    

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer==12)
        {
            if (StaticObject.instance.canOpenDoor) return;
            StaticObject.instance.OpenCloseDoor(100);
            isEmpty = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            if (StaticObject.instance.canOpenDoor) return;
            isEmpty = true;
        }
    }

    private void Update()
    {
        if (StaticObject.instance.canOpenDoor) return;
        if (isEmpty)
        {
            StaticObject.instance.OpenCloseDoor(0);
        }
    }
}
