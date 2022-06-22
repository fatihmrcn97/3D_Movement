using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;

    private void Update()
    {
        transform.position = target.transform.position;
    }
}
