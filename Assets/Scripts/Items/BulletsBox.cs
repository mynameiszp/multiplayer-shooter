using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsBox : MonoBehaviour
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
    }
}
