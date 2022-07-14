using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBucket : MonoBehaviour
{
    [SerializeField] Transform point;
    [SerializeField] Animator trashAnim;
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Holder>(out Holder holder))
        {
            holder.DropObject(point);
            trashAnim.SetTrigger("roll");
        }
    }
}
