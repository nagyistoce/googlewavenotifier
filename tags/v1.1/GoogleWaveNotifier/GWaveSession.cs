using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using ManagedHttp.IO;
using ManagedHttp.Net.Client;

namespace GoogleWaveNotifier
{
    public class GWaveSession
    {
        public ICookieContainer Cookies { get; set; }
        public bool IsAuthenticated { get; set; }

        public string AuthenticationToken { get; private set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public GWaveSession()
        {
            AuthenticationToken = null;
            Cookies = CreateCookieContainer();
        }

        public IHttpClient CreateClient()
        {
            return new HttpClient
                       {
                           CookieContainer = Cookies,
                           CookieSerializer = new CookieSerializer()
                       };
        }

        public ICookieContainer CreateCookieContainer()
        {
            return new SimpleCookieContainer();
        }

        public static string Login(IHttpClient client, string email, string password)
        {
            var clientLoginUri = new Uri("https://www.google.com/accounts/ClientLogin");
            var response = client.PostForm(clientLoginUri,
                                           new KeyValuePair<string, string>("accountType", "GOOGLE"),
                                           new KeyValuePair<string, string>("Email", email),
                                           new KeyValuePair<string, string>("Passwd", password),
                                           new KeyValuePair<string, string>("service", "wave"),
                                           new KeyValuePair<string, string>("source", "googlewavenotifier"));
            if (response.Header.StatusCode == 403)
                throw new AuthenticationException("Invalid username or password.");
            var responseText = response.Body.ReadAllText();
            string auth = Regex.Match(responseText, "Auth=([A-z0-9_-]+)").Groups[1].Value;
            if (string.IsNullOrEmpty(auth))
                throw new AuthenticationException("Could not login.");
            return auth;
        }

        public void Use(Action<IHttpClient, string> action)
        {
            for (int i = 0; i < 2; i++)
            {
                using (var client = CreateClient())
                {
                    if (!IsAuthenticated)
                    {
                        try
                        {
                            AuthenticationToken = Login(client, Email, Password);
                        }
                        catch (AuthenticationException e)
                        {
                            AuthenticationToken = null;
                            OnAuthenticationFailed(new ExceptionEventArgs(e));
                            throw new AuthenticationException("The specified email or password is invalid.", e);
                        }
                    }
                    try
                    {
                        action(client, AuthenticationToken);
                    }
                    catch (AuthenticationException)
                    {
                        // The cookies could be expired.
                        IsAuthenticated = false;
                        AuthenticationToken = null;
                        Cookies = CreateCookieContainer();

                        // Retry.
                        continue;
                    }

                    // I've used the authenticationtoken and am logged in. I now can not use the token anymore, but can use my cookies.
                    AuthenticationToken = null;
                    if (!IsAuthenticated)
                    {
                        IsAuthenticated = true;
                        OnAuthenticated(EventArgs.Empty);
                    }
                    //Done.
                    break;
                }
            }
        }

        #region Authenticated Event

        protected virtual void OnAuthenticated(EventArgs e)
        {
            if (Authenticated == null)
                return;
            Authenticated(this, e);
        }

        public event EventHandler<EventArgs> Authenticated;

        #endregion

        #region AuthenticationFailed Event

        protected virtual void OnAuthenticationFailed(ExceptionEventArgs e)
        {
            if (AuthenticationFailed == null)
                return;
            AuthenticationFailed(this, e);
        }

        public event EventHandler<ExceptionEventArgs> AuthenticationFailed;

        #endregion
    }
}