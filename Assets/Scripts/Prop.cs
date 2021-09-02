using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Prop : MonoBehaviour
{
    private StageManager stageManager;

    private void Awake()
    {
        stageManager = FindObjectOfType<StageManager>();
    }
    private void Start()
    {
        stageManager.RegisterMovingEntity(GetComponent<Rigidbody2D>());
    }
}
