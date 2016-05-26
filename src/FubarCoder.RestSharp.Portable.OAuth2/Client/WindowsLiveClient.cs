using Newtonsoft.Json.Linq;
using RestSharp.Portable.OAuth2.Configuration;
using RestSharp.Portable.OAuth2.Infrastructure;
using RestSharp.Portable.OAuth2.Models;
using RestSharp.Portable;

namespace RestSharp.Portable.OAuth2.Client
{
    /// <summary>
    /// Windows Live authentication client.
    /// </summary>
    public class WindowsLiveClient : OAuth2Client
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsLiveClient"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="configuration">The configuration.</param>
        public WindowsLiveClient(IRequestFactory factory, IClientConfiguration configuration)
            : base(factory, configuration)
        {
        }

        /// <summary>
        /// Defines URI of service which issues access code.
        /// </summary>
        protected override Endpoint AccessCodeServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = "https://login.live.com",
                    Resource = "/oauth20_authorize.srf"
                };
            }
        }

        /// <summary>
        /// Defines URI of service which issues access token.
        /// </summary>
        protected override Endpoint AccessTokenServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = "https://login.live.com",
                    Resource = "/oauth20_token.srf"
                };
            }
        }

        /// <summary>
        /// Defines URI of service which allows to obtain information about user which is currently logged in.
        /// </summary>
        protected override Endpoint UserInfoServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = "https://apis.live.net/v5.0",
                    Resource = "/me"
                };
            }
        }

        /// <summary>
        /// Called just before issuing request to third-party service when everything is ready.
        /// Allows to add extra parameters to request or do any other needed preparations.
        /// </summary>
        protected override void BeforeGetUserInfo(BeforeAfterRequestArgs args)
        {
            args.Request.AddOrUpdateParameter("access_token", AccessToken);
        }

        /// <summary>
        /// Should return parsed <see cref="UserInfo"/> from content received from third-party service.
        /// </summary>
        /// <param name="response">The response which is received from the provider.</param>
        protected override UserInfo ParseUserInfo(IRestResponse response)
        {
            var info = JObject.Parse(response.Content);
            const string avatarUriTemplate = @"https://cid-{0}.users.storage.live.com/users/0x{0}/myprofile/expressionprofile/profilephoto:Win8Static,{1},UserTileStatic/MeControlXXLUserTile?ck=2&ex=24";
            return new UserInfo
            {
                Id = info["id"].Value<string>(),
                FirstName = info["first_name"].Value<string>(),
                LastName = info["last_name"].Value<string>(),
                Email = info["emails"]["preferred"].SafeGet(x => x.Value<string>()),
                AvatarUri =
                    {
                        Small = string.Format(avatarUriTemplate, info["id"].Value<string>(), "UserTileSmall"),
                        Normal = string.Format(avatarUriTemplate, info["id"].Value<string>(), "UserTileSmall"),
                        Large = string.Format(avatarUriTemplate, info["id"].Value<string>(), "UserTileLarge")
                    }
            };
        }

        /// <summary>
        /// Friendly name of provider (OAuth2 service).
        /// </summary>
        public override string Name
        {
            get { return "WindowsLive"; }
        }
    }
}