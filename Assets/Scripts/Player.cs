using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Player : NetworkBehaviour
{
    //[Networked] public NetworkButtons ButtonsPrevious { get; set; }

    public void Init(Vector3 forward)
    {
        GetComponent<Rigidbody>().velocity = forward;
    }

    //public override void FixedUpdateNetwork()
    //{
    //    if (life.Expired(Runner))
    //        Runner.Despawn(Object);
    //}
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
        if (GetInput(out NetworkInputData data))
        {
            data.aimDirection.Normalize();
            gameObject.transform.position += (Vector3) data.aimDirection * Runner.DeltaTime;
        }
    }

    //public override void FixedUpdateNetwork()
    //{

    //    if (GetInput<NetworkInputData>(out var input) == false) return;

    //    // compute pressed/released state
    //    var pressed = input.buttons.GetPressed(ButtonsPrevious);
    //    var released = input.buttons.GetReleased(ButtonsPrevious);

    //    // store latest input as 'previous' state we had
    //    ButtonsPrevious = input.buttons;

    //    // movement (check for down)
    //    var vector = default(Vector3);

    //    if (input.buttons.IsSet(InputButtons.Forward)) { vector.z += 1; }
    //    if (input.buttons.IsSet(InputButtons.Backward)) { vector.z -= 1; }

    //    if (input.buttons.IsSet(InputButtons.Left)) { vector.x -= 1; }
    //    if (input.buttons.IsSet(InputButtons.Right)) { vector.x += 1; }

    //    DoMove(vector);
    //}

    //void DoMove(Vector3 vector)
    //{
    //    Debug.Log(vector);
    //}
}
