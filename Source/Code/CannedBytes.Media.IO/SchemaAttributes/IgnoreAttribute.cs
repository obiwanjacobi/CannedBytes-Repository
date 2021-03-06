﻿namespace CannedBytes.Media.IO.SchemaAttributes
{
    using System;

    /// <summary>
    /// Code attribute to indicate that the data member is not part of the file
    /// chunk stream and should be ignored in de (de)serialization process.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class IgnoreAttribute : Attribute
    {
    }
}