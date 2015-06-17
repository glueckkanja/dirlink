using System;
using System.DirectoryServices.Protocols;
using System.Globalization;

namespace DirLink
{
    /// <summary>
    ///     Helper class for generating <see cref="DirectoryAttributeModification"/> arrays.
    /// </summary>
    public static class Modification
    {
        /// <summary>
        ///     Deletes all values for the specified attribute.
        /// </summary>
        public static DirectoryAttributeModification Clear(string attributeName)
        {
            return new DirectoryAttributeModification { Name = attributeName, Operation = DirectoryAttributeOperation.Delete };
        }

        /// <summary>
        ///     Sets a single integer value for the specified attribute.
        /// </summary>
        /// <remarks>
        ///     All existing values will be removed (if any).
        /// </remarks>
        public static DirectoryAttributeModification Set(string attributeName, int value)
        {
            return Set(attributeName, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Sets a single long integer value for the specified attribute.
        /// </summary>
        /// <remarks>
        ///     All existing values will be removed (if any).
        /// </remarks>
        public static DirectoryAttributeModification Set(string attributeName, long value)
        {
            return Set(attributeName, value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Sets a single boolean value for the specified attribute.
        /// </summary>
        /// <remarks>
        ///     All existing values will be removed (if any).
        /// </remarks>
        public static DirectoryAttributeModification Set(string attributeName, bool value)
        {
            return Set(attributeName, value ? "TRUE" : "FALSE");
        }

        /// <summary>
        ///     Sets a single GUID value for the specified attribute.
        /// </summary>
        /// <remarks>
        ///     All existing values will be removed (if any).
        /// </remarks>
        public static DirectoryAttributeModification Set(string attributeName, Guid value)
        {
            return Set(attributeName, value.ToByteArray());
        }

        /// <summary>
        ///     Sets a single date and time value for the specified attribute.
        /// </summary>
        /// <param name="asFileTime">Specify <c>true</c> to store the value as file time long integer instead of a real date and time.</param>
        /// <remarks>
        ///     All existing values will be removed (if any).
        /// </remarks>
        public static DirectoryAttributeModification Set(string attributeName, DateTimeOffset value, bool asFileTime = false)
        {
            if (asFileTime)
            {
                return Set(attributeName, value.ToFileTime());
            }
            else
            {
                return Set(attributeName, value.ToString("yyyyMMddHHmmmss.fK", CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        ///     Sets a single blob value for the specified attribute.
        /// </summary>
        /// <remarks>
        ///     All existing values will be removed (if any).
        /// </remarks>
        public static DirectoryAttributeModification Set(string attributeName, byte[] value)
        {
            var dam = new DirectoryAttributeModification { Name = attributeName, Operation = DirectoryAttributeOperation.Replace };

            dam.Clear();
            dam.Add(value);

            return dam;
        }

        /// <summary>
        ///     Sets a single string value for the specified attribute.
        /// </summary>
        /// <remarks>
        ///     All existing values will be removed (if any).
        /// </remarks>
        public static DirectoryAttributeModification Set(string attributeName, string value)
        {
            var dam = new DirectoryAttributeModification { Name = attributeName, Operation = DirectoryAttributeOperation.Replace };

            dam.Clear();
            dam.Add(value);

            return dam;
        }

        /// <summary>
        ///     Sets multiple string values for the specified attribute.
        /// </summary>
        /// <remarks>
        ///     All existing values will be removed (if any).
        /// </remarks>
        public static DirectoryAttributeModification Set(string attributeName, string[] value)
        {
            var dam = new DirectoryAttributeModification { Name = attributeName, Operation = DirectoryAttributeOperation.Replace };

            dam.Clear();

            foreach (var v in value)
            {
                dam.Add(v);
            }

            return dam;
        }

        /// <summary>
        ///     Adds a single string value for the specified attribute.
        /// </summary>
        /// <remarks>
        ///     All existing values will be kept (if any).
        /// </remarks>
        public static DirectoryAttributeModification Add(string attributeName, string value)
        {
            var dam = new DirectoryAttributeModification { Name = attributeName, Operation = DirectoryAttributeOperation.Add };

            dam.Add(value);

            return dam;
        }

        /// <summary>
        ///     Removes a single string value from the specified attribute.
        /// </summary>
        /// <remarks>
        ///     All other existing values will be kept (if any).
        /// </remarks>
        public static DirectoryAttributeModification Remove(string attributeName, string value)
        {
            var dam = new DirectoryAttributeModification { Name = attributeName, Operation = DirectoryAttributeOperation.Delete };

            dam.Add(value);

            return dam;
        }
    }
}
