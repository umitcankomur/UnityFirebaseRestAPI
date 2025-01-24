using UnityEngine;
using System.Security.Cryptography;
using System;

#if UNITY_EDITOR
using UnityEditor;

namespace FirebaseRestAPI.Editor
{
    public class EncryptionKeyGenerator : EditorWindow
    {
        [MenuItem("Assets/FirebaseRestAPI/Encryption Key Generator")]
        public static void Open()
        {
            EncryptionKeyGenerator window = GetWindow<EncryptionKeyGenerator>();
        }

        private string[] keySizes = new string[] { "128", "192", "256" }; 
        
        private string generatedKey;
        private int selectedSize;

        private void OnGUI()
        {
            EditorGUILayout.Space(5);
            selectedSize = EditorGUILayout.Popup("Key Size", selectedSize, keySizes);

            EditorGUILayout.Space(5);
            if (GUILayout.Button("Generate"))
            {
                GenerateKey();
            }

            EditorGUILayout.Space(5);
            EditorGUILayout.TextArea(generatedKey, GUILayout.Height(120));
        }

        public void GenerateKey()
        {
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                int size = int.Parse(keySizes[selectedSize]);
                byte[] key = new byte[size / 8];
                randomNumberGenerator.GetBytes(key);
                generatedKey = Convert.ToBase64String(key);
            }
        }
    }
}
#endif
