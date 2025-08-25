using BUAProgramv5.Logging;
using BUAProgramv5.Messaging;
using BUAProgramv5.Models.AD;
using BUAProgramv5.Models.Config;
using BUAProgramv5.Models.RestSharp;
using BUAProgramv5.Utilities;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.API.ClientWorker.Service
{
    internal class ClientWorker
    {
        private ILogger _logger;
        private BUAConfiguration _Api;

        internal ClientWorker(ILogger logger)
        {
            _logger = logger;
            _Api = Config.GetAPIInfo("API");
        }

        /// <summary>
        /// Gets the security groups based on a specific organization group(s).
        /// </summary>
        /// <param name="organizationGroups"></param>
        /// <returns></returns>
        internal ObservableCollection<ADObjectCheckList> GetListSecurityGroups(List<string> organizationGroups)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/GetListSecurityGroups")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                WrapperModel model = new WrapperModel { OrganizationalGroups = organizationGroups };
                string json = JsonConvert.SerializeObject(model, Formatting.Indented);
                request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);

                try
                {
                    IRestResponse response = client.Execute(request);
                    ObservableCollection<ADObjectCheckList> securityGroups = JsonConvert.DeserializeObject<ObservableCollection<ADObjectCheckList>>(response.Content);
                    return securityGroups;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all security groups in the organization.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        internal ObservableCollection<ADObjectCheckList> GetAllSecurityGroups()
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/GetAllSecurityGroups")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();

                try
                {
                    IRestResponse response = client.Execute(request);
                    ObservableCollection<ADObjectCheckList> securityGroups = JsonConvert.DeserializeObject<ObservableCollection<ADObjectCheckList>>(response.Content);
                    return securityGroups;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets specific security group within information based on the security group itsself and selected search filter.
        /// </summary>
        /// <param name="securityGroup"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        internal ObservableCollection<ADObjectCheckList> GetSingleSecurityGroup(string securityGroup, string filter)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/GetSingleSecurityGroup")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("securityGroup", securityGroup);
                request.AddParameter("filter", filter);

                try
                {
                    IRestResponse response = client.Execute(request);
                    ObservableCollection<ADObjectCheckList> securityGroups = JsonConvert.DeserializeObject<ObservableCollection<ADObjectCheckList>>(response.Content);
                    return securityGroups;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all distribution groups in the organization.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        internal Collection<PSObject> GetAllDistroGroups()
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/GetAllDistroGroups")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();

                try
                {
                    IRestResponse response = client.Execute(request);
                    Collection<PSObject> securityGroups = JsonConvert.DeserializeObject<Collection<PSObject>>(response.Content);
                    return securityGroups;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets single distribution group in the organization.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal Collection<PSObject> GetIndividualDistro(string name)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/GetSingleDistroGroup")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("name", name);

                try
                {
                    IRestResponse response = client.Execute(request);
                    Collection<PSObject> securityGroups = JsonConvert.DeserializeObject<Collection<PSObject>>(response.Content);
                    return securityGroups;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets SAM Account.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        internal string GetSAMAccountUserName(string userName)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/GetSamAccountUserName")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("userName", userName);

                try
                {
                    IRestResponse response = client.Execute(request);
                    string samAccount = JsonConvert.DeserializeObject<string>(response.Content);
                    return samAccount;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Checks to see if user is apart of AD group.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="securityGroup"></param>
        /// <returns></returns>
        internal bool GetUserADGroup(string userName, string securityGroup)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/IsUserInGroup")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("userName", userName);
                request.AddParameter("securityGroup", securityGroup);

                try
                {
                    IRestResponse response = client.Execute(request);
                    bool samAccount = JsonConvert.DeserializeObject<bool>(response.Content);
                    return samAccount;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Makes API call to determine if Username Exists.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        internal bool DoesUserNameExist(string userName)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/DoesUserNameExist")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("userName", userName);

                try
                {
                    IRestResponse response = client.Execute(request);
                    bool result = JsonConvert.DeserializeObject<bool>(response.Content);
                    return result;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if user account is locked.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        internal bool IsAccountLocked(string userName)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/IsAccountLocked")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("userName", userName);

                try
                {
                    IRestResponse response = client.Execute(request);
                    bool result = JsonConvert.DeserializeObject<bool>(response.Content);
                    return result;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Unlocks user account based on username.
        /// </summary>
        /// <param name="userName"></param>
        internal void UnlockAccount(string userName)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/UnlockAccount")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("userName", userName);

                try
                {
                    IRestResponse response = client.Execute(request);
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
        }

        /// <summary>
        /// Locks user account based on username.
        /// </summary>
        /// <param name="userName"></param>
        internal void LockAccount(string userName)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/LockAccount")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("userName", userName);

                try
                {
                    IRestResponse response = client.Execute(request);
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
        }

        /// <summary>
        /// Create NAS directory for new users.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        internal bool CreateNASFolder(string userName)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/CreateNasFolder")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("userName", userName);

                try
                {
                    IRestResponse response = client.Execute(request);
                    bool result = JsonConvert.DeserializeObject<bool>(response.Content);
                    return result;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Maps user drive letted on their AD object.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="driveLetter"></param>
        /// <returns></returns>
        internal bool MapDriveLetter(string user, string driveLetter)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/MapUserDriveLetter")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("user", user);
                request.AddParameter("driveLetter", driveLetter);

                try
                {
                    IRestResponse response = client.Execute(request);
                    bool result = JsonConvert.DeserializeObject<bool>(response.Content);
                    return result;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets AD user information by display name.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        internal User GetUserByDisplayName(string displayName)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/GetDisplayName")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("displayName", displayName);

                try
                {
                    IRestResponse response = client.Execute(request);
                    User adUser = JsonConvert.DeserializeObject<User>(response.Content);
                    return adUser;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Searches AD for user by Last Name.
        /// </summary>
        /// <param name="surname"></param>
        /// <returns></returns>
        internal List<ADPrincipalObject> SearchByLastName(string surname)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/SearchByLastName")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("surname", surname);

                try
                {
                    IRestResponse response = client.Execute(request);
                    List<ADPrincipalObject> adUser = JsonConvert.DeserializeObject<List<ADPrincipalObject>>(response.Content);
                    return adUser;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets All users from AD.
        /// </summary>
        /// <returns></returns>
        internal List<ADPrincipalObject> GetAllUsers()
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/GetAllADUsers")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();

                try
                {
                    IRestResponse response = client.Execute(request);
                    List<ADPrincipalObject> adUser = JsonConvert.DeserializeObject<List<ADPrincipalObject>>(response.Content);
                    return adUser;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all User OUs within AD.
        /// </summary>
        /// <returns></returns>
        internal List<string> GetUserOUs()
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/GetAllUserOUs")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();

                try
                {
                    IRestResponse response = client.Execute(request);
                    List<string> userOUs = JsonConvert.DeserializeObject<List<string>>(response.Content);
                    return userOUs;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Searches AD for specific user based on username.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        internal ADPrincipalObject SearchADByName(string userName)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/SearchADByName")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("userName", userName);

                try
                {
                    IRestResponse response = client.Execute(request);
                    ADPrincipalObject adUser = JsonConvert.DeserializeObject<ADPrincipalObject>(response.Content);
                    return adUser;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Moves users between AD groups.
        /// </summary>
        /// <param name="oldOU"></param>
        /// <param name="newOU"></param>
        internal void MoveOUs(string oldOU, string newOU)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/MoveOUs")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("oldOU", oldOU);
                request.AddParameter("newOU", newOU);

                try
                {
                    IRestResponse response = client.Execute(request);
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines if username is available when creating a new account.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        internal bool IsUserNameAvailable(string userName)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/IsUsernameAvailable")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("userName", userName);

                try
                {
                    IRestResponse response = client.Execute(request);
                    bool result = JsonConvert.DeserializeObject<bool>(response.Content);
                    return result;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Uses Powershell to get all secuirty groups in AD.
        /// </summary>
        /// <returns></returns>
        internal Collection<PSObject> GetSecurityGroups()
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/GetSecurityGroups")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();

                try
                {
                    IRestResponse response = client.Execute(request);
                    Collection<PSObject> result = JsonConvert.DeserializeObject<Collection<PSObject>>(response.Content);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Uses powershell to get all sites in AD.
        /// </summary>
        /// <returns></returns>
        internal Collection<PSObject> GetSites()
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/GetSites")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();

                try
                {
                    IRestResponse response = client.Execute(request);
                    Collection<PSObject> result = JsonConvert.DeserializeObject<Collection<PSObject>>(response.Content);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Verifies User account is in AD.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        internal bool LDAPAuthentication(string userName, string password)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/LDAPAuthentication")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("userName", userName);
                request.AddParameter("password", password);

                try
                {
                    IRestResponse response = client.Execute(request);
                    bool result = JsonConvert.DeserializeObject<bool>(response.Content);
                    return result;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Changes user password in AD.
        /// </summary>
        /// <param name="samAccountName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        internal bool ChangeUserPassword(string samAccountName, string password)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/ChangeUserPassword")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("samAccountName", samAccountName);
                request.AddParameter("password", password);

                try
                {
                    IRestResponse response = client.Execute(request);
                    bool result = JsonConvert.DeserializeObject<bool>(response.Content);
                    return result;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all Computers from Active Directory.
        /// </summary>
        /// <returns></returns>
        internal string[] GetComputersFromActiveDirectory()
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/GetAllComputers")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();

                try
                {
                    IRestResponse response = client.Execute(request);
                    string[] result = JsonConvert.DeserializeObject<string[]>(response.Content);
                    return result;

                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Disables AD User and removes their groups.
        /// </summary>
        /// <param name="user"></param>
        internal void DisableUserandRemoveGroups(User user)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/DisableUserAndGroups")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                WrapperModel model = new WrapperModel { User = user };
                string json = JsonConvert.SerializeObject(model, Formatting.Indented);
                request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);

                try
                {
                    IRestResponse response = client.Execute(request);
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
        }

        /// <summary>
        /// Reenables AD User.
        /// </summary>
        /// <param name="reEnableUser"></param>
        internal void ReEnableExistingUser(User reEnableUser)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/ReEnableExistingUser")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                WrapperModel model = new WrapperModel { User = reEnableUser };
                string json = JsonConvert.SerializeObject(model, Formatting.Indented);
                request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);

                try
                {
                    IRestResponse response = client.Execute(request);
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
        }

        /// <summary>
        /// Removes groups from specific User.
        /// </summary>
        /// <param name="userDistinguishedName"></param>
        /// <param name="groups"></param>
        internal void RemoveUserFromGroups(string userDistinguishedName, List<ADPrincipalObject> groups)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/RemoveUserFromGroups")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                WrapperModel model = new WrapperModel { UserDistinguishedName = userDistinguishedName, ADPrincipalObjectGroups = groups };
                string json = JsonConvert.SerializeObject(model, Formatting.Indented);
                request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);

                try
                {
                    IRestResponse response = client.Execute(request);
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
        }

        /// <summary>
        /// Removes current groups with current list of new groups.
        /// </summary>
        /// <param name="userDistinguishedName"></param>
        /// <param name="groups"></param>
        internal void ReplaceUsersCurrentGroupWithNewGroup(string userDistinguishedName, List<ADPrincipalObject> groups, User user)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/ReplaceUsersCurrentGroups")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                WrapperModel model = new WrapperModel { UserDistinguishedName = userDistinguishedName, ADPrincipalObjectGroups = groups, User = user};
                string json = JsonConvert.SerializeObject(model, Formatting.Indented);
                request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);

                try
                {
                    IRestResponse response = client.Execute(request);
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates User job description in AD.
        /// </summary>
        /// <param name="userDistinguishedName"></param>
        /// <param name="jobDescription"></param>
        internal void UpdateUserJobDescription(string userDistinguishedName, string jobDescription)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/UpdateJobDescription")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("userDistinguishedName", userDistinguishedName);
                request.AddParameter("jobDescription", jobDescription);

                try
                {
                    IRestResponse response = client.Execute(request);
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates User site information in AD.
        /// </summary>
        /// <param name="userDistinguishedName"></param>
        /// <param name="phone"></param>
        /// <param name="office"></param>
        /// <param name="managerDistinguishedName"></param>
        internal void UpdateUserSiteInfo(string userDistinguishedName, string phone, string office, string managerDistinguishedName)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/UpdateSiteInfo")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("userDistinguishedName", userDistinguishedName);
                request.AddParameter("phone", phone);
                request.AddParameter("office", office);
                request.AddParameter("managerDistinguishedName", managerDistinguishedName);

                try
                {
                    IRestResponse response = client.Execute(request);
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates new User account in AD.
        /// </summary>
        /// <param name="user"></param>
        internal void CreateNewUser(User user)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/CreateUser")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                WrapperModel model = new WrapperModel { User = user };
                string json = JsonConvert.SerializeObject(model, Formatting.Indented);
                request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);

                try
                {
                    IRestResponse response = client.Execute(request);
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
        }

        /// <summary>
        /// Adds groups to specific User in AD.
        /// </summary>
        /// <param name="user"></param>
        internal void AddUserToGroups(User user)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/AddUserToGroups")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                WrapperModel model = new WrapperModel { User = user };
                string json = JsonConvert.SerializeObject(model, Formatting.Indented);
                request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);

                try
                {
                    IRestResponse response = client.Execute(request);
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
        }

        /// <summary>
        /// Moves newly disabled user accounts to disabled OU's. Helps SA's out from manually running processes.
        /// </summary>
        /// <param name="oldOU"></param>
        internal void MoveUsersToDisabledOU(string oldOU)
        {
            string token = string.Empty;
            token = Authentication();
            if (!string.IsNullOrEmpty(token))
            {
                RestClient client = new RestClient(string.Format(@"{0}/{1}", _Api.RootURL, "api/SMGAD/MoveUserToDisabledOU")) { CookieContainer = new CookieContainer() };
                client.Authenticator = new JwtAuthenticator(token);
                RestRequest request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddParameter("oldOU", oldOU);

                try
                {
                    IRestResponse response = client.Execute(request);
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErrorMessages.APICallError, ex.StackTrace, ex.Message));
                    throw;
                }
            }
        }

        /// <summary>
        /// Does authentication to connect to api.
        /// </summary>
        /// <returns></returns>
        private string Authentication()
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                ((sender, certificate, chain, sslPolicyErrors) => true);
                RestClient client = new RestClient(_Api.AuthURL) { CookieContainer = new CookieContainer() };
                RestRequest request = new RestRequest(Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.Parameters.Clear();
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("grant_type", "password");
                request.AddParameter("username", _Api.User);
                request.AddParameter("password", _Api.Password);

                IRestResponse response = client.Execute(request);
                string result = string.Empty;
                result = JsonConvert.DeserializeObject<ApiAuthentication>(response.Content).access_token;

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AuthenticationError, ex.StackTrace, ex.Message));
                throw;
            }
        }
    }
}
