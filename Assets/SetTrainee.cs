using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

//public class SetTrainee : NetworkBehaviour
//{
//    public NetworkVariable<bool> IsTrainee = new NetworkVariable<bool>(false);
//    
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.E))
//        {
//            if (IsOwner)
//            {
//                SetTraineeServerRpc(true);
//            }
//
//            //GetComponent<MeshRenderer>().enabled = this.IsTrainee.Value;
//
//
//        }
//
//        if (Input.GetKeyDown(KeyCode.Q))
//        {
//            if (IsOwner)
//            {
//                SetTraineeServerRpc(false);
//            }
//            GetComponent<MeshRenderer>().enabled = this.IsTrainee.Value;
//        }
//
//    }
//
//
//
//    [ServerRpc]
//    void SetTraineeServerRpc(bool _isTrainee)
//    {
//        IsTrainee.Value = _isTrainee;
//        //FindObjectOfType<GameManager>().UpdateTraineeMeshStatusClientRpc();
//    }
//}
//