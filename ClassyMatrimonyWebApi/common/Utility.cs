using MohsyWebApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace MohsyWebApi.common
{
    public static class Utility
    {
        static MohsyDBEntities dbcontext = new MohsyDBEntities();
        public static string GetConfigValue(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }


        //static List<SelectItems> _courcelist = new List<SelectItems>();
        //public static List<SelectItems> GetCourseLookups()
        //{
        //    if (_courcelist.Any()) return _courcelist;
        //    dbcontext = new MohsyDBEntities();
        //    var _clist = dbcontext.CourseLookups.ToList();
        //    _courcelist = _clist.Select(x => new SelectItems { Value = Convert.ToString(x.ID), Text = x.CourseName }).ToList();
        //    return _courcelist;
        //}

        static List<SelectItems> _countrylist = new List<SelectItems>();
        //public static List<SelectItems> GetCountryLookups()
        //{
        //    try
        //    {
        //        if (_countrylist.Any()) return _countrylist;
        //        dbcontext = new MohsyDBEntities();
        //        var _clist = dbcontext.CountryLookups.ToList();
        //        _countrylist = _clist.Select(x => new SelectItems() { Value = Convert.ToString(x.ID), Text = x.CountryName }).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return _countrylist;
        //}

        //static List<SelectItems> _divisionlist = new List<SelectItems>();
        //public static List<SelectItems> GetDivisionLookup()
        //{
        //    if (_divisionlist.Any()) return _divisionlist;
        //    dbcontext = new MohsyDBEntities();
        //    var _clist = dbcontext.DivisionLookups.ToList();
        //    _divisionlist = _clist.Select(x => new SelectItems { Value = Convert.ToString(x.ID), Text = x.Name }).ToList();
        //    return _divisionlist;
        //}

        static List<SelectItems> _languageslist = new List<SelectItems>();
        //public static List<SelectItems> GetLanguagesLookup()
        //{
        //    if (_languageslist.Any()) return _languageslist;
        //    dbcontext = new MohsyDBEntities();
        //    var _clist = dbcontext.LanguagesLookups.ToList();
        //    _languageslist = _clist.Select(x => new SelectItems { Value = Convert.ToString(x.Id), Text = x.LanguagesName }).ToList();
        //    return _languageslist;
        //}



        //static List<SelectItems> _employeelist = new List<SelectItems>();
        //public static List<SelectItems> GetEmployeeLookup()
        //{
        //    if (_employeelist.Any()) return _employeelist;
        //    dbcontext = new MohsyDBEntities();
        //    var _clist = dbcontext.EmployeeInLookups.ToList();
        //    _employeelist = _clist.Select(x => new SelectItems { Value = Convert.ToString(x.ID), Text = x.Name } ).ToList();
        //    return _employeelist;
        //}

        static readonly List<SelectItems> Statuslist = new List<SelectItems>();
        public static IEnumerable<SelectItems> GetStatusup()
        {
            if (Agelist.Any()) return Agelist;
            for (var i = 20; i <= 70; i++)
            {
                var item = new SelectItems
                {
                    Text = i.ToString(CultureInfo.InvariantCulture),
                    Value = i.ToString(CultureInfo.InvariantCulture)
                };
                if (Agelist != null) Agelist.Add(item);
            }
            return Agelist;
        }

        //static List<CitiesLookup> _cityslist = new List<CitiesLookup>();
        //public static List<CitiesLookup> GetCitysLookup()
        //{
        //    if (_cityslist.Any()) return _cityslist;
        //    dbcontext = new MohsyDBEntities();
        //    _cityslist = dbcontext.CitiesLookups.ToList();
        //    return _cityslist;
        //}

        static readonly List<SelectItems> Heightlist = new List<SelectItems>();
        public static List<SelectItems> GetHeightLookup()
        {
            if (Heightlist.Any()) return Heightlist;
            for (var i = 121; i <= 241; i++)
            {
                var height = i + " cm";
                var feet = Convert.ToDouble(i) * 0.0328084;
                var heightfeetandinches = height + " (Or) " + Math.Round(feet, 2) + " feet";
                var item = new SelectItems { Text = heightfeetandinches, Value = height };
                if (Heightlist != null) Heightlist.Add(item);
            }
            return Heightlist;
        }
        static readonly List<SelectItems> Weightlist = new List<SelectItems>();
        public static List<SelectItems> GetWeightLookup()
        {
            if (Weightlist.Any()) return Weightlist;
            for (var i = 40; i <= 200; i++)
            {
                var item = new SelectItems { Text = i.ToString(), Value = i.ToString() };
                if (Weightlist != null) Weightlist.Add(item);
            }
            return Weightlist;
        }

        static readonly List<SelectItems> Incomelist = new List<SelectItems>();
        public static List<SelectItems> GetIncomeLookup()
        {
            if (Incomelist.Any()) return Incomelist;
            var item = new SelectItems { Text = "Any", Value = "Any" };
            var itemNotWorking = new SelectItems { Text = "Not Working", Value = "Not Working" };
            if (Incomelist != null)
            {
                Incomelist.Add(itemNotWorking);
                for (var i = 1; i < 20; i++)
                {
                    var j = i + 1;
                    item = new SelectItems
                    {
                        Text = i + " Lakhs to " + j + " Lakhs",
                        Value = i + " Lakhs to " + j + " Lakhs"
                    };
                    Incomelist.Add(item);
                }
                item = new SelectItems { Text = "Above 20 Lakhs", Value = "Above 20 Lakhs" };
                Incomelist.Add(item);
            }
            return Incomelist;
        }
        static readonly List<SelectItems> Agelist = new List<SelectItems>();
        public static IEnumerable<SelectItems> GetAgeLookup()
        {
            if (Agelist.Any()) return Agelist;
            for (var i = 20; i <= 70; i++)
            {
                var item = new SelectItems
                {
                    Text = i.ToString(CultureInfo.InvariantCulture),
                    Value = i.ToString(CultureInfo.InvariantCulture)
                };
                if (Agelist != null) Agelist.Add(item);
            }
            return Agelist;
        }

    }

}