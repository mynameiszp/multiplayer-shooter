using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicalAid : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController player))
        {
            player.RecoverHealth();
        }
        Runner.Despawn(GetComponent<NetworkObject>());
    }
}
