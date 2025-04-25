using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Unicorn.Backend.Utils
{
    /// <summary>
    /// Utility for building URLs from parts with ability to add url encoded parameters.
    /// </summary>
    public class UrlBuilder
    {
        private readonly StringBuilder _urlSb;
        private bool hasAnyParameters = false;

        /// <summary>
        /// Initializes a new instance of <see cref="UrlBuilder"/> with base URL.
        /// </summary>
        /// <param name="basePath">base url string</param>
        public UrlBuilder(string basePath)
        {
            _urlSb = new StringBuilder();
            _urlSb.Append(basePath.TrimEnd('/'));
        }

        /// <summary>
        /// Appends new part (url encoded) to existing URL. All slashes are handled automatically.
        /// </summary>
        /// <param name="urlPart">url part to append</param>
        /// <returns>this instance</returns>
        public UrlBuilder AppendUrl(string urlPart)
        {
            _urlSb.Append('/').Append(HttpUtility.UrlEncode(urlPart.Trim('/')));
            return this;
        }

        /// <summary>
        /// Appends url encoded key-value pair for specified parameter to the URL.
        /// </summary>
        /// <param name="key">parameter key</param>
        /// <param name="value">parameter value</param>
        /// <returns>this instance</returns>
        public UrlBuilder AppendParam(string key, object value)
        {
            AppendParamsSectionStartIfNotExists();
            AppendAndIfNeeded();
            _urlSb.Append(key).Append('=').Append(HttpUtility.UrlEncode(value.ToString()));

            return this;
        }

        /// <summary>
        /// Appends url encoded key-value pairs for specified parameter to the URL.
        /// </summary>
        /// <param name="key">parameter key</param>
        /// <param name="values">parameter values list</param>
        /// <returns>this instance</returns>
        public UrlBuilder AppendParams(string key, IEnumerable values)
        {
            AppendParamsSectionStartIfNotExists();
            AppendAndIfNeeded();

            int cnt = 0;

            foreach (object obj in values)
            {
                if (cnt++ > 0)
                {
                    _urlSb.Append("&");
                }

                _urlSb.Append(key).Append('=').Append(HttpUtility.UrlEncode(obj.ToString()));
            }

            return this;
        }

        /// <summary>
        /// Appends dictionary with parameters in form of key-value pairs (url encoded) for specified parameter to the URL.
        /// </summary>
        /// <param name="parameters">dictionary of parameters</param>
        /// <returns>this instance</returns>
        public UrlBuilder AppendParams(Dictionary<string, object> parameters)
        {
            foreach (KeyValuePair<string, object> pair in parameters)
            {
                if (pair.Value is IEnumerable enumerable)
                {
                    AppendParams(pair.Key, enumerable);
                }
                else
                {
                    AppendParam(pair.Key, pair.Value);
                }
            }

            return this;
        }

        /// <summary>
        /// Builds URL string.
        /// </summary>
        /// <returns>URL string</returns>
        public string Build() =>
            _urlSb.ToString();

        /// <summary>
        /// Gets URL string.
        /// </summary>
        /// <returns>URL string</returns>
        public override string ToString() =>
            _urlSb.ToString();

        private void AppendParamsSectionStartIfNotExists()
        {
            if (!hasAnyParameters)
            {
                _urlSb.Append("?");
            }
        }

        private void AppendAndIfNeeded()
        {
            if (hasAnyParameters)
            {
                _urlSb.Append("&");
            }
            else
            {
                hasAnyParameters = true;
            }
        }
    }
}
