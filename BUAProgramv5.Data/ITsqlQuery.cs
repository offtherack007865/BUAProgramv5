using BUAProgramv5.Models.AD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Data
{
    public interface ITsqlQuery
    {
        Task<List<UserInformation>> GetUserTypes();
        void EditUserTemplate(UserInformation userTemplate);
        void AddUserTemplate(UserInformation userTemplate);
        bool DoesTableExist(string tableName);
        void DeleteTableData(string tableName);
        Task<List<JobTitleInformation>> GetJobTitles();
        Task<List<SiteInformation>> GetSiteInfo();
        void AddSite(SiteInformation newSiteDetails);
        void EditSite(SiteInformation site);
    }
}
