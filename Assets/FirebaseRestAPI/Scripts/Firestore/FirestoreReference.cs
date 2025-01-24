using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace FirebaseRestAPI
{
    public class FirestoreReference
    {
        protected string path;
        protected DocumentReference document;
        protected CollectionReference collection;

        public FirestoreReference()
        {
            this.path = "";
            document = new DocumentReference(this);
            collection = new CollectionReference(this);
        }

        public CollectionReference Collection(string collectionName)
        {
            path += $"{collectionName}/";
            return collection;
        }

        public class DocumentReference
        {
            private FirestoreReference reference;

            public DocumentReference(FirestoreReference reference)
            {
                this.reference = reference;
            }

            public CollectionReference Collection(string collectionName)
            {
                reference.path += $"{collectionName}/";
                return reference.collection;
            }

            public FirestoreSetRequest Set(Dictionary<string, object> data)
            {
                return new FirestoreSetRequest().Set(reference.path, data);
            }

            public FirestoreGetRequest Get()
            {
                return new FirestoreGetRequest().Get(reference.path);
            }

            public FirestoreDeleteRequest Delete()
            {
                return new FirestoreDeleteRequest().Delete(reference.path);
            }
        }

        public class CollectionReference
        {
            private FirestoreReference reference;

            public CollectionReference(FirestoreReference reference)
            {
                this.reference = reference;
            }

            public DocumentReference Document(string documentName)
            {
                reference.path += $"{documentName}/";
                return reference.document;
            }

            public FirestorePushRequest Push(Dictionary<string, object> data)
            {
                return new FirestorePushRequest().Push(reference.path, data);
            }

            public QueryReference Where(string field, FieldOperator fieldOperator, object value)
            {
                return new QueryReference(reference).Where(field, fieldOperator, value);
            }

            public QueryReference Where(Filter filter)
            {
                return new QueryReference(reference).Where(filter);
            }

            public QueryReference OrderBy(string field, OrderDirection orderDirection)
            {
                return new QueryReference(reference).OrderBy(field, orderDirection);
            }
        }

        public class QueryReference
        {
            private FirestoreReference reference;
            private Query query;

            public QueryReference(FirestoreReference reference)
            {
                this.reference = reference;
                this.query = new Query()
                {
                    StructuredQuery = new StructuredQuery()
                };
            }

            public QueryReference Where(string field, FieldOperator fieldOperator, object value)
            {
                query.StructuredQuery.Where = new Filter
                {
                    FieldFilter = new FieldFilter
                    {
                        Field = new FieldReference
                        {
                            FieldPath = field
                        },
                        Operator = fieldOperator,
                        Value = Value.ObjectToValue(value)
                    }
                };
                return this;
            }

            public QueryReference Where(Filter filter)
            {
                query.StructuredQuery.Where = filter;
                return this;
            }

            public QueryReference OrderBy(string field, OrderDirection orderDirection)
            {
                query.StructuredQuery.OrderBy = new Order[]
                {
                    new Order
                    {
                        Field = new FieldReference
                        {
                            FieldPath = field
                        },
                        Direction = orderDirection
                    }
                };
                return this;
            }

            public QueryReference StartAt(object value)
            {
                query.StructuredQuery.StartAt = new Cursor
                {
                    Values = new Value[]
                    {
                        Value.ObjectToValue(value)
                    }
                };
                return this;
            }

            public QueryReference EndAt(object value)
            {
                query.StructuredQuery.EndAt = new Cursor
                {
                    Values = new Value[]
                    {
                        Value.ObjectToValue(value)
                    }
                };
                return this;
            }

            public QueryReference Limit(int limit)
            {
                query.StructuredQuery.Limit = limit;
                return this;
            }

            public FirestoreQueryRequest RunQuery()
            {
                string collectionPath = reference.path.Trim('/');

                query.StructuredQuery.From = new CollectionSelector
                {
                    CollectionId = collectionPath.Split('/').Last()
                };

                if (collectionPath.Contains("/"))
                {
                    return new FirestoreQueryRequest().RunQuery(query, collectionPath.Substring(0, collectionPath.LastIndexOf('/')));
                }
                else
                {
                    return new FirestoreQueryRequest().RunQuery(query, "");
                }
            }
        }
    }

}