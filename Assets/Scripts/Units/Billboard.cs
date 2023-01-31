using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera cam;
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(cam.transform);

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
