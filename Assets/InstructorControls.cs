using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InstructorControls : NetworkBehaviour
{
    [SerializeField] private Camera[] cameraList;
    [SerializeField] private Canvas instructorCanvas;
    
    private int currentCamera = 0;

    private void OnEnable()
    {
        if (IsOwner && IsClient)
        {
            cameraList[currentCamera].enabled = true;
            cameraList[currentCamera].GetComponent<AudioListener>().enabled = true;
            instructorCanvas.enabled = true;
        }
    }

    // Right-arrow button to advance forward
    public void NextCamera()
    {
        SwitchCamera(currentCamera + 1);
    }
    
    // Left-arrow button to traverse backward
    public void PreviousCamera()
    {
        SwitchCamera(currentCamera - 1);
    }
    
    // Drop down menu camera selection
    public void SwitchCameraDropdown(Dropdown selection)
    {
        if (IsOwner && IsClient)
        {
            SwitchCamera(selection.value);
        }
    }

    private void SwitchCamera(int cameraId)
    {
        if (IsOwner && IsClient)
        {
            if (currentCamera == cameraId)
                return;
            
            cameraId = Mathf.Clamp(cameraId, 0, cameraList.Length);
            
            cameraList[currentCamera].enabled = false;
            cameraList[currentCamera].GetComponent<AudioListener>().enabled = false;
            
            cameraList[cameraId].enabled = true;
            cameraList[cameraId].GetComponent<AudioListener>().enabled = true;

            currentCamera = cameraId;
        }
    }
}
