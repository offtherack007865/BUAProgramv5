using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Messaging
{
    public class ErrorMessages
    {
        public static readonly string SendEmailError = @"Failed to send email see error: {0}. Stacktrace: {1}.";
        public static readonly string NASCreationError = @"Cannot write {0} to network location. See error: {1}. Stacktrace: {2}.";
        public static readonly string PostAccountCreationError = @"Some errors occurred after the creation of the account.  Double check that the network folder and home drive have been mapped, and/or email address has been created";
        public static readonly string FailedStateNotification = @"BUA Program failed to initialize pages.";
        public static readonly string GetAllSecurityGroupsError = @"GetAllSecurityGroups method failed, see error: {0}. Stacktrace: {1}.";
        public static readonly string DoesUserNameExistError = @"DoesUserNameExist method has failed, see error: {0} Stacktrace: {1}.";
        public static readonly string IsAccountLockedError = @"IsAccountLocked method has failed, see error: {0}. Stacktrace: {1}.";
        public static readonly string UnlockAccountError = @"UnlockAccount method has failed, see error: {0} Stacktrace: {1}.";
        public static readonly string DoesntExistError = @"Username does not exist";
        public static readonly string NetworkFolderError = @"User exists, but does not have a Network folder";
        public static readonly string MapHomeFolderError = @"MapHomeFolder method has failed, see error: {0}. Stacktrace: {1}.";
        public static readonly string GetAdUserByDisplayError = @"GetAdUserByDisplay method has failed, see error: {0}. Stacktrace: {1}.";
        public static readonly string DisableUserAndRemoveFromGroupsError = @"DisableUserAndRemoveFromGroups method has failed, see error: {0}. Stacktrace: {1}.";
        public static readonly string UserGroupsError = @"Error disabling UserGroups, see error: {0}. Stacktrace: {1}";
        public static readonly string EmailandGroupsError = @"Error sending email and disabling groups, see error: {0}. Stacktrace: {1}.";
        public static readonly string BlockedAVError = @"Error sending email.  May have been blocked by AntiVirus Software.  Please notify Dev Team.";
        public static readonly string SearchADByLastNameError = @"SearchADByLastName method has failed, see error: {0}. Stacktrace: {1}.";
        public static readonly string GetAllUsersError = @"GetAllUsers method has failed, see error: {0}. Stacktrace: {1}.";
        public static readonly string SearchADByNameError = @"SearchADByName method has failed, see error: {0}. Stacktrace: {1}.";
        public static readonly string MovingUserAccountError = @"Error moving account to new location See error: {0}. Stacktrace: {1}.";
        public static readonly string MovingUserAccountNotification = @"An error occurred while moving the account to the new OU";
        public static readonly string ReEnablingUserAccountError = @"Error re-enabling the account.. See error: {0}. Stacktrace: {1}.";
        public static readonly string ReEnablingUserAccountNotification = @"An error occurred while re-enabling the account. See error: {0}.";
        public static readonly string MoveUserToNewOUError = @"MoveUserToNewOU method has failed, see error: {0}. Stacktrace: {1}.";
        public static readonly string ReEnableExistingUserError = @"ReEnableExistingUser method has failed, see error: {0}. Stacktrace: {1}.";
        public static readonly string RemoveUsersFromGroupError = @"RemoveUsersFromGroup method has failed, see error: {0}. Stacktrace: {1}.";
        public static readonly string ReplaceUsersCurrentGroupError = @"ReplaceUsersCurrentGroup method has failed, see error: {0}. Stacktrace: {1}.";
        public static readonly string DistinguishedADNameError = @"There is an error with the users current site in AD. Site listed is {0}.";
        public static readonly string UpdateUserJobDescriptionError = @"UpdateUserJobDescription method has failed, see error: {0}. Stacktrace: {1}.";
        public static readonly string UpdateUserSiteInfoError = @"UpdateUserSiteInfo method has failed, see error: {0}. Stacktrace {1}.";
        public static readonly string EmailFailureNotification = @"Failed to send Email. See error: {0}. Stacktrace: {1}.";
        public static readonly string MovingOrganizationalUnitError = @"Error Moving user to Organizational Unit. See error: {0}. Stacktrace: {1}.";
        public static readonly string UpdatingSiteError = @"Error updating site information. See error: {0}. Stacktrace: {1}.";
        public static readonly string UpdatingJobDescriptionError = @"Error updating job description. See error: {0}. Stacktrace: {1}.";
        public static readonly string AddingUsersToSecurityGroupError = @"Could not add user to Security Groups. See error: {0}. Stacktrace: {1}.";
        public static readonly string RemoveOldSecurityGroupsError = @"Could not remove user from old SecurityGroup(s). See error: {0}. Stacktrace: {1}.";
        public static readonly string ProgramCrashError = @"The program has crashed, see error: {0}";
        public static readonly string CheckUserNameAvailabilityError = @"CheckUserNameAvailability method has failed, see error: {0}. Stacktrace: {1}.";
        public static readonly string AddUserToGroups = @"AddUserToGroups method has failed, see error: {0}. Stacktrace: {1}.";
        public static readonly string ActiveDirectoryError = @"Error creating Active Directory Account.";
        public static readonly string AccountCreationError = @"Error creating account.. See error: {0}. Stacktrace: {1}";
        public static readonly string NetworkStorageError = @"Error creating NAS folder.";
        public static readonly string CreatingNetworkFolderError = @"Error creating Network Folder. See error: {0}. Stacktrace: {1}";
        public static readonly string PartialAccountCreationError = @"{0} Errors occurred during account creation for {1}";
        public static readonly string PowershellError = @"Cannot make Powershell call, see error: {0}. Stacktrace: {1}.";
        public static readonly string AuthenticationError = @"Cannot authenticate to API Endpoint, or recieved bad data. See errors. Stacktrace: {0}. \r\n Message: {1}";
        public static readonly string APICallError = @"Cannot connect properly to API call. See errors. Stacktrace: {0} \r\n Message: {1}";
        public static readonly string APIPostError = @"Api post call for appointments has returned false.";
        public static readonly string AzureCreateUserError = @"Cannot create user in Azure. See error: {0}. Stacktrace: {1}.";
        public static readonly string AzureGetAllUsersError = @"Cannot get all users from Azure. See error: {0}. Stacktrace: {1}.";
        public static readonly string AzurePopulateUserDetails = @"Cannot update user details in Azure. See error: {0}. Stacktrace: {1}.";
        public static readonly string AzureFetchLicensesError = @"Cannot fetch Azure licenses from 365. See error: {0}. Stacktrace: {1}.";
        public static readonly string AzureRemoveLicenseError = @"Cannot remove license from Azure user, see error: {0}. Stacktrace: {1}.";
        public static readonly string AzureAddLicenseError = @"Cannot add license from Azure user, see error: {0}. Stacktrace: {1}.";
        public static readonly string AzureDeleteUserError = @"Cannot delete user in Azure, see error:{0}. Stacktrace:{1}.";
        public static readonly string AzureGetDeletedUsersError = @"Cannot get delted users from Office 365. See error: {0}. Stacktrace: {1}.";
        public static readonly string AzureRestoreUserError = @"Cannot Restore user in Office365, see error: {0}. Stacktrace: {1}.";
        public static readonly string AzureFetchADGroupsError = @"Cannot get all Azure AD groups, see error: {0}. Stacktrace: {1}.";
        public static readonly string AzureGetIndividualGroupError = @"Cannot get individual group in Azure, see error: {0}. Stacktrace: {1}.";
        public static readonly string AzureAddUserToGroupError = @"Cannot add user to group in Azure, see error: {0}. Stacktrace: {1}.";
        public static readonly string AzureRemoveUserFromGroupError = @"Cannot remove user to group in Azure, see error: {0}. Stacktrace: {1}.";
        public static readonly string AzureGetUserObjectIdError = @"Cannot fetch user ObjectId inAzure, see error: {0}. Stacktrace: {1}.";
        public static readonly string AzureGetSkuInformation = @"Cannot get sku tenant information from Azure, see error: {0}. Stacktrace: {1}.";
        public static readonly string AzureAssignLicenseToUserError = @"Cannot assign license to user in Azure, see error: {0}. Stacktrace: {1}.";
        public static readonly string AzureAssignPasswordPolicyError = @"Cannot set password for password policy for Azure, see error: {0}. Stacktrace: {1}.";
        public static readonly string EditSiteError = @"Cannot edit Site, see error:{0}. Stacktrace: {1}.";
    }
}
