using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsBox : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            Weapon weapon = player.GetComponentInChildren<Weapon>();
            if (weapon != null)
            {
                weapon.RefillBullets();
            }
        }
        Runner.Despawn(GetComponent<NetworkObject>());
    }
}
