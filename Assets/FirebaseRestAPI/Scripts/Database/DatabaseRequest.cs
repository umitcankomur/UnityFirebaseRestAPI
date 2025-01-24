using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace FirebaseRestAPI
{
    public class DatabaseRequest
    {
        private Action<string> onSuccess;
        private Action<DatabaseError> onError;

        public DatabaseRequest OnSuccess(Action<string> onSuccess)
        {
            this.onSuccess = onSuccess;
            return this;
        }

        public DatabaseRequest OnError(Action<DatabaseError> onError)
        {
            this.onError = onError;
            return this;
        }

        private void Error(string error)
        {
            var databaseError = JsonConvert.DeserializeObject<DatabaseError>(error);
            if (databaseError == null)
            {
                databaseError = new DatabaseError();
            }
            onError?.Invoke(databaseError);
        }

        public DatabaseRequest Set(string url, object data)
        {
            FirebaseRequestHandler.Instance.Put(url, data, (response) => onSuccess?.Invoke(response), (e) => Error(e));
            return this;
        }

        public DatabaseRequest Get(string url)
        {
            FirebaseRequestHandler.Instance.Get(url, (response) => onSuccess?.Invoke(response), (e) => Error(e));
            return this;
        }

        public DatabaseRequest Push(string url, object data)
        {
            FirebaseRequestHandler.Instance.Post(url, data,
                (response) =>
                {
                    JObject data = JObject.Parse(response);
                    var name = data["name"].ToString();
                    onSuccess?.Invoke(name);
                },
                (e) => Error(e));
            return this;
        }

        public DatabaseRequest Delete(string url)
        {
            FirebaseRequestHandler.Instance.Delete(url, (response) => onSuccess?.Invoke(response), (e) => Error(e));
            return this;
        }
    }
}
