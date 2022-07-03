// timeout
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace VRAcademy.HttpBasic
{
    internal sealed class Example06 : MonoBehaviour
    {
        [SerializeField] private int timeout = 1;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Request(Utility.LARGE_FILE_URL).Forget();
            }
        }

        private async UniTask Request(string url)
        {
            try
            {
                using (var req = UnityWebRequest.Get(url))
                {
                    { // リクエスト送信前の準備(ここではタイムアウトする秒数を指定している).
                        req.timeout = this.timeout;
                    }
                    await req.SendWebRequest().ToUniTask(Progress.Create<float>(x => Debug.Log($"progress: {x * 100}")));
                    Debug.Log($"completed => {req.url}");
                }
            }
            catch (UnityWebRequestException e)
            {
                Debug.LogWarning($"TimeOut!! -> {e.Error}");
                throw;
            }
        }
    }
}

