using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform follow;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(follow.position.x, follow.position.y, transform.position.z);
    }
}
