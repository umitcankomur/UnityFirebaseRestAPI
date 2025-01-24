using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;

namespace FirebaseRestAPI
{
    [CreateAssetMenu(fileName = "DefaultSessionSaver", menuName = "FirebaseRestAPI/DefaultSessionSaver")]
    public class DefaultSessionSaver : SessionSaver
    {
        public static string USER_SESSION_NAME = "User";

        [SerializeField] private string encryptionKey;

        public override void SaveSession(FirebaseUser user)
        {
            string userJson = JsonConvert.SerializeObject(user);
#if UNITY_WEBGL
                WritePlayerPrefs(USER_SESSION_NAME, userJson, encryptionKey);
#else
            WriteFile(Application.persistentDataPath + $"/{USER_SESSION_NAME}.txt", userJson, encryptionKey);
#endif
        }

        public override FirebaseUser LoadSession()
        {
            string userJson = null;

#if UNITY_WEBGL
                if (PlayerPrefs.HasKey(USER_SESSION_NAME))
                {
                    userJson = ReadPlayerPrefs(USER_SESSION_NAME, encryptionKey);
                }
#else
            if (File.Exists(Application.persistentDataPath + $"/{USER_SESSION_NAME}.txt"))
            {
                userJson = ReadFile(Application.persistentDataPath + $"/{USER_SESSION_NAME}.txt", encryptionKey);
            }
#endif
            if (!string.IsNullOrEmpty(userJson))
            {
                var user = JsonConvert.DeserializeObject<FirebaseUser>(userJson);
                if (user != null)
                {
                    return user;
                }
            }
            return null;
        }

        public override void DeleteSession()
        {
#if UNITY_WEBGL
            PlayerPrefs.DeleteKey(USER_SESSION_NAME);
            PlayerPrefs.Save();
#else
            File.Delete(Application.persistentDataPath + $"/{USER_SESSION_NAME}.txt");
#endif
        }

#if UNITY_WEBGL
        private string ReadPlayerPrefs(string key, string encryptionKey)
        {
            // Retrieve the base64-encoded string from PlayerPrefs
            if (!PlayerPrefs.HasKey(key))
                throw new KeyNotFoundException($"Key '{key}' not found in PlayerPrefs.");

            string encryptedData = PlayerPrefs.GetString(key);
            byte[] dataBytes = Convert.FromBase64String(encryptedData);
            return Decrypt(dataBytes, encryptionKey);
        }
        private void WritePlayerPrefs(string key, string data, string encryptionKey)
        {
            // Store the base64 string in PlayerPrefs
            var dataBytes = Encoding.UTF8.GetBytes(data);
            PlayerPrefs.SetString(key, Encrypt(dataBytes, encryptionKey));
            PlayerPrefs.Save();
        }
#else
        private void WriteFile(string path, string data, string encryptionKey)
        {
            using FileStream stream = File.Create(path);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using var aes = new AesCryptoServiceProvider();
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Generate a random IV and write it to the beginning of the file
            aes.GenerateIV();
            stream.Write(aes.IV, 0, aes.IV.Length);

            using ICryptoTransform cryptoTransform = aes.CreateEncryptor();
            using CryptoStream cryptoStream = new CryptoStream(stream, cryptoTransform, CryptoStreamMode.Write);

            cryptoStream.Write(dataBytes, 0, dataBytes.Length);
        }
        private string ReadFile(string path, string encryptionKey)
        {
            byte[] fileBytes = File.ReadAllBytes(path);

            return Decrypt(fileBytes, encryptionKey);
        }
#endif
        private string Decrypt(byte[] dataBytes, string encryptionKey)
        {
            try
            {
                using var aes = new AesCryptoServiceProvider();
                aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Extract the IV from the beginning of the data
                byte[] iv = new byte[aes.BlockSize / 8];
                byte[] cipherText = new byte[dataBytes.Length - iv.Length];
                Array.Copy(dataBytes, iv, iv.Length);
                Array.Copy(dataBytes, iv.Length, cipherText, 0, cipherText.Length);
                aes.IV = iv;

                using ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);
                using MemoryStream decryptionStream = new MemoryStream(cipherText);
                using CryptoStream cryptoStream = new CryptoStream(decryptionStream, cryptoTransform, CryptoStreamMode.Read);
                using StreamReader reader = new StreamReader(cryptoStream);

                return reader.ReadToEnd();
            }catch (Exception e)
            {
                Debug.LogWarning(e);
                return null;
            }
        }

        private string Encrypt(byte[] dataBytes, string encryptionKey)
        {
            using var aes = new AesCryptoServiceProvider();
            aes.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Generate a random IV
            aes.GenerateIV();

            using ICryptoTransform cryptoTransform = aes.CreateEncryptor();
            using MemoryStream encryptionStream = new MemoryStream();
            encryptionStream.Write(aes.IV, 0, aes.IV.Length);

            using CryptoStream cryptoStream = new CryptoStream(encryptionStream, cryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(dataBytes, 0, dataBytes.Length);
            cryptoStream.FlushFinalBlock();

            // Convert the encrypted bytes to a base64 string
            string encryptedData = Convert.ToBase64String(encryptionStream.ToArray());

            return encryptedData;
        }


#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/FirebaseRestAPI/DefaultSessionSaver/Delete User Session")]
        public static void DeleteSessionMenuItem()
        {
            FirebaseConfig.SessionSaver.DeleteSession();
        }
#endif
    }
}
