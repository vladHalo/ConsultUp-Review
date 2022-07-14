using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]

public class BossMovement : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    [SerializeField] Transform[] pointWay;
    [SerializeField] float speed;
    [SerializeField] Transform pointRes;
    public SphereCollider colliderActive;
    public List<Transform> poolObjects;
    public bool canMove, backPosition;
    Transform currentPoint;
    int currentPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentPoint = pointWay[Random.Range(0, pointWay.Length)];
        poolObjects = new List<Transform>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (canMove) Move();
        else
        {
            transform.localEulerAngles = new Vector3(0, -90, 0);
            animator.SetBool("Run", false);
        }
    }

    public void Move()
    {
        foreach (var i in poolObjects)
            i.position =new Vector3(pointRes.position.x,i.position.y ,pointRes.position.z);
        animator.SetBool("Run", true);

        //transform.LookAt(currentPoint.GetChild(currentPosition).position);
        //transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);

        agent.SetDestination(new Vector3(currentPoint.GetChild(currentPosition).position.x, transform.position.y, currentPoint.GetChild(currentPosition).position.z));
        
        if (Vector3.Distance(transform.position,currentPoint.GetChild(currentPosition).position)<=1)
        {
            if (backPosition)
            {
                currentPosition = 0;
                
                canMove = false;
                backPosition = false;
                return;
            }
            currentPosition++;
            if (currentPosition == currentPoint.childCount)
            {
                currentPoint = pointWay[Random.Range(0, pointWay.Length)];
                currentPosition = 0;
            }
        }
    }

    public void GoBackInTable()
    {
        currentPosition = currentPoint.childCount-1;
        backPosition = true;
        colliderActive.enabled = false;
    }

    public void Refresh()
    {
        foreach (var i in poolObjects)
            i.gameObject.SetActive(false);
        poolObjects.Clear();
    }
}
