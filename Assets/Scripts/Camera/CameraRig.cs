using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{

    public float speed = 3f;
    public Transform follow;
    Transform _transform;


    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;

    private void Awake()
    {
        _transform = transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (follow)
        {
            _transform.position = Vector3.Lerp(_transform.position, follow.position, speed * Time.deltaTime);
        }
    }

    public void ResetPosition()
    {
        _transform.position = Vector3.Lerp(_transform.position, follow.position, speed * Time.deltaTime);
    }

    public void Center()
    {
        transform.position = follow.position;
    }
}
