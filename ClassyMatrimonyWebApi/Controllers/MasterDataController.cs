using MohsyWebApi;
using MohsyWebApi.ViewModels;
using MohsyWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;

namespace MohsyWebApi.Controllers
{
    public class MasterDataController : ApiController
    {

        MohsyDBEntities _dbcontext = null;
        public MasterDataController()
        {
            _dbcontext = new MohsyDBEntities();
        }

        [Route("api/states/GetAllStates/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<StateViewModel>> GetAllStates(int page = 1)
        {
            int currentPage = page;
            int pageSize = 10;
            int currentPageSize = pageSize;
            var totalStates = _dbcontext.StateMasters.Count();
            var totalPages = (int)Math.Ceiling((double)totalStates / pageSize);

            IEnumerable<StateViewModel> _states = _dbcontext.StateMasters.Join(_dbcontext.CountryMasters,
                            x => x.CountryID,
                            y => y.Id,
                           (x, y) => new StateViewModel { Id = x.Id, StateName = x.Name, CountryName = y.Name }).OrderBy(x => x.StateName)
                .Skip((currentPage - 1) * currentPageSize)
                .Take(currentPageSize)
                .ToList();
            var _pagination = new Pagination(currentPage, pageSize, totalStates, totalPages);
            var result = new PaginatedResult<IEnumerable<StateViewModel>>(_states, _pagination);
            return result;
        }


        #region Staff

        [Route("api/mohsyStaff/GetAllStaffs/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<MohsyStaff>> GetAllStaffs(IFilter filter)
        {
            int currentPage = filter.page;
            int currentPageSize = filter.pageSize;
            var totalUsers = _dbcontext.MohsyStaffs.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / filter.pageSize);
            IEnumerable<MohsyStaff> _users = null;
            if (!string.IsNullOrWhiteSpace(filter.serchKey))
            {
                filter.serchKey = filter.serchKey.ToLowerInvariant();
                _users = _dbcontext.MohsyStaffs.Where(x => x.Name.StartsWith(filter.serchKey) || x.EmailId.StartsWith(filter.serchKey) || x.Designation.StartsWith(filter.serchKey) || x.Role.StartsWith(filter.serchKey))
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            else
            {
                _users = _dbcontext.MohsyStaffs
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            var _pagination = new Pagination(currentPage, filter.pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<MohsyStaff>>(_users, _pagination);
            return result;
        }

        [Route("api/mohsyStaff/Login")]
        [HttpPost]
        public MohsyStaff Login([FromBody]MohsyStaff model)
        {
            var user = new MohsyStaff();
            if (ModelState.IsValid && model != null)
                user = _dbcontext.MohsyStaffs.FirstOrDefault(x => x.EmailId == model.EmailId && x.Password == model.Password);
            return user;
        }

        [Route("api/mohsyStaff/ManageUsers")]
        [HttpPost]
        public bool ManageStaffs([FromBody]MohsyStaff model)
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
                        _dbcontext.MohsyStaffs.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {
                        model.UpdatedDate = dt;
                        //  model.UpdatedBy = "";

                        using (var dbCtx = new MohsyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.MohsyStaffs.Attach(model);
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

        [HttpGet]
        [Route("api/mohsyStaff/GetStaffDetails/{id}")]
        public MohsyStaff GetStaffDetails(int id)
        {
            return _dbcontext.MohsyStaffs.FirstOrDefault(s => s.Id == id);
        }

        [HttpGet]
        [Route("api/mohsyStaff/RemoveStaff/{id}")]
        public bool RemoveStaff(int id)
        {
            var result = 0;
            var user = _dbcontext.MohsyStaffs.FirstOrDefault(s => s.Id == id);
            if (user != null)
            {
                _dbcontext.MohsyStaffs.Remove(user);
                result = _dbcontext.SaveChanges();
            }
            if (result > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Settings

        [Route("api/mohsy/GetAllSettings/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<ITConfigSetting>> GetAllSettings(IFilter filter)
        {
            int currentPage = filter.page;
            int pageSize = 10;
            int currentPageSize = filter.pageSize;
            var totalUsers = _dbcontext.ITConfigSettings.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            IEnumerable<ITConfigSetting> _list = null;

            if (!string.IsNullOrWhiteSpace(filter.serchKey))
            {
                _list = _dbcontext.ITConfigSettings.Where(x => x.Name.StartsWith(filter.serchKey) || x.Value.StartsWith(filter.serchKey) || x.Comments.StartsWith(filter.serchKey))
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            else
            {
                _list = _dbcontext.ITConfigSettings
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<ITConfigSetting>>(_list, _pagination);
            return result;
        }



        [Route("api/mohsy/ManageSettings")]
        [HttpPost]
        public bool ManageSettings([FromBody]ITConfigSetting model)
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
                        model.CreatedOn = dt;
                        model.CreatedBy = model.CreatedBy;
                        model.IsActive = true;
                        _dbcontext.ITConfigSettings.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {
                        model.CreatedOn = dt;
                        model.UpdatedBy = model.CreatedBy;

                        using (var dbCtx = new MohsyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.ITConfigSettings.Attach(model);
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

        [HttpGet]
        [Route("api/mohsy/GetSettingsDetails/{id}")]
        public ITConfigSetting GetSettingsDetails(int id)
        {
            return _dbcontext.ITConfigSettings.FirstOrDefault(s => s.Id == id);
        }

        [HttpGet]
        [Route("api/mohsy/RemoveSettings/{id}")]
        public bool RemoveSettings(int id)
        {
            var result = 0;
            var det = _dbcontext.ITConfigSettings.FirstOrDefault(s => s.Id == id);
            if (det != null)
            {
                _dbcontext.ITConfigSettings.Remove(det);
                result = _dbcontext.SaveChanges();
            }
            if (result > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Country

        [Route("api/mohsy/GetAllCountrys/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<MasterItem>> GetAllCountrys(IFilter filter)
        {
            int currentPage = filter.page;
            int pageSize = 10;
            int currentPageSize = filter.pageSize;
            var totalUsers = _dbcontext.CountryMasters.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            IEnumerable<CountryMaster> _list = null;

            if (!string.IsNullOrWhiteSpace(filter.serchKey))
            {
                _list = _dbcontext.CountryMasters.Where(x => x.Name.StartsWith(filter.serchKey) || x.CountryCode.StartsWith(filter.serchKey))
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            else
            {
                _list = _dbcontext.CountryMasters
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }

            var list = _list.Select(x => new MasterItem { Id = x.Id, Name = x.Name, IsActive = x.IsActive });
            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<MasterItem>>(list, _pagination);
            return result;
        }



        [Route("api/mohsy/ManageCountrys")]
        [HttpPost]
        public bool ManageCountryMasters([FromBody]CountryMaster model)
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
                        model.IsActive = true;
                        _dbcontext.CountryMasters.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {


                        using (var dbCtx = new MohsyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.CountryMasters.Attach(model);
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

        [HttpGet]
        [Route("api/mohsy/GetCountryDetails/{id}")]
        public MasterItem GetCountryDetails(int id)
        {
            var x = _dbcontext.CountryMasters.FirstOrDefault(s => s.Id == id);
            return new MasterItem { Id = x.Id, Name = x.Name, IsActive = x.IsActive };
        }

        [HttpGet]
        [Route("api/mohsy/RemoveCountrys/{id}")]
        public bool RemoveCountrys(int id)
        {
            var result = 0;
            var det = _dbcontext.CountryMasters.FirstOrDefault(s => s.Id == id);
            if (det != null)
            {
                _dbcontext.CountryMasters.Remove(det);
                result = _dbcontext.SaveChanges();
            }
            if (result > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Course

        [Route("api/mohsy/GetAllCourses/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<CourseMaster>> GetAllCourses(IFilter filter)
        {
            int currentPage = filter.page;
            int pageSize = 10;
            int currentPageSize = filter.pageSize;
            var totalUsers = _dbcontext.CourseMasters.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            IEnumerable<CourseMaster> _list = null;

            if (!string.IsNullOrWhiteSpace(filter.serchKey))
            {
                _list = _dbcontext.CourseMasters.Where(x => x.Name.StartsWith(filter.serchKey))
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            else
            {
                _list = _dbcontext.CourseMasters
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<CourseMaster>>(_list, _pagination);
            return result;
        }



        [Route("api/mohsy/ManageCourses")]
        [HttpPost]
        public bool ManageCourseMasters([FromBody]CourseMaster model)
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
                        model.IsActive = true;
                        _dbcontext.CourseMasters.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {


                        using (var dbCtx = new MohsyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.CourseMasters.Attach(model);
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

        [HttpGet]
        [Route("api/mohsy/GetCourseDetails/{id}")]
        public CourseMaster GetCoursesDetails(int id)
        {
            return _dbcontext.CourseMasters.FirstOrDefault(s => s.Id == id);
        }

        [HttpGet]
        [Route("api/mohsy/RemoveCourses/{id}")]
        public bool RemoveCourses(int id)
        {
            var result = 0;
            var det = _dbcontext.CourseMasters.FirstOrDefault(s => s.Id == id);
            if (det != null)
            {
                _dbcontext.CourseMasters.Remove(det);
                result = _dbcontext.SaveChanges();
            }
            if (result > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region MotherTongue

        [Route("api/mohsy/GetAllMotherTongues/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<MotherTongueMaster>> GetAllMotherTongues(IFilter filter)
        {
            int currentPage = filter.page;
            int pageSize = 10;
            int currentPageSize = filter.pageSize;
            var totalUsers = _dbcontext.MotherTongueMasters.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            IEnumerable<MotherTongueMaster> _list = null;

            if (!string.IsNullOrWhiteSpace(filter.serchKey))
            {
                _list = _dbcontext.MotherTongueMasters.Where(x => x.Name.StartsWith(filter.serchKey))
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            else
            {
                _list = _dbcontext.MotherTongueMasters
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<MotherTongueMaster>>(_list, _pagination);
            return result;
        }



        [Route("api/mohsy/ManageMotherTongues")]
        [HttpPost]
        public bool ManageMotherTongueMasters([FromBody]MotherTongueMaster model)
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
                        model.IsActive = true;
                        _dbcontext.MotherTongueMasters.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {


                        using (var dbCtx = new MohsyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.MotherTongueMasters.Attach(model);
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

        [HttpGet]
        [Route("api/mohsy/GetMotherTongueDetails/{id}")]
        public MotherTongueMaster GetMotherTonguesDetails(int id)
        {
            return _dbcontext.MotherTongueMasters.FirstOrDefault(s => s.Id == id);
        }

        [HttpGet]
        [Route("api/mohsy/RemoveMotherTongues/{id}")]
        public bool RemoveMotherTongues(int id)
        {
            var result = 0;
            var det = _dbcontext.MotherTongueMasters.FirstOrDefault(s => s.Id == id);
            if (det != null)
            {
                _dbcontext.MotherTongueMasters.Remove(det);
                result = _dbcontext.SaveChanges();
            }
            if (result > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Division

        [Route("api/mohsy/GetAllDivisions/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<DivisionMaster>> GetAllDivisions(IFilter filter)
        {
            int currentPage = filter.page;
            int pageSize = 10;
            int currentPageSize = filter.pageSize;
            var totalUsers = _dbcontext.DivisionMasters.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            IEnumerable<DivisionMaster> _list = null;

            if (!string.IsNullOrWhiteSpace(filter.serchKey))
            {
                _list = _dbcontext.DivisionMasters.Where(x => x.Name.StartsWith(filter.serchKey))
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            else
            {
                _list = _dbcontext.DivisionMasters
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<DivisionMaster>>(_list, _pagination);
            return result;
        }



        [Route("api/mohsy/ManageDivisions")]
        [HttpPost]
        public bool ManageDivisionMasters([FromBody]DivisionMaster model)
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
                        model.IsActive = true;
                        _dbcontext.DivisionMasters.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {


                        using (var dbCtx = new MohsyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.DivisionMasters.Attach(model);
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

        [HttpGet]
        [Route("api/mohsy/GetDivisionDetails/{id}")]
        public DivisionMaster GetDivisionsDetails(int id)
        {
            return _dbcontext.DivisionMasters.FirstOrDefault(s => s.Id == id);
        }

        [HttpGet]
        [Route("api/mohsy/RemoveDivisions/{id}")]
        public bool RemoveDivisions(int id)
        {
            var result = 0;
            var det = _dbcontext.DivisionMasters.FirstOrDefault(s => s.Id == id);
            if (det != null)
            {
                _dbcontext.DivisionMasters.Remove(det);
                result = _dbcontext.SaveChanges();
            }
            if (result > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Template

        [Route("api/mohsy/GetAllTemplates/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<TemplateMaster>> GetAllTemplates(IFilter filter)
        {
            int currentPage = filter.page;
            int pageSize = 10;
            int currentPageSize = filter.pageSize;
            var totalUsers = _dbcontext.TemplateMasters.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            IEnumerable<TemplateMaster> _list = null;

            if (!string.IsNullOrWhiteSpace(filter.serchKey))
            {
                _list = _dbcontext.TemplateMasters.Where(x => x.Name.StartsWith(filter.serchKey))
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            else
            {
                _list = _dbcontext.TemplateMasters
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<TemplateMaster>>(_list, _pagination);
            return result;
        }



        [Route("api/mohsy/ManageTemplates")]
        [HttpPost]
        public bool ManageTemplateMasters([FromBody]TemplateMaster model)
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
                        model.IsActive = true;
                        _dbcontext.TemplateMasters.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {


                        using (var dbCtx = new MohsyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.TemplateMasters.Attach(model);
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

        [HttpGet]
        [Route("api/mohsy/GetTemplateDetails/{id}")]
        public TemplateMaster GetTemplatesDetails(int id)
        {
            return _dbcontext.TemplateMasters.FirstOrDefault(s => s.Id == id);
        }

        [HttpGet]
        [Route("api/mohsy/RemoveTemplates/{id}")]
        public bool RemoveTemplates(int id)
        {
            var result = 0;
            var det = _dbcontext.TemplateMasters.FirstOrDefault(s => s.Id == id);
            if (det != null)
            {
                _dbcontext.TemplateMasters.Remove(det);
                result = _dbcontext.SaveChanges();
            }
            if (result > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Caste

        [Route("api/mohsy/GetAllCastes/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<CasteMaster>> GetAllCastes(IFilter filter)
        {
            int currentPage = filter.page;
            int pageSize = 10;
            int currentPageSize = filter.pageSize;
            var totalUsers = _dbcontext.CasteMasters.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            IEnumerable<CasteMaster> _list = null;

            if (!string.IsNullOrWhiteSpace(filter.serchKey))
            {
                _list = _dbcontext.CasteMasters.Where(x => x.Name.StartsWith(filter.serchKey))
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            else
            {
                _list = _dbcontext.CasteMasters
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<CasteMaster>>(_list, _pagination);
            return result;
        }



        [Route("api/mohsy/ManageCastes")]
        [HttpPost]
        public bool ManageCasteMasters([FromBody]CasteMaster model)
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
                        model.IsActive = true;
                        _dbcontext.CasteMasters.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {


                        using (var dbCtx = new MohsyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.CasteMasters.Attach(model);
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

        [HttpGet]
        [Route("api/mohsy/GetCasteDetails/{id}")]
        public CasteMaster GetCastesDetails(int id)
        {
            return _dbcontext.CasteMasters.FirstOrDefault(s => s.Id == id);
        }

        [HttpGet]
        [Route("api/mohsy/RemoveCastes/{id}")]
        public bool RemoveCastes(int id)
        {
            var result = 0;
            var det = _dbcontext.CasteMasters.FirstOrDefault(s => s.Id == id);
            if (det != null)
            {
                _dbcontext.CasteMasters.Remove(det);
                result = _dbcontext.SaveChanges();
            }
            if (result > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region City

        [Route("api/mohsy/GetAllCitys/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<CityMaster>> GetAllCitys(IFilter filter)
        {
            int currentPage = filter.page;
            int pageSize = 10;
            int currentPageSize = filter.pageSize;
            var totalUsers = _dbcontext.CityMasters.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            IEnumerable<CityMaster> _list = null;

            if (!string.IsNullOrWhiteSpace(filter.serchKey))
            {
                _list = _dbcontext.CityMasters.Where(x => x.Name.StartsWith(filter.serchKey))
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            else
            {
                _list = _dbcontext.CityMasters
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<CityMaster>>(_list, _pagination);
            return result;
        }



        [Route("api/mohsy/ManageCitys")]
        [HttpPost]
        public bool ManageCityMasters([FromBody]CityMaster model)
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
                        // model.IsActive = true;
                        _dbcontext.CityMasters.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {


                        using (var dbCtx = new MohsyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.CityMasters.Attach(model);
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

        [HttpGet]
        [Route("api/mohsy/GetCityDetails/{id}")]
        public CityMaster GetCitysDetails(int id)
        {
            return _dbcontext.CityMasters.FirstOrDefault(s => s.Id == id);
        }

        [HttpGet]
        [Route("api/mohsy/RemoveCitys/{id}")]
        public bool RemoveCitys(int id)
        {
            var result = 0;
            var det = _dbcontext.CityMasters.FirstOrDefault(s => s.Id == id);
            if (det != null)
            {
                _dbcontext.CityMasters.Remove(det);
                result = _dbcontext.SaveChanges();
            }
            if (result > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Religion

        [Route("api/mohsy/GetAllReligions/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<ReligionMaster>> GetAllReligions(IFilter filter)
        {
            int currentPage = filter.page;
            int pageSize = 10;
            int currentPageSize = filter.pageSize;
            var totalUsers = _dbcontext.ReligionMasters.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            IEnumerable<ReligionMaster> _list = null;

            if (!string.IsNullOrWhiteSpace(filter.serchKey))
            {
                _list = _dbcontext.ReligionMasters.Where(x => x.Name.StartsWith(filter.serchKey))
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            else
            {
                _list = _dbcontext.ReligionMasters
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<ReligionMaster>>(_list, _pagination);
            return result;
        }



        [Route("api/mohsy/ManageReligions")]
        [HttpPost]
        public bool ManageReligionMasters([FromBody]ReligionMaster model)
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
                        model.IsActive = true;
                        _dbcontext.ReligionMasters.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {


                        using (var dbCtx = new MohsyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.ReligionMasters.Attach(model);
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

        [HttpGet]
        [Route("api/mohsy/GetReligionDetails/{id}")]
        public ReligionMaster GetReligionsDetails(int id)
        {
            return _dbcontext.ReligionMasters.FirstOrDefault(s => s.Id == id);
        }

        [HttpGet]
        [Route("api/mohsy/RemoveReligions/{id}")]
        public bool RemoveReligions(int id)
        {
            var result = 0;
            var det = _dbcontext.ReligionMasters.FirstOrDefault(s => s.Id == id);
            if (det != null)
            {
                _dbcontext.ReligionMasters.Remove(det);
                result = _dbcontext.SaveChanges();
            }
            if (result > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Designation

        [Route("api/mohsy/GetAllDesignations/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<DesignationMaster>> GetAllDesignations(IFilter filter)
        {
            int currentPage = filter.page;
            int pageSize = 10;
            int currentPageSize = filter.pageSize;
            var totalUsers = _dbcontext.DesignationMasters.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            IEnumerable<DesignationMaster> _list = null;

            if (!string.IsNullOrWhiteSpace(filter.serchKey))
            {
                _list = _dbcontext.DesignationMasters.Where(x => x.Name.StartsWith(filter.serchKey))
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            else
            {
                _list = _dbcontext.DesignationMasters
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<DesignationMaster>>(_list, _pagination);
            return result;
        }



        [Route("api/mohsy/ManageDesignations")]
        [HttpPost]
        public bool ManageDesignationMasters([FromBody]DesignationMaster model)
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
                        model.IsActive = true;
                        _dbcontext.DesignationMasters.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {


                        using (var dbCtx = new MohsyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.DesignationMasters.Attach(model);
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

        [HttpGet]
        [Route("api/mohsy/GetDesignationDetails/{id}")]
        public DesignationMaster GetDesignationsDetails(int id)
        {
            return _dbcontext.DesignationMasters.FirstOrDefault(s => s.Id == id);
        }

        [HttpGet]
        [Route("api/mohsy/RemoveDesignations/{id}")]
        public bool RemoveDesignations(int id)
        {
            var result = 0;
            var det = _dbcontext.DesignationMasters.FirstOrDefault(s => s.Id == id);
            if (det != null)
            {
                _dbcontext.DesignationMasters.Remove(det);
                result = _dbcontext.SaveChanges();
            }
            if (result > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region EmployeeIn

        [Route("api/mohsy/GetAllEmployeeIns/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<EmployeeInMaster>> GetAllEmployeeIns(IFilter filter)
        {
            int currentPage = filter.page;
            int pageSize = 10;
            int currentPageSize = filter.pageSize;
            var totalUsers = _dbcontext.EmployeeInMasters.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            IEnumerable<EmployeeInMaster> _list = null;

            if (!string.IsNullOrWhiteSpace(filter.serchKey))
            {
                _list = _dbcontext.EmployeeInMasters.Where(x => x.Name.StartsWith(filter.serchKey))
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            else
            {
                _list = _dbcontext.EmployeeInMasters
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<EmployeeInMaster>>(_list, _pagination);
            return result;
        }



        [Route("api/mohsy/ManageEmployeeIns")]
        [HttpPost]
        public bool ManageEmployeeInMasters([FromBody]EmployeeInMaster model)
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
                        model.IsActive = true;
                        _dbcontext.EmployeeInMasters.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {


                        using (var dbCtx = new MohsyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.EmployeeInMasters.Attach(model);
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

        [HttpGet]
        [Route("api/mohsy/GetEmployeeInDetails/{id}")]
        public EmployeeInMaster GetEmployeeInsDetails(int id)
        {
            return _dbcontext.EmployeeInMasters.FirstOrDefault(s => s.Id == id);
        }

        [HttpGet]
        [Route("api/mohsy/RemoveEmployeeIns/{id}")]
        public bool RemoveEmployeeIns(int id)
        {
            var result = 0;
            var det = _dbcontext.EmployeeInMasters.FirstOrDefault(s => s.Id == id);
            if (det != null)
            {
                _dbcontext.EmployeeInMasters.Remove(det);
                result = _dbcontext.SaveChanges();
            }
            if (result > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region State

        [Route("api/mohsy/GetAllstates/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<StateMaster>> GetAllStates(IFilter filter)
        {
            int currentPage = filter.page;
            int pageSize = 10;
            int currentPageSize = filter.pageSize;
            var totalUsers = _dbcontext.StateMasters.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            IEnumerable<StateMaster> _list = null;

            if (!string.IsNullOrWhiteSpace(filter.serchKey))
            {
                _list = _dbcontext.StateMasters.Where(x => x.Name.StartsWith(filter.serchKey))
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            else
            {
                _list = _dbcontext.StateMasters
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<StateMaster>>(_list, _pagination);
            return result;
        }



        [Route("api/mohsy/ManageStates")]
        [HttpPost]
        public bool ManageStateMasters([FromBody]StateMaster model)
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
                        //model.IsActive = true;
                        _dbcontext.StateMasters.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {


                        using (var dbCtx = new MohsyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.StateMasters.Attach(model);
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

        [HttpGet]
        [Route("api/mohsy/GetStateDetails/{id}")]
        public StateMaster GetStatesDetails(int id)
        {
            return _dbcontext.StateMasters.FirstOrDefault(s => s.Id == id);
        }

        [HttpGet]
        [Route("api/mohsy/RemoveStates/{id}")]
        public bool RemoveStates(int id)
        {
            var result = 0;
            var det = _dbcontext.StateMasters.FirstOrDefault(s => s.Id == id);
            if (det != null)
            {
                _dbcontext.StateMasters.Remove(det);
                result = _dbcontext.SaveChanges();
            }
            if (result > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region UserPhotos

        [Route("api/mohsy/GetAllUserPhotos/{page?}")]
        [HttpGet]
        [HttpPost]
        public PaginatedResult<IEnumerable<UserPhoto>> GetAllUserPhotos(IFilter filter)
        {
            int currentPage = filter.page;
            int pageSize = 10;
            int currentPageSize = filter.pageSize;
            var totalUsers = _dbcontext.UserPhotos.Count();
            var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            IEnumerable<UserPhoto> _list = null;

            if (!string.IsNullOrWhiteSpace(filter.serchKey))
            {
                _list = _dbcontext.UserPhotos.Where(x => x.FileName.StartsWith(filter.serchKey))
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            else
            {
                _list = _dbcontext.UserPhotos
                    .OrderBy(u => u.Id)
                    .Skip((currentPage - 1) * currentPageSize)
                    .Take(currentPageSize)
                    .ToList();
            }
            var _pagination = new Pagination(currentPage, pageSize, totalUsers, totalPages);
            var result = new PaginatedResult<IEnumerable<UserPhoto>>(_list, _pagination);
            return result;
        }



        [Route("api/mohsy/ManageUserPhotos")]
        [HttpPost]
        public bool ManageUserPhotos([FromBody]UserPhoto model)
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
                        model.IsActive = true;
                        _dbcontext.UserPhotos.Add(model);
                        result = _dbcontext.SaveChanges();
                    }
                    else
                    {


                        using (var dbCtx = new MohsyDBEntities())
                        {
                            dbCtx.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            _dbcontext.UserPhotos.Attach(model);
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

        [HttpGet]
        [Route("api/mohsy/GetUserPhotoDetails/{id}")]
        public UserPhoto GetUserPhotosDetails(int id)
        {
            return _dbcontext.UserPhotos.FirstOrDefault(s => s.Id == id);
        }

        [HttpGet]
        [Route("api/mohsy/RemoveUserPhotos/{id}")]
        public bool RemoveUserPhotos(int id)
        {
            var result = 0;
            var det = _dbcontext.UserPhotos.FirstOrDefault(s => s.Id == id);
            if (det != null)
            {
                _dbcontext.UserPhotos.Remove(det);
                result = _dbcontext.SaveChanges();
            }
            if (result > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region Common 
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
        #endregion


        //[Route("api/countries/GetAllCountries")]
        //[HttpGet]
        //public IEnumerable<CountryMaster> GetAllCountries()
        //{
        //    return _dbcontext.CountryMasters.ToList();
        //}
    }
}
