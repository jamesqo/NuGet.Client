// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Net.Http;
using NuGet.Common;

namespace NuGet.Protocol
{
    internal static class HttpRequestMessageExtensions
    {
        public static readonly string NuGetLoggerKey = "NuGet_Logger";

        /// <summary>
        /// Clones an <see cref="HttpRequestMessage" /> request.
        /// </summary>
        public static HttpRequestMessage Clone(this HttpRequestMessage request)
        {
            Debug.Assert(request.Content == null, "Cloning the request content is not yet implemented.");

            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Content = request.Content,
                Version = request.Version
            };

            foreach (var header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            foreach (var property in request.Properties)
            {
                clone.Properties.Add(property);
            }

            return clone;
        }

        public static ILogger GetLogger(this HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return request.GetProperty<ILogger>(NuGetLoggerKey);
        }

        public static void SetLogger(this HttpRequestMessage request, ILogger logger)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            request.Properties[NuGetLoggerKey] = logger;
        }

        private static T GetProperty<T>(this HttpRequestMessage request, string key)
        {
            object result;
            request.Properties.TryGetValue(key, out result);
            return (T)result;
        }
    }
}
