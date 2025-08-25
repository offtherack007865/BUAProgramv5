using BUAProgramv5.Logging;
using BUAProgramv5.Models.AD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.API.ClientWorker.Service
{
    public class ClientService : IClientService
    {
        private ILogger _logger;
        private ClientWorker _worker;

        public ClientService(ILogger logger)
        {
            _logger = logger;
            _worker = new ClientWorker(_logger);
        }

        /// <summary>
        /// Gets Security Groups based on a list.
        /// </summary>
        /// <param name="organizationalGroups"></param>
        /// <returns></returns>
        public ObservableCollection<ADObjectCheckList> GetListSecurityGroups(List<string> organizationalGroups)
        {
            ObservableCollection<ADObjectCheckList> results = _worker.GetListSecurityGroups(organizationalGroups);
            return results;
        }

        /// <summary>
        /// Gets all Security Groups.
        /// </summary>
        /// <param name="organizationalGroups"></param>
        /// <returns></returns>
        public ObservableCollection<ADObjectCheckList> GetAllSecurityGroups()
        {
            ObservableCollection<ADObjectCheckList> results = _worker.GetAllSecurityGroups();
            return results;
        }

        /// <summary>
        /// Gets specific Security Group.
        /// </summary>
        /// <param name="securityGroup"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public ObservableCollection<ADObjectCheckList> GetSingleSecurityGroup(string securityGroup, string filter)
        {
            ObservableCollection<ADObjectCheckList> results = _worker.GetSingleSecurityGroup(securityGroup, filter);
            return results;
        }

        /// <summary>
        /// Gets all Distrobution Groups.
        /// </summary>
        /// <param name="organizationalGroups"></param>
        /// <returns></returns>
        public Collection<PSObject> GetAllDistroGroups()
        {
            Collection<PSObject> results = _worker.GetAllDistroGroups();
            return results;
        }


        public Collection<PSObject> GetIndividualDistroGroup(string name)
        {
            Collection<PSObject> results = _worker.GetIndividualDistro(name);
            return results;
        }

        /// <summary>
        /// Gets SAM Account name.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string GetSAMAccountName(string userName)
        {
            return _worker.GetSAMAccountUserName(userName);
        }

        /// <summary>
        /// Determines if username exists in AD.
        /// </summary>
        /// <returns></returns>
        public bool DoesNameExist(string userName)
        {
            return _worker.DoesUserNameExist(userName);
        }

        /// <summary>
        /// Determines if user is in AD group.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="secuirtyGroup"></param>
        /// <returns></returns>
        public bool UserADGroup(string userName, string secuirtyGroup)
        {
            return _worker.GetUserADGroup(userName, secuirtyGroup);
        }

        /// <summary>
        /// Checks to see if account is locked.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsAccountLocked(string userName)
        {
            return _worker.IsAccountLocked(userName);
        }

        /// <summary>
        /// Unlocks user account using AD.
        /// </summary>
        /// <param name="userName"></param>
        public void UnlockAccount(string userName)
        {
            _worker.UnlockAccount(userName);
        }

        /// <summary>
        /// Locks user account using AD.
        /// </summary>
        /// <param name="userName"></param>
        public void LockAccount(string userName)
        {
            _worker.LockAccount(userName);
        }

        /// <summary>
        /// Creates new NAS Directory for users.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool CreateNasFolder(string userName)
        {
            return _worker.CreateNASFolder(userName);
        }

        /// <summary>
        /// Maps User map drive to their AD object.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="driveLetter"></param>
        /// <returns></returns>
        public bool MapDriveLetter(string user, string driveLetter)
        {
            return _worker.MapDriveLetter(user, driveLetter);
        }

        /// <summary>
        /// Gets user information based on AD Display name.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public User GetUserByDisplayName(string displayName)
        {
            return _worker.GetUserByDisplayName(displayName);
        }

        /// <summary>
        /// Searches AD for user based on last name.
        /// </summary>
        /// <param name="surname"></param>
        /// <returns></returns>
        public List<ADPrincipalObject> SearchByLastName(string surname)
        {
            return _worker.SearchByLastName(surname);
        }

        /// <summary>
        /// Gets all users from AD.
        /// </summary>
        /// <returns></returns>
        public List<ADPrincipalObject> GetAllUsers()
        {
            return _worker.GetAllUsers();
        }

        /// <summary>
        /// Gets User OUs from AD.
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserOUs()
        {
            return _worker.GetUserOUs();
        }

        /// <summary>
        /// Search AD for user based on username.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ADPrincipalObject SearchADByName(string userName)
        {
            return _worker.SearchADByName(userName);
        }

        /// <summary>
        /// Moves user from one OU to another OU in AD.
        /// </summary>
        /// <param name="oldOU"></param>
        /// <param name="newOU"></param>
        public void MoveOUs(string oldOU, string newOU)
        {
            _worker.MoveOUs(oldOU, newOU);
        }

        /// <summary>
        /// Checks ad if username was available.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsUserNameAvailable(string userName)
        {
            return _worker.IsUserNameAvailable(userName);
        }

        /// <summary>
        /// Users powershell to get real time list of security groups.
        /// </summary>
        /// <returns></returns>
        public Collection<PSObject> GetSecurityGroups()
        {
            return _worker.GetSecurityGroups();
        }

        /// <summary>
        /// Uses powershell to get real time list of sites.
        /// </summary>
        /// <returns></returns>
        public Collection<PSObject> GetSites()
        {
            return _worker.GetSites();
        }

        /// <summary>
        /// Checks user credentials to make sure they are valid in AD.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool LDAPAuthentication(string userName, string password)
        {
            return _worker.LDAPAuthentication(userName, password);
        }

        /// <summary>
        /// Change user password in AD.
        /// </summary>
        /// <param name="samAccountName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ChangeUserPassword(string samAccountName, string password)
        {
            return _worker.ChangeUserPassword(samAccountName, password);
        }

        /// <summary>
        /// Gets all computers within SMG AD enviroment, ordered by decending.
        /// </summary>
        /// <returns></returns>
        public string[] GetComputersFromActiveDirectory()
        {
            return _worker.GetComputersFromActiveDirectory();
        }

        /// <summary>
        /// Disables User account in AD.
        /// </summary>
        /// <param name="user"></param>
        public void DisableUserandRemoveGroups(User user)
        {
            _worker.DisableUserandRemoveGroups(user);
        }

        /// <summary>
        /// Re-enables User account in AD.
        /// </summary>
        /// <param name="reEnableUser"></param>
        public void ReEnableExistingUser(User reEnableUser)
        {
            _worker.ReEnableExistingUser(reEnableUser);
        }

        /// <summary>
        /// Removes User from groups in AD except for Domain User.
        /// </summary>
        /// <param name="userDistinguishedName"></param>
        /// <param name="groups"></param>
        public void RemoveUserFromGroups(string userDistinguishedName, List<ADPrincipalObject> groups)
        {
            _worker.RemoveUserFromGroups(userDistinguishedName, groups);
        }

        /// <summary>
        /// Replaces User's current groups in AD with specified new groups.
        /// </summary>
        /// <param name="userDistinguishedName"></param>
        /// <param name="groups"></param>
        public void ReplaceUsersCurrentGroupWithNewGroup(string userDistinguishedName, List<ADPrincipalObject> groups, User user)
        {
            _worker.ReplaceUsersCurrentGroupWithNewGroup(userDistinguishedName, groups, user);
        }

        /// <summary>
        /// Updates User job info in AD.
        /// </summary>
        /// <param name="userDistinguishedName"></param>
        /// <param name="jobDescription"></param>
        public void UpdateUserJobDescription(string userDistinguishedName, string jobDescription)
        {
            _worker.UpdateUserJobDescription(userDistinguishedName, jobDescription);
        }

        /// <summary>
        /// Updates User site info in AD.
        /// </summary>
        /// <param name="userDistinguishedName"></param>
        /// <param name="phone"></param>
        /// <param name="office"></param>
        /// <param name="managerDistinguishedName"></param>
        public void UpdateUserSiteInfo(string userDistinguishedName, string phone, string office, string managerDistinguishedName)
        {
            _worker.UpdateUserSiteInfo(userDistinguishedName, phone, office, managerDistinguishedName);
        }

        /// <summary>
        /// Creates new user in AD.
        /// </summary>
        /// <param name="user"></param>
        public void CreateNewUser(User user)
        {
            _worker.CreateNewUser(user);
        }

        /// <summary>
        /// Add User to groups in AD.
        /// </summary>
        /// <param name="user"></param>
        public void AddUserToGroups(User user)
        {
            _worker.AddUserToGroups(user);
        }

        /// <summary>
        /// Moves user to disabled OU.
        /// </summary>
        /// <param name="oldOU"></param>
        public void MoveUsersToDisabledOU(string oldOU)
        {
            _worker.MoveUsersToDisabledOU(oldOU);
        }
    }
}
