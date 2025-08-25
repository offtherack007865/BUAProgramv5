using BUAProgramv5.Logging;
using BUAProgramv5.Models.Config;
using BUAProgramv5.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using BUAProgramv5.Models.AD;
using BUAProgramv5.Models.Enum.Azure;
using System.Management.Automation.Runspaces;
using Microsoft.Open.AzureAD.Model;
using System.Windows;
using BUAProgramv5.Messaging;

namespace BUAProgramv5.ServerFunctions.Azure
{
    //These are Azure 2.0 powershell commands to do basic CRUD functions in 365.
    public class AzureWorker : IAzureWorker
    {
        private ILogger _logger;
        private BUAConfiguration _office365;

        public AzureWorker(ILogger logger)
        {
            _logger = logger;
            _office365 = Config.GetAuthenticationInfo("Office365");
        }

        /// <summary>
        /// Creates user in Azure AD.
        /// </summary>
        /// <param name="user"></param>
        public void CreateUserAccount(Models.AD.User user)
        {
            try
            {
                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "AzureAD" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-AzureAD");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));
                Command getUserCommand = new Command("New-AzureADUser");
                getUserCommand.Parameters.Add((new CommandParameter("AccountEnabled", true)));
                getUserCommand.Parameters.Add((new CommandParameter("PasswordProfile", SetAzurePassword(user.UserPassword))));
                getUserCommand.Parameters.Add((new CommandParameter("DisplayName", user.DisplayName)));
                getUserCommand.Parameters.Add((new CommandParameter("MailNickName", user.Username)));
                getUserCommand.Parameters.Add((new CommandParameter("JobTitle", user.JobDescription)));
                getUserCommand.Parameters.Add((new CommandParameter("GivenName", user.FirstName)));
                getUserCommand.Parameters.Add((new CommandParameter("SurName", user.LastName)));
                getUserCommand.Parameters.Add((new CommandParameter("Department", user.Department)));
                getUserCommand.Parameters.Add((new CommandParameter("PhysicalDeliveryOfficeName", user.SiteName)));
                getUserCommand.Parameters.Add((new CommandParameter("UserPrincipalName", user.Username + GeneralMessages.CompanyEmail)));
                getUserCommand.Parameters.Add((new CommandParameter("UsageLocation", "US")));
                //getUserCommand.Parameters.Add((new CommandParameter("LicenseAssignment", GeneralMessages.LicenseAssignment)));

                using (Runspace psRunSpace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    psRunSpace.Open();
                    foreach (Command com in new Command[] { connectCommand, getUserCommand })
                    {
                        Pipeline pipe = psRunSpace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();

                        if (error.Count > 0 && com == connectCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }

                        if (error.Count > 0 && com == getUserCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }
                    }
                    psRunSpace.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureCreateUserError, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Gets Azure AD users by selected total.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Collection<PSObject> Get365Users(int amount)
        {
            try
            {
                Collection<PSObject> userList = null;

                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "AzureAD" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-AzureAD");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));
                Command getUserCommand = new Command("Get-AzureADUser");
                getUserCommand.Parameters.Add((new CommandParameter("All", amount)));

                using (Runspace psRunSpace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    psRunSpace.Open();

                    foreach (Command com in new Command[] { connectCommand, getUserCommand })
                    {
                        Pipeline pipe = psRunSpace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();

                        if (error.Count > 0 && com == connectCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                            return null;
                        }

                        if (error.Count > 0 && com == getUserCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                            return null;
                        }

                        else
                        {
                            userList = results;
                        }
                    }
                    psRunSpace.Close();
                }
                return userList;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureGetAllUsersError, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Sets User values in Azure AD.
        /// </summary>
        /// <param name="user"></param>
        public void PopulateUserDetails(Models.AD.User user)
        {
            try
            {
                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "AzureAD" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-AzureAD");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

                Command userCommand = new Command("Set-AzureADUser");
                userCommand.Parameters.Add((new CommandParameter("ObjectId", user.Username + GeneralMessages.CompanyEmail)));
                userCommand.Parameters.Add((new CommandParameter("DisplayName", user.DisplayName)));
                userCommand.Parameters.Add((new CommandParameter("MailNickName", user.Username)));
                userCommand.Parameters.Add((new CommandParameter("JobTitle", user.JobDescription)));
                userCommand.Parameters.Add((new CommandParameter("GivenName", user.FirstName)));
                userCommand.Parameters.Add((new CommandParameter("SurName", user.LastName)));
                userCommand.Parameters.Add((new CommandParameter("Department", user.Department)));
                userCommand.Parameters.Add((new CommandParameter("PhysicalDeliveryOfficeName", user.SiteName)));
                userCommand.Parameters.Add((new CommandParameter("TelephoneNumber", user.PhoneNumber)));
                using (Runspace psRunSpace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    psRunSpace.Open();

                    foreach (Command com in new Command[] { connectCommand, userCommand })
                    {
                        Pipeline pipe = psRunSpace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();
                        if (error.Count > 0 && com == userCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                            return;
                        }
                    }
                    psRunSpace.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzurePopulateUserDetails, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Gets Azure 365 Licenses.
        /// </summary>
        /// <returns></returns>
        public Collection<PSObject> Get365Licenses()
        {
            try
            {
                Collection<PSObject> licenses = null;

                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "AzureAD" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-AzureAD");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

                Command userCommand = new Command("Get-AzureADSubscribedSku");
                using (Runspace runspace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    runspace.Open();
                    foreach (Command com in new Command[] { connectCommand, userCommand })
                    {
                        Pipeline pipe = runspace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();

                        if (error.Count > 0 && com == connectCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                            return null;
                        }

                        if (error.Count > 0 && com == userCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                            return null;
                        }
                        else
                        {
                            licenses = results;
                        }
                    }
                }
                return licenses;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureFetchLicensesError, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Removes Azure Office License from User.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="versionEnum"></param>
        public void RemoveLicensesFromUsers(Models.AD.User user, VersionEnum versionEnum)
        {
            try
            {
                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "AzureAD" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-AzureAD");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

                Command userCommand = new Command("Set-AzureADUserLicense");
                userCommand.Parameters.Add("AssignedLicenses", AssignedLicense(user, AzureLicense.Remove, versionEnum));
                userCommand.Parameters.Add("ObjectId", GetUserObjectID(user));

                using (Runspace runspace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    runspace.Open();
                    foreach (Command com in new Command[] { connectCommand, userCommand })
                    {
                        Pipeline pipe = runspace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();

                        if (error.Count > 0 && com == connectCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }

                        if (error.Count > 0 && com == userCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }
                    }
                    runspace.Close();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureRemoveLicenseError, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Assigns E3 license to Azure AD user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="versionEnum"></param>
        public void AssignLicensesToUsers(Models.AD.User user, VersionEnum versionEnum)
        {
            try
            {
                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "AzureAD" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-AzureAD");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

                Command userCommand = new Command("Set-AzureADUserLicense");
                userCommand.Parameters.Add("AssignedLicenses", AssignedLicense(user, AzureLicense.Add, versionEnum));
                userCommand.Parameters.Add("ObjectId", GetUserObjectID(user));

                using (Runspace runspace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    runspace.Open();
                    foreach (Command com in new Command[] { connectCommand, userCommand })
                    {
                        Pipeline pipe = runspace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();

                        if (error.Count > 0 && com == connectCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }

                        if (error.Count > 0 && com == userCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }
                    }
                    runspace.Close();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureAddLicenseError, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Deletes User Account in Azure AD.
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUserAccount(Models.AD.User user)
        {
            try
            {
                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "AzureAD" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-AzureAD");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

                Command userCommand = userCommand = new Command("Remove-AzureADUser");
                userCommand.Parameters.Add("ObjectId", GetUserObjectID(user));

                using (Runspace runspace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    runspace.Open();
                    foreach (Command com in new Command[] { connectCommand, userCommand })
                    {
                        Pipeline pipe = runspace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();

                        if (error.Count > 0 && com == connectCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }

                        if (error.Count > 0 && com == userCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }
                    }
                    runspace.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureDeleteUserError, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Gets deleted users from Azure AD.  Warning, this is PS 1.0, they removed this functionality in 2.0.
        /// </summary>
        /// <returns></returns>
        public Collection<PSObject> GetDeletedUsers()
        {
            try
            {
                Collection<PSObject> deletedUserList = null;

                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "MSOnline" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-MsolService");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

                Command userCommand = userCommand = new Command("Get-MsolUser");
                userCommand.Parameters.Add("ReturnDeletedUsers");

                using (Runspace runspace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    runspace.Open();
                    foreach (Command com in new Command[] { connectCommand, userCommand })
                    {
                        Pipeline pipe = runspace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();

                        if (error.Count > 0 && com == connectCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }

                        if (error.Count > 0 && com == userCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }
                        else
                        {
                            deletedUserList = results;
                        }
                    }
                    runspace.Close();
                }

                return deletedUserList;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureGetDeletedUsersError, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Restores Azure AD user. Warning, this is PS 1.0, they removed this functionality in 2.0.
        /// </summary>
        /// <param name="user"></param>
        public void RestoreUsers(Models.AD.User user)
        {
            try
            {
                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "MSOnline" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-MsolService");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

                Command userCommand = new Command("Restore-MsolUser");
                userCommand.Parameters.Add("UserPrincipalName", user.EmailAddress);

                using (Runspace runspace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    runspace.Open();
                    foreach (Command com in new Command[] { connectCommand, userCommand })
                    {
                        Pipeline pipe = runspace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();

                        if (error.Count > 0 && com == connectCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }

                        if (error.Count > 0 && com == userCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }
                    }
                    runspace.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureRestoreUserError, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Gets All groups from Azure AD.
        /// </summary>
        /// <returns></returns>
        public Collection<PSObject> GetAllAzureGroups()
        {
            try
            {
                Collection<PSObject> groupList = null;

                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "AzureAD" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-AzureAD");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));
                Command getUserCommand = new Command("Get-AzureADGroup");
                getUserCommand.Parameters.Add((new CommandParameter("All", true)));

                using (Runspace psRunSpace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    psRunSpace.Open();

                    foreach (Command com in new Command[] { connectCommand, getUserCommand })
                    {
                        Pipeline pipe = psRunSpace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();

                        if (error.Count > 0 && com == connectCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                            return null;
                        }

                        if (error.Count > 0 && com == getUserCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                            return null;
                        }

                        else
                        {
                            groupList = results;
                        }
                    }
                    psRunSpace.Close();
                }
                return groupList;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureFetchADGroupsError, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Gets individual group from Azure AD.
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public PSObject GetIndividualAzureGroup(string groupName)
        {
            try
            {
                Collection<PSObject> groupList = null;
                PSObject result = null;
                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "AzureAD" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-AzureAD");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));
                Command getUserCommand = new Command("Get-AzureADGroup");
                getUserCommand.Parameters.Add((new CommandParameter("All", true)));

                using (Runspace psRunSpace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    psRunSpace.Open();

                    foreach (Command com in new Command[] { connectCommand, getUserCommand })
                    {
                        Pipeline pipe = psRunSpace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();

                        if (error.Count > 0 && com == connectCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                            return null;
                        }

                        if (error.Count > 0 && com == getUserCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                            return null;
                        }

                        else
                        {
                            groupList = results;
                        }
                    }
                    psRunSpace.Close();

                    foreach (PSObject group in groupList)
                    {
                        if (!string.IsNullOrEmpty(group.Properties["DisplayName"].Value.ToString()))
                        {
                            if (group.Properties["DisplayName"].Value.ToString() == groupName)
                            {
                                result = group;
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureGetIndividualGroupError, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Adds user to group in Azure.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="groupName"></param>
        public void AddUserToAzureGroup(Models.AD.User user, string groupName)
        {
            try
            {
                string userId = GetUserObjectID(user);
                PSObject group = GetIndividualAzureGroup(groupName);
                string groupId = group.Properties["ObjectId"].Value.ToString();

                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "AzureAD" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-AzureAD");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

                Command userCommand = new Command("Add-AzureADGroupMember");
                userCommand.Parameters.Add((new CommandParameter("ObjectId", groupId)));
                userCommand.Parameters.Add((new CommandParameter("RefObjectId", userId)));

                using (Runspace runspace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    runspace.Open();
                    foreach (Command com in new Command[] { connectCommand, userCommand })
                    {
                        Pipeline pipe = runspace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();

                        if (error.Count > 0 && com == connectCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }

                        if (error.Count > 0 && com == userCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }
                    }
                    runspace.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureAddUserToGroupError, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Removes User from AD group in Azure.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="groupName"></param>
        public void RemoveUserFromAzureGroup(Models.AD.User user, string groupName)
        {
            try
            {
                string userId = GetUserObjectID(user);
                PSObject group = GetIndividualAzureGroup(groupName);
                string groupId = group.Properties["ObjectId"].Value.ToString();

                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "AzureAD" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-AzureAD");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

                Command userCommand = new Command("Remove-AzureADGroupMember");
                userCommand.Parameters.Add((new CommandParameter("ObjectId", groupId)));
                userCommand.Parameters.Add((new CommandParameter("MemberId", userId)));

                using (Runspace runspace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    runspace.Open();
                    foreach (Command com in new Command[] { connectCommand, userCommand })
                    {
                        Pipeline pipe = runspace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();

                        if (error.Count > 0 && com == connectCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }

                        if (error.Count > 0 && com == userCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with getting users");
                            _logger.LogError(error[0].ToString());
                        }
                    }
                    runspace.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureRemoveUserFromGroupError, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Sets temporary password for user, and then forces them to change the password.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        public void ResetUserPassword(Models.AD.User user, string password)
        {
            try
            {
                string userId = GetUserObjectID(user);

                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "AzureAD" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-AzureAD");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

                Command userCommand = new Command("Set-AzureADUserPassword");
                userCommand.Parameters.Add((new CommandParameter("ObjectId", userId)));
                userCommand.Parameters.Add((new CommandParameter("Password", password.ToSecureString())));
                userCommand.Parameters.Add((new CommandParameter("EnforceChangePasswordPolicy", true)));

                using (Runspace runspace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    runspace.Open();
                    foreach (Command com in new Command[] { connectCommand, userCommand })
                    {
                        Pipeline pipe = runspace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();

                        if (error.Count > 0 && com == connectCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }

                        if (error.Count > 0 && com == userCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }
                    }
                    runspace.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureAssignPasswordPolicyError, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Gets objectID based on userprinciplename from Azure AD.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GetUserObjectID(Models.AD.User user)
        {
            try
            {
                string result = string.Empty;
                Collection<PSObject> userList = null;
                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "AzureAD" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-AzureAD");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));
                Command getUserCommand = new Command("Get-AzureADUser");
                getUserCommand.Parameters.Add((new CommandParameter("ObjectId", user.Username + GeneralMessages.CompanyEmail)));

                using (Runspace psRunSpace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    psRunSpace.Open();
                    foreach (Command com in new Command[] { connectCommand, getUserCommand })
                    {
                        Pipeline pipe = psRunSpace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();

                        if (error.Count > 0 && com == connectCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }

                        if (error.Count > 0 && com == getUserCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }
                        else
                        {
                            userList = results;
                        }
                    }
                    psRunSpace.Close();

                    foreach (PSObject item in userList)
                    {
                        if (item != null)
                        {
                            string objectID = item.Properties["ObjectId"].Value.ToString();
                            if (!string.IsNullOrEmpty(objectID))
                            {
                                result = objectID;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureGetUserObjectIdError, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Gets office SKU from Azure Tenant.
        /// </summary>
        private PSObject GetSkuFromTenant(VersionEnum versionEnum)
        {
            try
            {
                Collection<PSObject> skuList = null;
                PSObject result = null;
                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "AzureAD" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-AzureAD");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));
                Command getUserCommand = new Command("Get-AzureADSubscribedSku");
                if (versionEnum.ToString().Contains("E3"))
                {
                    getUserCommand.Parameters.Add((new CommandParameter("ObjectId", "26f211c8-652a-4673-8495-cdeeea6ff167_6fd2c87f-b296-42f0-b197-1e91e994b900")));
                }
                else
                {
                    getUserCommand.Parameters.Add((new CommandParameter("ObjectId", "26f211c8-652a-4673-8495-cdeeea6ff167_18181a46-0d4e-45cd-891e-60aabd171b4e")));
                }

                using (Runspace psRunSpace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    psRunSpace.Open();
                    foreach (Command com in new Command[] { connectCommand, getUserCommand })
                    {
                        Pipeline pipe = psRunSpace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();

                        if (error.Count > 0 && com == connectCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }

                        if (error.Count > 0 && com == getUserCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }
                        else
                        {
                            skuList = results;
                        }
                    }
                    psRunSpace.Close();

                    foreach (PSObject sku in skuList)
                    {
                        if (versionEnum.ToString().Contains("E3"))
                        {
                            if (sku.Properties["SkuPartNumber"].Value.ToString().Contains("ENTERPRISEPACK"))
                            {
                                result = sku;
                            }
                        }
                        else
                        {
                            if (sku.Properties["SkuPartNumber"].Value.ToString().Contains("STANDARDPACK"))
                            {
                                result = sku;
                            }
                        }
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureGetSkuInformation, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Sets license from user objectid and user info in Azure AD.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private AssignedLicenses AssignedLicense(Models.AD.User user, AzureLicense licenseEnum, VersionEnum versionEnum)
        {
            try
            {
                PSObject sku = GetSkuFromTenant(versionEnum);
                Collection<PSObject> userList = null;
                InitialSessionState initialSession = InitialSessionState.CreateDefault();
                initialSession.ImportPSModule(new[] { "AzureAD" });

                PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

                Command connectCommand = new Command("Connect-AzureAD");
                connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));
                Command getUserCommand = new Command("Get-AzureADUser");
                getUserCommand.Parameters.Add((new CommandParameter("ObjectId", user.Username + GeneralMessages.CompanyEmail)));

                using (Runspace psRunSpace = RunspaceFactory.CreateRunspace(initialSession))
                {
                    psRunSpace.Open();
                    foreach (Command com in new Command[] { connectCommand, getUserCommand })
                    {
                        Pipeline pipe = psRunSpace.CreatePipeline();
                        pipe.Commands.Add(com);

                        Collection<PSObject> results = pipe.Invoke();
                        Collection<object> error = pipe.Error.ReadToEnd();

                        if (error.Count > 0 && com == connectCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }

                        if (error.Count > 0 && com == getUserCommand)
                        {
                            MessageBox.Show(error[0].ToString(), "Problem with login");
                            _logger.LogError(error[0].ToString());
                        }
                        else
                        {
                            userList = results;
                        }
                    }
                    psRunSpace.Close();

                    AssignedLicenses license = new AssignedLicenses();

                    foreach (PSObject item in userList)
                    {
                        if (item != null)
                        {
                            string objectID = item.Properties["ObjectId"].Value.ToString();
                            if (!string.IsNullOrEmpty(objectID))
                            {
                                if (!string.IsNullOrEmpty(sku.Properties["SkuId"].Value.ToString()))
                                {
                                    AssignedLicense temp = new AssignedLicense();
                                    temp.SkuId = sku.Properties["SkuId"].Value.ToString();
                                    if (licenseEnum.ToString().Equals("Add"))
                                    {
                                        license.AddLicenses = new List<AssignedLicense>();
                                        license.AddLicenses.Add(temp);
                                    }
                                    else
                                    {
                                        license.RemoveLicenses = new List<string>();
                                        license.RemoveLicenses.Add(temp.SkuId);
                                    }
                                }
                                else
                                {
                                    return null;
                                }
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return license;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureAssignLicenseToUserError, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Uses Microsoft Graph to set passwordProfile.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private PasswordProfile SetAzurePassword(string password)
        {
            try
            {
                //MUST INSTALL AZURE AD in powershell. Then reference the .dll manually
                //DLL Location
                //C:\Program Files\WindowsPowerShell\Modules\AzureADPreview\2.0.0.85
                //OR
                //C:\Program Files\WindowsPowerShell\Modules\AzureAD\2.0.2.130
                PasswordProfile passwordProfile = new PasswordProfile();
                passwordProfile.Password = password;
                passwordProfile.EnforceChangePasswordPolicy = true;
                return passwordProfile;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.AzureAssignPasswordPolicyError, ex.Message, ex.StackTrace));
                throw;
            }
        }
    }
}
