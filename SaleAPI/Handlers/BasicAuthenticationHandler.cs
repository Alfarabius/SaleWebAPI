using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SaleAPI.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            ) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() // ToDo learn more about async problem
        {
            string userName = null;
            try
            {
                var header = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(header.Parameter)).Split(':');
                userName = credentials.FirstOrDefault();
                var password = credentials.LastOrDefault();

                if (userName != Config.Name || password != Config.Password) 
                {
                    throw new ArgumentException("Invalid credentials");
                }                
            }
            catch (Exception ex) 
            {
                return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
            }

            var claims = new[] {
                new Claim(ClaimTypes.Name, userName)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
           
            return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
        }
    }
}
