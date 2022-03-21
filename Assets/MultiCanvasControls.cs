using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MultiCanvasControls : NetworkBehaviour
{
    // Script for the multi-camera instructor view

    // 0 = Top left, 1 = Top right, 2 = Bottom left, 3 = Bottom right
    [SerializeField] private Dropdown[] dropdowns = new Dropdown[4];
    [SerializeField] private RenderTexture[] renderTextures = new RenderTexture[4]; // Materials for render textures
   
    private InstructorControls instructorControls;
    
    // 0 = Top left, 1 = Top right, 2 = Bottom left, 3 = Bottom right 
    // Used to track the previously assigned camera after the dropdown changes value
    private int[] previousCam = new int[4];
    
    // Set when the value of a dropdown menu changes, to identify which dropdown is being changed
    public int currentDropdown { set; private get; }
    

    private void OnEnable()
    {
        if (IsOwner)
        {
            instructorControls = GetComponent<InstructorControls>();
            // Assign some default values (Cameras 1-4 for the 4 views)
            for (int i = 0; i < 4; i++)
            {
                instructorControls.cameraList[i].enabled = true;
                instructorControls.cameraList[i].targetTexture = renderTextures[i];
                
                dropdowns[i].value = i;
                previousCam[i] = i;
            }
        }

    }

    public void SwitchCameraDropdown(Dropdown dropdown)
    {
        if (IsOwner)
        {
            // Check required to prevent unintended behaviour when first switching to multi-view
            if (instructorControls.cameraList[dropdown.value].enabled == false)
            {
                // Disable/remove RT from old cam
                instructorControls.cameraList[previousCam[currentDropdown]].targetTexture = null;
                instructorControls.cameraList[previousCam[currentDropdown]].enabled = false;

                // Assign new cam
                instructorControls.cameraList[dropdown.value].enabled = true;
                instructorControls.cameraList[dropdown.value].targetTexture = renderTextures[currentDropdown];

                // Update new 'previous' value
                previousCam[currentDropdown] = dropdown.value; 
            }

        }
    }

    public void UpdateDropdowns(List<string> dropdownOptions)
    {
        if (IsOwner)
        {
            for(int i = 0; i < 4; i++)
            {
                int currentValue = dropdowns[i].value;
                // Repopulate lists and set active value to their existing value
                dropdowns[i].ClearOptions();
                dropdowns[i].AddOptions(dropdownOptions);
                dropdowns[i].value = currentValue;
            }
        }

    }
}
