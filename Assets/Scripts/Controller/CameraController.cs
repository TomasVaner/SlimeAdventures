/*
 * Copyright (c) Tomas Vaner
 * https://github.com/TomasVaner
*/

using UnityEngine;

namespace Controller
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform follow;

        // Update is called once per frame
        private void FixedUpdate()
        {
            transform.position = new Vector3(follow.position.x, follow.position.y, transform.position.z);
        }
    }
}
