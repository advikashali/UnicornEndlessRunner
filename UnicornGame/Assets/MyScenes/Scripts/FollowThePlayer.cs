using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowThePlayer : MonoBehaviour
{

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = Vector3.forward * playerTransform.position.z;
    }
}
