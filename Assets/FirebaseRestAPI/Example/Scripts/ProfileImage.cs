using FirebaseRestAPI;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace FirebaseRestAPI.Example
{
    [RequireComponent(typeof(RawImage))]
    public class ProfileImage : MonoBehaviour
    {
        private RawImage rawImage;
        private string url;

        private void Awake()
        {
            rawImage = GetComponent<RawImage>();
        }
        private void Update()
        {
            if (FirebaseAuth.IsAuthenticated && FirebaseAuth.CurrentUser.UserData != null
                && url != FirebaseAuth.CurrentUser.UserData.PhotoUrl)
            {
                url = FirebaseAuth.CurrentUser.UserData.PhotoUrl;
                StartCoroutine(DownloadImage(url));
            }
        }

        private IEnumerator DownloadImage(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                yield break;
            }

            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogWarning($"Failed to download image: {request.error}");
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);
                    rawImage.texture = texture;
                }
            }
        }
    }
}
