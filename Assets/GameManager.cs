using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkManager
{
    void OnGUI()
    {
        if (!NetworkManager.Singleton.IsClient)
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            GUILayout.EndArea();
        }
    }

    void StartButtons()
    {
        if (GUILayout.Button("Host"))
        {
            NetworkManager.Singleton.StartHost();
        }

        if (GUILayout.Button("Client"))
        { 
            NetworkManager.Singleton.StartClient();
        }
    }
}
