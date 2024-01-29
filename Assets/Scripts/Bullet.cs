using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class Bullet : NetworkBehaviour
{
    public float Harm { get; set; }
    public Action<NetworkObject> Disabled;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!HasStateAuthority) return;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeHealth(Harm);
            if (enemy.GetHealth() <= 0)
            {
                Runner.Despawn(collision.gameObject.GetComponent<NetworkObject>());
            }
            Disabled?.Invoke(gameObject.GetComponent<NetworkObject>());
        }
    }

}
