using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : NetworkBehaviour
{
    [SerializeField] private float _radius;
    private Vector2 _center;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _center = transform.position;
        Collider2D[] objects = Physics2D.OverlapCircleAll(_center, _radius);
        foreach (var obj in objects)
        {
            if (obj.TryGetComponent(out Enemy enemy))
            {
                enemy.Destroy();
            }
        }
        Runner.Despawn(GetComponent<NetworkObject>());
    }
}
