using System;
using System.Collections.Generic;

namespace FirebaseRestAPI
{
    public class DocumentSnapshot
    {
        public string DocumentId { get; private set; }
        public Dictionary<string, object> Data { get; private set; }
        public DateTime CreateTime { get; private set; }
        public DateTime UpdateTime { get; private set; }

        public static DocumentSnapshot FromDocument(Document document)
        {
            DocumentSnapshot snapshot = new DocumentSnapshot();

            snapshot.Data = new Dictionary<string, object>();
            foreach (var field in document.Fields)
            {
                snapshot.Data.Add(field.Key, field.Value.ToObject());
            }

            snapshot.DocumentId = document.Name.Split("/")[^1];
            snapshot.CreateTime = document.CreateTime;
            snapshot.UpdateTime = document.UpdateTime;

            return snapshot;
        }
    }
}

