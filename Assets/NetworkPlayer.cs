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

    // thisUser flag disables these scripts for that user, not just other users
    // i.e., to disable trainee scripts if the user selects instructor (and vice-versa)
    // If thisUser is false, it disables the scripts of all other users
    // This makes it so that one user doesn't control all users
    private void DisableInstructorScripts(bool thisUser = false)
    {
        if(IsClient && !IsOwner || thisUser)
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
            GetComponentInChildren<InstructorControls>().enabled = false;
            GetComponentInChildren<MultiCanvasControls>().enabled = false;
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


    private void ConfigureInstructor()
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
        NetworkManager.Singleton.NetworkTickSystem.Tick += UpdateOtherClientsServerRpc;
    }

    private void ConfigureTrainee()
    {
        if (IsOwner && IsClient && !UserTypeSelected)
        {
            UserTypeSelected = true;
            SetTraineeServerRpc(true);
            DisableInstructorScripts(true);
        }
        
        // Need to wait for network variable to be updated across clients
        NetworkManager.Singleton.NetworkTickSystem.Tick += UpdateOtherClientsServerRpc;
    }

    // Client tells server to run an update on all clients
    [ServerRpc(RequireOwnership = false)]
    private void UpdateOtherClientsServerRpc()
    {
        UpdateOtherClientsClientRpc();
    }

    // Rpc to update all clients when a new user selects user type
    // Makes trainees visible and adds that trainee's camera to the instructor menu
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

        // Remove client update after a tick
        NetworkManager.Singleton.NetworkTickSystem.Tick -= UpdateOtherClientsServerRpc;
    }

    void Start()
    {
        if (IsOwner && IsClient)
        {
            // Enable user type selection canvas
            UserSelectionUI.SetActive(true);
        }
    }

    // UI buttons for selecting user type
    public void SetUserTypeButton(bool isTrainee)
    {
        UserSelectionUI.SetActive(false);

        if (isTrainee)
        {
            ConfigureTrainee();
        }
        else
        {
            ConfigureInstructor();
        }
        
        foreach (NetworkPlayer player in FindObjectsOfType<NetworkPlayer>())
        {
            //if (player.IsTrainee.Value)
            {
                player.UpdateOtherClientsServerRpc();
            }
        }
        
    }


}
