﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Serilog.Converters;
using Serilog.Core;
using Serilog.Events;
using Serilog.Providers;

namespace Serilog.Enrichers
{
    public class RequestEnricher : ILogEventEnricher
    {
        private readonly IHttpRequestProvider _httpRequestProvider;

        public RequestEnricher()
            : this(new HttpRequestProvider())
        {
        }

        internal RequestEnricher(IHttpRequestProvider httpRequestProvider)
        {
            _httpRequestProvider = httpRequestProvider;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var httpRequest = _httpRequestProvider.GetRequest();

            if (httpRequest == null)
                return;

            propertyFactory
                .CreateProperty("Request.AcceptTypes", CreateSequence(httpRequest.AcceptTypes, x => x))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.AnonymousID", new ScalarValue(httpRequest.AnonymousID))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.ApplicationPath", new ScalarValue(httpRequest.ApplicationPath))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.ContentEncoding", new ScalarValue(httpRequest.ContentEncoding))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.ContentLength", new ScalarValue(httpRequest.ContentLength))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.ContentType", new ScalarValue(httpRequest.ContentType))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.HttpMethod", new ScalarValue(httpRequest.HttpMethod))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.IsAuthenticated", new ScalarValue(httpRequest.IsAuthenticated))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.IsLocal", new ScalarValue(httpRequest.IsLocal))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.IsSecureConnection", new ScalarValue(httpRequest.IsSecureConnection))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.PhysicalApplicationPath", new ScalarValue(httpRequest.PhysicalApplicationPath))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.PhysicalPath", new ScalarValue(httpRequest.PhysicalPath))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.RawUrl", new ScalarValue(httpRequest.RawUrl))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.RequestType", new ScalarValue(httpRequest.RequestType))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.TotalBytes", new ScalarValue(httpRequest.TotalBytes))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.Url", new ScalarValue(Convert.ToString(httpRequest.Url)))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.UrlReferrer", new ScalarValue(Convert.ToString(httpRequest.UrlReferrer)))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.UserAgent", new ScalarValue(httpRequest.UserAgent))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.UserHostAddress", new ScalarValue(httpRequest.UserHostAddress))
                .AddIfAbsent(logEvent);
            propertyFactory
                .CreateProperty("Request.UserHostName", new ScalarValue(httpRequest.UserHostName))
                .AddIfAbsent(logEvent);

            foreach (var property in ExtractLogEventProperties(httpRequest.Cookies, "Request.Cookies", propertyFactory))
                property.AddIfAbsent(logEvent);
            
            foreach (var property in ExtractLogEventProperties(httpRequest.Files, "Request.Files", propertyFactory))
                property.AddIfAbsent(logEvent);

            foreach (var property in ExtractLogEventProperties(httpRequest.Form, "Request.Form", propertyFactory))
                property.AddIfAbsent(logEvent);

            foreach (var property in ExtractLogEventProperties(httpRequest.Headers, "Request.Headers", propertyFactory))
                property.AddIfAbsent(logEvent);

            foreach (var property in ExtractLogEventProperties(httpRequest.Params, "Request.Params", propertyFactory))
                property.AddIfAbsent(logEvent);
        }

        private static SequenceValue CreateSequence<TSource, TDest>(
            IEnumerable<TSource> values,
            Func<TSource, TDest> converter)
        {
            return new SequenceValue(values.Select(x => new ScalarValue(converter(x))));
        }

        private static IEnumerable<LogEventProperty> ExtractLogEventProperties(
            NameValueCollection collection,
            string propertyName,
            ILogEventPropertyFactory propertyFactory)
        {
            var converter = new NameValueCollectionLogEventPropertyConverter(propertyFactory, propertyName);
            return converter.Convert(collection);
        }

        private static IEnumerable<LogEventProperty> ExtractLogEventProperties(
            HttpCookieCollection collection,
            string propertyName,
            ILogEventPropertyFactory propertyFactory)
        {
            var converter = new HttpCookieCollectionLogEventPropertyConverter(propertyFactory, propertyName);
            return converter.Convert(collection);
        }

        private static IEnumerable<LogEventProperty> ExtractLogEventProperties(
            IHttpFileCollectionWrapper collection,
            string propertyName,
            ILogEventPropertyFactory propertyFactory)
        {
            var converter = new HttpFileCollectionWrapperLogEventPropertyConverter(propertyFactory, propertyName);
            return converter.Convert(collection);
        }
    }
}