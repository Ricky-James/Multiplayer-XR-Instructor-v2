using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.Collections;
using Unity.Netcode.Samples;
using Unity.Netcode.Transports;

internal struct TraineeDictionary
{
    public ulong clientId { get; set; }
    public bool IsTrainee { get; set; }
}

public class GameManager : NetworkManager
{

    public delegate void PlayerJoined();

    public static event PlayerJoined OnPlayerJoin;

    //private Dictionary<GameObject, bool> ClientIdTraineeStatus = new Dictionary<GameObject, bool>();

    private NetworkList<bool> _clientTraineeStatus;

    private void Start()
    {
        _clientTraineeStatus = new NetworkList<bool>();
        Singleton.ConnectionApprovalCallback += ApprovalCheck;
        //Singleton.OnClientConnectedCallback += ClientConnected;
        //OnPlayerJoin += UpdateTraineeMeshStateClientRpc;
        //ClientTraineeStatus.OnListChanged += UpdateMeshClientRpc;
    }

    //[ClientRpc]
    //void UpdateMeshClientRpc(NetworkListEvent<bool> status)
    //{
    //    for (int i = 0; i < ClientTraineeStatus.Count; i++)
    //    {
    //        var user = ClientTraineeStatus[i];
    //        NetworkManager.Singleton.cli
    //        user.GetComponentInChildren<MeshRenderer>().enabled = user.Value;
    //        NetworkLog.LogInfoServer($"Updating Mesh for {user.Key.name}");
    //        //ClientIdTraineeStatus.ElementAt(i).Key.GetComponent<MeshRenderer>().enabled = 
    //    }
    //}

    //[ServerRpc]
    //public void AddNewUserServerRpc(GameObject user, bool isTrainee)
    //{
    //    string UserName = user.name;
    //    NetworkLog.LogInfoServer($"New user added {user.name}");
    //    if (IsHost)
    //    {
    //        ClientIdTraineeStatus.Add(user, isTrainee);
    //    }
    //    UpdateMeshClientRpc();
    //    
    //}
    [ServerRpc]
    public void AddNewUser(ulong clientId)
    {
        if (IsHost)
        {
            _clientTraineeStatus.Add(false);
        }

    }

    [ServerRpc]
    public void UpdateUserTypeServerRpc(ulong clientId, bool isTrainee)
    {
        if (IsHost)
        {
            _clientTraineeStatus[(int)clientId] = isTrainee;
            for (int i = 0; i < _clientTraineeStatus.Count; i++)
            {
                UpdateMeshClientRpc(NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject, _clientTraineeStatus[(int)clientId]);
            }
        }
    }
    
    [ClientRpc]
    private void UpdateMeshClientRpc(GameObject user, bool IsTrainee)
    {
        user.GetComponentInChildren<MeshRenderer>().enabled = IsTrainee;
    }
    
    //[ClientRpc]
    //private void UpdateMeshClientRpc()
    //{
    //    
    //    for (int i = 0; i < ClientIdTraineeStatus.Count; i++)
    //    {
    //        var user = ClientIdTraineeStatus.ElementAt(i);
    //        user.Key.GetComponentInChildren<MeshRenderer>().enabled = user.Value;
    //        NetworkLog.LogInfoServer($"Updating Mesh for {user.Key.name}");
    //        //ClientIdTraineeStatus.ElementAt(i).Key.GetComponent<MeshRenderer>().enabled = 
    //    }
    //}

    public void Join()
    {
        if(OnPlayerJoin != null)
            OnPlayerJoin();
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

    //[ClientRpc]
    //public void UpdateTraineeMeshStatusClientRpc()
    //{
    //    foreach (SetTrainee g in FindObjectsOfType<SetTrainee>())
    //    {
    //        g.GetComponent<MeshRenderer>().enabled = g.IsTrainee.Value;
    //    }
    //}

    //private void ClientConnected(ulong clientId)
    //{
    //    foreach (SetTrainee g in FindObjectsOfType<SetTrainee>())
    //    {
    //        GetComponent<MeshRenderer>().enabled = g.IsTrainee.Value;
    //    }
    //}

    //[ClientRpc]
    //public void UpdateTraineeMeshStateClientRpc()
    //{
    //    NetworkLog.LogInfoServer("Setting Meshes...");
    //    foreach (NetworkPlayer User in FindObjectsOfType<NetworkPlayer>())
    //    {
    //        if (User.IsTrainee.Value)
    //        {
    //            NetworkLog.LogInfoServer("Mesh enabled");
    //            User.GetComponentInChildren<MeshRenderer>().enabled = true;
    //        }
    //    }
    //}

    //[ServerRpc]
    //public void SetBoolServerRpc(NetworkVariable<bool> _bool, bool value)
    //{
    //    if (IsHost)
    //    {
    //        _bool.Value = value;
    //    }
    //    UpdateTraineeMeshStateClientRpc();
    //}

    private void Update()
    {
        //UpdateTraineeMeshStateClientRpc();
    }
}
