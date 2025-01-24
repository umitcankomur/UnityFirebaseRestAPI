using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace FirebaseRestAPI
{
    public class FirebaseRequestHandler : MonoBehaviour
    {
        private static FirebaseRequestHandler instance;
        public static FirebaseRequestHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("FirebaseRequestHandler").AddComponent<FirebaseRequestHandler>();
                    DontDestroyOnLoad(instance.gameObject);
                }
                return instance;
            }
        }

        private IEnumerator SendRequest(UnityWebRequest request, Action<string> onSuccess, Action<string> onFailure)
        {
            // Add package name and SHA1 fingerprint to headers
#if UNITY_ANDROID && !UNITY_EDITOR
            request.SetRequestHeader("X-Android-Package", SecurityUtils.GetPackageName());
            request.SetRequestHeader("X-Android-Cert", SecurityUtils.GetSHA1Fingerprint().Replace(":",""));
#elif UNITY_IOS && !UNITY_EDITOR
            request.SetRequestHeader("X-Ios-Bundle-Identifier", SecurityUtils.GetPackageName());
#endif

            var operation = request.SendWebRequest();

            // Wait for request to complete
            while (!operation.isDone)
                yield return null;

            if (request.result != UnityWebRequest.Result.Success)
            {
                // Log error
                Debug.Log($"Request failed: {request.error}");

                if (request.downloadHandler != null)
                    onFailure?.Invoke(request.downloadHandler.text);
                else onFailure?.Invoke("");
            }
            else
            {
                if (request.downloadHandler != null)
                    onSuccess?.Invoke(request.downloadHandler.text);
                else onSuccess?.Invoke("");
            }
        }

        public void Get(string url, Action<string> onSuccess, Action<string> onFailure, Dictionary<string, string> headers = null)
        {
            StartCoroutine(Get_Coroutine(url, onSuccess, onFailure, headers));
        }

        private IEnumerator Get_Coroutine(string url, Action<string> onSuccess, Action<string> onFailure, Dictionary<string, string> headers = null)
        {
            using (var request = UnityWebRequest.Get(url))
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.SetRequestHeader(header.Key, header.Value);
                    }
                }

                yield return SendRequest(request, onSuccess, onFailure);
            }
        }

        public void Post(string url, object data, Action<string> onSuccess, Action<string> onFailure, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            StartCoroutine(Post_Coroutine(url, data, onSuccess, onFailure, contentType, headers));
        }

        private IEnumerator Post_Coroutine(string url, object data, Action<string> onSuccess, Action<string> onFailure, string contentType, Dictionary<string, string> headers)
        {
            var json = ObjectToJson(data);
            using (var request = UnityWebRequest.PostWwwForm(url, json))
            {
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", contentType);

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.SetRequestHeader(header.Key, header.Value);
                    }
                }

                yield return SendRequest(request, onSuccess, onFailure);
            }
        }

        public void Put(string url, object data, Action<string> onSuccess, Action<string> onFailure, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            StartCoroutine(Put_Coroutine(url, data, onSuccess, onFailure, contentType, headers));
        }

        private IEnumerator Put_Coroutine(string url, object data, Action<string> onSuccess, Action<string> onFailure, string contentType, Dictionary<string, string> headers)
        {
            var json = ObjectToJson(data);
            using (var request = UnityWebRequest.Put(url, json))
            {
                request.SetRequestHeader("Content-Type", contentType);

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.SetRequestHeader(header.Key, header.Value);
                    }
                }

                yield return SendRequest(request, onSuccess, onFailure);
            }
        }

        public void Patch(string url, object data, Action<string> onSuccess, Action<string> onFailure, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            StartCoroutine(Patch_Coroutine(url, data, onSuccess, onFailure, contentType, headers));
        }

        private IEnumerator Patch_Coroutine(string url, object data, Action<string> onSuccess, Action<string> onFailure, string contentType, Dictionary<string, string> headers)
        {
            var json = ObjectToJson(data);
            using (var request = new UnityWebRequest(url, "PATCH"))
            {
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", contentType);

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.SetRequestHeader(header.Key, header.Value);
                    }
                }

                yield return SendRequest(request, onSuccess, onFailure);
            }
        }

        public void Delete(string url, Action<string> onSuccess, Action<string> onFailure, Dictionary<string, string> headers = null)
        {
            StartCoroutine(Delete_Coroutine(url, onSuccess, onFailure, headers));
        }

        private IEnumerator Delete_Coroutine(string url, Action<string> onSuccess, Action<string> onFailure, Dictionary<string, string> headers = null)
        {
            using (var request = UnityWebRequest.Delete(url))
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.SetRequestHeader(header.Key, header.Value);
                    }
                }

                yield return SendRequest(request, onSuccess, onFailure);
            }
        }

        private string ObjectToJson(object data)
        {
            if (data.GetType() == typeof(string) && IsJsonString(data.ToString()))
            {
                return data.ToString();
            }
            else
            {
                return JsonConvert.SerializeObject(data);
            }
        }

        private bool IsJsonString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.Trim();
            if ((input.StartsWith("{") && input.EndsWith("}")) || (input.StartsWith("[") && input.EndsWith("]")))
            {
                try
                {
                    var obj = JToken.Parse(input);
                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
