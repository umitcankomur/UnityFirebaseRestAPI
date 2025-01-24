
using Newtonsoft.Json;

namespace FirebaseRestAPI
{
    public class AuthError
    {
        public static readonly string EMAIL_EXISTS = "EMAIL_EXISTS";
        public static readonly string EMAIL_NOT_FOUND = "EMAIL_NOT_FOUND";
        public static readonly string INVALID_PASSWORD = "INVALID_PASSWORD";
        public static readonly string USER_DISABLED = "USER_DISABLED";
        public static readonly string INVALID_LOGIN_CREDENTIALS = "INVALID_LOGIN_CREDENTIALS";
        public static readonly string INVALID_EMAIL = "INVALID_EMAIL";
        public static readonly string INVALID_REFRESH_TOKEN = "INVALID_REFRESH_TOKEN";
        public static readonly string OPERATION_NOT_ALLOWED = "OPERATION_NOT_ALLOWED";
        public static readonly string INVALID_ID_TOKEN = "INVALID_ID_TOKEN";
        public static readonly string WEAK_PASSWORD = "WEAK_PASSWORD";
        public static readonly string TOO_MANY_ATTEMPTS_TRY_LATER = "TOO_MANY_ATTEMPTS_TRY_LATER";
        public static readonly string USER_NOT_FOUND = "USER_NOT_FOUND";
        public static readonly string INVALID_IDP_RESPONSE = "INVALID_IDP_RESPONSE";

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
