using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.Netcode;

public class NetworkStartup : MonoBehaviour
{
    private void Start()
    {
        if(SceneTransitionHandler.sceneTransitionHandler.InitializeAsHost)
        {
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}
