using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpringJoint2D))]
public class Bird : MonoBehaviour
{
    private bool hasBeenLaunched = false;

    protected Rigidbody2D rigidBody;
    private Camera mainCam;
    private Transform slingShotPosition;
    private Sling slingShot;
    private SpringJoint2D springJoint;
    
    private float distanceToLaunch = 0.8f;
    private bool allowLaunch = false;
    
    private bool dragging = false;
    
    private const float MinVelocity = 0.2f;
    private const float MinPullDistanceToLaunch = 0.5f;
    private const float MaxDrag = 2.0f;

    private Vector2 launchVelocity;

    protected bool specialActivated = false;
    
    public virtual void Setup()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        slingShotPosition = FindObjectOfType<Sling>().GetComponent<Transform>();
        slingShot = FindObjectOfType<Sling>();
        mainCam = Camera.main;
        springJoint = GetComponent<SpringJoint2D>();
        springJoint.connectedBody = slingShot.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (dragging && !allowLaunch)
        {
            Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            launchVelocity = ((Vector2)slingShotPosition.position - rigidBody.position) * Mathf.Pow(springJoint.frequency,  3) ;
            if (Vector2.Distance(mousePos, slingShotPosition.position) > MaxDrag)
            {
                rigidBody.position = (Vector2)slingShotPosition.position + (mousePos - (Vector2)slingShotPosition.position).normalized * MaxDrag;
            }
            else
            {
                rigidBody.position = mousePos;
            }
            
            slingShot.ShowTrajectory();
        }
        else
        {
            if (slingShotPosition.gameObject.activeInHierarchy && allowLaunch &&
                Vector2.Distance(rigidBody.position, slingShotPosition.position) < MinPullDistanceToLaunch)
            {
                Launch();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (hasBeenLaunched) ActivateSpecial();
        }
    } 
    
    private void Launch()
    {
        hasBeenLaunched = true;
        slingShot.StopRenderingLines();
        slingShotPosition.gameObject.SetActive(false);
        StageManager.instance.RegisterMovingEntity(rigidBody);
        StageManager.instance.Launched();
    }

    public Vector2 CalculatePosition(float elapsedTime){
        return Physics2D.gravity * elapsedTime * elapsedTime * 0.5f +
               launchVelocity * elapsedTime + (Vector2)slingShotPosition.position;
    }
    
    private void OnMouseDown()
    {
        if (!hasBeenLaunched)
        {
            dragging = true;

            if (rigidBody != null)
            {
                rigidBody.isKinematic = true;
                slingShot.RenderLines(transform);
            }
            else
            {
                Debug.Log("Rigidbody Not Found on the bird");
            }
        }
    }
    
    protected virtual void ActivateSpecial()
    {
        if (!specialActivated)
        {
            Debug.Log("Default Special Activated");
            specialActivated = true;
        }
    }

    private void OnMouseUp()
    {
        if (!hasBeenLaunched)
        {
            dragging = false;

            if (rigidBody != null)
            {
                slingShot.HideTrajectory();
                rigidBody.isKinematic = false;
                if (Vector2.Distance(rigidBody.position, slingShotPosition.position) > distanceToLaunch)
                {
                    allowLaunch = true;
                }
            }
            else
            {
                Debug.Log("Rigidbody Not Found on the bird");
            }
        }
    }
}