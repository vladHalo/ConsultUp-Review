using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    private Joystick joystick;
    private Rigidbody rb;
    private Vector3 moveVector;
    private Animator anim;
    public GameObject instruction8;

    private void Awake()
    {
        joystick = FindObjectOfType<Joystick>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Lvl"))
            instruction8.SetActive(true);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("mainLvl",LevelManager.mainLvl);
        YsoCorp.GameUtils.YCManager.instance.OnGameFinished(false);
    }

    public void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        moveVector = new Vector3(-joystick.Horizontal, 0, -joystick.Vertical);
        rb.velocity = new Vector3(moveVector.x * speed, rb.velocity.y, moveVector.z * speed);

        if (Vector3.Angle(Vector3.forward, moveVector) > 1f || Vector3.Angle(Vector3.forward, moveVector) == 0)
        {
            Vector3 direct = Vector3.RotateTowards(transform.forward, moveVector, speed * 4f * Time.fixedDeltaTime, 0.0f);
            rb.rotation = Quaternion.LookRotation(direct);
        }

        anim.SetFloat("Speed", rb.velocity.magnitude / speed);
    }
}
