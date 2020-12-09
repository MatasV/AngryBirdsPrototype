using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform TargetPosition;
    public Transform StartingPosition;

    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    public IEnumerator StartingCameraMove()
    {
        
    }
}
