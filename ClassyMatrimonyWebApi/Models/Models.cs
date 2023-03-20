using MohsyWebApi.Models;
using System;
using System.Web;

namespace MohsyWebApi
{

    public partial class SelectItems
    {
        public SelectItems() { }
        public SelectItems(string value, string text) { Value = value; Text = text; }
        public bool Selected { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
    }


    public partial class MasterItem
    {
        public bool? IsActive { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public partial class IFilter
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public string serchKey { get; set; }
    }

    public partial class Filter1
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public string allocatedto { get; set; }
        public string SalesType { get; set; }

        public string Name { get; set; }
        public string MatId { get; set; }
        public string MobileNo { get; set; }

        public string EmailId { get; set; }
        public string City { get; set; }
        public string lookingFor { get; set; }
        public string CashType { get; set; }
        public string Duration { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Amount { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime NextFollowup { get; set; }
        public DateTime CreatedDate { get; set; }
        public int allocatedtoId { get; set; }
        public string status { get; set; }
        public string registerType { get; set; }
        public string matrimonyid { get; set; }
    }


    public partial class ProfileResult
    {

        public int OpenLeadsCount { get; set; }
        public int TodayFollowupCount { get; set; }
        public int FollowupCount { get; set; }

        public int PromiseToPayCount { get; set; }
        public int WalkinCount { get; set; }

        public int LeadClosedCount { get; set; }

        public int TotalAllCount { get; set; }


    }

    public partial class UserModel
    {
        public int PersonDetailsID { get; set; }
        public string Religion { get; set; }
        public string Caste { get; set; }
        public string SubCaste { get; set; }
        public string EatingHabit { get; set; }
        public string Smoking { get; set; }
        public string DrinkType { get; set; }
        public string LifeStyle { get; set; }
        public string Hobbies { get; set; }
        public string EducationalQualificationId { get; set; }
        public string NameoftheInstitution { get; set; }
        public string Occupation { get; set; }
        public string Designation { get; set; }
        public string Income { get; set; }
        public string BusinessType { get; set; }
        public string Aboutmyself { get; set; }
        public string FamilyType { get; set; }
        public string FamilyValues { get; set; }
        public string FamilyStatus { get; set; }
        public string FamilyOrigin { get; set; }
        public string NoofSiblings { get; set; }
        public string NOofmarriedSiblings { get; set; }
        public string Fathersoccupation { get; set; }
        public string Mothersoccupation { get; set; }
        public string FamilyBusinesstype { get; set; }
        public string FamilyBusinessTenure { get; set; }
        public string AboutmyFamily { get; set; }

        public string Manglik { get; set; }
        public string HoroscopeAvailable { get; set; }
        public string Raasi { get; set; }
        public string Star { get; set; }
        public string Gothram { get; set; }
        public string Turnover { get; set; }

        public string file { get; set; }
    }

}