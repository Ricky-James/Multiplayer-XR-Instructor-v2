using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Movement : NetworkBehaviour
{

    private NetworkVariable<Vector3> position { get; set; }

    private void Start()
    {
        position.Value = transform.position;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ResetPositionServerRpc();
        }

        if (Input.GetKey(KeyCode.W))
        {
            UpdatePositionServerRpc(Vector3.forward);
        }
        if (Input.GetKey(KeyCode.A))
        {
            UpdatePositionServerRpc(Vector3.left);
        }
        if (Input.GetKey(KeyCode.S))
        {
            UpdatePositionServerRpc(Vector3.back);
        }
        if (Input.GetKey(KeyCode.D))
        {
            UpdatePositionServerRpc(Vector3.right);
        }

        transform.position = position.Value;

    }

    [ServerRpc]
    void UpdatePositionServerRpc(Vector3 direction)
    {
        position.Value += direction * (Time.deltaTime * 5);
    }

    [ServerRpc]
    void ResetPositionServerRpc()
    {
        position.Value = Vector3.zero;
    }


}
