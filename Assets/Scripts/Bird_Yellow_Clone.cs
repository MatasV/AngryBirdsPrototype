using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bird_Yellow_Clone : MonoBehaviour
{
    public Rigidbody2D rb;

    public void Init(Rigidbody2D _parentRB) {
        rb = GetComponent<Rigidbody2D>();
        rb.GetCopyOf(_parentRB);

        StageManager.instance.RegisterMovingEntity(rb);

        rb.velocity *= 0.6f;

        Invoke(nameof(DestroyMyself), 3f);
    }

    private void DestroyMyself()
    {
        StageManager.instance.UnregisterMovingEntity(rb);
        Destroy(gameObject);
    }
}
