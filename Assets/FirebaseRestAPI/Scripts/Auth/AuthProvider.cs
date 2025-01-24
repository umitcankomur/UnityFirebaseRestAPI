using UnityEngine;

namespace FirebaseRestAPI
{
    public abstract class AuthProvider
    {
        public abstract string GetProviderId();
    }

    public class EmailAuthProvider : AuthProvider
    {
        public override string GetProviderId()
        {
            return "password";
        }
    }

    public class GoogleAuthProvider : AuthProvider
    {
        public override string GetProviderId()
        {
            return "google.com";
        }
    }

    public class FacebookAuthProvider : AuthProvider
    {
        public override string GetProviderId()
        {
            return "facebook.com";
        }
    }

    public class TwitterAuthProvider : AuthProvider
    {
        public override string GetProviderId()
        {
            return "twitter.com";
        }
    }

    public class GithubAuthProvider : AuthProvider
    {
        public override string GetProviderId()
        {
            return "github.com";
        }
    }

    public class AppleAuthProvider : AuthProvider
    {
        public override string GetProviderId()
        {
            return "apple.com";
        }
    }

    public class MicrosoftAuthProvider : AuthProvider
    {
        public override string GetProviderId()
        {
            return "microsoft.com";
        }
    }

    public class YahooAuthProvider : AuthProvider
    {
        public override string GetProviderId()
        {
            return "yahoo.com";
        }
    }

    public class PhoneAuthProvider : AuthProvider
    {
        public override string GetProviderId()
        {
            return "phone";
        }
    }

    public class PlayGamesAuthProvider : AuthProvider
    {
        public override string GetProviderId()
        {
            return "playgames.google.com";
        }
    }

    public class GameCenterAuthProvider : AuthProvider
    {
        public override string GetProviderId()
        {
            return "gc.apple.com";
        }
    }

    public class OAuthProvider : AuthProvider
    {
        private string providerId;

        public OAuthProvider(string providerId)
        {
            this.providerId = providerId;
        }

        public override string GetProviderId()
        {
            return providerId;
        }
    }
}

