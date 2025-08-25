using BUAProgramv5.Models.Enum.Azure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.ServerFunctions.Azure
{
    public interface IAzureWorker
    {
        void CreateUserAccount(Models.AD.User user);
        Collection<PSObject> Get365Users(int amount);
        void PopulateUserDetails(Models.AD.User user);
        Collection<PSObject> Get365Licenses();
        void RemoveLicensesFromUsers(Models.AD.User user, VersionEnum versionEnum);
        void AssignLicensesToUsers(Models.AD.User user, VersionEnum versionEnum);
        void DeleteUserAccount(Models.AD.User user);
        Collection<PSObject> GetDeletedUsers();
        void RestoreUsers(Models.AD.User user);
        Collection<PSObject> GetAllAzureGroups();
        PSObject GetIndividualAzureGroup(string groupName);
        void AddUserToAzureGroup(Models.AD.User user, string groupName);
        void RemoveUserFromAzureGroup(Models.AD.User user, string groupName);
        void ResetUserPassword(Models.AD.User user, string password);


    }
}
