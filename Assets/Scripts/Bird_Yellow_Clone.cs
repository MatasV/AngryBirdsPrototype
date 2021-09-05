using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bird_Yellow_Clone : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] private StageManager stageManager;
    public void Init(Rigidbody2D _parentRB, StageManager _stageManager) {
        rb = GetComponent<Rigidbody2D>();
        rb.GetCopyOf(_parentRB);

        stageManager = _stageManager;
        stageManager.RegisterMovingEntity(rb);

        rb.velocity *= 0.6f;

        Invoke(nameof(DestroyMyself), 3f);
    }

    private void DestroyMyself()
    {
        stageManager.UnregisterMovingEntity(rb);
        Destroy(gameObject);
    }
}
