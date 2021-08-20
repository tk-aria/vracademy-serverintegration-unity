using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace VRAcademy.Advanced.ServerIntegration
{
    internal sealed class Example04 : MonoBehaviour
    {
        [SerializeField] private Material material;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                UniTask.RunOnMainThread(async () => material.mainTexture = await Request(Utility.HostName + "/assets/icon.png"));
            }
        }

        async UniTask<Texture> Request(string url)
        {
            using (var req = UnityWebRequestTexture.GetTexture(url))
            {
                //if(headers != null)
                //{
                //    foreach(var header in headers)
                //    {
                //        webRequest.SetRequestHeader(header.key, header.value);
                //    }
                //}

                await req.SendWebRequest();

                if (req.isNetworkError)
                {
                    Debug.Log(req.error);
                    return null;
                }

                return DownloadHandlerTexture.GetContent(req);
            }
        }
    }
}
