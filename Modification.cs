using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirLink
{
    /// <summary>
    ///     Helper class for generating <see cref="DirectoryAttributeModification"/> arrays.
    /// </summary>
    public static class Modification
    {
        public static DirectoryAttributeModification Clear(string attributeName)
        {
            return new DirectoryAttributeModification { Name = attributeName, Operation = DirectoryAttributeOperation.Delete };
        }

        public static DirectoryAttributeModification Set(string attributeName, int value)
        {
            return Set(attributeName, value.ToString(CultureInfo.InvariantCulture));
        }

        public static DirectoryAttributeModification Set(string attributeName, long value)
        {
            return Set(attributeName, value.ToString(CultureInfo.InvariantCulture));
        }

        public static DirectoryAttributeModification Set(string attributeName, bool value)
        {
            return Set(attributeName, value ? "TRUE" : "FALSE");
        }

        public static DirectoryAttributeModification Set(string attributeName, Guid value)
        {
            return Set(attributeName, value.ToByteArray());
        }

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

        public static DirectoryAttributeModification Set(string attributeName, byte[] value)
        {
            var dam = new DirectoryAttributeModification { Name = attributeName, Operation = DirectoryAttributeOperation.Replace };

            dam.Clear();
            dam.Add(value);

            return dam;
        }

        public static DirectoryAttributeModification Set(string attributeName, string value)
        {
            var dam = new DirectoryAttributeModification { Name = attributeName, Operation = DirectoryAttributeOperation.Replace };

            dam.Clear();
            dam.Add(value);

            return dam;
        }

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

        public static DirectoryAttributeModification Add(string attributeName, string value)
        {
            var dam = new DirectoryAttributeModification { Name = attributeName, Operation = DirectoryAttributeOperation.Add };

            dam.Add(value);

            return dam;
        }

        public static DirectoryAttributeModification Remove(string attributeName, string value)
        {
            var dam = new DirectoryAttributeModification { Name = attributeName, Operation = DirectoryAttributeOperation.Delete };

            dam.Add(value);

            return dam;
        }
    }
}
