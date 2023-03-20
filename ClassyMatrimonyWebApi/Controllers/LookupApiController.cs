using ClassyMatrimonyApi.common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
namespace ClassyMatrimonyApi.Controllers
{

    public class LookupApiController : ApiController
    {
        public LookupApiController()
        {
        }

        //[Route("api/LookupApi/GetDivisionLookups")]
        //[HttpGet]
        //public IEnumerable<SelectItems> GetDivisionLookups()
        //{
        //    return Utility.GetDivisionLookup();
        //}

        //[Route("api/LookupApi/GetLanguagesLookups")]
        //[HttpGet]
        //public IEnumerable<SelectItems> GetLanguagesLookups()
        //{
        //    return Utility.GetLanguagesLookup();
        //}

        //[Route("api/LookupApi/GetHeightLookups")]
        //[HttpGet]
        //public IEnumerable<SelectItems> GetHeightLookups()
        //{
        //    return Utility.GetHeightLookup();
        //}
        //[Route("api/LookupApi/GetWeightLookup")]
        //[HttpGet]
        //public IEnumerable<SelectItems> GetWeightLookup()
        //{
        //    return Utility.GetWeightLookup();
        //}

        //[Route("api/LookupApi/GetIncomeLookups")]
        //[HttpGet]
        //public IEnumerable<SelectItems> GetIncomeLookups()
        //{
        //    return Utility.GetIncomeLookup();
        //}

        //[Route("api/LookupApi/GetCountryLookups")]
        //[HttpGet]
        //public IEnumerable<SelectItems> GetCountryLookups()
        //{
        //    return Utility.GetCountryLookups();
        //}

        //[Route("api/LookupApi/GetCourseLookups")]
        //[HttpGet]
        //public IEnumerable<SelectItems> GetCourseLookups()
        //{
        //    return Utility.GetCourseLookups();
        //}
        //[Route("api/LookupApi/GetEmployedinLookups")]
        //[HttpGet]
        //public IEnumerable<SelectItems> GetEmployedinLookups()
        //{
        //    return Utility.GetEmployeeLookup();
        //}
        //[Route("api/LookupApi/GetAgeLookup")]
        //[HttpGet]
        //public IEnumerable<SelectItems> GetAgeLookup()
        //{
        //    return Utility.GetAgeLookup();
        //}
        //[Route("api/LookupApi/GetStatesLookups")]
        //[HttpGet]
        //public IEnumerable<SelectItems> GetStatesLookups()
        //{
        //   var stateList = Utility.GetCitysLookup().Where(y => y != null).Select(x => x.State)
        //        .Distinct(StringComparer.CurrentCultureIgnoreCase).Select(x => new SelectItems(x, x)).ToList();
        //    return stateList;
        //}
        //[Route("api/LookupApi/GetCitysLookups/{stateName}")]
        //[HttpGet]
        //public IEnumerable<SelectItems> GetCitysLookups(string stateName)
        //{
        //    var cityList = Utility.GetCitysLookup().Where(x => x.State != null && x.State.Equals(stateName))
        //        .Select(x => x.Name).Distinct(StringComparer.CurrentCultureIgnoreCase)
        //        .Select(x => new SelectItems(x, x)).ToList();
        //    return cityList;
        //}

    }

}