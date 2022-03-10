using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class NetworkPlayer : NetworkBehaviour
{

    [SerializeField]
    private GameObject UserSelectionUI;
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
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
            GetComponentInChildren<InstructorControls>().enabled = false;
        }
    }

    private void DisableTraineeScripts(bool thisUser = false)
    {
        if (IsClient && !IsOwner || thisUser)
        {
            Trainee.GetComponentInChildren<Camera>().enabled = false;
            Trainee.GetComponentInChildren<AudioListener>().enabled = false;
            Trainee.GetComponent<Movement>().enabled = false;
        }

    }


    private IEnumerator ConfigureInstructor()
    {
        if (IsOwner && IsClient && !UserTypeSelected)
        {
            SetTraineeServerRpc(false);
            UserTypeSelected = true;
            DisableTraineeScripts(true);
            Instructor.GetComponentInChildren<InstructorControls>().enabled = true;
        }

        float delayedSync = (float)NetworkManager.Singleton.NetworkTimeSystem.HardResetThresholdSec;
        yield return new WaitForSeconds(delayedSync);
        UpdateMeshServerRpc();
        yield return null;
    }

    private IEnumerator ConfigureTrainee()
    {
        if (IsOwner && IsClient && !UserTypeSelected)
        {
            UserTypeSelected = true;
            SetTraineeServerRpc(true);
            DisableInstructorScripts(true);
        }
        yield return new WaitForSeconds(2f);
        UpdateMeshServerRpc();
        yield return null;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateMeshServerRpc()
    {
        UpdateMeshClientRpc();
    }

    [ClientRpc]
    void UpdateMeshClientRpc()
    {
        MeshRenderer r = GetComponentInChildren<MeshRenderer>();
        r.enabled = r.GetComponentInParent<NetworkPlayer>().IsTrainee.Value;
    }

    void Start()
    {
        if (IsOwner && IsClient)
        {
            UserSelectionUI.SetActive(true);
        }
    }

    public void SetUserTypeButton(bool isTrainee)
    {
        UserSelectionUI.SetActive(false);
        StartCoroutine(isTrainee ? nameof(ConfigureTrainee) : nameof(ConfigureInstructor));
        foreach (NetworkPlayer player in FindObjectsOfType<NetworkPlayer>())
        {
            if (player.IsTrainee.Value)
            {
                player.UpdateMeshServerRpc();
            }
        }
        
    }


}
