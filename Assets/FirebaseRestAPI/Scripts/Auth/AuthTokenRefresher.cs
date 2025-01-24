using System;
using UnityEngine;

namespace FirebaseRestAPI
{
    public class AuthTokenRefresher : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            GameObject go = new GameObject("AuthTokenRefresher");
            go.AddComponent<AuthTokenRefresher>();
            DontDestroyOnLoad(go);
        }

        private void Start()
        {
            // Check for token refresh every 60 seconds
            InvokeRepeating(nameof(RefreshToken), 0, 60);
        }

        private void RefreshToken()
        {
            // Check if user is authenticated
            // Refresh token if it is expired or about to expire in 5 minutes
            if (FirebaseAuth.IsAuthenticated 
                && !FirebaseAuth.IsTokenRefreshing
                && (FirebaseAuth.CurrentUser.IsTokenExpired() || DateTime.Now.AddMinutes(5) > FirebaseAuth.CurrentUser.TokenExpiration))
            {
                FirebaseAuth.RefreshIdToken(null, null);
            }
        }
    }
}
