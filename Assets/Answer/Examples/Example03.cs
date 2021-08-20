// post example.
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace VRAcademy.Advanced.ServerIntegration
{

    internal sealed class Example03 : MonoBehaviour
    {
        [SerializeField] InputField nameField;
        [SerializeField] InputField mailField;

        public async void SendRequest()
        {
            //var user = await Request(Utility.HostName + "/user/test", new User{ name = "k-tanaka", email="contact@k-tanaka.dev" });
            var user = await Request(Utility.HostName + "/user/test", new User{ name = nameField.text, email = mailField.text });
            Debug.Log($"response => name:{user.name}, email:{user.email}");
        }

        private async UniTask<User> Request(string url, User user)
        {
            var json     = JsonUtility.ToJson( user );
            var payload = Encoding.UTF8.GetBytes( json );

            using (var req = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST, new DownloadHandlerBuffer(), new UploadHandlerRaw( payload )))
            {
                req.SetRequestHeader( "Content-Type", "application/json" );

                await req.SendWebRequest();

                Debug.Log($"{req.url} : {req.downloadHandler.text}");

                return JsonUtility.FromJson<User>(req.downloadHandler.text);
            }
        }
    }
}
