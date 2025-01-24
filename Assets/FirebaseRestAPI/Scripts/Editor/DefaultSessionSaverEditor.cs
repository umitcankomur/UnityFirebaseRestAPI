using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace FirebaseRestAPI.Editor
{
    [CustomEditor(typeof(DefaultSessionSaver), true)]
    public class DefaultSessionSaverEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("The encryption key must be a 128-bit key. Use the Encryption Key Generator to generate a key.", MessageType.Info);

            base.OnInspectorGUI();

            if (GUILayout.Button("Delete User Session"))
            {
                FirebaseConfig.SessionSaver.DeleteSession();
            }
            if (GUILayout.Button("Encryption Key Generator"))
            {
                EncryptionKeyGenerator.Open();
            }
        }
    }
}
#endif
