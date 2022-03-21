// using System.Net;
// using Unity.Netcode;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class IPConfiguration : MonoBehaviour
// {
//     
//     [SerializeField] private Text hostButtonText;
//
//     void Start()
//     {
//         hostButtonText.text = $"Start host on:\nIP: {GetHostIP()}";
//     }
//
//     // Tries to retrieve local Ipv4 address
//     IPAddress GetHostIP()
//     {
//         try
//         {
//             var host = Dns.GetHostEntry(Dns.GetHostName());
//             return host.AddressList[1];
//         }
//         catch
//         {
//             return IPAddress.None; // 255.255.255.255
//         }
//
//     }
//
//     // IP field doesn't require data validation since the transport accepts any string
//     public void ClientIPInputFieldChange(InputField inputField)
//     {
//         // Assign IP input to network transport
//         UnityTransport UTP = NetworkManager.Singleton.GetComponent<UnityTransport>();
//         UTP.ConnectionData.Address = inputField.text;
//     }
//
//     // Port field requires some rigid data validation to avoid exceptions
//     public void ClientPortInputFieldChange(InputField inputField)
//     {
//         ushort port;
//         inputField.characterValidation = InputField.CharacterValidation.Integer;
//         inputField.text = inputField.text.TrimStart('0');
//         try
//         {
//             port = ushort.Parse(inputField.text);
//         }
//         catch
//         {
//             inputField.text = "";
//             port = 7777;
//         }
//         // Assign port input to network transport
//         UnityTransport UTP = NetworkManager.Singleton.GetComponent<UnityTransport>();
//         UTP.ConnectionData.Port = port;
//     }
//     
// }
