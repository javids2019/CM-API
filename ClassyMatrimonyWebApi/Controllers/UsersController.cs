using AutoMapper;
using ClassyMatrimonyApi;
using ClassyMatrimonyApi.Models;
using Scheduler.API.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace ClassyMatrimonyApi.Controllers
{

    public class UsersController : ApiController
    {
        ClassyMatrimonyDBEntities _dbcontext = null;
        public UsersController()
        {
            _dbcontext = new ClassyMatrimonyDBEntities();
        }

        [Route("api/users/GetAllUsers/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<User>> GetAllUsers(int page = 1)
        {
            int currentPage = page;
            int pageSize = 10;
            int currentPageSize = pageSize;
            var totalUsers = _dbcontext.Users.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            IEnumerable<User> _users = _dbcontext.Users
                .OrderBy(u => u.Id)
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();
            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<User>>(_users, _pagination);
            return result;
        }

        [HttpGet]
        [Route("api/Users/GetUserDetails/{id}")]
        public User GetUserDetails(int id)
        {
            return _dbcontext.Users.FirstOrDefault(s => s.Id == id);
        }

        [HttpGet]
        [Route("api/profiles/GetProfileDetails/{id}")]
        public RegisterProfile GetProfileDetails(int id)
        {
            return _dbcontext.RegisterProfiles.FirstOrDefault(s => s.Id == id);
        }

        [HttpGet]
        [Route("api/profiles/GetFollowupDetails/{id}")]
        public FollowupProfile GetFollowupDetails(int id)
        {
            return _dbcontext.FollowupProfiles.FirstOrDefault(s => s.Id == id);
        }


        [HttpGet]
        [Route("api/profiles/getFollowupsByProfileId/{id}")]
        public IEnumerable<FollowupProfile> getFollowupsByProfileId(int id)
        {
            return _dbcontext.FollowupProfiles.Where(s => s.RegisterProfileId == id).OrderByDescending(x => x.Id).ToList();
        }

        [HttpGet]
        [Route("api/Users/removeProfile/{id}")]
        public bool removeProfile(int id)
        {
            var dets = _dbcontext.FollowupProfiles.Where(s => s.RegisterProfileId == id);
            foreach (var item in dets)
                _dbcontext.FollowupProfiles.Remove(item);


            var _det = _dbcontext.RegisterProfiles.FirstOrDefault(s => s.Id == id);
            _dbcontext.RegisterProfiles.Remove(_det);
            _dbcontext.SaveChanges();

            return true;
        }


        [HttpGet]
        [Route("api/Users/RemoveUser/{id}")]
        public bool RemoveUser(int id)
        {
            var user = _dbcontext.Users.FirstOrDefault(s => s.Id == id);
            _dbcontext.Users.Remove(user);
            var result = _dbcontext.SaveChanges();
            if (result > 0)
                return true;
            else
                return false;
        }
        
        [Route("api/Users/Login")]
        [HttpPost]
        public User Login([FromBody]User  model)
        {
            var user = new User();
            if (ModelState.IsValid && model != null)
                user = _dbcontext.Users.FirstOrDefault(x => x.EmailId == model.EmailId && x.Password == model.Password);
            return user;
        }

        [Route("api/Users/ManageUsers")]
        [HttpPost]
        public bool ManageUsers([FromBody]User model)
        {
            var isvalid = false;
            if (ModelState.IsValid)
            {
                var result = 1;
                try
                {
                    var dt = GetISTDateTime(DateTime.Now);
                    if (model.Id <= 0)
                    {
                        model.CreatedDate = dt;
                        //model.CreatedBy = model.a;
                        model.IsActive = true;
                        _dbcontext.Users.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {
                        model.UpdatedDate = dt;
                        //  model.UpdatedBy = "";

                        using (var dbCtx = new ClassyMatrimonyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.Users.Attach(model);
                            result = dbCtx.SaveChanges();
                        }
                    }
                    isvalid = true;
                }
                catch (Exception ex)
                {
                    isvalid = false;
                }
            }
            return isvalid;
        }


        [Route("api/Profiles/ManageProfile")]
        [HttpPost]
        public bool ManageProfile([FromBody]RegisterProfile model)
        {
            var isvalid = false;
            if (ModelState.IsValid)
            {
                var result = 1;
                try
                {
                    var dt = GetISTDateTime(DateTime.Now);
                    if (model.Id <= 0)
                    {
                        model.CreatedDate = dt;
                        model.CreatedBy = model.AllocatedTo;
                        model.IsActive = true;
                        _dbcontext.RegisterProfiles.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {
                        model.UpdatedDate = dt;
                        model.UpdatedBy = model.AllocatedTo;

                        using (var dbCtx = new ClassyMatrimonyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.RegisterProfiles.Attach(model);
                            result = dbCtx.SaveChanges();
                        }
                    }
                    isvalid = true;
                }
                catch (Exception ex)
                {
                    isvalid = false;
                }
            }
            return isvalid;
        }


        [Route("api/Profiles/GetAllProfile/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<RegisterProfile>> GetAllProfile(int page = 1)
        {
            int currentPage = page;
            int pageSize = 10;
            int currentPageSize = pageSize;
            var totalUsers = _dbcontext.RegisterProfiles.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            var query = _dbcontext.RegisterProfiles.Where(x => x.IsActive == true);

            //if (allocatedto == "")
            //    query = query.Where(x => x.IsActive == true);
            //else if (allocatedto == "FreshLead")
            //    query = query.Where(x => x.AllocatedTo == null);
            //else if (!string.IsNullOrWhiteSpace(allocatedto))
            //    query = query.Where(x => x.AllocatedTo == allocatedto);

            IEnumerable<RegisterProfile> _list = query
                .OrderBy(u => u.Id)
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<RegisterProfile>>(_list, _pagination);
            return result;
        }

        [Route("api/Profiles/GetAllProfileByUser/{allocatedtoid?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<GetFilterProfileDetails_Result>> GetAllProfileByUser(int allocatedtoid = 0)
        {
            int currentPage = 1;
            int pageSize = 1000;
            int currentPageSize = pageSize;
            var totalUsers = _dbcontext.RegisterProfiles.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);
            var allocatedto = _dbcontext.Users.FirstOrDefault(x => x.Id == allocatedtoid).EmailId;
            // query = _dbcontext.RegisterProfiles.Where(x => x.AllocatedTo == allocatedto).ToList();
            var query = _dbcontext.GetFilterProfileDetails("");

            IEnumerable<GetFilterProfileDetails_Result> _list = query.Where(x => x.AllocatedToId == allocatedtoid)
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<GetFilterProfileDetails_Result>>(_list, _pagination);
            return result;
        }



        [Route("api/Profiles/getFreshLeadProfiles/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<RegisterProfile>> GetFreshLeadProfiles(int page = 1)
        {
            int currentPage = page;
            int pageSize = 10;
            int currentPageSize = pageSize;
            var totalUsers = _dbcontext.RegisterProfiles.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            var query = _dbcontext.RegisterProfiles.Where(x => x.IsActive == true && x.AllocatedTo == null);

            //if (allocatedto == "")
            //    query = query.Where(x => x.IsActive == true);
            //else if (allocatedto == "FreshLead")
            //    query = query.Where(x => x.AllocatedTo == null);
            //else if (!string.IsNullOrWhiteSpace(allocatedto))
            //    query = query.Where(x => x.AllocatedTo == allocatedto);

            IEnumerable<RegisterProfile> _list = query
                .OrderBy(u => u.Id)
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();

            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<RegisterProfile>>(_list, _pagination);
            return result;
        }


        [HttpGet]
        [Route("api/Profiles/GetProfiles/{id}")]
        public RegisterProfile GetProfiles(int id)
        {
            return _dbcontext.RegisterProfiles.FirstOrDefault(s => s.Id == id);
        }

        [Route("api/Profiles/ManageFollowupProfile")]
        [HttpPost]
        public bool ManageFollowupProfile([FromBody]FollowupProfile model)
        {
            var isvalid = false;
            if (ModelState.IsValid)
            {
                var result = 1;
                try
                {
                    if (model.Id <= 0)
                    {
                        var dt = GetISTDateTime(DateTime.Now);
                        model.FollowedDate = dt;
                        model.FollowedBy = "";
                        model.IsActive = true;
                        if (model.DispositionStatus == "Lead Closed" || (model.SalesType != null && model.SalesType == "SalesType"))
                            model.SalesType = "SalesType";
                        _dbcontext.FollowupProfiles.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {
                        var dt = GetISTDateTime(DateTime.Now);
                        model.UpdatedDate = dt;
                        model.UpdatedBy = "";

                        using (var dbCtx = new ClassyMatrimonyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.FollowupProfiles.Attach(model);
                            result = dbCtx.SaveChanges();
                        }
                    }
                    isvalid = true;
                }
                catch (Exception ex)
                {
                    isvalid = false;
                }
            }
            return isvalid;
        }

        [Route("api/Profiles/GetAllFollowupProfiles/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<FollowupProfile>> GetAllFollowupProfiles(int page = 1)
        {
            int currentPage = page;
            int pageSize = 10;
            int currentPageSize = pageSize;
            var totalUsers = _dbcontext.FollowupProfiles.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            IEnumerable<FollowupProfile> _list = _dbcontext.FollowupProfiles
                .OrderByDescending(u => u.Id)
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();
            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<FollowupProfile>>(_list, _pagination);
            return result;
        }

        [HttpGet]
        [Route("api/Profiles/GetFollowupProfiles/{id}")]
        public FollowupProfile GetFollowupProfiles(int id)
        {
            return _dbcontext.FollowupProfiles.FirstOrDefault(s => s.Id == id);
        }


        [Route("api/Profiles/GetFilterProfiles")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<GetFilterProfileDetails_Result>> GetFilterProfiles(Filter filter)
        {
            int currentPage = filter.page;
            int pageSize = 20;
            var query = _dbcontext.GetFilterProfileDetails("").AsQueryable();
            query = query.OrderBy(x => x.Id);


            if (filter.allocatedtoId > 0)
                query = query.Where(x => x.AllocatedToId == filter.allocatedtoId);

            if (!string.IsNullOrWhiteSpace(filter.SalesType))
                query = query.Where(x => x.SalesType == filter.SalesType);

            if (filter.registerType != "All")
            {
                if (filter.registerType == "CM")
                    query = query.Where(x => (x.RegisterType == filter.registerType || x.RegisterType == null));
                else
                    query = query.Where(x => x.RegisterType == filter.registerType);
            }
            if (filter.NextFollowup != DateTime.MinValue && filter.NextFollowup != null)
            {
                var _dt = GetISTDateTime(filter.NextFollowup).Date;
                query = query.Where(x => x.NextFollowupDate != null && x.NextFollowupDate.Value.Date == _dt);
            }

            if (filter.CreatedDate != DateTime.MinValue && filter.CreatedDate != null)
            {
                var _dt = GetISTDateTime(filter.CreatedDate).Date;
                query = query.Where(x => x.CreatedDate != null && x.CreatedDate.Value.Date == _dt);
            }

            if (!string.IsNullOrWhiteSpace(filter.allocatedto))
            {
                var strArray = filter.allocatedto.Split(' ');

                if (strArray.Length > 1)
                {
                    var name = strArray[1];
                    query = query.Where(x => x.AllocatedTo == name);
                }
                else if (strArray.Length > 1)
                    query = query.Where(x => x.AllocatedTo == strArray[0]);
            }
            var dt = GetISTDateTime(DateTime.Now).Date;
            if (filter.status == "All")
                query = query.Where(x => x.Id != 0);
            else if (filter.status == "TodaysFollowUp")
                query = query.Where(
                  x => x.NextFollowupDate != null
                && x.DispositionStatus == "FollowUp" &&
                x.NextFollowupDate.Value.Date == dt).OrderBy(x => x.NextFollowupDate);
            else if (filter.status == "OpenLeads")
                query = query.Where(x => x.DispositionStatus == null);
            else if (filter.status == null)
                query = query.Where(x => x.DispositionStatus == null);
            else if (!string.IsNullOrWhiteSpace(filter.status))
                query = query.Where(x => x.DispositionStatus == filter.status);


            var finalquery = query.ToList();

            if (!string.IsNullOrWhiteSpace(filter.MatId))
                finalquery = finalquery.Where(x => x.Matid != null && x.Matid.ToLowerInvariant().StartsWith(filter.MatId.ToLowerInvariant())).ToList();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                finalquery = finalquery.Where(x => x.Name != null && x.Name.ToLowerInvariant().StartsWith(filter.Name.ToLowerInvariant())).ToList();

            if (!string.IsNullOrWhiteSpace(filter.MobileNo))
                finalquery = finalquery.Where(x => x.Mobile != null && x.Mobile.ToLowerInvariant().StartsWith(filter.MobileNo.ToLowerInvariant())).ToList();

            if (!string.IsNullOrWhiteSpace(filter.EmailId))
                finalquery = finalquery.Where(x => x.AllocatedToEmailId != null && x.Email.ToLowerInvariant().StartsWith(filter.EmailId.ToLowerInvariant())).ToList();

            if (!string.IsNullOrWhiteSpace(filter.City))
                finalquery = finalquery.Where(x => x.City != null && x.City.ToLowerInvariant().StartsWith(filter.City.ToLowerInvariant())).ToList();

            if (!string.IsNullOrWhiteSpace(filter.lookingFor))
                finalquery = finalquery.Where(x => x.lookingfor != null && x.lookingfor.ToLowerInvariant().StartsWith(filter.lookingFor.ToLowerInvariant())).ToList();

            if (!string.IsNullOrWhiteSpace(filter.CashType))
                finalquery = finalquery.Where(x => x.CashType != null && x.CashType.ToLowerInvariant().StartsWith(filter.CashType.ToLowerInvariant())).ToList();

            if (!string.IsNullOrWhiteSpace(filter.Duration))
                finalquery = finalquery.Where(x => x.PackageDuration != null && x.PackageDuration.ToLowerInvariant().StartsWith(filter.Duration.ToLowerInvariant())).ToList();

            if (filter.Amount > 0)
                finalquery = finalquery.Where(x => x.Amount != null && x.Amount == filter.Amount).ToList();


            var totalnos = finalquery.Count();
            var totalPages = (int)Math.Ceiling((double)totalnos / pageSize);

            IEnumerable<GetFilterProfileDetails_Result> _list = finalquery
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var _pagination = new Pagination(currentPage, totalPages, totalnos, totalPages);
            var result = new PaginatedResult<IEnumerable<GetFilterProfileDetails_Result>>(_list, _pagination, null);
            return result;
        }

        private static DateTime GetStartOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);
        }
        private static DateTime GetEndOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999);
        }

        [Route("api/Profiles/GetDashBoardData")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<GetFilterProfileDetails_Result>> GetDashBoardData(Filter filter)
        {
            int currentPage = filter.page;
            int pageSize = 20;
            var subquery = _dbcontext.GetFilterProfileDetails("").AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.SalesType))
                subquery = subquery.Where(x => x.SalesType == filter.SalesType);

            if (filter.registerType != "All")
            {
                if (filter.registerType == "CM")
                    subquery = subquery.Where(x => (x.RegisterType == filter.registerType || x.RegisterType == null));
                else
                    subquery = subquery.Where(x => x.RegisterType == filter.registerType);
            }



            if (!string.IsNullOrWhiteSpace(filter.allocatedto))
            {
                var strArray = filter.allocatedto.Split(' ');

                if (strArray.Length > 1)
                {
                    var name = strArray[1];
                    subquery = subquery.Where(x => x.AllocatedTo == name);
                }
                else
                    subquery = subquery.Where(x => x.AllocatedTo == filter.allocatedto);
            }




            var subquerylist = subquery.ToList();

            var dt = GetISTDateTime(DateTime.Now).Date;
            var openLeadsCount = subquerylist.Where(x => x.DispositionStatus == null).Count();

            var todayFollowupCount = 0;
            var leadClosedCount = 0;
            var followupCount = 0;
            var notconnectedCount = 0;
            var notInterestedCount = 0;
            var promiseToPayCount = 0;
            var walkinCount = 0;
            var marriageFixedCount = 0;
            var duplicateRecordCount = 0;
            var rNRCount = 0;
            var switchedOffCount = 0;
            var callReachableCount = 0;
            var totalAllCount = 0;
            var callNotReachableCount = 0;

            todayFollowupCount = subquerylist.Where(x => x.NextFollowupDate != null && x.DispositionStatus == "FollowUp" && x.NextFollowupDate.Value.Date == dt).OrderBy(x => x.NextFollowupDate).Count();

            if (filter.StartDate != DateTime.MinValue && filter.EndDate != DateTime.MinValue && filter.StartDate != null && filter.EndDate != null)
            {
                var std = GetISTDateTime(filter.StartDate).Date;
                var etd = GetISTDateTime(filter.EndDate).Date;

                leadClosedCount = subquerylist.Where(x => x.DispositionStatus == "Lead Closed" && x.CreatedDate.Value.Date >= std && x.CreatedDate.Value.Date <= etd).Count();
                followupCount = subquerylist.Where(x => x.DispositionStatus == "FollowUp" && x.CreatedDate.Value.Date >= std && x.CreatedDate.Value.Date <= etd).Count();
                notconnectedCount = subquerylist.Where(x => x.DispositionStatus == "Not Connected" && x.CreatedDate.Value.Date >= std && x.CreatedDate.Value.Date <= etd).Count();
                notInterestedCount = subquerylist.Where(x => x.DispositionStatus == "Not Interested" && x.CreatedDate.Value.Date >= std && x.CreatedDate.Value.Date <= etd).Count();
                promiseToPayCount = subquerylist.Where(x => x.DispositionStatus == "Promise To Pay" && x.CreatedDate.Value.Date >= std && x.CreatedDate.Value.Date <= etd).Count();
                walkinCount = subquerylist.Where(x => x.DispositionStatus == "Walkin" && x.CreatedDate.Value.Date >= std && x.CreatedDate.Value.Date <= etd).Count();
                marriageFixedCount = subquerylist.Where(x => x.DispositionStatus == "Marriage Fixed" && x.CreatedDate.Value.Date >= std && x.CreatedDate.Value.Date <= etd).Count();
                duplicateRecordCount = subquerylist.Where(x => x.DispositionStatus == "Duplicate Record" && x.CreatedDate.Value.Date >= std && x.CreatedDate.Value.Date <= etd).Count();
                rNRCount = subquerylist.Where(x => x.DispositionStatus == "RNR" && x.CreatedDate.Value.Date >= std && x.CreatedDate.Value.Date <= etd).Count();
                switchedOffCount = subquerylist.Where(x => x.DispositionStatus == "Switched Off" && x.CreatedDate.Value.Date >= std && x.CreatedDate.Value.Date <= etd).Count();

                callReachableCount = subquerylist.Where(x => (x.DispositionStatus == "FollowUp" || x.DispositionStatus == "Lead Closed" ||
                    x.DispositionStatus == "Not Connected" || x.DispositionStatus == "Not Intrested" || x.DispositionStatus == "Promise To Pay" ||
                    x.DispositionStatus == "Walkin" || x.DispositionStatus == "MarriageFixed") && x.CreatedDate.Value.Date >= std && x.CreatedDate.Value.Date <= etd).Count();

                callNotReachableCount = subquerylist.Where(x => (x.DispositionStatus == "RNR" || x.DispositionStatus == "Switched Off") &&
                                        (x.CreatedDate.Value.Date >= std && x.CreatedDate.Value.Date <= etd)).Count();
            }
            else
            {
                leadClosedCount = subquerylist.Where(x => x.DispositionStatus == "Lead Closed").Count();
                followupCount = subquerylist.Where(x => x.DispositionStatus == "FollowUp").Count();
                notconnectedCount = subquerylist.Where(x => x.DispositionStatus == "Not Connected").Count();
                notInterestedCount = subquerylist.Where(x => x.DispositionStatus == "Not Interested").Count();
                promiseToPayCount = subquerylist.Where(x => x.DispositionStatus == "Promise To Pay").Count();
                walkinCount = subquerylist.Where(x => x.DispositionStatus == "Walkin").Count();
                marriageFixedCount = subquerylist.Where(x => x.DispositionStatus == "Marriage Fixed").Count();
                duplicateRecordCount = subquerylist.Where(x => x.DispositionStatus == "Duplicate Record").Count();
                rNRCount = subquerylist.Where(x => x.DispositionStatus == "RNR").Count();
                switchedOffCount = subquerylist.Where(x => x.DispositionStatus == "Switched Off").Count();
                callReachableCount = subquerylist.Where(x => (x.DispositionStatus == "FollowUp" || x.DispositionStatus == "Lead Closed" ||
                       x.DispositionStatus == "Not Connected" || x.DispositionStatus == "Not Intrested" || x.DispositionStatus == "Promise To Pay" ||
                       x.DispositionStatus == "Walkin" || x.DispositionStatus == "MarriageFixed")).Count();
                totalAllCount = subquerylist.Count();

                callNotReachableCount = subquerylist.Where(x => x.DispositionStatus == "RNR" || x.DispositionStatus == "Switched Off").Count();
            }




            Nullable<decimal> targetamount = 0;
            if (filter.allocatedtoId > 0)
                targetamount = _dbcontext.Users.FirstOrDefault(x => x.Id == filter.allocatedtoId).TargetAmount;
            else if (filter.allocatedtoId == 0)
                targetamount = _dbcontext.Users.Where(x => x.Role == "Executive").Select(x => x.TargetAmount).Sum();


            DateTime date = GetISTDateTime(DateTime.Now);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);


            var amount = subquerylist.Where(x => x.DispositionStatus == "Lead Closed" && x.CreatedDate >= firstDayOfMonth && x.CreatedDate
            <= lastDayOfMonth).Select(y => y.Amount).Sum();
            var remAmount = targetamount - amount;
            var totalnos = totalAllCount;
            var totalPages = (int)Math.Ceiling((double)totalnos / pageSize);

            var _pagination = new Pagination(currentPage, totalPages, totalnos, totalPages);
            var _leadsCountResult = new LeadsCountResult(openLeadsCount, todayFollowupCount,
                followupCount, promiseToPayCount, walkinCount, leadClosedCount, totalAllCount,
                callReachableCount, callNotReachableCount, Convert.ToDecimal(targetamount), Convert.ToDecimal(remAmount),
                 notconnectedCount, notInterestedCount, rNRCount, switchedOffCount, marriageFixedCount
                );
            var result = new PaginatedResult<IEnumerable<GetFilterProfileDetails_Result>>(null, _pagination, _leadsCountResult);
            return result;
        }

        private DateTime GetISTDateTime(DateTime dt)
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            if (localZone.StandardName != "India Standard Time")
            {
                var date = dt.AddHours(5.5).ToString();
                return Convert.ToDateTime(date);
            }
            else
                return dt;
        }

    }




}