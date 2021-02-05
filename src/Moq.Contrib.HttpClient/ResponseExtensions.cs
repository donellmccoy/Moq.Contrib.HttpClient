﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq.Language;
using Moq.Language.Flow;

namespace Moq.Contrib.HttpClient
{
    public static partial class MockHttpMessageHandlerExtensions
    {
        private static HttpResponseMessage CreateResponse(
            HttpRequestMessage request = null,
            HttpStatusCode statusCode = HttpStatusCode.OK,
            HttpContent content = null,
            string mediaType = null,
            Action<HttpResponseMessage> configure = null)
        {
            var response = new HttpResponseMessage(statusCode)
            {
                RequestMessage = request,
                Content = content
            };

            if (content != null && mediaType != null)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            }

            configure?.Invoke(response);

            return response;
        }

        /// <summary>
        /// Specifies the response to return.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="configure">An action to further configure the response such as setting headers.</param>
        public static IReturnsResult<HttpMessageHandler> ReturnsResponse(
            this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup,
            HttpStatusCode statusCode,
            Action<HttpResponseMessage> configure = null)
        {
            return setup.ReturnsAsync((HttpRequestMessage request, CancellationToken _) =>
            {
                return CreateResponse
                (
                    request: request,
                    statusCode: statusCode,
                    configure: configure
                );
            });
        }

        /// <summary>
        /// Specifies the response to return in sequence.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="configure">An action to further configure the response such as setting headers.</param>
        public static ISetupSequentialResult<Task<HttpResponseMessage>> ReturnsResponse(
            this ISetupSequentialResult<Task<HttpResponseMessage>> setup,
            HttpStatusCode statusCode,
            Action<HttpResponseMessage> configure = null)
        {
            return setup.ReturnsAsync(CreateResponse(statusCode: statusCode, configure: configure));
        }

        /// <summary>
        /// Specifies the response to return.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="configure">An action to further configure the response such as setting headers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="content" /> is null.</exception>
        public static IReturnsResult<HttpMessageHandler> ReturnsResponse(
            this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup,
            HttpStatusCode statusCode,
            HttpContent content,
            Action<HttpResponseMessage> configure = null)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            return setup.ReturnsAsync((HttpRequestMessage request, CancellationToken _) =>
            {
                return CreateResponse
                (
                    request: request,
                    statusCode: statusCode,
                    content: content,
                    configure: configure
                );
            });
        }

        /// <summary>
        /// Specifies the response to return in sequence.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="configure">An action to further configure the response such as setting headers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="content" /> is null.</exception>
        public static ISetupSequentialResult<Task<HttpResponseMessage>> ReturnsResponse(
            this ISetupSequentialResult<Task<HttpResponseMessage>> setup,
            HttpStatusCode statusCode,
            HttpContent content,
            Action<HttpResponseMessage> configure = null)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            return setup.ReturnsAsync(CreateResponse(
                statusCode: statusCode,
                content: content,
                configure: configure));
        }

        /// <summary>
        /// Specifies the response to return, as <see cref="StringContent" />.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="content">The response body.</param>
        /// <param name="mediaType">The media type. Defaults to text/plain.</param>
        /// <param name="encoding">The character encoding. Defaults to <see cref="Encoding.UTF8" />.</param>
        /// <param name="configure">An action to further configure the response such as setting headers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="content" /> is null.</exception>
        public static IReturnsResult<HttpMessageHandler> ReturnsResponse(
            this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup,
            HttpStatusCode statusCode,
            string content,
            string mediaType = null,
            Encoding encoding = null,
            Action<HttpResponseMessage> configure = null)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            return setup.ReturnsAsync((HttpRequestMessage request, CancellationToken _) =>
            {
                return CreateResponse(
                    request: request,
                    statusCode: statusCode,
                    content: new StringContent(content, encoding, mediaType),
                    configure: configure);
            });
        }

        /// <summary>
        /// Specifies the response to return in sequence, as <see cref="StringContent" />.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="content">The response body.</param>
        /// <param name="mediaType">The media type. Defaults to text/plain.</param>
        /// <param name="encoding">The character encoding. Defaults to <see cref="Encoding.UTF8" />.</param>
        /// <param name="configure">An action to further configure the response such as setting headers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="content" /> is null.</exception>
        public static ISetupSequentialResult<Task<HttpResponseMessage>> ReturnsResponse(
            this ISetupSequentialResult<Task<HttpResponseMessage>> setup,
            HttpStatusCode statusCode,
            string content,
            string mediaType = null,
            Encoding encoding = null,
            Action<HttpResponseMessage> configure = null)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            return setup.ReturnsAsync(CreateResponse(
                statusCode: statusCode,
                content: new StringContent(content, encoding, mediaType),
                configure: configure));
        }

        /// <summary>
        /// Specifies the response to return, as <see cref="StringContent" /> with <see cref="HttpStatusCode.OK" />.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="content">The response body.</param>
        /// <param name="mediaType">The media type. Defaults to text/plain.</param>
        /// <param name="encoding">The character encoding. Defaults to <see cref="Encoding.UTF8" />.</param>
        /// <param name="configure">An action to further configure the response such as setting headers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="content" /> is null.</exception>
        public static IReturnsResult<HttpMessageHandler> ReturnsResponse(
            this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup,
            string content,
            string mediaType = null,
            Encoding encoding = null,
            Action<HttpResponseMessage> configure = null)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            return setup.ReturnsAsync((HttpRequestMessage request, CancellationToken _) =>
            {
                return CreateResponse(
                    request: request,
                    content: new StringContent(content, encoding, mediaType),
                    configure: configure);
            });
        }

        /// <summary>
        /// Specifies the response to return in sequence, as <see cref="StringContent" /> with <see
        /// cref="HttpStatusCode.OK" />.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="content">The response body.</param>
        /// <param name="mediaType">The media type. Defaults to text/plain.</param>
        /// <param name="encoding">The character encoding. Defaults to <see cref="Encoding.UTF8" />.</param>
        /// <param name="configure">An action to further configure the response such as setting headers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="content" /> is null.</exception>
        public static ISetupSequentialResult<Task<HttpResponseMessage>> ReturnsResponse(
            this ISetupSequentialResult<Task<HttpResponseMessage>> setup,
            string content,
            string mediaType = null,
            Encoding encoding = null,
            Action<HttpResponseMessage> configure = null)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            return setup.ReturnsAsync(CreateResponse(
                content: new StringContent(content, encoding, mediaType),
                configure: configure));
        }

        /// <summary>
        /// Specifies the response to return, as <see cref="ByteArrayContent" />.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="content">The response body.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="configure">An action to further configure the response such as setting headers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="content" /> is null.</exception>
        public static IReturnsResult<HttpMessageHandler> ReturnsResponse(
            this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup,
            HttpStatusCode statusCode,
            byte[] content,
            string mediaType = null,
            Action<HttpResponseMessage> configure = null)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            return setup.ReturnsAsync((HttpRequestMessage request, CancellationToken _) =>
            {
                return CreateResponse(
                    request: request,
                    statusCode: statusCode,
                    content: new ByteArrayContent(content),
                    mediaType: mediaType,
                    configure: configure);
            });
        }

        /// <summary>
        /// Specifies the response to return in sequence, as <see cref="ByteArrayContent" />.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="content">The response body.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="configure">An action to further configure the response such as setting headers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="content" /> is null.</exception>
        public static ISetupSequentialResult<Task<HttpResponseMessage>> ReturnsResponse(
            this ISetupSequentialResult<Task<HttpResponseMessage>> setup,
            HttpStatusCode statusCode,
            byte[] content,
            string mediaType = null,
            Action<HttpResponseMessage> configure = null)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            return setup.ReturnsAsync(CreateResponse(
                statusCode: statusCode,
                content: new ByteArrayContent(content),
                mediaType: mediaType,
                configure: configure));
        }

        /// <summary>
        /// Specifies the response to return, as <see cref="ByteArrayContent" /> with <see cref="HttpStatusCode.OK" />.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="content">The response body.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="configure">An action to further configure the response such as setting headers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="content" /> is null.</exception>
        public static IReturnsResult<HttpMessageHandler> ReturnsResponse(
            this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup,
            byte[] content,
            string mediaType = null,
            Action<HttpResponseMessage> configure = null)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            return setup.ReturnsAsync((HttpRequestMessage request, CancellationToken _) =>
            {
                return CreateResponse(
                    request: request,
                    content: new ByteArrayContent(content),
                    mediaType: mediaType,
                    configure: configure);
            });
        }

        /// <summary>
        /// Specifies the response to return in sequence, as <see cref="ByteArrayContent" /> with <see
        /// cref="HttpStatusCode.OK" />.
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="content">The response body.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="configure">An action to further configure the response such as setting headers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="content" /> is null.</exception>
        public static ISetupSequentialResult<Task<HttpResponseMessage>> ReturnsResponse(
            this ISetupSequentialResult<Task<HttpResponseMessage>> setup,
            byte[] content,
            string mediaType = null,
            Action<HttpResponseMessage> configure = null)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            return setup.ReturnsAsync(CreateResponse(
                content: new ByteArrayContent(content),
                mediaType: mediaType,
                configure: configure));
        }

        /// <summary>
        /// Specifies the response to return, as <see cref="StreamContent" />. If the stream is seekable, it will be
        /// wrapped to allow for reuse across multiple requests (each request maintains an independent stream position).
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="content">The response body.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="configure">An action to further configure the response such as setting headers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="content" /> is null.</exception>
        public static IReturnsResult<HttpMessageHandler> ReturnsResponse(
            this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup,
            HttpStatusCode statusCode,
            Stream content,
            string mediaType = null,
            Action<HttpResponseMessage> configure = null)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            return setup.ReturnsAsync((HttpRequestMessage request, CancellationToken _) =>
            {
                return CreateResponse(
                    request: request,
                    statusCode: statusCode,
                    content: CreateStreamContent(content),
                    mediaType: mediaType,
                    configure: configure);
            });
        }

        /// <summary>
        /// Specifies the response to return in sequence, as <see cref="StreamContent" />. If the stream is seekable, it
        /// will be wrapped to allow for reuse across multiple requests (each request maintains an independent stream
        /// position).
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="content">The response body.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="configure">An action to further configure the response such as setting headers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="content" /> is null.</exception>
        public static ISetupSequentialResult<Task<HttpResponseMessage>> ReturnsResponse(
            this ISetupSequentialResult<Task<HttpResponseMessage>> setup,
            HttpStatusCode statusCode,
            Stream content,
            string mediaType = null,
            Action<HttpResponseMessage> configure = null)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            return setup.ReturnsAsync(CreateResponse(
                statusCode: statusCode,
                content: CreateStreamContent(content),
                mediaType: mediaType,
                configure: configure));
        }

        /// <summary>
        /// Specifies the response to return, as <see cref="StreamContent" /> with <see cref="HttpStatusCode.OK" />. If
        /// the stream is seekable, it will be wrapped to allow for reuse across multiple requests (each request
        /// maintains an independent stream position).
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="content">The response body.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="configure">An action to further configure the response such as setting headers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="content" /> is null.</exception>
        public static IReturnsResult<HttpMessageHandler> ReturnsResponse(
            this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> setup,
            Stream content,
            string mediaType = null,
            Action<HttpResponseMessage> configure = null)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            return setup.ReturnsAsync((HttpRequestMessage request, CancellationToken _) =>
            {
                return CreateResponse(
                    request: request,
                    content: CreateStreamContent(content),
                    mediaType: mediaType,
                    configure: configure);
            });
        }

        /// <summary>
        /// Specifies the response to return in sequence, as <see cref="StreamContent" /> with <see
        /// cref="HttpStatusCode.OK" />. If the stream is seekable, it will be wrapped to allow for reuse across
        /// multiple requests (each request maintains an independent stream position).
        /// </summary>
        /// <param name="setup">The setup.</param>
        /// <param name="content">The response body.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="configure">An action to further configure the response such as setting headers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="content" /> is null.</exception>
        public static ISetupSequentialResult<Task<HttpResponseMessage>> ReturnsResponse(
            this ISetupSequentialResult<Task<HttpResponseMessage>> setup,
            Stream content,
            string mediaType = null,
            Action<HttpResponseMessage> configure = null)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            return setup.ReturnsAsync(CreateResponse(
                content: CreateStreamContent(content),
                mediaType: mediaType,
                configure: configure));
        }

        private static StreamContent CreateStreamContent(Stream content) =>
            new StreamContent(content.CanSeek ? new ResponseStream(content) : content);
    }
}
