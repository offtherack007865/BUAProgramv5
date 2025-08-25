using BUAProgramv5.Logging;
using BUAProgramv5.Models.AD;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Data
{
    public class TsqlQuery : ITsqlQuery
    {
        private ILogger _logger;
        private ConnectionString _connectionString;
        public TsqlQuery(ILogger logger)
        {
            _logger = logger;
            _connectionString = new ConnectionString(DbType.Tsql);
        }

        /// <summary>
        /// Sql call to get all groups and distinguished names.
        /// </summary>
        /// <returns></returns>
        public Task<List<UserInformation>> GetUserTypes()
        {
            Task<List<UserInformation>> fetchUserInfo = Task.Run(() => 
            {
                List<UserInformation> userTypeList = new List<UserInformation>();
                using (SqlConnection conn = new SqlConnection(_connectionString.Value))
                {
                    string sql = @"select * from UserTypeGroups
                            join UserTypes on UserTypes.id = UserTypeGroups.Type_ID
                            join SecurityGroups on SecurityGroups.id = UserTypeGroups.Group_ID";

                    dynamic data = conn.QueryAsync<dynamic>(sql).Result.ToList();

                    foreach (dynamic item in data)
                    {
                        if (userTypeList.Any(x => x.Name == item.TypeName))
                        {
                            userTypeList.SingleOrDefault(x => x.Name == item.TypeName).SecurityGroups.Add(new ADPrincipalObject { Name = item.GroupName, DistinguishedName = item.DistinguishedName });
                            continue;
                        }
                        UserInformation type = new UserInformation() { Name = item.TypeName, SecurityGroups = new List<ADPrincipalObject>() };
                        type.SecurityGroups.Add(new ADPrincipalObject { Name = item.GroupName, DistinguishedName = item.DistinguishedName });
                        userTypeList.Add(type);
                    }
                }
                return userTypeList;
            });
            Task.WhenAll(fetchUserInfo);
            return fetchUserInfo;
        }

        /// <summary>
        /// Sql call to get Job Titles With Whether the job is permanent or contractor.
        /// </summary>
        /// <returns></returns>
        public Task<List<JobTitleInformation>> GetJobTitles()
        {
            Task<List<JobTitleInformation>> fetchJobTitleInfo = Task.Run(() => 
            {
                List<JobTitleInformation> jobTitleInformationList = new List<JobTitleInformation>();
                using (SqlConnection conn = new SqlConnection(_connectionString.Value))
                {
                    string sql = @"select PermanentOrContractor, JobTitle from OnboardingJobTitle
                            order by JobTitle asc";

                    dynamic data = conn.QueryAsync<dynamic>(sql).Result.ToList();

                    foreach (dynamic item in data)
                    {
                        JobTitleInformation jobTitleInfo = new JobTitleInformation (item.PermanentOrContractor, item.JobTitle);
                        jobTitleInformationList.Add(jobTitleInfo);
                    }
                }
                return jobTitleInformationList;
            });
            Task.WhenAll(fetchJobTitleInfo);
            return fetchJobTitleInfo;
        }

        /// <summary>
        /// Allows for modification of a User type for administrator functions.
        /// </summary>
        /// <param name="userTemplate"></param>
        public void EditUserTemplate(UserInformation userTemplate)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString.Value))
            {
                Int32 userId = conn.QueryAsync<int>(@"SELECT id from UserTypes where TypeName = @TypeName", new { TypeName = userTemplate.Name }).Result.SingleOrDefault();

                conn.ExecuteAsync(@"DELETE FROM UserTypeGroups where Type_ID = @ID", new { ID = userId });

                foreach (ADPrincipalObject group in userTemplate.SecurityGroups)
                {
                    Int32 groupId = conn.QueryAsync<int>(@"if not exists (select * from SecurityGroups where GroupName = @GroupName)
                                                        BEGIN
                                                            INSERT INTO SecurityGroups(GroupName,DistinguishedName) VALUES(@GroupName,@DistinguishedName);
                                                            SELECT CAST(SCOPE_IDENTITY() as int)
                                                        END
                                                    ELSE
                                                        SELECT id from SecurityGroups where GroupName = @GroupName",
                                                        new { GroupName = group.Name, DistinguishedName = group.DistinguishedName }).Result.SingleOrDefault();

                    conn.ExecuteAsync(@"INSERT INTO UserTypeGroups(Type_ID,Group_ID) VALUES(@Type_ID,@Group_ID)", new { Type_ID = userId, Group_ID = groupId });
                }
            }
        }

        /// <summary>
        /// Creates new user Template.
        /// </summary>
        /// <param name="userTemplate"></param>
        public void AddUserTemplate(UserInformation userTemplate)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString.Value))
            {

                Int32 userId = conn.QueryAsync<int>(@"INSERT INTO UserTypes(TypeName) VALUES(@TypeName);
                                                SELECT CAST(SCOPE_IDENTITY() as int)",
                                                new { TypeName = userTemplate.Name }).Result.SingleOrDefault();

                foreach (ADPrincipalObject group in userTemplate.SecurityGroups)
                {
                    Int32 groupId = conn.QueryAsync<int>(@"if not exists (select * from SecurityGroups where GroupName = @GroupName)
                                                        BEGIN
                                                            INSERT INTO SecurityGroups(GroupName,DistinguishedName) VALUES(@GroupName,@DistinguishedName);
                                                            SELECT CAST(SCOPE_IDENTITY() as int)
                                                        END
                                                    ELSE
                                                        SELECT id from SecurityGroups where GroupName = @GroupName",
                                                        new { GroupName = group.Name, DistinguishedName = group.DistinguishedName }).Result.SingleOrDefault();

                    conn.ExecuteAsync(@"INSERT INTO UserTypeGroups(Type_ID,Group_ID) VALUES(@Type_ID,@Group_ID)", new { Type_ID = userId, Group_ID = groupId });
                }
            }
        }

        /// <summary>
        /// Queries table for data, if not data present, first time setup will begin.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool DoesTableExist(string tableName)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(_connectionString.Value))
            {
                try
                {
                    string sql = string.Format(@"SELECT COUNT(*) FROM {0}", tableName);
                    dynamic value = conn.QueryAsync<string>(sql).Result.FirstOrDefault();
                    if (Convert.ToInt32(value) == 0 || value == null)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
                catch
                {
                    result = false;
                }
            }

                return result;
        }

        /// <summary>
        /// Deletes table data and columns when database needs refreshed.
        /// </summary>
        /// <param name="tableName"></param>
        public void DeleteTableData(string tableName)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString.Value))
            {
                try
                {
                    string sql = string.Format(@"DELETE FROM dbo.{0}", tableName.Trim());
                    conn.Execute(sql);
                }
                catch(Exception ex)
                {
                    _logger.LogError(string.Format(@"Failed to Delete rows from table, see errors: {0}. Stacktrace:{1}.", ex.Message, ex.StackTrace));
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets all Site info
        /// </summary>
        /// <returns></returns>
        public Task<List<SiteInformation>> GetSiteInfo()
        {
            Task<List<SiteInformation>> siteTask = Task.Run(() => 
            {
                List<SiteInformation> siteInfo = new List<SiteInformation>();
                using (SqlConnection conn = new SqlConnection(_connectionString.Value))
                {
                    dynamic data = conn.QueryAsync<dynamic>(@"select * from Site
                                                right join Site_SecurityGroups on Site_SecurityGroups.site_id = Site.id order by Name").Result.ToList();

                    foreach (dynamic item in data)
                    {
                        if (siteInfo.Any(x => x.Name == item.Name))
                        {
                            siteInfo.SingleOrDefault(x => x.Name == item.Name).SecurityGroups.Add(new ADPrincipalObject { Name = item.GroupName, DistinguishedName = item.DistinguishedName });
                            continue;
                        }
                        bool result = OmitADGroups(item.Name);
                        if (!result)
                        {
                            SiteInformation site = new SiteInformation() { ID = item.id, Name = item.Name, OU = item.OU, Phone = item.Phone, SecurityGroups = new List<ADPrincipalObject>() };
                            site.SecurityGroups.Add(new ADPrincipalObject { Name = item.GroupName, DistinguishedName = item.DistinguishedName });
                            siteInfo.Add(site);
                        }
                        else
                        {
                            _logger.LogInfo(string.Format(@"{0} has been skipped", item.Name));
                        }
                    }
                }
                return siteInfo;
            });
            Task.WhenAll(siteTask);
            return siteTask;
        }

        /// <summary>
        /// Checks for old AD Directories, if it matches do not add to site information list, since those are decommissioned.
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        private bool OmitADGroups(string groupName)
        {
            bool result = false;
            List<string> groups = new List<string>() { "099 - Accounting", "099 - Central IT", "099 - Central", "099 - Helpdesk", "099 - PSD-HR", "099 - Central PT", "099 - Quality" };
            foreach (string group in groups)
            {
                if (groupName.Equals(group))
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Insert statement for new site.
        /// </summary>
        /// <param name="newSiteDetails"></param>
        public void AddSite(SiteInformation newSiteDetails)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString.Value))
            {
                Int32 siteId = conn.Query<int>(@"INSERT INTO Site(Name,Phone,OU) VALUES(@Name,@Phone,@OU);
                                        SELECT CAST(SCOPE_IDENTITY() as int)",
                                        new { name = newSiteDetails.Name, phone = newSiteDetails.Phone, ou = newSiteDetails.OU }).SingleOrDefault();

                foreach (ADPrincipalObject group in newSiteDetails.SecurityGroups)
                {
                    conn.Execute(@"INSERT INTO Site_SecurityGroups(site_id,GroupName,DistinguishedName) VALUES(@site_id,@GroupName,@DistinguishedName)",
                                            new { site_id = siteId, GroupName = group.Name, DistinguishedName = group.DistinguishedName });
                }
            }
        }

        /// <summary>
        /// Modifies existing site.
        /// </summary>
        /// <param name="site"></param>
        public void EditSite(SiteInformation site)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString.Value))
            {
                conn.Execute(@"UPDATE Site
                                SET Name = @name, Phone = @phone, OU = @ou
                                WHERE id = @ID",
                                new { name = site.Name, phone = site.Phone, ou = site.OU, ID = site.ID });

                conn.Execute(@"DELETE FROM Site_SecurityGroups where site_id = @ID", new { ID = site.ID });

                foreach (ADPrincipalObject group in site.SecurityGroups)
                {
                    conn.Execute(@"INSERT INTO Site_SecurityGroups(site_id,GroupName,DistinguishedName) VALUES(@site_id,@GroupName,@DistinguishedName)",
                                    new { site_id = site.ID, GroupName = group.Name, DistinguishedName = group.DistinguishedName });
                }
            }
        }
    }
}
