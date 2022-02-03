using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace VRAcademy.HttpBasic
{

    internal sealed class Example05 : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("Send Request");
                SendRequestAsync().Forget();
            }
        }

        async UniTaskVoid SendRequestAsync()
        {
            var res = await Request(Utility.HostName + "/assets/sample.wav");
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource.clip = res;
            audioSource.Play();
        }

        private async UniTask<AudioClip> Request(string url)
        {
            using (var req = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
            {
                //await req.SendWebRequest();
                await req.SendWebRequest().ToUniTask(Progress.Create<float>(x => Debug.Log($"progress: {x * 100}")));
                Debug.Log($"completed => {req.url}");
                return DownloadHandlerAudioClip.GetContent(req);
            }
        }
    }
}
