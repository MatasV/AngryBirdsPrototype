using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Prop : MonoBehaviour
{
    [SerializeField] private StageManager stageManager;
    private void Start()
    {
        stageManager.RegisterMovingEntity(GetComponent<Rigidbody2D>());
    }
}
