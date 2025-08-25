using BUAProgramv5.API.ClientWorker.Service;
using BUAProgramv5.Data;
using BUAProgramv5.Models.AD;
using BUAProgramv5.Models.Enum;
using BUAProgramv5.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.ConsoleApp.Helper
{
    internal class ConsoleHelper
    {
        private IClientService _service;
        private ITsqlQuery _tsql;
        internal ConsoleHelper(IClientService service, ITsqlQuery tsql)
        {
            _service = service;
            _tsql = tsql;
        }

        /// <summary>
        /// Seeds tables BUA database tables dynamically from powershell and matching criteria.
        /// </summary>
        internal void SeedTables()
        {
            List<string> siteNames = new List<string>();
            List<SiteInformation> sites = GetSites();
            ADPrincipalObject secGroup = new ADPrincipalObject();

            foreach(SiteInformation site in sites)
            {
                string[] siteName = site.Name.Split(' ');
                if (siteName.Count() > 2)
                {
                    MatchSecurityGroupsManually(site, site.Name, secGroup);
                    continue;
                }
                else
                {
                    secGroup = GetSpecificSecurityGroups(siteName[1]);
                }
                if (secGroup != null)
                {
                    if(!string.IsNullOrEmpty(secGroup.DistinguishedName) && !string.IsNullOrEmpty(secGroup.Name))
                    {
                        site.SecurityGroups = new List<ADPrincipalObject>();
                        site.SecurityGroups.Add(secGroup);
                    }
                    else
                    {
                        MatchSecurityGroupsManually(site, site.Name, secGroup);
                    }
                } 
            }

            _tsql.DeleteTableData("Site_SecurityGroups");
            _tsql.DeleteTableData("Site");

            foreach (SiteInformation site in sites)
            {
                if(!site.Name.Contains("CFP Float Pool") && !site.Name.Contains("NonSummit Accounts"))
                {
                    string updatedSiteName = ReplaceFirst(site.Name, " ", " ");
                    site.Name = updatedSiteName;
                }
                _tsql.AddSite(site);
            }
        }

        /// <summary>
        /// This is for sites that cannot be matched by first part of name using wildcard matching. This handles outliers that SG and Site names that do not match.
        /// </summary>
        /// <param name="site"></param>
        /// <param name="siteName"></param>
        /// <param name="secGroup"></param>
        private void MatchSecurityGroupsManually(SiteInformation site, string siteName, ADPrincipalObject secGroup)
        {
            site.SecurityGroups = new List<ADPrincipalObject>();
            ADPrincipalObject temp = new ADPrincipalObject();
            switch (siteName)
            {
                case "131 Norwood":
                    temp.DistinguishedName = "CN=Norwood SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Norwood SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "135 Seymour FP":
                    temp.DistinguishedName = "Seymour FP SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Seymour FP SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "117 Ft Loudon":
                    temp.DistinguishedName = "CN=Ft Loudon SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Ft Loudon SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "118 Ftn City":
                    temp.DistinguishedName = "CN=Ftn City SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Ftn City SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "139 Ft Sanders":
                    temp.DistinguishedName = "CN=Ft Sanders SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Ft Sanders SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "130 Middle Creek":
                    temp.DistinguishedName = "CN=Middle Creek SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Middle Creek SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "127 SMG at Maryville":
                    temp.DistinguishedName = "CN=SMG at Maryville SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "SMG at Maryville SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "129 SMG at Parkwest":
                    temp.DistinguishedName = "CN=SMG at Parkwest SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "SMG at Parkwest SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "152 SMG of Campbell County":
                    temp.DistinguishedName = "CN=SMG of Campbell County SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "SMG of Campbell County SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "137 Deane Hill":
                    temp.DistinguishedName = "CN=Deane Hill SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Deane Hill SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "148 Strawberry Plains":
                    temp.DistinguishedName = "CN=Strawberry Plains SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Strawberry Plains SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "120 Greeneville IMFM":
                    temp.DistinguishedName = "CN=Greeneville IMFM SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Greeneville IMFM SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "149 Summit Family Medicine":
                    temp.DistinguishedName = "CN=Summit Family Medicine SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Family Medicine SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "146 Oak Ridge":
                    temp.DistinguishedName = "CN=Oak Ridge SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Oak Ridge SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "104 Razzak":
                    temp.DistinguishedName = "CN=Fry-Razzak SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Fry-Razzak SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "101 Central Lab":
                    temp.DistinguishedName = "CN=Central Lab SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Central Lab SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "071 IMG Wellington":
                    temp.DistinguishedName = "CN=IMG Wellington SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "IMG Wellington SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "067 IMG Cedar Bluff":
                    temp.DistinguishedName = "CN=IMG Cedar Bluff SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "IMG Cedar Bluff SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "050 Sleep Lab":
                    temp.DistinguishedName = "CN=Sleep Lab SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Sleep Lab SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "177 SEC Ftn City":
                    temp.DistinguishedName = "CN=SEC Ftn City SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "SEC Ftn City SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "147 South Knox":
                    temp.DistinguishedName = "CN=South Knox SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "South Knox SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "215 PC_Family Medicine Center":
                    temp.DistinguishedName = "CN=PCFMC SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "PCFMC SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "109 Concord Lenoir":
                    temp.DistinguishedName = "CN=Concord SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Concord SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "228 IMA Etowah":
                    temp.DistinguishedName = "CN=IMA Etowah SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "IMA Etowah SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "176 SEC Farragut":
                    temp.DistinguishedName = "CN=SEC Farragut SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "SEC Farragut SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "119 Greeneville FM":
                    temp.DistinguishedName = "CN=Greeneville FM SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Greeneville FM SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "151 TN Valley":
                    temp.DistinguishedName = "CN=TN Valley SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "TN Valley SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "116 Farragut Family":
                    temp.DistinguishedName = "CN=Farragut Family SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Farragut Family SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "134 Rockwood":
                    temp.DistinguishedName = "CN=Rockwood SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Rockwood SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "122 Hardin Valley":
                    temp.DistinguishedName = "CN=Hardin Valley SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Hardin Valley SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "155 Tusculum":
                    temp.DistinguishedName = "CN=Tusculum SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Tusculum SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "107 Caring Medical":
                    temp.DistinguishedName = "CN=Caring Medical SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Caring Medical SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "336 Family Medicine Greeneville":
                    temp.DistinguishedName = "CN=Family Medicine Greeneville SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Family Medicine Greeneville SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "141 Newport":
                    temp.DistinguishedName = "CN=Newport SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Newport SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "114 West Knox":
                    temp.DistinguishedName = "CN=West Knox SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "West Knox SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "136 Seymour Med":
                    temp.DistinguishedName = "CN=Seymour Med SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Seymour Med SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "133 Powell Family":
                    temp.DistinguishedName = "CN=Powell Family SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Powell Family SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "143 Tellico":
                    temp.DistinguishedName = "CN=Tellico SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Tellico SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "345 Family Medicine Surgoinsville":
                    temp.DistinguishedName = "CN=Surgoinsville SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Surgoinsville SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "140 Middlebrook":
                    temp.DistinguishedName = "CN=Middlebrook SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Middlebrook SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "165 Maryville Peds":
                    temp.DistinguishedName = "CN=Maryville Peds SG,OU=Sites,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Maryville Peds SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "164 Shults":
                    temp.DistinguishedName = "CN=Shults Pediatrics SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Shults Pediatrics SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "159 Hometown Pediatric Morristown":
                    temp.DistinguishedName = "CN=Hometown Pediatric Morristown SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Hometown Pediatric Morristown SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "158 Hometown Pediatric Jefferson City":
                    temp.DistinguishedName = "CN=Hometown Pediatric Jefferson City SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Hometown Pediatric Jefferson City SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "160 Hometown Pediatric Sevierville":
                    temp.DistinguishedName = "CN=Hometown Pediatric Sevierville SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Hometown Pediatric Sevierville SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "157 Children's Faith":
                    temp.DistinguishedName = "CN=Children's Faith SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Childrens Faith SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "153 Trinity Knox":
                    temp.DistinguishedName = "CN=Trinity Knox SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Trinity Knox SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "154 Trinity Maryville":
                    temp.DistinguishedName = "CN=Trinity Maryville SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Trinity Maryville SGG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "163 Pediatric Huxley":
                    temp.DistinguishedName = "CN=Pediatric Huxley SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Pediatric Huxley SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "161 Pediatric Hardin Valley":
                    temp.DistinguishedName = "CN=Pediatric Hardin Valley SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Pediatric Hardin Valley SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "400 Summit Health Advantage":
                    temp.DistinguishedName = "CN=Summit Landing Employees,OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Landing Employees";
                    site.SecurityGroups.Add(temp);
                    break;
                case "179 SEC Powell":
                    temp.DistinguishedName = "CN=SEC Powell SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "SEC Powell SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "070 Extensivist Clinic":
                    temp.DistinguishedName = "CN=Extensivist Clinic SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Extensivist Clinic SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "162 Pediatric Seymour":
                    temp.DistinguishedName = "CN= Pediatric Seymour SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = " Pediatric Seymour SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "702 Summit Neurology":
                    temp.DistinguishedName = "CN=Summit Neurology SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Neurology SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "196 Athens":
                    temp.DistinguishedName = "CN=Athens SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Athens SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "069 IMG Greeneville":
                    temp.DistinguishedName = "CN=IMG Greeneville SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "IMG Greeneville SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "068 IMG Ftn City":
                    temp.DistinguishedName = "CN=IMG Ftn City SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "IMG Ftn City SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "6010 SHS Admin":
                    temp.DistinguishedName = "CN=Summit Landing Employees,OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Landing Employees";
                    site.SecurityGroups.Add(temp);
                    break;
                case "6015 Care Coordination":
                    temp.DistinguishedName = "CN=Summit Landing Employees,OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Landing Employees";
                    site.SecurityGroups.Add(temp);
                    break;
                case "6020 Chart Review":
                    temp.DistinguishedName = "CN=Summit Landing Employees,OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Landing Employees";
                    site.SecurityGroups.Add(temp);
                    break;
                case "168 PT Ftn City":
                    temp.DistinguishedName = "CN=Physical Therapy,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Physical Therapy";
                    site.SecurityGroups.Add(temp);
                    break;
                case "166 Cedar Bluff":
                    temp.DistinguishedName = "CN=Cedar Bluff SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Cedar Bluff SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "172 PT Powell":
                    temp.DistinguishedName = "OU=Users,OU=172 Powell PT,OU=Sites,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Physical Therapy";
                    site.SecurityGroups.Add(temp);
                    break;
                case "167 PT Farragut":
                    temp.DistinguishedName = "OU=Users,OU=167 PT Farragut,OU=Sites,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Physical Therapy";
                    site.SecurityGroups.Add(temp);
                    break;
                case "175 SEC Bearden":
                    temp.DistinguishedName = "OU=Users,OU=175 SEC Bearden,OU=Sites,DC=ad,DC=sumg,DC=int";
                    temp.Name = "SEC Bearden SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "170 PT Jefferson City":
                    temp.DistinguishedName = "OU=Users,OU=170 PT Jefferson City,OU=Sites,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Physical Therapy";
                    site.SecurityGroups.Add(temp);
                    break;
                case "9970 Accounting":
                    temp.DistinguishedName = "CN=Summit Landing Employees,OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Landing Employees";
                    site.SecurityGroups.Add(temp);
                    break;
                case "9975 Accounts Receivable":
                    temp.DistinguishedName = "CN=Summit Landing Employees,OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Landing Employees";
                    site.SecurityGroups.Add(temp);
                    break;
                case "9987 SMG Administration":
                    temp.DistinguishedName = "CN=Summit Landing Employees,OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Landing Employees";
                    site.SecurityGroups.Add(temp);
                    break;
                case "9980 SSS Administration":
                    temp.DistinguishedName = "CN=Summit Landing Employees,OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Landing Employees";
                    site.SecurityGroups.Add(temp);
                    break;
                case "9981 Provider Enrollment":
                    temp.DistinguishedName = "CN=Summit Landing Employees,OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Landing Employees";
                    site.SecurityGroups.Add(temp);
                    break;
                case "9982 Operations":
                    temp.DistinguishedName = "CN=Summit Landing Employees,OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Landing Employees";
                    site.SecurityGroups.Add(temp);
                    break;
                case "Fountain":
                    temp.DistinguishedName = "CN=Summit Landing Employees,OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Landing Employees";
                    site.SecurityGroups.Add(temp);
                    break;
                case "9983 Quality":
                    temp.DistinguishedName = "CN=Summit Landing Employees,OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Landing Employees";
                    site.SecurityGroups.Add(temp);
                    break;
                case "9984 Recruiting":
                    temp.DistinguishedName = "CN=Summit Landing Employees,OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Landing Employees";
                    site.SecurityGroups.Add(temp);
                    break;
                case "9985 Board":
                    temp.DistinguishedName = "CN=Summit Landing Employees,OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Landing Employees";
                    site.SecurityGroups.Add(temp);
                    break;
                case "9986 Marketing & Communications":
                    temp.DistinguishedName = "CN=Summit Landing Employees,OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Landing Employees";
                    site.SecurityGroups.Add(temp);
                    break;
                case "9990 Compliance":
                    temp.DistinguishedName = "CN=Compliance SG,OU=Security Groups,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Compliance SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "9995 Human Resources or TSOD":
                    temp.DistinguishedName = "CN=Human Resources _ secure,OU=Security Groups,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Human Resources _ secure";
                    site.SecurityGroups.Add(temp);
                    break;
                case "9999 Information Systems":
                    temp.DistinguishedName = "CN=Summit Landing Employees,OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Summit Landing Employees";
                    site.SecurityGroups.Add(temp);
                    break;
                case "NonSummit Accounts":
                    temp.DistinguishedName = "CN=NonSummitAccount SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "NonSummitAccount SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "CFP Float Pool":
                    temp.DistinguishedName = "CN=Float Pool SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Float Pool SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "219 Cool Springs":
                    temp.DistinguishedName = "CN=Cool Springs SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Cool Springs SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "220 North Franklin":
                    temp.DistinguishedName = "CN=North Franklin SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "North Franklin SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "156 Summit Medical Group at Sevierville":
                    temp.DistinguishedName = "CN=Fry-Razzak SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Fry-Razzak SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "231 Goodlettsville Pediatrics":
                    temp.DistinguishedName = "CN=Goodlettsville Pediatrics SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Goodlettsville Pediatrics SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "241 PC_SMG Cookeville":
                    temp.DistinguishedName = "CN=PC_SMG Cookeville SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "PC_SMG Cookeville SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "247 IMG Powell":
                    temp.DistinguishedName = "CN=IMG Powell SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "IMG Powell SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "244 Shults Maryville":
                    temp.DistinguishedName = "CN=Shults Maryville SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Shults Maryville SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "266 Grace Primary Care":
                    temp.DistinguishedName = "CN=Grace Primary Care SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Grace Primary Care SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "268 Upper Cumberland Family Physicians":
                    temp.DistinguishedName = "CN=Upper Cumberland Family Physicians SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Upper Cumberland Family Physicians SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "250 EYE TEC Lenoir City":
                    temp.DistinguishedName = "CN=EYE TEC Lenoir City SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "EYE TEC Lenoir City SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "251 EYE TEC Knoxville":
                    temp.DistinguishedName = "CN=EYE TEC Knoxville SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "EYE TEC Knoxville SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "252 EYE TEC Harriman":
                    temp.DistinguishedName = "CN=EYE TEC Harriman SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "EYE TEC Harriman SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "253 EYE TEC Powell":
                    temp.DistinguishedName = "CN=EYE TEC Powell SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "EYE TEC Powell SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "254 EYE TEC Morristown":
                    temp.DistinguishedName = "CN=EYE TEC Morristown SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "EYE TEC Morristown SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "270 Vora Satellite":
                    temp.DistinguishedName = "CN=Vora Satellite SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Vora Satellite SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "269 Chattanooga Family Physicians":
                    temp.DistinguishedName = "CN=Chattanooga Family Physicians SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Chattanooga Family Physicians SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "258 TEC Hyde":
                    temp.DistinguishedName = "CN=TEC Hyde SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "TEC Hyde SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "278 Hermitage Primary Care":
                    temp.DistinguishedName = "CN=Hermitage Primary Care SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Hermitage Primary Care SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "280 Brentwood Children's Clinic":
                    temp.DistinguishedName = "CN=Brentwood Children's Clinic SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Brentwood Children's Clinic SG";
                    site.SecurityGroups.Add(temp);
                    break;
                case "286 Pistol Creek":
                    temp.DistinguishedName = "CN=Pistol Creek SG,OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int";
                    temp.Name = "Pistol Creek SG";
                    site.SecurityGroups.Add(temp);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Gets sites from AD via powershell, then filters out actual SMG sites based on Name and Distinguished name.
        /// </summary>
        private List<SiteInformation> GetSites()
        {
            List<SiteInformation> adResults = new List<SiteInformation>();
            Collection<PSObject> sites = _service.GetSites();
            

            foreach (PSObject site in sites)
            {
                string distinguishedName = site.Properties["DistinguishedName"].Value.ToString();
                string groupName = site.Properties["Name"].Value.ToString();

                if (!OmitADGroups(groupName))
                {
                    if (distinguishedName.Contains(@"OU=Users,"))
                    {
                        if (!IsWrongGroup(distinguishedName))
                        {
                            SiteInformation temp = new SiteInformation();
                            if (FindADDistinguishedNames(distinguishedName, temp))
                            {
                                temp.OU = distinguishedName;
                                string[] words = distinguishedName.Split(',');
                                temp.Name = words[1].Replace("OU=", "");
                                adResults.Add(temp);
                            }
                        }
                    }
                }
            }

            AddNonStandardAccounts(adResults);
            List<SiteInformation> results = adResults.OrderBy(x => x.Name).ToList();
            return results;
        }

        /// <summary>
        /// Getting specific powershell security group.
        /// </summary>
        /// <param name="securityGroup"></param>
        /// <returns></returns>
        private ADPrincipalObject GetSpecificSecurityGroups(string filter)
        {
            ObservableCollection<ADObjectCheckList> securityGroup =_service.GetSingleSecurityGroup(@"OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int", filter);
            ADPrincipalObject results = new ADPrincipalObject();
            if (securityGroup.Count == 0)
            {
                return results;
            }
            if (securityGroup != null)
            {
                if(securityGroup.Count > 1)
                {
                    foreach(ADObjectCheckList group in securityGroup)
                    {
                        if (group.Name == filter + " SG")
                        {
                            results.DistinguishedName = group.DistinguishedName;
                            results.Name = group.Name;
                            return results;
                        }
                    }
                }
                if(securityGroup.FirstOrDefault().DistinguishedName != null && securityGroup.FirstOrDefault().Name != null)
                {
                    results.DistinguishedName = securityGroup.FirstOrDefault().DistinguishedName;
                    results.Name = securityGroup.FirstOrDefault().Name;
                }
            }

            return results;
        }

        /// <summary>
        /// Checks for old AD Directories, if it matches do not add to site information list, since those are decommissioned.
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        private bool OmitADGroups(string groupName)
        {
            bool result = false;
            List<string> groups = new List<string>()
            {
                //"099 - Accounting" will have to be checked by distinguished name since AD has it just multiple listings as accounting.
                "Central IT", "Central", "Helpdesk", "PSD-HR", "PT", "Quality", "099 Central",
                "Sites", "Unsorted Computers", "Training", "Waiting Room Kiosks", "Kiosk", "Exam Room PCs", "MeetingRoomPC"
            };
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
        /// Check for groups that share same names for multiple groups. Have to check Distinguished names due to this. 
        /// </summary>
        /// <param name="distinguishedName"></param>
        /// <returns></returns>
        private bool IsWrongGroup(string distinguishedName)
        {
            bool result = false;
            List<string> omitList = new List<string>()
            {
                "OU=Users,OU=099 Central,OU=Sites,DC=ad,DC=sumg,DC=int", "OU=Users,OU=Accounting,OU=099 Central,OU=Sites,DC=ad,DC=sumg,DC=int"
            };
            foreach (string dn in omitList)
            {
                if (distinguishedName.Equals(dn))
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// Finds explicit match for AD names and sets needed value for existing sites.
        /// </summary>
        /// <param name="distinguishedName"></param>
        /// <returns></returns>
        private bool FindADDistinguishedNames(string distinguishedName, SiteInformation siteInfo)
        {
            bool result = false;
            IEnumerable<SiteEnum> sites = Enum.GetValues(typeof(SiteEnum)).Cast<SiteEnum>();
            foreach (SiteEnum site in sites)
            {
                MultiDescriptionHelper item = site.GetMultiDescription();
                if(distinguishedName.Equals(item.Description))
                {
                    siteInfo.Phone = item.Value;
                    return true;
                }
            }

            return result;
        }

        /// <summary>
        /// Mapping for account that are Non-Standard Accounts for Non-Summit employees that work between multiple hospitals/offices. Since OU is not apart of Users OU these would never be found unless manually mapped.
        /// </summary>
        /// <param name="sites"></param>
        private void AddNonStandardAccounts(List<SiteInformation> sites)
        {
            sites.Add(new SiteInformation() { Name = "NonSummit Accounts", Phone = "865-584-4747", OU = "OU=NonSummit Accounts,OU=Sites,DC=ad,DC=sumg,DC=int" });
            sites.Add(new SiteInformation() { Name = "CFP Float Pool", Phone = "865-584-4747", OU = "OU=CFP Float Pool,OU=Sites,DC=ad,DC=sumg,DC=int" });
        }

        /// <summary>
        /// Replaces the first item of searched value with different value.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="search"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        private string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}
