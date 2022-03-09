using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class NetworkPlayer : NetworkBehaviour
{

    //[SerializeField]
    //private Image UserSelectionUI;
    [SerializeField]
    private GameObject Instructor;
    [SerializeField]
    private GameObject Trainee;

    private bool UserTypeSelected = false;
    
    public NetworkVariable<bool> IsTrainee = new NetworkVariable<bool>(false);


    
    public override void OnNetworkSpawn()
    {
        DisableOtherClientInput();
        
    }
    
    [ServerRpc]
    void SetTraineeServerRpc(bool _isTrainee)
    {
        IsTrainee.Value = _isTrainee;
    }


    

    public void DisableOtherClientInput()
    {
        
        // Disable scripts of other users
        if(IsClient && !IsOwner)
        {
            DisableInstructorScripts();
            DisableTraineeScripts();
        }
    }

    private void DisableInstructorScripts(bool thisUser = false)
    {
        if(IsClient && !IsOwner || thisUser)
        {
            var clientCamera = Instructor.GetComponentInChildren<Camera>();
            var audioListener = Instructor.GetComponentInChildren<AudioListener>();

            clientCamera.enabled = false;
            audioListener.enabled = false;
        }
    }

    private void DisableTraineeScripts(bool thisUser = false)
    {
        if (IsClient && !IsOwner || thisUser)
        {
            var clientCamera = Trainee.GetComponentInChildren<Camera>();
            var audioListener = Trainee.GetComponentInChildren<AudioListener>();
            var movement = Trainee.GetComponent<Movement>();

            movement.enabled = false;
            clientCamera.enabled = false;
            audioListener.enabled = false;

        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(nameof(ConfigureInstructor));

        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(nameof(ConfigureTrainee));
        }

    }

    IEnumerator ConfigureInstructor()
    {
        if (IsOwner && IsClient && !UserTypeSelected)
        {
            SetTraineeServerRpc(false);
            SpawnAsInstructor();
        }

        yield return new WaitForSeconds((float)NetworkManager.Singleton.NetworkTimeSystem.HardResetThresholdSec);
        UpdateMeshServerRpc();
        yield return null;
    }

    IEnumerator ConfigureTrainee()
    {
        if (IsOwner && IsClient && !UserTypeSelected)
        {
            SetTraineeServerRpc(true);
            SpawnAsTrainee();
        }
        yield return new WaitForSeconds(2f);
        UpdateMeshServerRpc();
        yield return null;
    }

    [ServerRpc(RequireOwnership = false)]
    void UpdateMeshServerRpc()
    {
        MeshRenderer r = GetComponentInChildren<MeshRenderer>();
        r.enabled = r.GetComponentInParent<NetworkPlayer>().IsTrainee.Value;
        UpdateMeshClientRpc();
    }

    [ClientRpc(Delivery = RpcDelivery.Reliable)]
    void UpdateMeshClientRpc()
    {
        MeshRenderer r = GetComponentInChildren<MeshRenderer>();
        r.enabled = r.GetComponentInParent<NetworkPlayer>().IsTrainee.Value;
    }

    void SpawnAsInstructor()
    {

        UserTypeSelected = true;
        DisableTraineeScripts(true);

    }
    
    void SpawnAsTrainee()
    {

        UserTypeSelected = true;
        DisableInstructorScripts(true);

    }

}
