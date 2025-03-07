using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace FirebaseRestAPI
{
    public abstract class FirestoreRequest<T>
    {
        protected Action<T> onSuccess;
        protected Action<FirestoreError> onError;

        public FirestoreRequest<T> OnSuccess(Action<T> onSuccess)
        {
            this.onSuccess = onSuccess;
            return this;
        }

        public FirestoreRequest<T> OnError(Action<FirestoreError> onError)
        {
            this.onError = onError;
            return this;
        }

        protected void HandleFirestoreError(string error)
        {
            FirestoreError firestoreError;

            if (ParseErrorJson(error, out firestoreError) || ParseErrorJsonArray(error, out firestoreError))
            {
                onError?.Invoke(firestoreError);
                return;
            }

            firestoreError = new FirestoreError { Message = "Unknown error" };
            onError?.Invoke(firestoreError);

            bool ParseErrorJson(string error, out FirestoreError firestoreError)
            {
                try
                {
                    JObject errorJson = JObject.Parse(error);
                    firestoreError = JsonConvert.DeserializeObject<FirestoreError>(errorJson["error"].ToString());
                    return true;
                }
                catch (Exception)
                {
                    firestoreError = null;
                    return false;
                }
            }
            bool ParseErrorJsonArray(string error, out FirestoreError firestoreError)
            {
                try
                {
                    JArray errorJson = JArray.Parse(error);
                    firestoreError = JsonConvert.DeserializeObject<FirestoreError>(errorJson[0]["error"].ToString());
                    return true;
                }
                catch (Exception)
                {
                    firestoreError = null;
                    return false;
                }
            }
        }
    }

    public class FirestoreSetRequest : FirestoreRequest<DocumentSnapshot>
    {
        public FirestoreSetRequest Set(string path, Dictionary<string, object> data)
        {
            var url = $"https://firestore.googleapis.com/v1/projects/{FirebaseConfig.ProjectId}/databases/(default)/documents/{path}";

            Value val = Value.ObjectToValue(data);
            var serializedData = JsonConvert.SerializeObject(val.MapValue);

            FirebaseRequestHandler.Instance.Patch(url, serializedData, (response) =>
            {
                var document = JsonConvert.DeserializeObject<Document>(response);
                var snapshot = DocumentSnapshot.FromDocument(document);
                onSuccess?.Invoke(snapshot);

            }, (error) => HandleFirestoreError(error)
            , headers: FirebaseAuth.GetAuthHeader());

            return this;
        }
    }

    public class FirestorePushRequest : FirestoreRequest<DocumentSnapshot>
    {
        public FirestorePushRequest Push(string path, Dictionary<string, object> data)
        {
            var url = $"https://firestore.googleapis.com/v1/projects/{FirebaseConfig.ProjectId}/databases/(default)/documents/{path}";

            Value val = Value.ObjectToValue(data);
            var serializedData = JsonConvert.SerializeObject(val.MapValue);

            FirebaseRequestHandler.Instance.Post(url, serializedData, (response) =>
            {
                var document = JsonConvert.DeserializeObject<Document>(response);
                var snapshot = DocumentSnapshot.FromDocument(document);
                onSuccess?.Invoke(snapshot);

            }, (error) => HandleFirestoreError(error)
            , headers: FirebaseAuth.GetAuthHeader());

            return this;
        }
    }

    public class FirestoreDeleteRequest : FirestoreRequest<string>
    {
        public FirestoreDeleteRequest Delete(string path)
        {
            var url = $"https://firestore.googleapis.com/v1/projects/{FirebaseConfig.ProjectId}/databases/(default)/documents/{path}";
            FirebaseRequestHandler.Instance.Delete(url, (response) => onSuccess?.Invoke(response)
            , (error) => HandleFirestoreError(error)
            , headers: FirebaseAuth.GetAuthHeader());

            return this;
        }
    }

    public class FirestoreGetRequest : FirestoreRequest<DocumentSnapshot>
    {
        public FirestoreGetRequest Get(string path)
        {
            var url = $"https://firestore.googleapis.com/v1/projects/{FirebaseConfig.ProjectId}/databases/(default)/documents/{path}";
            FirebaseRequestHandler.Instance.Get(url, (response) =>
            {
                var document = JsonConvert.DeserializeObject<Document>(response);
                var snapshot = DocumentSnapshot.FromDocument(document);
                onSuccess?.Invoke(snapshot);

            }, (error) => HandleFirestoreError(error)
            , headers: FirebaseAuth.GetAuthHeader());

            return this;
        }
    }

    public class FirestoreQueryRequest : FirestoreRequest<List<DocumentSnapshot>>
    {
        public FirestoreQueryRequest RunQuery(Query query, string parentDocumentPath)
        {
            var url = $"https://firestore.googleapis.com/v1/projects/{FirebaseConfig.ProjectId}/databases/(default)/documents/{parentDocumentPath}:runQuery";
            FirebaseRequestHandler.Instance.Post(url, query, (response) =>
            {
                JArray jsonArray = JArray.Parse(response);
                var snapshots = new List<DocumentSnapshot>();
                foreach (var item in jsonArray)
                {
                    if (item["document"] == null)
                        continue;

                    var document = JsonConvert.DeserializeObject<Document>(item["document"].ToString());
                    var snapshot = DocumentSnapshot.FromDocument(document);
                    snapshots.Add(snapshot);
                }
                onSuccess?.Invoke(snapshots);

            }, (error) => HandleFirestoreError(error), headers: FirebaseAuth.GetAuthHeader());

            return this;
        }
    }
}

