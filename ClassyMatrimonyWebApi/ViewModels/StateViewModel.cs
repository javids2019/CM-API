using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MohsyWebApi.ViewModels
{
    public class StateViewModel
    {
        public int Id { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}