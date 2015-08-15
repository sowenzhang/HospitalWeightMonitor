using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Hospital.Web.Common.ActionResults
{
    public class UpdatedActionResult<T> : IHttpActionResult
    {
        private readonly NegotiatedContentResult<T> _innerResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatedActionResult{T}"/> class.
        /// </summary>
        /// <param name="entity">The updated entity.</param>
        /// <param name="controller">The controller from which to obtain the dependencies needed for execution.</param>
        public UpdatedActionResult(T entity, ApiController controller)
            : this(new NegotiatedContentResult<T>(HttpStatusCode.OK, CheckNull(entity), controller))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatedActionResult{T}"/> class.
        /// </summary>
        /// <param name="entity">The updated entity.</param>
        /// <param name="contentNegotiator">The content negotiator to handle content negotiation.</param>
        /// <param name="request">The request message which led to this result.</param>
        /// <param name="formatters">The formatters to use to negotiate and format the content.</param>
        public UpdatedActionResult(T entity, IContentNegotiator contentNegotiator, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
            : this(new NegotiatedContentResult<T>(HttpStatusCode.OK, CheckNull(entity), contentNegotiator, request, formatters))
        {
        }

        private UpdatedActionResult(NegotiatedContentResult<T> innerResult)
        {
            Contract.Assert(innerResult != null);
            _innerResult = innerResult;
        }

        /// <summary>
        /// Gets the entity that was updated.
        /// </summary>
        public T Entity
        {
            get
            {
                return _innerResult.Content;
            }
        }

        /// <summary>
        /// Gets the content negotiator to handle content negotiation.
        /// </summary>
        public IContentNegotiator ContentNegotiator
        {
            get
            {
                return _innerResult.ContentNegotiator;
            }
        }

        /// <summary>
        /// Gets the request message which led to this result.
        /// </summary>
        public HttpRequestMessage Request
        {
            get
            {
                return _innerResult.Request;
            }
        }

        /// <summary>
        /// Gets the formatters to use to negotiate and format the content.
        /// </summary>
        public IEnumerable<MediaTypeFormatter> Formatters
        {
            get
            {
                return _innerResult.Formatters;
            }
        }

        /// <inheritdoc/>
        public virtual async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            IHttpActionResult result = GetInnerActionResult();
            return await result.ExecuteAsync(cancellationToken);
        }

        internal IHttpActionResult GetInnerActionResult()
        {
            if (RequestPrefersReturnContent(_innerResult.Request))
            {
                return _innerResult;
            }

            return new StatusCodeResult(HttpStatusCode.NoContent, _innerResult.Request);
        }

        private const string PreferHeaderName = "Prefer";
        private const string ReturnContentHeaderValue = "return-content";
        private static bool RequestPrefersReturnContent(HttpRequestMessage request)
        {
            IEnumerable<string> preferences;
            if (request.Headers.TryGetValues(PreferHeaderName, out preferences))
            {
                return preferences.Contains(ReturnContentHeaderValue);
            }
            return false;
        }

        private static T CheckNull(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            return entity;
        }
    }
}
