using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Movement : NetworkBehaviour
{
    void Update()
    {
        if (IsOwner && IsClient)
        {
            if (Input.GetKey(KeyCode.W))
            {
                MovementServerRpc(Vector3.forward);
            }
            if (Input.GetKey(KeyCode.A))
            {
                MovementServerRpc(Vector3.left);
            }
            if (Input.GetKey(KeyCode.S))
            {
                MovementServerRpc(Vector3.back);
            }
            if (Input.GetKey(KeyCode.D))
            {
                MovementServerRpc(Vector3.right);
            }


        }
    }

    [ServerRpc]
    void MovementServerRpc(Vector3 direction)
    {
        Debug.Log("Executing movement");

        transform.position += direction * (Time.deltaTime * 5);


    }
}
