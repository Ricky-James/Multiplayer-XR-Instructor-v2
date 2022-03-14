using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
public class SceneTransitionHandler : MonoBehaviour
{
    public static SceneTransitionHandler sceneTransitionHandler { get; private set; }

    [SerializeField]
    public string DefaultMainMenu = "SampleScene";


    public bool InitializeAsHost { get; set; }


    
    private void Awake()
    {
        if (sceneTransitionHandler != this && sceneTransitionHandler != null)
        {
            GameObject.Destroy(sceneTransitionHandler.gameObject);
        }
        sceneTransitionHandler = this;
    }


    public void Initialize()
    {
            SceneManager.LoadScene(DefaultMainMenu);
    }

}