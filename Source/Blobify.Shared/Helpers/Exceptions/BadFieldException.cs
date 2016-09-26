using System;
using System.Diagnostics.CodeAnalysis;

namespace Blobify.Shared
{
    [SuppressMessage("Microsoft.Usage",
        "CA2237:MarkISerializableTypesWithSerializable")]
    public class BadFieldException : Exception
    {
        public BadFieldException(string fieldName)
            : base($"The \"{fieldName}\" field is invalid!")
        {
            FieldName = fieldName;
        }

        public string FieldName { get; }
    }
}