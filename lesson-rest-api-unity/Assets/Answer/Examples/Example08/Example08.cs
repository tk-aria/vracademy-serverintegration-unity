// cancellation process.
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace VRAcademy.HttpBasic
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
                Request(Utility.LARGE_FILE_URL, cts).Forget();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Cancel();
            }
        }

        public void Cancel()
        {
            cts.Cancel();
        }

        private async UniTaskVoid Request(string url, CancellationTokenSource cts)
        {
            try
            {
                using (var req = UnityWebRequest.Get(url))
                {
                    //req.timeout = this.timeout;
                    cts.CancelAfterSlim(TimeSpan.FromSeconds(this.timeout));

                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    await req.SendWebRequest().ToUniTask(Progress.Create<float>(x => Debug.Log($"progress: {x * 100}, Elapsed: {sw.Elapsed}")), cancellationToken: cts.Token);

                    Debug.Log($"completed => {req.url}");
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

                    switch (uwe.Result)
                    {
                        case UnityWebRequest.Result.InProgress:
                            break;

                        case UnityWebRequest.Result.ConnectionError:// isNetworkError
                            {
                                if (uwe.Error == "Request timeout")
                                {
                                    Debug.LogError($"TimeOut!! -> {uwe.Error}");
                                }
                            }
                            break;

                        case UnityWebRequest.Result.ProtocolError:// isHttpError
                            break;

                        case UnityWebRequest.Result.DataProcessingError:
                            break;

                        default:
                            break;
                    }

                    Debug.LogWarning($"TimeOut!! -> {uwe.Error}");
                    throw new OperationCanceledException();
                }
                throw;
            }
        }
    }
}

