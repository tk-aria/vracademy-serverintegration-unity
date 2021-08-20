using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace VRAcademy.Advanced.ServerIntegration
{

    internal sealed class Example05 : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("Send Request");
                UniTask.RunOnMainThread(async () => {
                    var res = await Request(Utility.HostName + "/assets/sample.wav");
                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }
                    audioSource.clip = res;
                    audioSource.Play();
                });
            }
        }

        private async UniTask<AudioClip> Request(string url)
        {
            using (var req = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
            {
                //if(headers != null)
                //{
                //    foreach(var header in headers)
                //    {
                //        webRequest.SetRequestHeader(header.key, header.value);
                //    }
                //}

                //await req.SendWebRequest();
                await req.SendWebRequest().ToUniTask(Progress.Create<float>(x => Debug.Log($"progress: {x*100}")));


                if (req.isNetworkError)
                {
                    Debug.Log(req.error);
                    return null;
                }

                return DownloadHandlerAudioClip.GetContent(req);
            }
        }
    }
}
