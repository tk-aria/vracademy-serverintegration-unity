using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace VRAcademy.Advanced.ServerIntegration
{

    internal sealed class Example02 : MonoBehaviour
    {
        [SerializeField] private Text messageText;

        public async void SendRequest()
            => messageText.text = await Request(Utility.HostName);

        private async UniTask<string> Request(string url)
        {
            using (var req = UnityWebRequest.Get(url))
            {
                await req.SendWebRequest();
                Debug.Log($"{req.url} : {req.downloadHandler.text}");
                return req.downloadHandler.text;
            }
        }
    }
}
