using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Unicorn.Backend.Services.RestService;
using Unicorn.Taf.Core.Utility;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Backend.Matchers.RestMatchers
{
    /// <summary>
    /// Matcher to check if REST service JSON response has list of tokens matching specified 
    /// JSONPath with expected values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HasTokensListWithValuesMatcher<T> : TypeSafeMatcher<RestResponse>
    {
        private readonly string _jsonPath;
        private readonly IList<T> _tokensValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="HasTokensListWithValuesMatcher{T}"/> class with JSONPath and token values list.
        /// </summary>
        /// <param name="jsonPath">JSONPath to search for tokens</param>
        /// <param name="tokensValues">expected token value</param>
        public HasTokensListWithValuesMatcher(string jsonPath, IList<T> tokensValues)
        {
            _jsonPath = jsonPath;
            _tokensValues = tokensValues;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HasTokensListWithValuesMatcher{T}"/> class with JSONPath and tokens values list.
        /// </summary>
        /// <param name="jsonPath">JSONPath to search for tokens</param>
        /// <param name="tokensValues">expected token value</param>
        public HasTokensListWithValuesMatcher(string jsonPath, params T[] tokensValues)
            : this(jsonPath, tokensValues.ToList())
        {
        }

        /// <summary>
        /// Gets verification description.
        /// </summary>
        public override string CheckDescription =>
            $"has list of tokens '{_jsonPath}' with values '[{string.Join(",", _tokensValues)}]'";

        /// <summary>
        /// Checks if target <see cref="RestResponse"/> matches condition corresponding to specific matcher implementations.
        /// </summary>
        /// <param name="actual">REST response under assertion</param>
        /// <returns>true - if object matches specific condition; otherwise - false</returns>
        public override bool Matches(RestResponse actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            JToken response;

            if (actual.Content.StartsWith("["))
            {
                response = JArray.Parse(actual.Content);
            }
            else
            {
                response = actual.AsJObject;
            }

            IEnumerable<T> castedValues = response.SelectTokens(_jsonPath).Select(t => t.Value<T>());

            CollectionsComparer<T> comparer = new CollectionsComparer<T>()
                .TrimOutputTo(1000)
                .UseItemsBulletsInOutput(">");

            bool result = comparer.AreTheSame(castedValues, _tokensValues);
            DescribeMismatch(Environment.NewLine + comparer.Output);

            return result;
        }
    }
}
