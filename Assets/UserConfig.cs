using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class UserConfig : NetworkBehaviour
{

    public GameObject Instructor;
    public GameObject Trainee;
    public GameObject UserTypeCanvas;

    public override void OnNetworkSpawn()
    {
        if (IsOwner && IsClient)
        {
            UserTypeCanvas.SetActive(true);
        }

        if (!IsOwner && IsClient)
        {
            UserTypeCanvas.SetActive(false);

        }
    }

    public void SpawnTrainee()
    {
        if (IsOwner && IsClient)
        {
            Instructor.SetActive(false);
            UserTypeCanvas.SetActive(false);
        }
        if (!IsOwner && IsClient)
        {
            TraineeConfig();
        }
    }

    public void SpawnInstructor()
    {
        if (IsOwner && IsClient)
        {
            Debug.Log("Spawn instructor");
            Trainee.SetActive(false);
            UserTypeCanvas.SetActive(false);
        }
        if (!IsOwner && IsClient)
        {
            InstructorConfig();
        }
    }

    // Disable all other player's scripts
    private void TraineeConfig()
    {
        Camera.main.enabled = false;
        Trainee.GetComponent<Camera>().enabled = false;
    }
    private void InstructorConfig()
    {
        Camera.main.enabled = false;
        Instructor.GetComponent<Camera>().enabled = false;
        Instructor.GetComponent<Movement>().enabled = false;
    }

}
