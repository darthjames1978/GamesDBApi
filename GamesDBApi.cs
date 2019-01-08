// Licensed under the MIT license

using System;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace GamesDBApi
{
    /// <summary>
    /// Class using TheGamesDBAPI V2
    /// </summary>
    public class GamesDBApi
    {
        /// <summary>
        /// TheGamesDB Public API Key. This must be unique to your application
        /// </summary>
        protected string ApiKey = "";

        /// <summary>
        /// Private field for singleton
        /// </summary>
        private static GamesDBApi _instance;

        /// <summary>
        /// Gets the public singleton property
        /// </summary>
        public static GamesDBApi Instance => _instance ?? (_instance = new GamesDBApi());

        /// <summary>
        /// Gets or sets a value indicating whether the API is initialized
        /// </summary>
        protected bool _isInitialized;

        /// <summary>
        /// HttpClient for communicating over the wire
        /// </summary>
        private HttpClient _httpClient;

        /// <summary>
        /// Base API Endpoint
        /// </summary>
        private static readonly string _BASE_URL = "https://api.thegamesdb.net/";

        /// <summary>
        /// ByGameName API Endpoint
        /// </summary>
        private static readonly string _GAMES_BY_GAME_NAME = "Games/ByGameName?";

        /// <summary>
        /// APIKEY querystring
        /// </summary>
        private static readonly string _API_KEY_FMT = "apikey={0}";

        /// <summary>
        /// Name querystring
        /// </summary>
        private static readonly string _NAME_FMT = "&name={0}";

        /// <summary>
        /// Platform filter querystring
        /// </summary>
        private static readonly string _PLATFORM_FMT = "&filter[platform]={0}";

        /// <summary>
        /// Optional fields for Games/ByGameName API
        /// </summary>
        private static readonly string _GAMES_BY_GAME_NAME_OPTIONAL_FIELDS = "&include=boxart,platform&fields=players,publishers,genres,overview,platform,alternates";

        /// <summary>
        /// Initializes GamesDBApi
        /// </summary>
        /// <param name="apiKey">The API Key to use</param>
        /// <returns>True or false</returns>
        public bool Initialize(string apiKey)
        {
            if (!String.IsNullOrEmpty(apiKey))
            {
                ApiKey = apiKey;
                _isInitialized = true;
            }
            else
            {
                throw new ArgumentException("API Key must not be null");
            }

            _httpClient = new HttpClient();

            return _isInitialized;
        }

        /// <summary>
        /// IsInitialized method
        /// </summary>
        /// <returns>True or False</returns>
        public bool IsInitialized()
        {
            return _isInitialized;
        }

        /// <summary>
        /// Invokes the Games/ByGameName API
        /// </summary>
        /// <param name="name">Game Name to search for</param>
        /// <param name="platform">Platform ID to filter</param>
        /// <param name="includeOptional">Include optional parameters</param>
        /// <returns>ByGameName result</returns>
        public async Task<ByGameName> GetByGameNameAsync(string name, int platform = -1, bool includeOptional = true)
        {
            if (!IsInitialized())
            {
                throw new InvalidOperationException("API is not initialized");
            }

            var uriString = new StringBuilder();
            uriString.Append(_BASE_URL);
            uriString.Append(_GAMES_BY_GAME_NAME);
            uriString.Append(String.Format(_API_KEY_FMT, ApiKey));
            uriString.Append(String.Format(_NAME_FMT, name));

            if (includeOptional)
            {
                uriString.Append(_GAMES_BY_GAME_NAME_OPTIONAL_FIELDS);
            }

            if (platform != -1)
            {
                uriString.Append(String.Format(_PLATFORM_FMT, platform));
            }

            var uri = new Uri(uriString.ToString());

            var jsonResult = await _httpClient.GetStringAsync(uri);

            return ByGameName.FromJson(jsonResult);
        }
    }
}
