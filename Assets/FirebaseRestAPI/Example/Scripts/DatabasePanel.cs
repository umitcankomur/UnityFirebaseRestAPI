using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using FirebaseRestAPI;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Converters;

namespace FirebaseRestAPI.Example
{
    public class DatabasePanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField pathInput;
        [SerializeField] private TMP_InputField valueInput;
        [SerializeField] private TMP_InputField orderByInput;
        [SerializeField] private TMP_InputField startAtInput;
        [SerializeField] private TMP_InputField endAtInput;
        [SerializeField] private TMP_InputField equalToInput;
        [SerializeField] private TMP_InputField limitToFirstInput;
        [SerializeField] private TMP_InputField limitToLastInput;
        [SerializeField] private Button setButton;
        [SerializeField] private Button getButton;
        [SerializeField] private Button pushButton;
        [SerializeField] private Button deleteButton;

        void Awake()
        {
            setButton.onClick.AddListener(Set);
            getButton.onClick.AddListener(Get);
            pushButton.onClick.AddListener(Push);
            deleteButton.onClick.AddListener(Delete);
        }

        private void Update()
        {
            bool showQueryInputs = !string.IsNullOrEmpty(orderByInput.text);
            startAtInput.gameObject.SetActive(showQueryInputs);
            endAtInput.gameObject.SetActive(showQueryInputs);
            equalToInput.gameObject.SetActive(showQueryInputs);
            limitToFirstInput.gameObject.SetActive(showQueryInputs);
            limitToLastInput.gameObject.SetActive(showQueryInputs);
        }

        public void Set()
        {
            new DatabaseReference(pathInput.text)
                .Set(valueInput.text)
                .OnSuccess((response) =>
                {
                    Toast.Show("Data set successfully");
                    Debug.Log(response);
                })
                .OnError((error) =>
                {
                    Toast.Show($"Data set failed : {error.Error}");
                });
        }
        public void Get()
        {
            DatabaseReference dbRef = new DatabaseReference(pathInput.text);
            dbRef = AddQueryFilters(dbRef);

            dbRef.Get()
                .OnSuccess((data) =>
                {
                    Toast.Show("Data fetched successfully");
                    Debug.Log(JsonConvert.SerializeObject(data));
                })
                .OnError((error) =>
                {
                    Toast.Show($"Data fetch failed : {error.Error}");
                });
        }
        public void Push()
        {
            new DatabaseReference(pathInput.text)
                .Push(valueInput.text)
                .OnSuccess((childName) =>
                {
                    Toast.Show($"Data pushed successfully : {childName}");
                })
                .OnError((error) =>
                {
                    Toast.Show($"Data push failed : {error.Error}");
                });
        }
        public void Delete()
        {
            DatabaseReference dbRef = new DatabaseReference(pathInput.text);
            dbRef = AddQueryFilters(dbRef);

            dbRef.Delete()
                .OnSuccess((response) =>
                {
                    Toast.Show("Data deleted successfully");
                    Debug.Log(response);
                })
                .OnError((error) =>
                {
                    Toast.Show($"Data delete failed : {error.Error}");
                });
        }

        private DatabaseReference AddQueryFilters(DatabaseReference dbRef)
        {
            if (!string.IsNullOrEmpty(orderByInput.text))
            {
                dbRef = dbRef.OrderByChild(orderByInput.text);

                // StartAt
                if (!string.IsNullOrEmpty(startAtInput.text))
                {
                    dbRef = dbRef.StartAt(ParseInput(startAtInput.text));
                }

                // EndAt
                if (!string.IsNullOrEmpty(endAtInput.text))
                {
                    dbRef = dbRef.EndAt(ParseInput(endAtInput.text));
                }

                // EqualTo
                if (!string.IsNullOrEmpty(equalToInput.text))
                {
                    dbRef = dbRef.EqualTo(ParseInput(equalToInput.text));
                }

                // LimitToFirst
                if (!string.IsNullOrEmpty(limitToFirstInput.text))
                {
                    dbRef = dbRef.LimitToFirst(int.Parse(limitToFirstInput.text));
                }

                // LimitToLast
                if (!string.IsNullOrEmpty(limitToLastInput.text))
                {
                    dbRef = dbRef.LimitToLast(int.Parse(limitToLastInput.text));
                }
            }
            return dbRef;
        }
        private object ParseInput(string input)
        {
            if (int.TryParse(input, out int intValue))
            {
                return intValue;
            }
            if (float.TryParse(input, out float floatValue))
            {
                return floatValue;
            }
            else if (bool.TryParse(input, out bool boolValue))
            {
                return boolValue;
            }
            else
            {
                return input;
            }
        }
    }
}
