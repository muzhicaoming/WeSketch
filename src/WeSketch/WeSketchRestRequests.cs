using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using WeSketchSharedDataModels;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WeSketch
{
    /// <summary>
    /// Sends all rest requests to WeSketchAPI.
    /// </summary>
    class WeSketchRestRequests : IDisposable
    {
#if DEBUG
        private string _url = ConfigurationManager.AppSettings["debugUrl"];
#else
        private string _url = ConfigurationManager.AppSettings["url"];
#endif

        private HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Logins the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        public async Task<User> Login(string user, string password)
        {
            var data = new Dictionary<string, string>
            {
                { "user", user },
                { "password", password }
            };
            var dataFormContent = new FormUrlEncodedContent(data);
            HttpResponseMessage postResult = await _httpClient.PostAsync($"{_url}Login", dataFormContent);
            if (postResult.IsSuccessStatusCode)
            {
                Result result = JsonConvert.DeserializeObject<Result>(await postResult.Content.ReadAsStringAsync());

                if (!result.Error)
                {
                    return JsonConvert.DeserializeObject<User>(result.ResultJSON);
                }
                else
                {
                    throw new Exception(result.ErrorMessage);
                }
            }
            else
            {
                throw new Exception($"Error:{postResult.StatusCode}-{postResult.ReasonPhrase}");
            }
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        public async Task<User> CreateUser(string user, string password)
        {
            // TODO: Add code to send post request to create user.
            var data = new Dictionary<string, string>
            {
                { "user", user },
                { "password", password }
            };
            var dataFormContent = new FormUrlEncodedContent(data);
            HttpResponseMessage postResult = await _httpClient.PostAsync($"{_url}CreateUser", dataFormContent);
            if (postResult.IsSuccessStatusCode)
            {
                Result result = JsonConvert.DeserializeObject<Result>(await postResult.Content.ReadAsStringAsync());

                if (!result.Error)
                {
                    return JsonConvert.DeserializeObject<User>(result.ResultJSON);
                }
                else
                {
                    throw new Exception(result.ErrorMessage);
                }
            }
            else
            {
                throw new Exception($"Error:{postResult.StatusCode}-{postResult.ReasonPhrase}");
            }
        }

        /// <summary>
        /// Joins the board.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="hostUser">The host user.</param>
        /// <exception cref="Exception"></exception>
        public async Task<Board> JoinBoard(string user, string hostUser)
        {
            var data = new Dictionary<string, string>
            {
                { "user", user },
                { "hostUser", hostUser }
            };
            var dataFormContent = new FormUrlEncodedContent(data);
            HttpResponseMessage postResult = await _httpClient.PostAsync($"{_url}JoinBoard", dataFormContent);
            if (postResult.IsSuccessStatusCode)
            {
                Result result = JsonConvert.DeserializeObject<Result>(await postResult.Content.ReadAsStringAsync());

                if (!result.Error)
                {
                    return JsonConvert.DeserializeObject<Board>(result.ResultJSON);
                }
                else
                {
                    throw new Exception(result.ErrorMessage);
                }
            }
            else
            {
                throw new Exception($"Error:{postResult.StatusCode}-{postResult.ReasonPhrase}");
            }
        }

        /// <summary>
        /// Invites the user to board.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="boardId">The board identifier.</param>
        public void InviteUserToBoard(string user, Guid boardId)
        {
            // TODO: Add code to send post request to server to send an invitation to the specified user.
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _httpClient.Dispose();
            _httpClient = null;
        }
    }
}
