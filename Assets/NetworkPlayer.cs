using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkPlayer : NetworkBehaviour
{

    //[SerializeField]
    //private Image UserSelectionUI;
    [SerializeField]
    private GameObject Instructor;
    [SerializeField]
    private GameObject Trainee;

    private bool UserTypeSelected = false;

    public NetworkVariable<bool> IsInstructor = new NetworkVariable<bool>(false);

    public override void OnNetworkSpawn()
    {
        // Find associated user settings
        //foreach(UserSettings settings in FindObjectsOfType<UserSettings>())
        //{
        //    NetworkObject o = settings.GetComponent<NetworkObject>();
        //    if(o.OwnerClientId == )
        //}


        IsInstructor = FindObjectOfType<UserSettings>().IsInstructor;
        DisableOtherClientInput();

        if (IsOwner && IsClient && !UserTypeSelected)
        {
            if (IsInstructor.Value)
            {
                UserTypeSelected = true;
                DisableTraineeScripts(true);

            }
            else
            {
               // SetTraineeServerRpc();
                UserTypeSelected = true;
                DisableInstructorScripts(true);

            }
        }
    }

    ////UI Button
    //public void SetUserTypeTrainee()
    //{
    //    Debug.Log("Trainee set 1");
    //    //if(IsOwner && IsClient)
    //    {
    //        Debug.Log("Trainee set 2");
    //        userType = UserType.Trainee;
    //        //UserSelectionUI.enabled = false;
    //        DisableClientInput();
    //    }
    //}

    //// UI Button
    //public void SetUserTypeInstructor()
    //{
    //    //if (IsOwner && IsClient)
    //    {
    //        userType = UserType.Instructor;
    //        //UserSelectionUI.enabled = false;
    //        DisableClientInput();
    //    }
    //}

    

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

    }



    //[ServerRpc]
    //private void SetTraineeServerRpc()
    //{
    //    isTrainee.Value = true;
    //    Trainee.GetComponent<MeshRenderer>().enabled = true;
    //}


    // Function runs on player connection to enable the meshes of trainees
    public void EnableMesh()
    {
        //foreach (var x in GameObject.FindGameObjectsWithTag("User"))
        //{
        //    if (x.GetComponent<NetworkPlayer>().isTrainee.Value)
        //    {
        //        x.GetComponentInChildren<MeshRenderer>().enabled = true;
        //    }
        //}
    }

    private void Start()
    {

        if (IsClient && IsOwner)
        {
            Debug.Log("Test");
            //UserSelectionUI = transform.Find("UserTypeSelectionCanvas").gameObject;
            //Instructor = transform.Find("Instructor").gameObject;
            //Trainee = transform.Find("Trainee").gameObject;

            //if(Input.GetKeyDown(KeyCode.Q))
            //{
                //Instructor.SetActive(true);
                //UserSelectionUI.SetActive(false);
            //}
            //if(Input.GetKeyDown(KeyCode.E))
            //{
            //    Trainee.SetActive(true);
            //    UserSelectionUI.SetActive(false);
            //}
        }
    }
}
