using UnityEngine;

namespace FirebaseRestAPI
{
    public enum FieldOperator
    {
        /// <summary>
        /// The given field is less than the given value.
        /// </summary>
        LESS_THAN,

        /// <summary>
        /// The given field is less than or equal to the given value.
        /// </summary>
        LESS_THAN_OR_EQUAL,

        /// <summary>
        /// The given field is greater than the given value.
        /// </summary>
        GREATER_THAN,

        /// <summary>
        /// The given field is greater than or equal to the given value.
        /// </summary>
        GREATER_THAN_OR_EQUAL,

        /// <summary>
        /// The given field is equal to the given value.
        /// </summary>
        EQUAL,

        /// <summary>
        /// The given field is not equal to the given value.
        /// </summary>
        NOT_EQUAL,

        /// <summary>
        /// The given field is an array that contains the given value.
        /// </summary>
        ARRAY_CONTAINS,

        /// <summary>
        /// The given field is equal to at least one value in the given array.
        /// </summary>
        IN,

        /// <summary>
        /// The given field is an array that contains any of the values in the given array.
        /// </summary>
        ARRAY_CONTAINS_ANY,

        /// <summary>
        /// The value of the field is not in the given array.
        /// </summary>
        NOT_IN,
    }
}
