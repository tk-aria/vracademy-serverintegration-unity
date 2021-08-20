// cancellation process.
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace VRAcademy.Advanced.ServerIntegration
{
    internal sealed class Example08 : MonoBehaviour
    {
        [SerializeField] private int timeout;
        private CancellationTokenSource cts;

        void Start()
        {
            //cts = new CancellationTokenSource();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                cts = new CancellationTokenSource();
                Request(Utility.LARGE_FILE_URL);
            }

            if(Input.GetKeyDown(KeyCode.C))
            {
                Cancel();
            }
        }

        public void Cancel()
        {
            cts.Cancel();
        }

        private async UniTask Request(string url)
        {
            try
            {
                using (var req = UnityWebRequest.Get(url))
                {
                    //req.timeout = this.timeout;
                    cts.CancelAfterSlim(TimeSpan.FromSeconds(this.timeout));

                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    await req.SendWebRequest().ToUniTask(Progress.Create<float>(x => Debug.Log($"progress: {x*100}, Elapsed: {sw.Elapsed}")), cancellationToken: cts.Token);

                    /*
                    var prev = req.downloadProgress;
                    float timer = 0f;
                    bool clear = true;

                    while (!req.isDone && clear)
                    {
                        Debug.Log($"{req.url} : {req.downloadProgress * 100}");

                        timer += Time.deltaTime;

                        if (timer >= timeout)
                        {
                            //if (prev == req.downloadProgress)
                            {
                                Debug.Log("cancel");
                                cts.Cancel();
                                clear = false;
                                req.Abort();
                            }
                            //timer = 0;
                        }

                        prev = req.downloadProgress;

                        await UniTask.Yield();
                    }
                    */

                    Debug.Log("complete");

                    // エラーハンドリング
                    if (req.isNetworkError || req.isHttpError) throw new Exception(req.error);

                    Debug.Log($"{req.url}");
                }
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                {
                    Debug.LogWarning("Canceled!");
                    throw;
                }

                if (ex is UnityWebRequestException uwe)
                {
                    Debug.Log($"status* {uwe.ResponseCode} => [{uwe.Error}] : {uwe.Message}");
                    throw new OperationCanceledException();
                    // ステータスコードを使って、タイトルに戻す例外です、とかリトライさせる例外です、とかハンドリングさせると便利
                    // if (uwe.ResponseCode) { }...
                }
                throw;
            }
        }
    }
}

