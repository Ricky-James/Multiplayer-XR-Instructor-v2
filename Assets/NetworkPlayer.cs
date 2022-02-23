using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField]
    private Vector2 placementArea = new Vector2(-10.0f, 10.0f);
    [SerializeField]
    private GameObject UserSelectionUI;
    [SerializeField]
    private GameObject Instructor;
    [SerializeField]
    private GameObject Trainee;

    public override void OnNetworkSpawn()
    {
        DisableClientInput();

    }

    public void DisableClientInput()
    {
        // Disable scripts of other users
        if(IsClient && !IsOwner)
        {
            var movement = GetComponentInChildren<Movement>();
            var clientCamera = GetComponentInChildren<Camera>();

            movement.enabled = false;
            clientCamera.enabled = false;

        }
    }

    private void Start()
    {
        if(IsClient && IsOwner)
        {
            Debug.Log("Test");
            //UserSelectionUI = transform.Find("UserTypeSelectionCanvas").gameObject;
            //Instructor = transform.Find("Instructor").gameObject;
            //Trainee = transform.Find("Trainee").gameObject;

            //if(Input.GetKeyDown(KeyCode.Q))
            //{
                Instructor.SetActive(true);
                UserSelectionUI.SetActive(false);
            //}
            //if(Input.GetKeyDown(KeyCode.E))
            //{
            //    Trainee.SetActive(true);
            //    UserSelectionUI.SetActive(false);
            //}
            transform.position = new Vector3(Random.Range(placementArea.x, placementArea.y), transform.position.y, Random.Range(placementArea.x, placementArea.y));
        }
    }
}
