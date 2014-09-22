﻿using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DirLink
{
    public class DirectoryRepository : IDisposable
    {
        private readonly NetworkCredential _credentials;
        private readonly LdapDirectoryIdentifier _identifier;

        private LdapConnection _connection;
        private RootDse _lastRootDse;

        /// <summary>
        ///     Creates a new instance of <see cref="DirectoryRepository" /> with the specified connections parameters and
        ///     credentials.
        /// </summary>
        public DirectoryRepository(LdapDirectoryIdentifier identifier, NetworkCredential credentials)
        {
            _identifier = identifier;
            _credentials = credentials;
        }

        public Action<LdapConnection> ConfigureConnectionCallback { get; set; }

        public void Dispose()
        {
            if (_connection == null) return;

            _connection.Dispose();
            _connection = null;
        }

        /// <summary>
        ///     Tests the specified <paramref name="credentials" /> against <paramref name="host" /> using fast concurrent bind.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="credentials"></param>
        /// <param name="configCallback"></param>
        /// <returns></returns>
        public static Task<TestBindResult> TestBind(LdapDirectoryIdentifier identifier, NetworkCredential credentials,
            Action<LdapConnection> configCallback = null)
        {
            Action<LdapConnection> config = configCallback ?? DefaultConnectionConfiguration;

            var connection = new LdapConnection(identifier)
            {
                AutoBind = false,
                AuthType = AuthType.Basic,
                Timeout = TimeSpan.FromSeconds(10)
            };

            config(connection);

            var tcs = new TaskCompletionSource<TestBindResult>();

            Task.Run(() =>
            {
                try
                {
                    connection.SessionOptions.FastConcurrentBind();
                    connection.Bind(credentials);

                    tcs.TrySetResult(new TestBindResult {Success = true});
                }
                catch (LdapException e)
                {
                    tcs.TrySetResult(new TestBindResult
                    {
                        Success = false,
                        ErrorCode = e.ErrorCode,
                        ServerErrorMessage = e.ServerErrorMessage
                    });
                }
                finally
                {
                    connection.Dispose();
                }
            });

            return tcs.Task;
        }

        /// <summary>
        ///     Asynchronously executes a <paramref name="request" />.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="option"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IList<SearchResultWrapper>> Search(
            SearchRequest request,
            SearchOption option = SearchOption.DomainScope,
            int pageSize = 1000)
        {
            await EnsureBind();

            var result = new List<SearchResponse>();

            request.Controls.Add(new SearchOptionsControl(option));

            var pager = new PageResultRequestControl(pageSize);
            request.Controls.Add(pager);

            while (true)
            {
                IAsyncResult ar = _connection.BeginSendRequest(request, default(PartialResultProcessing), null, null);

                SearchResponse response =
                    await Task.Factory.FromAsync(ar, x => (SearchResponse) _connection.EndSendRequest(x));

                result.Add(response);

                PageResultResponseControl pagerResponse = response.Controls
                    .OfType<PageResultResponseControl>()
                    .FirstOrDefault();

                if (pagerResponse == null || pagerResponse.Cookie.Length == 0)
                    break;

                pager.Cookie = pagerResponse.Cookie;
            }

            return result.SelectMany(x => x.Entries
                .Cast<SearchResultEntry>())
                .Select(x => new SearchResultWrapper(x))
                .ToList();
        }

        /// <summary>
        ///     Asynchronous wrapper for simple searches. If no <paramref name="startDn" /> is defined,
        ///     <see cref="RootDse.DefaultNamingContext" /> will be used. The RootDse will be acquired if necessary.
        /// </summary>
        /// <param name="filter">A LDAP filter.</param>
        /// <param name="startDn">
        ///     The DN of the object to start searching from -or- <c>null</c> if the
        ///     <see cref="RootDse.DefaultNamingContext" /> should be used.
        /// </param>
        /// <returns></returns>
        public async Task<IList<SearchResultWrapper>> Query(string filter, string startDn = null)
        {
            if (startDn == null)
            {
                await EnsureRootDse();
                startDn = _lastRootDse.DefaultNamingContext;
            }

            return await Search(new SearchRequest(startDn, filter, SearchScope.Subtree));
        }

        public async Task<RootDse> GetRootDse()
        {
            var search = new SearchRequest(null, "(objectClass=*)", SearchScope.Base);

            SearchResultWrapper result = (await Search(search)).FirstOrDefault();

            if (result == null)
                return null;

            var rootDse = new RootDse(result);

            _lastRootDse = rootDse;

            return rootDse;
        }

        private Task<bool> EnsureBind()
        {
            if (_connection != null) return Task.FromResult(false);

            Action<LdapConnection> config = ConfigureConnectionCallback ?? DefaultConnectionConfiguration;

            var connection = new LdapConnection(_identifier) {AutoBind = false};
            config(connection);

            var tcs = new TaskCompletionSource<bool>();

            Task.Run(() =>
            {
                try
                {
                    connection.Bind(_credentials);
                    _connection = connection;

                    tcs.TrySetResult(true);
                }
                catch (Exception e)
                {
                    tcs.TrySetException(e);
                }
            });

            return tcs.Task;
        }

        private async Task EnsureRootDse()
        {
            if (_lastRootDse != null) return;

            await GetRootDse();
        }

        private static void DefaultConnectionConfiguration(LdapConnection connection)
        {
            connection.SessionOptions.SecureSocketLayer = true;
        }

        public class TestBindResult
        {
            public bool Success { get; set; }
            public int ErrorCode { get; set; }
            public string ServerErrorMessage { get; set; }
        }
    }
}