using Newtonsoft.Json;
using System;

namespace FirebaseRestAPI
{
    interface IFirebaseUser
    {
        void SetTokens(string idToken, string refreshToken, DateTime expiresIn);
        void SetUserData(UserData userData);
        void SetEmail(string email);
    }

    public class FirebaseUser : IFirebaseUser
    {
        [JsonProperty]
        public string UserId { get; private set; }
        [JsonProperty]
        public string Email { get; private set; }
        [JsonProperty]
        public string RefreshToken { get; private set; }
        [JsonProperty]
        public string IdToken { get; private set; }
        [JsonProperty]
        public DateTime TokenExpiration { get; private set; }
        /// <summary>
        /// Call FirebaseAuth.GetUserData() to get the user data.
        /// </summary>
        [JsonProperty]
        public UserData UserData { get; private set; }

        public FirebaseUser(string localId, string idToken, string email, string refreshToken, DateTime expiresIn)
        {
            UserId = localId;
            IdToken = idToken;
            Email = email;
            RefreshToken = refreshToken;
            TokenExpiration = expiresIn;
            UserData = new UserData();
        }

        public bool IsTokenExpired()
        {
            return DateTime.Now > TokenExpiration;
        }

        void IFirebaseUser.SetTokens(string idToken, string refreshToken, DateTime expiresIn)
        {
            IdToken = idToken;
            RefreshToken = refreshToken;
            TokenExpiration = expiresIn;
        }

        void IFirebaseUser.SetUserData(UserData userData)
        {
            UserData = userData;
        }

        void IFirebaseUser.SetEmail(string email)
        {
            Email = email;
        }
    }


    interface IUserData
    {
        void SetDisplayName(string displayName);
        void SetPhotoUrl(string photoUrl);
    }

    public class UserData : IUserData
    {
        [JsonProperty("localId")]
        public string LocalId { get; private set; }

        [JsonProperty("email")]
        public string Email { get; private set; }

        [JsonProperty("emailVerified")]
        public bool EmailVerified { get; private set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; private set; }

        [JsonProperty("providerUserInfo")]
        public ProviderInfo[] ProviderUserInfo { get; private set; }

        [JsonProperty("photoUrl")]
        public string PhotoUrl { get; private set; }

        [JsonProperty("passwordHash")]
        public string PasswordHash { get; private set; }

        [JsonProperty("passwordUpdatedAt")]
        public long PasswordUpdatedAt { get; private set; }

        [JsonProperty("validSince")]
        public string ValidSince { get; private set; }

        [JsonProperty("disabled")]
        public bool Disabled { get; private set; }

        [JsonProperty("lastLoginAt")]
        public string LastLoginAt { get; private set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; private set; }

        [JsonProperty("customAuth")]
        public bool CustomAuth { get; private set; }

        void IUserData.SetDisplayName(string displayName)
        {
            DisplayName = displayName;
        }

        void IUserData.SetPhotoUrl(string photoUrl)
        {
            PhotoUrl = photoUrl;
        }
    }

    public class ProviderInfo
    {
        [JsonProperty("providerId")]
        public string ProviderId { get; private set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; private set; }

        [JsonProperty("photoUrl")]
        public string PhotoUrl { get; private set; }

        [JsonProperty("federatedId")]
        public string FederatedId { get; private set; }

        [JsonProperty("email")]
        public string Email { get; private set; }

        [JsonProperty("rawId")]
        public string RawId { get; private set; }

        [JsonProperty("screenName")]
        public string ScreenName { get; private set; }
    }

}

