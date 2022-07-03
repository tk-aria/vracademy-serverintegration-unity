using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace VRAcademy.HttpBasic
{
    internal sealed class Example04_v2 : MonoBehaviour
    {
        public void UploadFile(string filePath)
            => SendRequestAsync($"{Utility.HostName}/upload", filePath).Forget();

        //"image/jpeg"
        private async UniTaskVoid SendRequestAsync(string url, string filePath, string fileType = "multipart/form-data")
        {
            byte[] img = File.ReadAllBytes(filePath);

            WWWForm form = new WWWForm();
            form.AddBinaryData("file", img, Path.GetFileName(filePath), fileType);

            using (var req = UnityWebRequest.Post(url, form))
            {
                await req.SendWebRequest().ToUniTask();
                Debug.Log($"{req.url} : {req.downloadHandler.text}");
            }
        }
    }
}
