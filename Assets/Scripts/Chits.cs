using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chits : MonoBehaviour
{
    float time, timer=10;
    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out Holder holder))
        {
            time += Time.deltaTime;
            if(time>timer)
            {
                holder.AddPopular(10000);
                time = 0;
            }
        }
    }
}
