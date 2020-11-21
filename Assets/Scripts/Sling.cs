using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Sling : MonoBehaviour
{
    private Transform birdTransform;
    private Bird birdComponent;

    [SerializeField] private LineRenderer leftLineRenderer;
    [SerializeField] private LineRenderer rightLineRenderer;

    [SerializeField] private GameObject trajectoryDotObj;
    
    private bool render = false;

    private int numberOfTrajectoryDots = 10;
    private float trajectoryTimeStep = 0.05f;

    public List<GameObject> trajectoryDots = new List<GameObject>();
    public void RenderLines([NotNull]Transform _birdTransform)
    {
        leftLineRenderer.positionCount = 2;
        rightLineRenderer.positionCount = 2;
        birdTransform = _birdTransform;
        render = true;
        birdComponent = birdTransform.GetComponent<Bird>();
        leftLineRenderer.enabled = true;
        rightLineRenderer.enabled = true;
    }
    private void InstantiateTrajectory()
    {
        for (int i = 0; i < numberOfTrajectoryDots; i++) {
            var trajectoryDot = Instantiate (trajectoryDotObj);
            trajectoryDots.Add(trajectoryDot);
            var color = trajectoryDot.GetComponent<SpriteRenderer>().color;
            color.a = 1.0f / (numberOfTrajectoryDots+1.0f) * (numberOfTrajectoryDots-i+1.0f);
            trajectoryDot.GetComponent<SpriteRenderer>().color = color;
            trajectoryDot.SetActive(false);
        }
    }
    public void ShowTrajectory()
    {
        for (int i = 0; i < numberOfTrajectoryDots; i++) {
            trajectoryDots[i].SetActive(true);
            trajectoryDots[i].transform.position = birdComponent.CalculatePosition(trajectoryTimeStep * i);
        }
    }
    public void HideTrajectory()
    {
        for (int i = 0; i < numberOfTrajectoryDots; i++) {
            trajectoryDots[i].SetActive(false);
        }
    }
    public void StopRenderingLines()
    {
        leftLineRenderer.positionCount = 1;
        rightLineRenderer.positionCount = 1;
        birdTransform = null;
        render = false;
        leftLineRenderer.enabled = false;
        rightLineRenderer.enabled = false;
    }
    private void Start()
    {
        leftLineRenderer.enabled = true;
        rightLineRenderer.enabled = true;
        leftLineRenderer.positionCount = 1;
        rightLineRenderer.positionCount = 1;
        leftLineRenderer.SetPosition(0, leftLineRenderer.transform.position);
        rightLineRenderer.SetPosition(0, rightLineRenderer.transform.position);
        leftLineRenderer.sortingOrder = 3;
        rightLineRenderer.sortingOrder = -1;
        InstantiateTrajectory();
    }

    void Update()
    {
        if (render && birdTransform != null)
        {
            leftLineRenderer.SetPosition(1, birdTransform.position + (birdTransform.position - leftLineRenderer.transform.position).normalized * (birdTransform.localScale.x/4));
            rightLineRenderer.SetPosition(1,birdTransform.position + (birdTransform.position - leftLineRenderer.transform.position).normalized * (birdTransform.localScale.x/4));
        }
    }
}
