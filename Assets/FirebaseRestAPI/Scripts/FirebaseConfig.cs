using UnityEngine;

namespace FirebaseRestAPI
{
    [CreateAssetMenu(fileName = "FirebaseConfig", menuName = "FirebaseRestAPI/FirebaseConfig")]
    public class FirebaseConfig : ScriptableObject
    {
        public static string WebApiKey => Config.webApiKey;
        public static string ProjectId => Config.projectId;
        public static bool SaveSession => Config.saveSession;
        public static SessionSaver SessionSaver => Config.sessionSaver;

        private static FirebaseConfig config;
        private static FirebaseConfig Config
        {
            get
            {
                if (config == null)
                {
                    config = Resources.Load<FirebaseConfig>("FirebaseConfig");

                    if (config == null)
                    {
                        Debug.LogError("FirebaseConfig not found in Resources folder. Please create a FirebaseConfig asset.");
                    }
                }
                return config;
            }
        }

        [SerializeField] private string webApiKey;
        [SerializeField] private string projectId;
        [SerializeField] private bool saveSession;
        [SerializeField] private SessionSaver sessionSaver;
    }
}
