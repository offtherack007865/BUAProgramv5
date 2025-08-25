using BUAProgramv5.Models.AD;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace BUAProgramv5.ServerFunctions.O365
{
    public interface IO365Worker
    {
        void Create365Users(User userName, bool assignLicense);
        Collection<PSObject> Get365Users(int amount);
        void PopulateUserDetails(User userName);
        Collection<PSObject> Get365Licenses();
        Collection<PSObject> GetUnlicensedUsers(int amount);
        void RemoveLicensesFromUsers(User userName);
        void AssignLicensesToUsers(User username);
        void DeleteUserAccount(User userName);
        Collection<PSObject> GetDeletedUsers();
        void RestoreUsers(User userName);
    }
}
