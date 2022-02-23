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

    public override void OnNetworkSpawn()
    {
        DisableOtherClientInput();
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
        if(IsOwner && IsClient && !UserTypeSelected)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                UserTypeSelected = true;
                DisableTraineeScripts(true);
            }
            if(Input.GetKeyDown(KeyCode.E))
            {
                UserTypeSelected = true;
                Trainee.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                DisableInstructorScripts(true);
                
            }
        }

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
