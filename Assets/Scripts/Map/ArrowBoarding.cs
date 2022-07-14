using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBoarding : MonoBehaviour
{
    Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, camera.transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
