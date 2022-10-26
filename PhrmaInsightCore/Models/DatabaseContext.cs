using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhrmaInsightCore.Models;
using PhrmaInsightCore.Models.DB;

namespace PhrmaInsightCore.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
           : base(options)
        { }

        public DbSet<SharePointUserCompany> SharePointUserCompany { get; set; }
        public DbSet<ImpexiumMembers> ImpexiumMembers { get; set; }
        public DbSet<KastleVisitor> KastleVisitor { get; set; }
        public DbSet<KastleVisitorAttendance> KastleVisitorAttendance { get; set; }
        public DbSet<KastleVisitorHistory> KastleVisitorHistory { get; set; }
        public DbSet<KastleVisitorDelete> KastleVisitorDelete { get; set; }
        public DbSet<KastleVisitorModify> KastleVisitorModify { get; set; }
        public DbSet<KastleVisitorRecurring> KastleVisitorRecurring { get; set; }
        public DbSet<SharePointUserSection> SharePointUserSection { get; set; }
        public DbSet<SharePointUserSubSection> SharePointUserSubSection { get; set; }
        public DbSet<Kastle> Kastle { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<UserActivity> UserActivity { get; set; }
        public DbSet<SoftCommitmentForecast> SoftCommitmentForecast { get; set; }
        public DbSet<Security> Security { get; set; }
        public DbSet<MainAccounts> MainAccounts { get; set; }
        public DbSet<Vendors> Vendors { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<Locations> Locations { get; set; }
        public DbSet<PostSoftCommitmentForecast> PostSoftCommitmentForecast { get; set; }
        public DbSet<CongressDistricts> CongressDistricts { get; set; }
        public DbSet<ProjectManager> ProjectManager { get; set; }
        public DbSet<Calendar> Calendar { get; set; }
        public DbSet<TransactionDetail> TransactionDetail { get; set; }
        public DbSet<ReferenceCode> ReferenceCode { get; set; }
        public DbSet<UserTitle> UserTitle { get; set; }
        public DbSet<SenateTracker> SenateTracker { get; set; }
        public DbSet<Senators> Senators { get; set; }
        public DbSet<HouseMembers> HouseMembers { get; set; }
        public DbSet<SCFFinancials> SCFFinancials { get; set; }
        public DbSet<WeatherWidget> WeatherWidgets { get; set; }
        public DbSet<BoardMeetingTracker> BoardMeetingTracker { get; set; }
        public DbSet<BoardMember> BoardMembers { get; set; }
        public DbSet<SharePointUserActivity> SharePointUserActivity { get; set; }
        public DbSet<MemberNote> MemberNotes { get; set; }

        public DbSet<Forecasting> Forecasting { get; set; }
        public DbSet<FederalAdvocacyStore> FederalAdvocacyStore { get; set; }
        public DbSet<Budgeting> Budgeting { get; set; }
        public DbSet<SharePointDocumentStore> SharePointDocumentStore { get; set; }
        public DbSet<Prophix_Departments> Prophix_Departments { get; set; }
        public DbSet<AzureADUsers> AzureADUsers { get; set; }
        public DbSet<CRMSource> CRMSource { get; set; }
        public DbSet<ASARegion> ASARegion { get; set; }
        public DbSet<FederalAdvocacyFiles> FederalAdvocacyFiles { get; set; }
        public DbSet<FederalAdvocacyFolders> FederalAdvocacyFolders { get; set; }
        public DbSet<PhrmaEmployeeIssueAreas> PhrmaEmployeeIssueAreas { get; set; }
        public DbSet<UserIssueAreas> UserIssueAreas { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<OrgChartUsers> OrgChartUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserInfo>();
            modelBuilder.Entity<Kastle>().ToTable("KASTLE");
            modelBuilder.Entity<AzureADUsers>().ToTable("AZURE_AD_USERS");
            modelBuilder.Entity<Forecasting>().ToTable("SCF_FORECAST");
            modelBuilder.Entity<UserActivity>().ToTable("USERACTIVITY");
            modelBuilder.Entity<SoftCommitmentForecast>().ToTable("SoftCommitmentForecast");
            modelBuilder.Entity<Security>().ToTable("SECURITY");
            modelBuilder.Entity<TransactionDetail>().ToTable("TransactionDetail");
            modelBuilder.Entity<MainAccounts>().ToTable("SCF_MAINACCOUNTS");
            modelBuilder.Entity<Vendors>().ToTable("SCF_VENDORS");
            modelBuilder.Entity<Projects>().ToTable("SCF_PROJECTS");
            modelBuilder.Entity<Departments>().ToTable("SCF_DEPARTMENTS");
            modelBuilder.Entity<ReferenceCode>().ToTable("SCF_REFERENCECODES");
            modelBuilder.Entity<Locations>().ToTable("SCF_LOCATIONS");
            modelBuilder.Entity<CongressDistricts>().ToTable("CONGRESSDISTRICTS");
            modelBuilder.Entity<ProjectManager>().ToTable("SCF_PROJECTMANAGERS");
            modelBuilder.Entity<UserTitle>().ToTable("SCF_USERTITLES");
            modelBuilder.Entity<SenateTracker>().ToTable("SENATETRACKER");
            modelBuilder.Entity<Senators>().ToTable("SENATORS");
            modelBuilder.Entity<HouseMembers>().ToTable("HOUSE_MEMBERS");
            modelBuilder.Entity<SCFFinancials>().ToTable("SCF_FINANCIALS");
            modelBuilder.Entity<WeatherWidget>().ToTable("WEATHER_WIDGET");
            modelBuilder.Entity<BoardMeetingTracker>().ToTable("BOARD_MEETING_TRACKER");
            modelBuilder.Entity<BoardMember>().ToTable("BOARD_MEMBERS");
            modelBuilder.Entity<SharePointUserActivity>().ToTable("SHAREPOINT_USER_ACTIVITY");
            modelBuilder.Entity<MemberNote>().ToTable("MEMBER_NOTES");
            modelBuilder.Entity<FederalAdvocacyStore>().ToTable("FEDERAL_ADVOCACY_STORE");
            modelBuilder.Entity<Budgeting>().ToTable("SCF_BUDGETING");
            modelBuilder.Entity<SharePointDocumentStore>().ToTable("SCF_SHAREPOINT_DOCUMENTS_FA");
            modelBuilder.Entity<Prophix_Departments>().ToTable("PROPHIX_DEPARTMENTS");
            modelBuilder.Entity<KastleVisitor>().ToTable("KASTLE_VISITOR");
            modelBuilder.Entity<CRMSource>().ToTable("KASTLE_CRM_SOURCE");
            modelBuilder.Entity<KastleVisitorHistory>().ToTable("KASTLE_VISITOR_HISTORY");
            modelBuilder.Entity<KastleVisitorDelete>().ToTable("KASTLE_VISITOR_DELETE");
            modelBuilder.Entity<KastleVisitorModify>().ToTable("KASTLE_VISITOR_MODIFY");
            modelBuilder.Entity<KastleVisitorRecurring>().ToTable("KASTLE_VISITOR_RECURRING");            
            modelBuilder.Entity<KastleVisitorAttendance>().ToTable("KASTLE_VISITOR_ATTENDANCE");
            modelBuilder.Entity<SharePointUserSection>().ToTable("SHAREPOINT_USER_ACTIVITY_SECTION");
            modelBuilder.Entity<SharePointUserSubSection>().ToTable("SHAREPOINT_USER_ACTIVITY_SUBSECTION");
            modelBuilder.Entity<ASARegion>().ToTable("ASA_REGION");
            modelBuilder.Entity<ImpexiumMembers>().ToTable("IMPEXIUM_MEMBERS");
            modelBuilder.Entity<SharePointUserCompany>().ToTable("SHAREPOINT_USER_ACTIVITY_COMPANY");
            modelBuilder.Entity<FederalAdvocacyFiles>().ToTable("FEDERAL_ADVOCACY_FILES");
            modelBuilder.Entity<FederalAdvocacyFolders>().ToTable("FEDERAL_ADVOCACY_FOLDERS");
            modelBuilder.Entity<PhrmaEmployeeIssueAreas>().ToTable("PHRMA_EMPLOYEE_ISSUE_AREAS");
            modelBuilder.Entity<UserIssueAreas>().ToTable("USER_ISSUE_AREAS");
            modelBuilder.Entity<Employees>().ToTable("EMPLOYEES");
            modelBuilder.Entity<OrgChartUsers>().ToTable("ORG_CHART_USERS");
        }
    }
}
