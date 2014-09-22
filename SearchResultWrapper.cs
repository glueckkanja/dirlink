using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;

namespace DirLink
{
    public class SearchResultWrapper
    {
        private readonly SearchResultEntry _entry;

        internal SearchResultWrapper(SearchResultEntry entry)
        {
            _entry = entry;
        }

        public string DistinguishedName
        {
            get { return _entry.DistinguishedName; }
        }

        public IDictionary<string, DirAttribute> Attributes
        {
            get
            {
                return _entry.Attributes
                    .Cast<DictionaryEntry>()
                    .Select(x => (DirectoryAttribute) x.Value)
                    .ToDictionary(x => x.Name, x => new DirAttribute(x));
            }
        }

        public DirectoryControl[] Controls
        {
            get { return _entry.Controls; }
        }
    }
}