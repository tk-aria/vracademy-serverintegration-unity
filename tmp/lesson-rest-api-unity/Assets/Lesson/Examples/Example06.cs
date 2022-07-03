// timeout
#pragma warning disable 1998
#pragma warning disable 0414
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
        [SerializeField] private int timeout = 3;

        private async UniTask Request(string url)
        {
            try
            {
                //using (/* Getリクエストの生成 */)
                //{
                //    // タイムアウトの設定を行う
                //
                //    // リクエスト投げる
                //    Debug.Log($"completed => {req.url}");
                //}
            }
            catch (UnityWebRequestException e)
            {
                Debug.LogWarning($"TimeOut!! -> {e.Error}");
                throw;
            }
        }
    }
}

