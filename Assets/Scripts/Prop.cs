using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Prop : MonoBehaviour
{
    private void Start()
    {
        StageManager.instance.RegisterMovingEntity(GetComponent<Rigidbody2D>());
    }
}
