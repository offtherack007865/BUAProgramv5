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
    public interface IClientService
    {
        ObservableCollection<ADObjectCheckList> GetListSecurityGroups(List<string> organizationalGroups);
        ObservableCollection<ADObjectCheckList> GetAllSecurityGroups();
        ObservableCollection<ADObjectCheckList> GetSingleSecurityGroup(string securityGroup, string filter);
        Collection<PSObject> GetAllDistroGroups();
        Collection<PSObject> GetIndividualDistroGroup(string name);
        string GetSAMAccountName(string userName);
        bool DoesNameExist(string userName);
        bool UserADGroup(string userName, string secuirtyGroup);
        bool IsAccountLocked(string userName);
        void UnlockAccount(string userName);
        void LockAccount(string userName);
        bool CreateNasFolder(string userName);
        bool MapDriveLetter(string user, string driveLetter);
        User GetUserByDisplayName(string displayName);
        List<ADPrincipalObject> SearchByLastName(string surname);
        List<ADPrincipalObject> GetAllUsers();
        List<string> GetUserOUs();
        ADPrincipalObject SearchADByName(string userName);
        void MoveOUs(string oldOU, string newOU);
        bool IsUserNameAvailable(string userName);
        Collection<PSObject> GetSecurityGroups();
        Collection<PSObject> GetSites();
        bool LDAPAuthentication(string userName, string password);
        bool ChangeUserPassword(string samAccountName, string password);
        string[] GetComputersFromActiveDirectory();
        void DisableUserandRemoveGroups(User user);
        void ReEnableExistingUser(User reEnableUser);
        void RemoveUserFromGroups(string userDistinguishedName, List<ADPrincipalObject> groups);
        void ReplaceUsersCurrentGroupWithNewGroup(string userDistinguishedName, List<ADPrincipalObject> groups, User user);
        void UpdateUserJobDescription(string userDistinguishedName, string jobDescription);
        void UpdateUserSiteInfo(string userDistinguishedName, string phone, string office, string managerDistinguishedName);
        void CreateNewUser(User user);
        void AddUserToGroups(User user);
        void MoveUsersToDisabledOU(string oldOU);
    }
}
