using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{

    public Transform lookatTarget; //unicorn
    public Vector3 offset = new Vector3(0f, -2f, -1f);

    private void Start()
    {
        transform.position = lookatTarget.position + offset;
    }

    private void Update()
    {
        Vector3 desiredPosition = lookatTarget.position + offset;
        desiredPosition.x = 0f;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
    }
}
