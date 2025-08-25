using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Messaging
{
    public class GeneralMessages
    {
        public static readonly string CompanyEmail = "@summithealthcare.com";
        public static readonly string LicenseAssignment = @"summitmedicalgrouppllc1:OFFICESUBSCRIPTION";
        public static readonly string SenderEmailAddress = @"AccountCreation@summithealthcare.com";
        public static readonly string DisplayName = @"Account Creation";
        public static readonly string BlockedAVNotification = @"Network Folder creation failed due to permissions. Please contact Sys Admin or Development Team for further instructions";
        public static readonly string ReadyStateNotification = @"Welcome {0}";
        public static readonly string NonAuthorizedNotification = @"{0} is not authorized to use this application. Exiting program and writing to log.";
        public static readonly string UnlockAccountNotification = @"Unlock Account";
        public static readonly string UsernameDoesNotExistNotification = @"Username does not exist";
        public static readonly string AccountIsLockedNotification = @"User account is currently locked.";
        public static readonly string AccountUnlockNotification = @"Click HERE To Unlock It!";
        public static readonly string AccountIsUnlockedNotification = @"User account is NOT currently locked.";
        public static readonly string CreateNetworkFolderNotification = @"Create Network Folder";
        public static readonly string FolderFoundNotification = @"Folder found for {0}";
        public static readonly string NetworkFolderNotification = @"Click HERE To Create One!";
        public static readonly string HomeFolderCreation = @"Network folder has been created for {0}";
        public static readonly string EmailErrorNotification = @"SOMETHING WENT WRONG..... ACCOUNT FAILED TO DISABLE<BR>";
        public static readonly string DisableUserAndGroups = @"Disabling Active Directory account and removing group memberships.";
        public static readonly string AccountDisableFailure = @"Account did not disable for: ";
        public static readonly string AccountDisableSuccessful = @"Account has been disabled for: ";
        public static readonly string ReEnableAccountNotification = @"Re-Enabling account...";
        public static readonly string RelocateUserAccountNotification = @"Moving account to new location: ";
        public static readonly string AccountReEnabledEmailNotification = @"Account Re-Enabled for ";
        public static readonly string AccountReEnabledNotification = @"Account has been re-enabled";
        public static readonly string MovedAccount = @"User account has been moved.  A confirmation email will be sent shortly.";
        public static readonly string UserAccountMovedNotification = @"User Account moved for: ";
        public static readonly string UserAccountMovedSuccessful = @"User Account moved for {0}";
        public static readonly string EmailFailureNotification = @"Failed to send email.  {0}{1}{2}{3}";
        public static readonly string DataGatheringNotification = @"Error gathering data. See error: {0}. Stacktrace: {1}.";
        public static readonly string ActiveDirectoryAccount = @"Creating Active Directory Account...";
        public static readonly string CreateNetworkFolder = @"Creating NAS folder and mapping home drive...";
        public static readonly string PartialAccountCreation = @"{0} Errors occurred during account creation for {1}, contact development team for assistance.";
        public static readonly string AccountCreationSuccessful = @"Account successfully created for ";
        public static readonly string EmailNotification = @"Account created succesfully.  You should receive a confirmation email shortly.";
        public static readonly string NewAccountCreated = @"New Account Created For: ";
        public static readonly string CreateNasSyncMessageNotification = @"Delayed Syncing issue, attempting to create NAS directory.";
        public static readonly string CreateMapDriveSyncMessageNotification = @"Delayed Syncing issue, attempting to map user's map drive to their account.";
    }
}
