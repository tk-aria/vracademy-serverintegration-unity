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

                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    await req.SendWebRequest().ToUniTask(Progress.Create<float>(x => Debug.Log($"progress: {x*100}, Elapsed: {sw.Elapsed}")));
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

                    // more than Unity2020 => https://docs.unity3d.com/2020.2/Documentation/ScriptReference/Networking.UnityWebRequest.Result.html
                    /*switch ( request.result )
                    {
                        case UnityWebRequest.Result.InProgress:
                            Debug.Log( "リクエスト中" );
                            break;

                        case UnityWebRequest.Result.Success:
                            Debug.Log( "リクエスト成功" );
                            break;

                        case UnityWebRequest.Result.ConnectionError:// isNetworkError
                            Debug.Log
                            (
                                @"サーバとの通信に失敗。
            リクエストが接続できなかった、
            セキュリティで保護されたチャネルを確立できなかったなど。"
                            );
                            break;

                        case UnityWebRequest.Result.ProtocolError:// isHttpError
                            Debug.Log
                            (
                                @"サーバがエラー応答を返した。
            サーバとの通信には成功したが、
            接続プロトコルで定義されているエラーを受け取った。"
                            );
                            break;

                        case UnityWebRequest.Result.DataProcessingError:
                            Debug.Log
                            (
                                @"データの処理中にエラーが発生。
            リクエストはサーバとの通信に成功したが、
            受信したデータの処理中にエラーが発生。
            データが破損しているか、正しい形式ではないなど。"
                            );
                            break;

                        default: throw new ArgumentOutOfRangeException();
                    }*/

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
                throw new OperationCanceledException();
            }
        }
    }
}

