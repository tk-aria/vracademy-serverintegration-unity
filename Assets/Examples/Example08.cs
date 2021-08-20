// cancellation process.
using System;
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

        private async UniTask Request(string url)
        {
            try
            {
                using (var req = UnityWebRequest.Get(url))
                {
                    req.timeout = this.timeout;
                    //CancelAfterSlim(TimeSpan.FromSeconds(this.timeout));

                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    await req.SendWebRequest().ToUniTask(Progress.Create<float>(x => Debug.Log($"progress: {x*100}, Elapsed: {sw.Elapsed}")));

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

