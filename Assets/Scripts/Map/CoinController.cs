using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    Collider coll;
    Rigidbody rb;
    public Transform followedTarget;
    SmoothLerp smoothLerp;

    [SerializeField] float upForce = 10f;
    [SerializeField] float torqueForce = 2f;

    public bool fastFollow = false;

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        smoothLerp = GetComponent<SmoothLerp>();
    }

    private void OnEnable()
    {
        
    }

    private void OnBecameVisible()
    {
        smoothLerp.parentObject = null;

        if (!fastFollow)
        {
            coll.isTrigger = false;
            rb.isKinematic = false;

            rb.velocity = Vector3.zero;

            float randTorque = Random.Range(-torqueForce, torqueForce);

            rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
            rb.AddTorque(Vector3.right * randTorque + Vector3.forward * randTorque, ForceMode.Impulse);
            rb.velocity = Random.onUnitSphere * randTorque;

            StartCoroutine(SetTargetCoroutine());
        }
        else
        {
            SetFollowedTarget();
        }
    }

    IEnumerator SetTargetCoroutine()
    {
        yield return new WaitForSeconds(2f);
        SetFollowedTarget();
    }

    public void SetFollowedTarget()
    {
        coll.isTrigger = true;
        rb.isKinematic = true;

        smoothLerp.parentObject = followedTarget;
        smoothLerp.lerpTime = 30;
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == followedTarget)
        {
            if (other.TryGetComponent<Holder>(out Holder holder))
            {
                holder.GetCoin();
            }

            gameObject.SetActive(false);
        }
    }
}
