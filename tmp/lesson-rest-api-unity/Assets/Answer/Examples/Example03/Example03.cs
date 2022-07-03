// post example.
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace VRAcademy.HttpBasic
{

    internal sealed class Example03 : MonoBehaviour
    {
        [SerializeField] InputField nameField;
        [SerializeField] InputField mailField;

        public void OnEnterInputForm()
            => SendRequestAsync().Forget();

        public async UniTaskVoid SendRequestAsync()
        {
            var user = await Request(Utility.HostName + "/user/test", new User { name = nameField.text, email = mailField.text });
            Debug.Log($"response => name:{user.name}, email:{user.email}");
        }

        private async UniTask<User> Request(string url, User user)
        {
            var json = JsonUtility.ToJson(user);
            var payload = Encoding.UTF8.GetBytes(json);

            using (var req = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST, new DownloadHandlerBuffer(), new UploadHandlerRaw(payload)))
            {
                { // リクエスト送信前の準備(ここではヘッダーに’ContentーType’を追加している).
                    req.SetRequestHeader("Content-Type", "application/json");
                }
                await req.SendWebRequest();
                Debug.Log($"{req.url} : {req.downloadHandler.text}");
                return JsonUtility.FromJson<User>(req.downloadHandler.text);
            }
        }
    }
}
