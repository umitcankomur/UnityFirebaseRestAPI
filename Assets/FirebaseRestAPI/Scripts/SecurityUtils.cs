using System.Text;
using UnityEngine;
using System.Security.Cryptography;
using System.IO;
using System;
using System.Collections.Generic;

namespace FirebaseRestAPI
{
    public class SecurityUtils
    {
        public static string GetPackageName()
        {
            return Application.identifier;
        }
        public static string GetSHA256Fingerprint()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    using (AndroidJavaClass sha1Helper = new AndroidJavaClass("com.unity.securityutils.SecurityUtils"))
                    {
                        string sha1Fingerprint = sha1Helper.CallStatic<string>("getSHA256Fingerprint", context);
                        return sha1Fingerprint;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to retrieve SHA256: " + e.Message);
                return null;
            }
#else
            Debug.LogWarning("SHA256 fingerprint is only available on Android");
            return string.Empty;
#endif
        }
        public static string GetSHA1Fingerprint()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    using (AndroidJavaClass sha1Helper = new AndroidJavaClass("com.unity.securityutils.SecurityUtils"))
                    {
                        string sha1Fingerprint = sha1Helper.CallStatic<string>("getSHA1Fingerprint", context);
                        return sha1Fingerprint;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to retrieve SHA1: " + e.Message);
                return null;
            }
#else
            Debug.LogWarning("SHA1 fingerprint is only available on Android");
            return string.Empty;
#endif
        }
    }

}