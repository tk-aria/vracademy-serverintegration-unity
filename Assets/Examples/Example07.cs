// error handling

// https://developer.mozilla.org/ja/docs/Web/HTTP/Status
// https://ja.wikipedia.org/wiki/HTTP%E3%82%B9%E3%83%86%E3%83%BC%E3%82%BF%E3%82%B9%E3%82%B3%E3%83%BC%E3%83%89

using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace VRAcademy.Advanced.ServerIntegration
{
    internal sealed class Example07 : MonoBehaviour
    {

        [SerializeField] private int timeout = 3;

        private async UniTask Request(string url)
        {
            try
            {
                using (var req = UnityWebRequest.Get(url))
                {
                    req.timeout = this.timeout;

                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    await req.SendWebRequest().ToUniTask(Progress.Create<float>(x => Debug.Log($"progress: {x*100}, Elapsed: {sw.Elapsed}")));

                    // HttpStatusCodeに関して (https://developer.mozilla.org/ja/docs/Web/HTTP/Status)

                    // Unity2020以前のエラーハンドリング.
                    //if(req.isHttpError)
                    //{
                    //    Debug.Log($"[Error] Response Code : {req.responseCode}");
                    //}
                    //else if (req.isNetworkError)
                    //{
                    //    if (req.error == "Request timeout")
                    //    {
                    //        // タイムアウト時の処理
                    //        Debug.Log($"[Error] TimeOut");
                    //        throw new UnityWebRequestException(req);
                    //    }
                    //    else
                    //    {
                    //        Debug.Log($"[Error] Message : {req.error}");
                    //        throw new OperationCanceledException();
                    //    }
                    //}

                    // Unity2020以降のエラーハンドリング => https://docs.unity3d.com/2020.2/Documentation/ScriptReference/Networking.UnityWebRequest.Result.html
                    //switch ( request.result )
                    //{
                    //    case UnityWebRequest.Result.InProgress:
                    //        break;
                    //
                    //    case UnityWebRequest.Result.Success:
                    //        break;
                    //
                    //    case UnityWebRequest.Result.ConnectionError:// isNetworkError
                    //        break;
                    //
                    //    case UnityWebRequest.Result.ProtocolError:// isHttpError
                    //        break;
                    //
                    //    case UnityWebRequest.Result.DataProcessingError:
                    //        break;
                    //
                    //    default:
                    //        throw new ArgumentOutOfRangeException();
                    //}

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

