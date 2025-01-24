using UnityEngine;

namespace FirebaseRestAPI
{
    public abstract class SessionSaver : ScriptableObject
    {
        public abstract void SaveSession(FirebaseUser user);
        public abstract FirebaseUser LoadSession();
        public abstract void DeleteSession();
    }
}
