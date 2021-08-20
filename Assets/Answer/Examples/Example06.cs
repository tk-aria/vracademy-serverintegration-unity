// timeout
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace VRAcademy.Advanced.ServerIntegration
{
    internal sealed class Example06 : MonoBehaviour
    {
        [SerializeField] private int timeout = 1;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Request(Utility.LARGE_FILE_URL);
            }
        }

        private async UniTask Request(string url)
        {
            try
            {
                using (var req = UnityWebRequest.Get(url))
                {
                    req.timeout = this.timeout;

                    // 送受信開始
                    await req.SendWebRequest().ToUniTask(Progress.Create<float>(x => Debug.Log($"progress: {x*100}")));
                                //.ToUniTask(Progress.Create<float>(x =>
                                //        // 値の変化を設定
                                //        _downloadProgress.Value = x * 100),
                                //        // cancellationToken: のラベルはつけること
                                //        cancellationToken: _token
                                //        );

                    if(req.isHttpError) {
                        Debug.Log($"[Error]Response Code : {req.responseCode}");
                    }
                    else if (req.isNetworkError) {
                        if (req.error == "Request timeout") {
                            // タイムアウト時の処理
                            Debug.Log($"[Error]TimeOut");
                            throw new UnityWebRequestException(req);
                        }
                        else {
                            Debug.Log($"[Error]Message : {req.error}");
                            throw new OperationCanceledException();
                        }
                    }
                    else{
                        // 成功したときの処理
                        Debug.Log($"[Success]");
                    }

                    /*
                    while (!req.isDone)
                    {
                        Debug.Log($"{req.url} : {req.progress * 100}");
                        await UniTask.DelayFrame(1);
                    }

                    var prev = req.progress;
                    float timeout = 30f;
                    float timer = 0f;
                    while(true)
                    {
                        timer += Time.deltaTime;

                        if (timer >= timeout)
                        {
                            if (prev < req.progress)
                            {
                                cts.Cancel();
                            }
                        }

                        prev = req.progress;
                    }
                    */

                    // エラーハンドリング
                    //if (req.isNetworkError || req.isHttpError) throw new Exception(req.error);

                    Debug.Log($"{req.url}");
                }
            }
            //catch (OperationCanceledException)
            //{
            //    Debug.LogWarning("Canceled!");
            //    throw new OperationCanceledException();
            //    //throw;
            //}
            catch (UnityWebRequestException e)
            {
                Debug.LogWarning($"TimeOut!! -> {e.Error}");
                throw;
                //throw new OperationCanceledException();
            }
        }
    }
}

