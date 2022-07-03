// download audio.
#pragma warning disable 1998
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
                UniTask.RunOnMainThread(async () =>
                {
                    //var res = await Request(Utility.HostName + "/assets/sample.wav");
                });
            }
        }

        //private async UniTask<AudioClip> Request(string url)
        //{
        //}
    }
}
