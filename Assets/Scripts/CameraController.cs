using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;
    private Vector3 startingPosition;
    
    public float moveSpeed = 10.0f;
    private float startTime;
    private float journeyLength;

    [SerializeField] private Camera cam;

    [SerializeField] private BoxCollider2D bounds;
    
    private float xMin = 0f;
    private float yMin = 0f;
    private float xMax = 0f;
    private float yMax = 0f;
    
    private float cameraOrthographicSize;
    private float cameraRatio;
    
    private GameObject targetFollow;
    private float followSpeed = 10;

    private bool introRunning = false;
    private bool forwards = true;
    

    private void Start()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null || targetPosition == null) return;

        if (bounds == null) bounds = GetComponent<BoxCollider2D>();
        
        var mapBounds = bounds.bounds;
        xMin = mapBounds.min.x;
        xMax = mapBounds.max.x;
        yMin = mapBounds.min.y;
        yMax = mapBounds.max.y;
        
        cameraOrthographicSize = cam.orthographicSize;
        cameraRatio = (xMax + cameraOrthographicSize) / 2.0f;
        
        startingPosition = cam.transform.position;
        startTime = Time.time;
        journeyLength =Vector3.Distance(startingPosition, targetPosition.position);
        StartCoroutine(nameof(StartingCameraMove));

        StageManager.instance.onBirdLaunched += GetNextFollowTargetPosition;
    }

    private void GetNextFollowTargetPosition(GameObject target)
    {
        if (target != null) targetFollow = target;
    }
    
    private void Update()
    {
        if (introRunning && Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            cam.transform.position = startingPosition;
        }

        if (targetFollow != null)
        {
            var camY = Mathf.Clamp(targetFollow.transform.position.y, yMin + cameraOrthographicSize, yMax - cameraOrthographicSize);
            var camX = Mathf.Clamp(targetFollow.transform.position.x, xMin + cameraRatio, xMax - cameraRatio);
            var smoothPos = Vector3.Lerp(this.transform.position, new Vector3(camX, camY, cam.transform.position.z), followSpeed);
            cam.transform.position = smoothPos;
        }
    }

    public IEnumerator StartingCameraMove()
    {
        introRunning = true;
        while (forwards)
        {
            var distanceCovered = (Time.time - startTime) * moveSpeed;

            var fractionOfJourney = distanceCovered / journeyLength;

            cam.transform.position = Vector2.Lerp(startingPosition, targetPosition.position, fractionOfJourney);

            fractionOfJourney = fractionOfJourney * fractionOfJourney * fractionOfJourney *
                                (fractionOfJourney * (6f * fractionOfJourney - 15f) + 10f);
            
            if (Vector2.Equals(cam.transform.position, targetPosition.position))
            {
                forwards = false;
                yield return new WaitForSeconds(1);
                journeyLength =Vector2.Distance(targetPosition.position, startingPosition);
                startTime = Time.time;
                yield return null;
            }
            
            yield return null;
        }
        
        while (!forwards)
        {
            var distanceCovered = (Time.time - startTime) * moveSpeed;

            var fractionOfJourney = distanceCovered / journeyLength;

            fractionOfJourney = fractionOfJourney * fractionOfJourney * fractionOfJourney *
                                (fractionOfJourney * (6f * fractionOfJourney - 15f) + 10f);
            
            cam.transform.position = Vector2.Lerp(targetPosition.position, startingPosition , fractionOfJourney);

            if (Vector2.Equals(cam.transform.position, startingPosition))
            {
                introRunning = false;
                yield break;
            }
            
            yield return null;
        }
        
    }
}
