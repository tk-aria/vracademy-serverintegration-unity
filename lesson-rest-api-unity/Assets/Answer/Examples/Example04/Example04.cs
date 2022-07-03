using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace VRAcademy.HttpBasic
{
    internal sealed class Example04 : MonoBehaviour
    {
        [SerializeField] private Material material;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                SendRequestAsync().Forget();
            }
        }

        private async UniTaskVoid SendRequestAsync()
            => material.mainTexture = await Request(Utility.HostName + "/assets/icon.png");

        async UniTask<Texture> Request(string url)
        {
            using (var req = UnityWebRequestTexture.GetTexture(url))
            {
                await req.SendWebRequest();
                Debug.Log($"completed => {req.url}");
                return DownloadHandlerTexture.GetContent(req);
            }
        }
    }
}
