using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Linq;

namespace DirLink
{
    public class DirAttribute
    {
        private readonly DirectoryAttribute _attribute;

        internal DirAttribute(DirectoryAttribute attribute)
        {
            _attribute = attribute;
        }

        public static explicit operator int(DirAttribute attribute)
        {
            return int.Parse((string) attribute);
        }

        public static explicit operator long(DirAttribute attribute)
        {
            return long.Parse((string) attribute);
        }

        public static explicit operator bool(DirAttribute attribute)
        {
            return bool.Parse((string) attribute);
        }

        public static explicit operator byte[](DirAttribute attribute)
        {
            byte[] bytes = attribute._attribute
                .GetValues(typeof (byte[]))
                .Cast<byte[]>()
                .FirstOrDefault();

            return bytes;
        }

        public static explicit operator Guid(DirAttribute attribute)
        {
            var bytes = (byte[]) attribute;

            return bytes == null ? Guid.Empty : new Guid(bytes);
        }

        public static explicit operator DateTimeOffset(DirAttribute attribute)
        {
            return DateTimeOffset.ParseExact((string) attribute, "yyyyMMddHHmmmss.fK", CultureInfo.InvariantCulture);
        }

        public static explicit operator TimeSpan(DirAttribute attribute)
        {
            return TimeSpan.FromTicks((long) attribute);
        }

        public static explicit operator string(DirAttribute attribute)
        {
            return GetStrings(attribute).FirstOrDefault();
        }

        public override string ToString()
        {
            return string.Join("; ", GetStrings(this));
        }

        public static explicit operator List<string>(DirAttribute attribute)
        {
            return GetStrings(attribute).ToList();
        }

        public static explicit operator string[](DirAttribute attribute)
        {
            return GetStrings(attribute).ToArray();
        }

        private static IEnumerable<string> GetStrings(DirAttribute attribute)
        {
            return attribute._attribute.GetValues(typeof (string)).Cast<string>();
        }
    }
}