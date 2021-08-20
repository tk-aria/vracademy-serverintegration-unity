using System;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace VRAcademy.Advanced.ServerIntegration
{

    internal sealed class Example01 : MonoBehaviour
    {
        async void Start()
        {
            var result = await Request(Utility.HostName);
            Debug.Log(result);
        }

        private async UniTask<string> Request(string url)
        {
            using (var req = UnityWebRequest.Get(url))
            {
                await req.SendWebRequest().ToUniTask();
                Debug.Log($"{req.url} : {req.downloadHandler.text}");
                return req.downloadHandler.text;
            }
        }
    }
}

