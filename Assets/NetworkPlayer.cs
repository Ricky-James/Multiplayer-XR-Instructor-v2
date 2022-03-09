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

    //public NetworkVariable<bool> IsTrainee = new NetworkVariable<bool>(false);
    //private NetworkVariable<bool> isInstructor = new NetworkVariable<bool>(false);

    //public bool IsTrainee { get; private set; }

    
    
    private GameManager gm;

    public override void OnNetworkSpawn()
    {
        gm = FindObjectOfType<GameManager>();
        if (IsOwner)
        {
          //  gm.AddNewUser(OwnerClientId);
        }

        //GameManager.OnPlayerJoin += DisableOtherClientInput;
        
        
        DisableOtherClientInput();
        
    }
    
    [ServerRpc]
    void SetTraineeServerRpc(bool _isTrainee)
    {
        IsTrainee.Value = _isTrainee;
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

            //movement.enabled = false;
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
