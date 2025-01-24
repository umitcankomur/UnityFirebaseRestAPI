using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace FirebaseRestAPI
{
    public class DatabaseReference
    {
        private string path;
        private Dictionary<string, object> queryParams;

        public DatabaseReference(string path = "")
        {
            this.path = path;
        }

        public DatabaseRequest Set(object data)
        {
            return new DatabaseRequest().Set(GetUrl(), data);
        }

        public DatabaseRequest Get()
        {
            return new DatabaseRequest().Get(GetUrl());
        }

        public DatabaseRequest Push(object data)
        {
            return new DatabaseRequest().Push(GetUrl(), data);
        }

        public DatabaseRequest Delete()
        {
            return new DatabaseRequest().Delete(GetUrl());
        }

        public DatabaseReference Child(string child)
        {
            this.path += $"{child}/";
            return this;
        }

        public DatabaseReference OrderByChild(string orderBy)
        {
            AddParam("orderBy", $"\"{orderBy}\"");
            return this;
        }

        public DatabaseReference OrderByKey()
        {
            return OrderByChild("$key");
        }

        public DatabaseReference OrderByValue()
        {
            return OrderByChild("$value");
        }

        public DatabaseReference StartAt(object startAt)
        {
            AddParam("startAt", JsonConvert.SerializeObject(startAt));
            return this;
        }

        public DatabaseReference EndAt(object endAt)
        {
            AddParam("endAt", JsonConvert.SerializeObject(endAt));
            return this;
        }

        public DatabaseReference EqualTo(object equalTo)
        {
            AddParam("equalTo", JsonConvert.SerializeObject(equalTo));
            return this;
        }

        public DatabaseReference LimitToFirst(int limitToFirst)
        {
            AddParam("limitToFirst", limitToFirst);
            return this;
        }

        public DatabaseReference LimitToLast(int limitToLast)
        {
            AddParam("limitToLast", limitToLast);
            return this;
        }

        private void AddParam(string key, object value)
        {
            if (queryParams == null)
            {
                queryParams = new Dictionary<string, object>();
            }

            if (queryParams.ContainsKey(key))
            {
                queryParams[key] = value;
                return;
            }

            queryParams.Add(key, value);
        }

        private string GetUrl()
        {
            if (FirebaseAuth.IsAuthenticated)
            {
                AddParam("auth", FirebaseAuth.CurrentUser.IdToken);
            }

            StringBuilder url = new StringBuilder();
            url.Append($"https://{FirebaseConfig.ProjectId}-default-rtdb.firebaseio.com/{path.TrimEnd('/')}.json");

            if (queryParams != null && queryParams.Count > 0)
            {
                url.Append("?");
                bool isFirst = true;
                foreach (var param in queryParams)
                {
                    if (!isFirst)
                        url.Append("&");
                    url.Append($"{param.Key}={UnityWebRequest.EscapeURL(param.Value.ToString())}");
                    isFirst = false;
                }
            }

            return url.ToString();
        }
    }
}