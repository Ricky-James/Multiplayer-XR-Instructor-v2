using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.Collections;
using Unity.Netcode.Samples;
using Unity.Netcode.Transports;

public class GameManager : NetworkManager
{

    private void Start()
    {
        Singleton.ConnectionApprovalCallback += ApprovalCheck;
        Singleton.OnClientConnectedCallback += ClientConnected;
        
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, ConnectionApprovedDelegate callback)
    {
        // Can add logic to approve connection (e.g. max players)
        bool approve = true;
        bool createPlayerObject = true;

        // Null defaults to player prefab
        uint? prefabHash = null;

        //If approve is true, the connection gets added. If it's false. The client gets disconnected
        callback(createPlayerObject, prefabHash, approve, Vector3.zero, Quaternion.identity);
    }

    private void ClientConnected(ulong clientId)
    {
        UpdateTraineeMeshStateClientRpc();
    }

    [ClientRpc]
    public void UpdateTraineeMeshStateClientRpc()
    {
        foreach (GameObject User in GameObject.FindGameObjectsWithTag("User"))
        {
            if (User.GetComponent<NetworkPlayer>().isTrainee.Value)
            {
                User.GetComponentInChildren<MeshRenderer>().enabled = true;
            }
        }
    }

    private void Update()
    {
        UpdateTraineeMeshStateClientRpc();
    }
}
