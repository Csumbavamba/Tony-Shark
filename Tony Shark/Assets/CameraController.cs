using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform objectToFollow;

    // Start is called before the first frame update
    void Start()
    {
        objectToFollow = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = objectToFollow.position;
        this.transform.up = Vector3.up;
    }
}
