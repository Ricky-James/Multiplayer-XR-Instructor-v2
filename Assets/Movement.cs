using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Movement : MonoBehaviour
{

    private Vector3 position { get; set; }

    void Update()
    {
        
        if (Input.GetKey(KeyCode.W))
        {
            UpdatePosition(Vector3.forward);
        }
        if (Input.GetKey(KeyCode.A))
        {
            UpdatePosition(Vector3.left);
        }
        if (Input.GetKey(KeyCode.S))
        {
            UpdatePosition(Vector3.back);
        }
        if (Input.GetKey(KeyCode.D))
        {
            UpdatePosition(Vector3.right);
        }

        transform.position = position;

    }

    void UpdatePosition(Vector3 direction)
    {
        position += direction * (Time.deltaTime * 5);
    }


}
