using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InstructorControls : NetworkBehaviour
{
    [SerializeField] private List<Camera> cameraList;
    [SerializeField] private Canvas instructorCanvas;
    [SerializeField] private Dropdown cameraDropdown;
    private List<string> dropdownOptions = new List<string>();
    
    private int playerCameraCount = 0;
    private int currentCamera = 0;

    private void OnEnable()
    {
        if (IsOwner && IsClient)
        {
            cameraList[currentCamera].enabled = true;
            cameraList[currentCamera].GetComponent<AudioListener>().enabled = true;
            instructorCanvas.enabled = true;
            for(int i = 0; i <cameraList.Count; i++)
            {
                dropdownOptions.Add($"CCTV Camera {i+1}");
            }
        }
    }

    // Arrow button method
    public void CameraButtons(bool nextCamera)
    {
        if (IsOwner && IsClient)
        {
            // Switches to next camera in the list if true (Next button) or previous cam if false (Previous button)
            if (nextCamera)
            {
                SwitchCamera(currentCamera + 1);
            }
            else
            {
                SwitchCamera(currentCamera - 1);
            }

            cameraDropdown.value = currentCamera;
        }

    }

    // Drop down menu camera selection
    public void SwitchCameraDropdown(Dropdown dropdown)
    {
        if (IsOwner && IsClient)
        {
            SwitchCamera(dropdown.value);
        }
    }

    private void SwitchCamera(int cameraId)
    {
        if (IsOwner && IsClient)
        {
            // Do nothing if the current camera is selected
            if (currentCamera == cameraId)
                return;
            
            // Prevent under/overflow
            // Count - 1 because .Count is not 0-index
            cameraId = Mathf.Clamp(cameraId, 0, cameraList.Count - 1);
            
            cameraList[currentCamera].enabled = false;
            cameraList[currentCamera].GetComponent<AudioListener>().enabled = false;
            
            cameraList[cameraId].enabled = true;
            cameraList[cameraId].GetComponent<AudioListener>().enabled = true;

            currentCamera = cameraId;
        }
    }

    // Used to add cameras of new players
    public void AddNewCamera(Camera cam)
    {
        if (IsOwner && IsClient)
        {
            // Counter used to uniquely number the trainee cams in the list
            playerCameraCount++;
            // Audio listeners only get enabled when the cam is active
            cam.GetComponent<AudioListener>().enabled = false;
            cameraList.Add(cam);

            // Clear existing options to refresh the list
            cameraDropdown.ClearOptions();

            // Create the new option (text field for the dropdown)
            string newCameraName = $"Trainee cam {playerCameraCount}";

            dropdownOptions.Add(newCameraName);
        
            // Assign new options to the list
            cameraDropdown.AddOptions(dropdownOptions);
            // Set active option to the active camera (resets to 0 after repopulating the list)
            cameraDropdown.value = currentCamera;
        }

    }
}
