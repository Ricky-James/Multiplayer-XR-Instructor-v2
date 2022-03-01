using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserSettings : NetworkBehaviour
{

    public NetworkVariable<bool> IsInstructor = new NetworkVariable<bool>(false);
    public Toggle InstructorToggle;

    private void Start()
    {
        DontDestroyOnLoad(this);

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (InstructorToggle == null)
                InstructorToggle = FindObjectOfType<Toggle>();
        }
        else
        {
            InstructorToggle = null;
        }
    }

    public void SetInstructor()
    {
        IsInstructor.Value = InstructorToggle.isOn;
    }
}
