using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

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
    private bool TraineeCamAddedRegistered = false;


    
    public override void OnNetworkSpawn()
    {
        DisableOtherClientInput();
        
    }
    
    [ServerRpc]
    void SetTraineeServerRpc(bool _isTrainee)
    {
        IsTrainee.Value = _isTrainee;
    }


    

    private void DisableOtherClientInput()
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
            InstructorControls instructorControls = Instructor.GetComponent<InstructorControls>();
            instructorControls.enabled = true;
        }
        // Need to wait for network variable to be updated across clients
        float delayedSync = (float)NetworkManager.Singleton.NetworkTimeSystem.HardResetThresholdSec;
        yield return new WaitForSeconds(delayedSync);
        UpdateOtherClientsServerRpc();
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
        float delayedSync = (float)NetworkManager.Singleton.NetworkTimeSystem.HardResetThresholdSec;
        yield return new WaitForSeconds(delayedSync);
        UpdateOtherClientsServerRpc();



        yield return null;
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateOtherClientsServerRpc()
    {
        UpdateOtherClientsClientRpc();
    }

    [ClientRpc]
    private void UpdateOtherClientsClientRpc()
    {
        // Toggle trainee meshes on
        MeshRenderer r = GetComponentInChildren<MeshRenderer>();
        r.enabled = r.GetComponentInParent<NetworkPlayer>().IsTrainee.Value;

        // If this user is a trainee, add their camera to the list of camera options for the instructor
        if (IsTrainee.Value && TraineeCamAddedRegistered == false)
        {
            TraineeCamAddedRegistered = true;
            Camera traineeCam = Trainee.GetComponentInChildren<Camera>();
            foreach (InstructorControls ic in FindObjectsOfType<InstructorControls>())
            {
                ic.AddNewCamera(traineeCam);
            }
        }

        

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
                player.UpdateOtherClientsServerRpc();
            }
        }
        
    }


}
