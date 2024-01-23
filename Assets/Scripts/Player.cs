using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Player : NetworkBehaviour
{
    public void Init(Vector3 forward)
    {
        GetComponent<Rigidbody>().velocity = forward;
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
        if (GetInput(out NetworkInputData data))
        {
            data.aimDirection.Normalize();
            gameObject.transform.position += (Vector3) data.aimDirection * Runner.DeltaTime;
        }
    }
}
