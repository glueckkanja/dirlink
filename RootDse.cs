using System;
using System.Collections.Generic;

namespace DirLink
{
    public class RootDse : ResultWrapper
    {
        public RootDse(SearchResultWrapper result) : base(result)
        {
        }

        public string ConfigurationNamingContext
        {
            get { return (string) RawResult.Attributes["configurationNamingContext"]; }
        }

        public DateTimeOffset CurrentTime
        {
            get { return (DateTimeOffset) RawResult.Attributes["currentTime"]; }
        }

        public string DefaultNamingContext
        {
            get { return (string) RawResult.Attributes["defaultNamingContext"]; }
        }

        public string DnsHostName
        {
            get { return (string) RawResult.Attributes["dnsHostName"]; }
        }

        public string DomainControllerFunctionality
        {
            get { return (string) RawResult.Attributes["domainControllerFunctionality"]; }
        }

        public string DomainFunctionality
        {
            get { return (string) RawResult.Attributes["domainFunctionality"]; }
        }

        public string DsServiceName
        {
            get { return (string) RawResult.Attributes["dsServiceName"]; }
        }

        public string ForestFunctionality
        {
            get { return (string) RawResult.Attributes["forestFunctionality"]; }
        }

        public long HighestCommittedUsn
        {
            get { return (long) RawResult.Attributes["highestCommittedUSN"]; }
        }

        public bool IsGlobalCatalogReady
        {
            get { return (bool) RawResult.Attributes["isGlobalCatalogReady"]; }
        }

        public bool IsSynchronized
        {
            get { return (bool) RawResult.Attributes["isSynchronized"]; }
        }

        public string LdapServiceName
        {
            get { return (string) RawResult.Attributes["ldapServiceName"]; }
        }

        public IList<string> NamingContexts
        {
            get { return (List<string>) RawResult.Attributes["namingContexts"]; }
        }

        public string RootDomainNamingContext
        {
            get { return (string) RawResult.Attributes["rootDomainNamingContext"]; }
        }

        public string SchemaNamingContext
        {
            get { return (string) RawResult.Attributes["schemaNamingContext"]; }
        }

        public string ServerName
        {
            get { return (string) RawResult.Attributes["serverName"]; }
        }

        public string SubschemaSubentry
        {
            get { return (string) RawResult.Attributes["subschemaSubentry"]; }
        }

        public IList<string> SupportedCapabilities
        {
            get { return (List<string>) RawResult.Attributes["supportedCapabilities"]; }
        }

        public IList<string> SupportedControls
        {
            get { return (List<string>) RawResult.Attributes["supportedControl"]; }
        }

        public IList<string> SupportedLdapPolicies
        {
            get { return (List<string>) RawResult.Attributes["supportedLDAPPolicies"]; }
        }

        public IList<string> SupportedLdapVersion
        {
            get { return (List<string>) RawResult.Attributes["supportedLDAPVersion"]; }
        }

        public IList<string> SupportedSaslMechanisms
        {
            get { return (List<string>) RawResult.Attributes["supportedSASLMechanisms"]; }
        }
    }
}