using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace FirebaseRestAPI
{
    public static class FirebaseAuth
    {
        public static string AUTH_URL = "https://identitytoolkit.googleapis.com/v1/";
        public static string SECURE_TOKEN_URL = "https://securetoken.googleapis.com/v1/";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            // Load user session
            if (FirebaseConfig.SaveSession)
            {
                var user = FirebaseConfig.SessionSaver.LoadSession();
                if (user != null)
                {
                    Debug.Log("User session loaded");
                    CurrentUser = user;
                }
            }
        }

        public static bool IsAuthenticated => CurrentUser != null;
        public static bool IsTokenRefreshing { get; private set; }
        public static FirebaseUser CurrentUser { get; private set; }

        public static void SignInWithOAuthCredential(string oauthIdToken, AuthProvider provider, Action<FirebaseUser> onSuccess, Action<AuthError> onError)
        {
            var requestData = new Dictionary<string, object>
            {
                { "requestUri", "http://localhost" },
                { "postBody", $"id_token={oauthIdToken}&providerId={provider.GetProviderId()}" },
                { "returnIdpCredential", true },
                { "returnSecureToken", true }
            };

            var url = $"{AUTH_URL}accounts:signInWithIdp?key={FirebaseConfig.WebApiKey}";
            FirebaseRequestHandler.Instance.Post(url, requestData, (response) => HandleLoginResponse(response, onSuccess), (e) => HandleAuthError(e, onError));
        }

        public static void SignUpWithEmailAndPassword(string email, string password, Action<FirebaseUser> onSuccess, Action<AuthError> onError)
        {
            var requestData = new Dictionary<string, object>
            {
                { "email", email },
                { "password", password },
                { "returnSecureToken", true }
            };

            var url = $"{AUTH_URL}accounts:signUp?key={FirebaseConfig.WebApiKey}";
            FirebaseRequestHandler.Instance.Post(url, requestData, (response) => HandleLoginResponse(response, onSuccess), (e) => HandleAuthError(e, onError));
        }

        public static void SignInWithEmailAndPassword(string email, string password, Action<FirebaseUser> onSuccess, Action<AuthError> onError)
        {
            var requestData = new Dictionary<string, object>
            {
                { "email", email },
                { "password", password },
                { "returnSecureToken", true }
            };

            var url = $"{AUTH_URL}accounts:signInWithPassword?key={FirebaseConfig.WebApiKey}";
            FirebaseRequestHandler.Instance.Post(url, requestData, (response) => HandleLoginResponse(response, onSuccess), (e) => HandleAuthError(e, onError));
        }

        public static void SendPasswordResetEmail(string email, Action onSuccess, Action<AuthError> onError)
        {
            var requestData = new Dictionary<string, object>
            {
                { "email", email },
                { "requestType", "PASSWORD_RESET" }
            };

            // Add language code to headers
            var headers = new Dictionary<string, string>
            {
                { "X-Firebase-Locale", GetSystemLanguageCode() }
            };

            var url = $"{AUTH_URL}accounts:sendOobCode?key={FirebaseConfig.WebApiKey}";
            FirebaseRequestHandler.Instance.Post(url, requestData, (response) => onSuccess?.Invoke(), (e) => HandleAuthError(e, onError));
        }

        public static void ChangePassword(string newPassword, Action onSuccess, Action<AuthError> onError)
        {
            if (CurrentUser == null)
            {
                Debug.LogError("User is not authenticated");
                return;
            }

            var requestData = new Dictionary<string, object>
            {
                { "idToken", CurrentUser.IdToken },
                { "password", newPassword },
                { "returnSecureToken", true }
            };

            var url = $"{AUTH_URL}accounts:update?key={FirebaseConfig.WebApiKey}";
            FirebaseRequestHandler.Instance.Post(url, requestData, (response) => onSuccess?.Invoke(), (e) => HandleAuthError(e, onError));
        }

        public static void GetUserData(Action<UserData> onSuccess, Action<AuthError> onError)
        {
            if (CurrentUser == null)
            {
                Debug.LogError("User is not authenticated");
                return;
            }

            var requestData = new Dictionary<string, object>
            {
                { "idToken", CurrentUser.IdToken }
            };

            var url = $"{AUTH_URL}accounts:lookup?key={FirebaseConfig.WebApiKey}";
            FirebaseRequestHandler.Instance.Post(url, requestData, (response) =>
            {
                var authData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
                var users = JsonConvert.DeserializeObject<UserData[]>(authData["users"].ToString());
                var userData = users[0];
                ((IFirebaseUser)CurrentUser).SetUserData(userData);
                
                if (FirebaseConfig.SaveSession)
                {
                    FirebaseConfig.SessionSaver.SaveSession(CurrentUser);
                }

                onSuccess?.Invoke(userData);
            }, (e) => HandleAuthError(e, onError));
        }

        public static void UpdateProfile(string displayName, string photoUrl, Action onSuccess, Action<AuthError> onError)
        {
            if (CurrentUser == null)
            {
                Debug.LogError("User is not authenticated");
                return;
            }

            var requestData = new Dictionary<string, object>
            {
                { "idToken", CurrentUser.IdToken },
                { "returnSecureToken", false }
            };

            if (!string.IsNullOrEmpty(displayName))
            {
                requestData.Add("displayName", displayName);
            }

            if (!string.IsNullOrEmpty(photoUrl))
            {
                requestData.Add("photoUrl", photoUrl);
            }

            var url = $"{AUTH_URL}accounts:update?key={FirebaseConfig.WebApiKey}";
            FirebaseRequestHandler.Instance.Post(url, requestData, (response) =>
            {
                var authData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

                if (CurrentUser.UserData == null)
                {
                    ((IFirebaseUser)CurrentUser).SetUserData(new UserData());
                }

                if (authData.ContainsKey("displayName"))
                {
                    var displayName = authData["displayName"].ToString();
                    ((IUserData)CurrentUser.UserData).SetDisplayName(displayName);
                }

                if (authData.ContainsKey("photoUrl"))
                {
                    var photoUrl = authData["photoUrl"].ToString();
                    ((IUserData)CurrentUser.UserData).SetPhotoUrl(photoUrl);
                }

                if (FirebaseConfig.SaveSession)
                {
                    FirebaseConfig.SessionSaver.SaveSession(CurrentUser);
                }

                onSuccess?.Invoke();

            }, (e) => HandleAuthError(e, onError));
        }

        public static void RefreshIdToken(Action onSuccess, Action<AuthError> onError)
        {
            if (CurrentUser == null)
            {
                Debug.LogError("User is not authenticated");
                return;
            }

            var requestData = new Dictionary<string, object>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", CurrentUser.RefreshToken }
            };

            var url = $"{SECURE_TOKEN_URL}token?key={FirebaseConfig.WebApiKey}";

            IsTokenRefreshing = true;
            FirebaseRequestHandler.Instance.Post(url, requestData, (response) => 
            {
                IsTokenRefreshing = false;
                HandleRefreshTokenResponse(response, onSuccess);
            }, (e) =>
            {
                IsTokenRefreshing = false;
                HandleAuthError(e, onError);
            });
        }

        public static void SignOut()
        {
            CurrentUser = null;

            // Delete user session
            if (FirebaseConfig.SaveSession)
            {
                FirebaseConfig.SessionSaver.DeleteSession();
            }
        }

        public static Dictionary<string, string> GetAuthHeader()
        {
            if (IsAuthenticated)
            {
                var headers = new Dictionary<string, string>
                {
                    { "Authorization", $"Bearer {FirebaseAuth.CurrentUser.IdToken}" }
                };
                return headers;
            }

            return null;
        }

        private static string GetSystemLanguageCode()
        {
            try
            {
                CultureInfo culture = CultureInfo.CurrentCulture;
                return culture.TwoLetterISOLanguageName;
            }
            catch
            {
                return "en";
            }
        }

        private static void HandleLoginResponse(string response, Action<FirebaseUser> onSuccess)
        {
            var authData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

            var idToken = authData["idToken"].ToString();
            var localId = authData["localId"].ToString();
            var refreshToken = authData["refreshToken"].ToString();
            var email = authData["email"].ToString();
            var expiresIn = DateTime.Now.AddSeconds(Convert.ToDouble(authData["expiresIn"]));

            FirebaseUser user = new FirebaseUser(localId, idToken, email, refreshToken, expiresIn);
            CurrentUser = user;

            // Save user session
            if (FirebaseConfig.SaveSession)
            {
                FirebaseConfig.SessionSaver.SaveSession(CurrentUser);
            }

            onSuccess?.Invoke(user);
        }

        private static void HandleRefreshTokenResponse(string response, Action onSuccess)
        {
            var authData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

            var idToken = authData["id_token"].ToString();
            var refreshToken = authData["refresh_token"].ToString();
            var expiresIn = DateTime.Now.AddSeconds(Convert.ToDouble(authData["expires_in"]));

            Debug.Log("Token refreshed");
            ((IFirebaseUser)CurrentUser).SetTokens(idToken, refreshToken, expiresIn);

            // Save user session
            if (FirebaseConfig.SaveSession)
            {
                FirebaseConfig.SessionSaver.SaveSession(CurrentUser);
            }

            onSuccess?.Invoke();
        }

        private static void HandleAuthError(string error, Action<AuthError> onError)
        {
            if (string.IsNullOrEmpty(error))
            {
                onError?.Invoke(new AuthError { Message = "Unknown error" });
                return;
            }

            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(error);

            if (dictionary == null || !dictionary.ContainsKey("error"))
            {
                onError?.Invoke(new AuthError { Message = "Unknown error" });
                return;
            }

            var authError = JsonConvert.DeserializeObject<AuthError>(dictionary["error"].ToString());
            onError?.Invoke(authError);
        }
    }
}