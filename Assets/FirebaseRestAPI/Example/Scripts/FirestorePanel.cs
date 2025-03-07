using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FirebaseRestAPI.Example
{
    public class FirestorePanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField collectionNameInput;
        [SerializeField] private TMP_InputField documentNameInput;
        [SerializeField] private TMP_InputField queryFieldInput;
        [SerializeField] private TMP_InputField queryValueInput;
        [SerializeField] private TMP_InputField queryFieldLimitInput;
        [SerializeField] private TMP_Dropdown operatorDropdown;
        [SerializeField] private TMP_InputField queryOrderByInput;
        [SerializeField] private TMP_InputField queryStartAtInput;
        [SerializeField] private TMP_InputField queryEndAtInput;
        [SerializeField] private TMP_InputField queryOrderLimitInput;
        [SerializeField] private TMP_Dropdown queryOrderDirectionDropdown;
        [SerializeField] private Button getButton;
        [SerializeField] private Button setButton;
        [SerializeField] private Button pushButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private Button queryButtonField;
        [SerializeField] private Button queryButtonOrderBy;

        private void Start()
        {
            setButton.onClick.AddListener(Set);
            deleteButton.onClick.AddListener(Delete);
            pushButton.onClick.AddListener(Push);
            getButton.onClick.AddListener(Get);
            queryButtonField.onClick.AddListener(Query_Field);
            queryButtonOrderBy.onClick.AddListener(Query_Order);
        }

        private void Set()
        {
            var collection = collectionNameInput.text;
            var document = documentNameInput.text;

            if (string.IsNullOrEmpty(collection) || string.IsNullOrEmpty(document))
            {
                Toast.Show("Collection or document is empty");
                return;
            }

            new FirestoreReference()
                .Collection(collection)
                .Document(document)
                .Set(GetUserDocument())
                .OnSuccess((response) =>
                {
                    Toast.Show("Document set successfully");
                })
                .OnError((error) =>
                {
                    Toast.Show("Error: " + error.Message);
                });
        }

        private void Push()
        {
            string collection = collectionNameInput.text;
            if (string.IsNullOrEmpty(collection))
            {
                Toast.Show("Collection is empty");
                return;
            }

            new FirestoreReference()
                .Collection(collection)
                .Push(GetUserDocument())
                .OnSuccess((documentId) =>
                {
                    Toast.Show($"Document pushed successfully : {documentId}");
                })
                .OnError((error) =>
                {
                    Toast.Show("Error: " + error.Message);
                });

        }

        private void Delete()
        {
            var collection = collectionNameInput.text;
            var document = documentNameInput.text;

            if (string.IsNullOrEmpty(collection) || string.IsNullOrEmpty(document))
            {
                Toast.Show("Collection or document is empty");
                return;
            }

            new FirestoreReference()
                .Collection(collection)
                .Document(document)
                .Delete()
                .OnSuccess((response) =>
                {
                    Toast.Show("Document deleted successfully");
                })
                .OnError((error) =>
                {
                    Toast.Show("Error: " + error.Message);
                });
        }

        private void Get()
        {
            var collection = collectionNameInput.text;
            var document = documentNameInput.text;

            if (string.IsNullOrEmpty(collection) || string.IsNullOrEmpty(document))
            {
                Toast.Show("Collection or document is empty");
                return;
            }

            new FirestoreReference()
                .Collection(collection)
                .Document(document)
                .Get()
                .OnSuccess((response) =>
                {
                    Toast.Show("Document fetched successfully");
                    Debug.Log(JsonConvert.SerializeObject(response));
                })
                .OnError((error) =>
                {
                    Toast.Show("Error: " + error.Message);
                });
        }

        private void Query_Field()
        {
            var collectionInput = collectionNameInput.text;
            var fieldInput = queryFieldInput.text;
            var valueInput = queryValueInput.text;
            var operatorIndex = operatorDropdown.value;
            var limitInput = queryFieldLimitInput.text;

            if (string.IsNullOrEmpty(collectionInput) || string.IsNullOrEmpty(fieldInput) || string.IsNullOrEmpty(valueInput))
            {
                Toast.Show("Collection, field or value is empty");
                return;
            }

            var query = new FirestoreReference()
                .Collection(collectionInput)
                .Where(fieldInput, (FieldOperator)operatorIndex, ParseInput(valueInput));

            if (!string.IsNullOrEmpty(limitInput))
            {
                query = query.Limit(int.Parse(limitInput));
            }

            query.RunQuery()
                .OnSuccess((documents) =>
                {
                    Toast.Show("Query fetched successfully");
                    Debug.Log(JsonConvert.SerializeObject(documents));
                })
                .OnError((error) =>
                {
                    Toast.Show("Error: " + error.Message);
                    Debug.Log(JsonConvert.SerializeObject(error));
                });
        }

        private void Query_Order()
        {
            var collectionInput = collectionNameInput.text;
            var limitInput = queryOrderLimitInput.text;
            var orderByInput = queryOrderByInput.text;
            var startAtInput = queryStartAtInput.text;
            var endAtInput = queryEndAtInput.text;
            var orderDirectionIndex = queryOrderDirectionDropdown.value;

            if (string.IsNullOrEmpty(collectionInput) || string.IsNullOrEmpty(orderByInput))
            {
                Toast.Show("Collection or orderBy is empty");
                return;
            }

            var query = new FirestoreReference()
                .Collection(collectionInput)
                .OrderBy(orderByInput, (OrderDirection)orderDirectionIndex);

            if (!string.IsNullOrEmpty(startAtInput))
            {
                query = query.StartAt(ParseInput(startAtInput));
            }

            if (!string.IsNullOrEmpty(endAtInput))
            {
                query = query.EndAt(ParseInput(endAtInput));
            }

            if (!string.IsNullOrEmpty(limitInput))
            {
                query = query.Limit(int.Parse(limitInput));
            }

            query.RunQuery()
                .OnSuccess((documents) =>
                {
                    Toast.Show("Query fetched successfully");
                    Debug.Log(JsonConvert.SerializeObject(documents));
                })
                .OnError((error) =>
                {
                    Toast.Show("Error: " + error.Message);
                });
        }

        private Dictionary<string, object> GetUserDocument()
        {
            var user = new Dictionary<string, object>();

            user.Add("age", 26);
            user.Add("name", "John");
            user.Add("isStudent", true);
            user.Add("address", new Dictionary<string, object>
            {
                { "city", "New York" },
                { "country", "USA" }
            });
            user.Add("hobbies", new List<object> 
            { 
                "Reading", "Gaming"
            });
            user.Add("createdAt", System.DateTime.Now);

            return user;
        }

        private object ParseInput(string input)
        {
            if (bool.TryParse(input, out bool boolValue))
            {
                return boolValue;
            }
            else if (int.TryParse(input, out int intValue))
            {
                return intValue;
            }
            else if (double.TryParse(input, out double doubleValue))
            {
                return doubleValue;
            }
            else
            {
                return input;
            }
        }
    }

}
