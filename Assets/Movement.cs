using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Movement : NetworkBehaviour
{

    private NetworkVariable<Vector3> position = new NetworkVariable<Vector3>();

    void Update()
    {
        if(IsOwner)
        {
            if (Input.GetKey(KeyCode.W))
            {
                UpdatePositionServerRpc(Vector3.forward, Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                UpdatePositionServerRpc(Vector3.left, Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                UpdatePositionServerRpc(Vector3.back, Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                UpdatePositionServerRpc(Vector3.right, Time.deltaTime);
            }

            transform.position = position.Value;

        }

    }

    [ServerRpc]
    void UpdatePositionServerRpc(Vector3 direction, float time)
    {
        position.Value += direction * (time * 5);
    }


}
