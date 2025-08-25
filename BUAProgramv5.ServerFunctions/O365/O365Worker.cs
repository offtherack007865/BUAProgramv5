using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using BUAProgramv5.Models.AD;
using BUAProgramv5.Logging;
using BUAProgramv5.Models.Config;
using BUAProgramv5.Utilities;
using System.Management.Automation.Runspaces;
using System.Windows;
using System.Diagnostics;
using BUAProgramv5.Messaging;

namespace BUAProgramv5.ServerFunctions.O365
{
    public class O365Worker : IO365Worker
    {
        //This is Azure powershell 1.0 commands to do CRUD functionality.
        private ILogger _logger;
        private BUAConfiguration _office365;
        public O365Worker(ILogger logger)
        {
            _logger = logger;
            _office365 = Config.GetAuthenticationInfo("Office365");
        }

        /// <summary>
        /// Assigns Office license to specified User.
        /// </summary>
        /// <param name="username"></param>
        public void AssignLicensesToUsers(User username)
        {
            InitialSessionState initialSession = InitialSessionState.CreateDefault();
            initialSession.ImportPSModule(new[] { "MSOnline" });

            PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

            Command connectCommand = new Command("Connect-MsolService");
            connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

            Command userCommand = new Command("Set-MsolUserLicense");
            userCommand.Parameters.Add("UserPrincipalName", username.EmailAddress);
            userCommand.Parameters.Add("AddLicenses", GeneralMessages.LicenseAssignment);

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

        /// <summary>
        /// Creates user in Office 365, if boolean is true, assigns office suite license.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="assignLicense"></param>
        public void Create365Users(User userName, bool assignLicense)
        {
            InitialSessionState initialSession = InitialSessionState.CreateDefault();
            initialSession.ImportPSModule(new[] { "MSOnline" });

            PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

            Command connectCommand = new Command("Connect-MsolService");
            connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));
            Command getUserCommand = new Command("New-MsolUser");
            getUserCommand.Parameters.Add((new CommandParameter("DisplayName", userName.DisplayName)));
            getUserCommand.Parameters.Add((new CommandParameter("FirstName", userName.FirstName)));
            getUserCommand.Parameters.Add((new CommandParameter("LastName", userName.LastName)));
            getUserCommand.Parameters.Add((new CommandParameter("UserPrincipalName", userName.Username + GeneralMessages.CompanyEmail)));
            getUserCommand.Parameters.Add((new CommandParameter("UsageLocation", "US")));
            if(assignLicense)
            {
                getUserCommand.Parameters.Add((new CommandParameter("LicenseAssignment", GeneralMessages.LicenseAssignment)));
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
                        MessageBox.Show(error[0].ToString(), "Problem with getting users");
                        _logger.LogError(error[0].ToString());
                    }
                }
                psRunSpace.Close();
            }
        }

        /// <summary>
        /// Soft deletes user from Office 365.
        /// </summary>
        /// <param name="userName"></param>
        public void DeleteUserAccount(User userName)
        {
            InitialSessionState initialSession = InitialSessionState.CreateDefault();
            initialSession.ImportPSModule(new[] { "MSOnline" });

            PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

            Command connectCommand = new Command("Connect-MsolService");
            connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

            Command userCommand = userCommand = new Command("Remove-MsolUser");
            userCommand.Parameters.Add("UserPrincipalName", userName.EmailAddress);

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

        /// <summary>
        /// Gets Office365 licenses.
        /// </summary>
        /// <returns></returns>
        public Collection<PSObject> Get365Licenses()
        {
            Collection<PSObject> licenses = null;

            InitialSessionState initialSession = InitialSessionState.CreateDefault();
            initialSession.ImportPSModule(new[] { "MSOnline" });

            PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

            Command connectCommand = new Command("Connect-MsolService");
            connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

            Command userCommand = new Command("Get-MsolAccountSku");
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
                        MessageBox.Show(error[0].ToString(), "Problem with getting users");
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

        /// <summary>
        /// Gets Office 365 users from Microsoft API.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Collection<PSObject> Get365Users(int amount)
        {
            Collection<PSObject> userList = null;

            InitialSessionState initialSession = InitialSessionState.CreateDefault();
            initialSession.ImportPSModule(new[] { "MSOnline" });

            PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

            Command connectCommand = new Command("Connect-MsolService");
            connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));
            Command getUserCommand = new Command("Get-MsolUser");
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
                        MessageBox.Show(error[0].ToString(), "Problem with getting users");
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

        /// <summary>
        /// Gets pending deleted Users from Office 365.
        /// </summary>
        /// <returns></returns>
        public Collection<PSObject> GetDeletedUsers()
        {
            Collection<PSObject> deletedUserList = null;

            InitialSessionState initialSession = InitialSessionState.CreateDefault();
            initialSession.ImportPSModule(new[] { "MSOnline" });

            PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

            Command connectCommand = new Command("Connect-MsolService");
            connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

            Command userCommand = userCommand = new Command("Get-MsolUser");
            userCommand.Parameters.Add("All", "ReturnDeletedUsers");

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
                    else
                    {
                        deletedUserList = results;
                    }
                }
                runspace.Close();
            }

            return deletedUserList;
        }

        /// <summary>
        /// Gets users within SMG company that do not have an office license subscription.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public Collection<PSObject> GetUnlicensedUsers(int amount)
        {
            Collection<PSObject> unLicensedList = null;

            InitialSessionState initialSession = InitialSessionState.CreateDefault();
            initialSession.ImportPSModule(new[] { "MSOnline" });

            PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

            Command connectCommand = new Command("Connect-MsolService");
            connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

            Command userCommand = new Command("Get-MsolUser");
            userCommand.Parameters.Add((new CommandParameter("UnlicensedUsersOnly")));
            userCommand.Parameters.Add((new CommandParameter("MaxResult", amount)));

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
                    else
                    {
                        unLicensedList = results;
                    }
                }
                runspace.Close();
            }
            return unLicensedList;
        }

        /// <summary>
        /// Populates user information about person in Office 365.
        /// </summary>
        /// <param name="userName"></param>
        public void PopulateUserDetails(User userName)
        {
            InitialSessionState initialSession = InitialSessionState.CreateDefault();
            initialSession.ImportPSModule(new[] { "MSOnline" });

            PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

            Command connectCommand = new Command("Connect-MsolService");
            connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

            Command userCommand = new Command("Set-MsolUser");
            userCommand.Parameters.Add((new CommandParameter("UserPrincipalName", userName.EmailAddress)));
            userCommand.Parameters.Add((new CommandParameter("DisplayName", userName.DisplayName)));
            userCommand.Parameters.Add((new CommandParameter("FirstName", userName.FirstName)));
            userCommand.Parameters.Add((new CommandParameter("LastName", userName.LastName)));
            userCommand.Parameters.Add((new CommandParameter("Department", userName.Department)));
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
                        MessageBox.Show(error[0].ToString());
                        _logger.LogError(error[0].ToString());
                        return;
                    }
                }
                psRunSpace.Close();
            }
        }

        /// <summary>
        /// Removes Office 365 license from user.
        /// </summary>
        /// <param name="userName"></param>
        public void RemoveLicensesFromUsers(User userName)
        {
            InitialSessionState initialSession = InitialSessionState.CreateDefault();
            initialSession.ImportPSModule(new[] { "MSOnline" });

            PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

            Command connectCommand = new Command("Connect-MsolService");
            connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

            Command userCommand = new Command("Set-MsolUserLicense");
            userCommand.Parameters.Add("UserPrincipalName", userName.EmailAddress);
            userCommand.Parameters.Add("RemoveLicenses", GeneralMessages.LicenseAssignment);

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

        /// <summary>
        /// Restores deleted Office 365 user.
        /// </summary>
        /// <param name="userName"></param>
        public void RestoreUsers(User userName)
        {
            InitialSessionState initialSession = InitialSessionState.CreateDefault();
            initialSession.ImportPSModule(new[] { "MSOnline" });

            PSCredential credential = new PSCredential(_office365.Office365User, _office365.Office365Password);

            Command connectCommand = new Command("Connect-MsolService");
            connectCommand.Parameters.Add((new CommandParameter("Credential", credential)));

            Command userCommand = new Command("Restore-MsolUser");
            userCommand.Parameters.Add("UserPrincipalName", userName.EmailAddress);

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
    }
}
