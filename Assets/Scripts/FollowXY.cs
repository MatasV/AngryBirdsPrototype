using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowXY : MonoBehaviour
{
    private Transform followTransform;

    public void Setup(Transform _transform)
    {
        followTransform = _transform;
    }

    // Update is called once per frame
    void Update()
    { 
        Vector2 newPos = new Vector2(followTransform.position.x, followTransform.position.y);
        transform.position = newPos;
    }
}
