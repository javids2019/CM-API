using AutoMapper;
using ClassyMatrimonyApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ClassyMatrimonyApi.Controllers
{

    public class ClassyMatriApiController : ApiController
    {
        //string connectionstr = "Data Source=;Initial Catalog=;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        string connectionstr = "";
        ClassyMatrimonyDBEntities _dbcontext = new ClassyMatrimonyDBEntities();

        public ClassyMatriApiController()
        {
            connectionstr = "Data Source=JAVID-TI9768;Initial Catalog=TestDatabse;Integrated Security=True";
        }




        public Employee GetEmployeeByName(string name)
        {
            var query = "SELECT TOP 1 [empid],[empname],[emppassword] FROM Employee where empname = '" + name + "'";
            Employee lst = ReadEmployee<Employee>(query);
            return lst;
        }


        public IEnumerable<Employee> GetEmployees()
        {
            var query = "SELECT [empid],[empname],[emppassword] FROM Employee";
            List<Employee> lst = ReadEmployees<Employee>(query);
            return lst;
        }

        public IEnumerable<Employee> SaveEmployee([FromBody]Employee emp)
        {
            Employee chkemployee = null;

            //if (emp.status == "D" || emp.status == "U")
            //    chkemployee = GetEmployeeById(emp.empid);
            //else if (emp.status == "I")
            //    chkemployee = GetEmployeeByName(emp.empname);

            if (emp.status == "D")
            {
                var query = "delete from Employee where empid=" + emp.empid + "";
                SetEmployee<Employee>(query);
            }
            else if (emp != null && chkemployee == null && emp.status == "I")
            {
                var query = "Insert into Employee(empname,emppassword) values('" + emp.empname + "','" + emp.emppassword + "')";
                SetEmployee<Employee>(query);
            }
            else if (emp != null && chkemployee != null && emp.status == "U")
            {
                var query = "update Employee set empname = '" + emp.empname + "',emppassword= '" + emp.emppassword + "' where empid = " + emp.empid + "";
                SetEmployee<Employee>(query);
            }

            IEnumerable<Employee> lst = GetEmployees();

            return lst;
        }

        private Employee GetEmployeeBytestId(int id = 0)
        {
            var query = "SELECT TOP 1 [empid],[empname],[emppassword] FROM Employee where empid = " + id.ToString() + "";
            Employee lst = ReadEmployee<Employee>(query);
            return lst;
        }


        //public List<SpecStatuses> GetSpecstatus([FromBody]SpecStatuses value)
        //{
        //    var lst = ReadSpec<SpecStatuses>("SELECT [specstatusid],[specstatusname] FROM SpecStatuses");
        //    return lst;
        //}

        public List<T> ReadSpec<T>(string queryString) where T : SpecStatuses
        {
            SqlConnection con = new SqlConnection(connectionstr);
            SqlCommand sqlCmd = new SqlCommand(queryString, con);

            List<T> lst = new List<T>();
            con.Open();
            var dr = sqlCmd.ExecuteReader();

            while (dr.Read())
            {
                T s = Activator.CreateInstance(typeof(T)) as T;
                s.specstatusid = Convert.ToInt16(dr["specstatusid"]);
                s.specstatusname = dr["specstatusname"].ToString();
                lst.Add(s);
            }

            con.Close();
            return lst;
        }

        public T ReadEmployee<T>(string queryString) where T : Employee
        {
            SqlConnection con = new SqlConnection(connectionstr);
            SqlCommand sqlCmd = new SqlCommand(queryString, con);
            T s = null;

            con.Open();
            var dr = sqlCmd.ExecuteReader();

            if (dr.Read())
            {
                s = Activator.CreateInstance(typeof(T)) as T;
                s.empid = Convert.ToInt16(dr["empid"]);
                s.empname = dr["empname"].ToString();
                s.emppassword = dr["emppassword"].ToString();
            }

            con.Close();

            return s;
        }

        public List<T> ReadEmployees<T>(string queryString) where T : Employee
        {
            SqlConnection con = new SqlConnection(connectionstr);
            SqlCommand sqlCmd = new SqlCommand(queryString, con);
            T s = null;

            con.Open();
            var dr = sqlCmd.ExecuteReader();

            List<T> emplist = new List<T>();

            while (dr.Read())
            {
                s = Activator.CreateInstance(typeof(T)) as T;
                s.empid = Convert.ToInt16(dr["empid"]);
                s.empname = dr["empname"].ToString();
                s.emppassword = dr["emppassword"].ToString();
                emplist.Add(s);
            }

            con.Close();

            return emplist;
        }

        public void SetEmployee<T>(string queryString) where T : Employee
        {
            SqlConnection con = new SqlConnection(connectionstr);
            SqlCommand sqlCmd = new SqlCommand(queryString, con);
            con.Open();
            var result = sqlCmd.ExecuteNonQuery();
            con.Close();
        }


        [Route("ClassyMatriApi/Test")]
        [HttpGet]
        [HttpPost]
        public Boolean Test()
        {
            return true;
        }

        [Route("api/ClassyMatriApi/SendMailToAdmin")]
        [HttpGet]
        [HttpPost]
        public bool SendMailToAdmin([FromBody]EnqueryUserModel user)
        {
            var isvalid = true;
            try
            {
                if (user != null && !string.IsNullOrWhiteSpace(user.name) && !string.IsNullOrWhiteSpace(user.lookingfor) &&
                    !string.IsNullOrWhiteSpace(user.mobileno) && !string.IsNullOrWhiteSpace(user.emailid) && !string.IsNullOrWhiteSpace(user.city))
                {
                    //var url = string.Format("{0}://{1}/", System.Web.HttpContext.Current.Request.Url.Scheme, System.Web.HttpContext.Current.Request.Url.Authority);
                    Task.Factory.StartNew(() =>
                    {
                        var subject = "New Enquery for www.ClassyMatrimony.com";
                        var body = "User Name : " + user.name + "</br>" +
                                   "</br>Looking For : " + user.lookingfor + "</br>" +
                                   "</br>Mobile Number : " + user.mobileno + "</br>" +
                                   "</br>EmailID : " + user.emailid + "</br>" +
                                   "</br>City : " + user.city + "</br>";

                        //if (url.Contains("localhost"))
                        //    isvalid = common.SendClientMail.SendDevMail(subject, body);
                        //else
                        isvalid = common.SendClientMail.SendMail(subject, body);

                        SaveRegisterdata(user);
                    });
                }
            }
            catch (Exception ex)
            {
                isvalid = false;
            }
            return isvalid;
        }

        private void SaveRegisterdata(EnqueryUserModel user)
        {
            var model = new RegisterProfile();
            model.Name = user.name;
            model.lookingfor = user.lookingfor;
            model.Mobile = user.mobileno;
            model.Email = user.emailid;
            model.City = user.city;
            model.AllocatedTo = "";
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = "";
            model.IsActive = true;
            _dbcontext = new ClassyMatrimonyDBEntities();
            _dbcontext.RegisterProfiles.Add(model);
            _dbcontext.SaveChanges();
        }

        [HttpPost]
        [Route("api/ClassyMatriApi/File")]
        [AcceptVerbs("POST")]
        public void File(System.Web.HttpPostedFileBase file)
        {
            //string path = "~/UploadedFiles/" + file.FileName;
            //file.SaveAs(path);
            //_transactionService.ImportFile(path);
        }

        [Route("api/ClassyMatriApi/UploadFile")]
        [HttpGet]
        [HttpPost]
        public byte[] UploadFile(HttpPostedFileBase file)
        {
            var basepath = System.Web.Hosting.HostingEnvironment.MapPath("~/UserPhotos").Replace("G:\\PleskVhosts\\", "www.")
                       .Replace("\\www.Classymatrimony.com", "")
                       .Replace("\\home\\UplosadFile", "");

            var url = System.Web.HttpContext.Current.Request.Url.ToString();

            //if (url.Contains("localhost"))
            //    basepath = Server.MapPath("UserPhotos").Replace("\\home\\UploadFile", "");
            //else
            //    basepath = Server.MapPath("UserPhotos").Replace("\\home\\UploadFile", "");

            //var extension = Path.GetExtension(file.FileName);
            //var name = "MAT000" + id + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";
            //var filepath = Path.Combine(basepath, name);
            //var savefile = Path.Combine("..\\..\\UserPhotos", name);
            return null;
        }


        [HttpPost]
        [Route("api/ClassyMatriApi/UploadFileApi/{id}")]
        public HttpResponseMessage UploadJsonFile(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/UploadFile/" + postedFile.FileName);
                    var url = System.Web.HttpContext.Current.Request.Url.ToString();
                    var fileName = Path.GetFileName(postedFile.FileName);
                    var basepath = "";

                    if (url.Contains("localhost"))
                        basepath = HttpContext.Current.Server.MapPath("App_Data\\UserPhotos\\").Replace("api\\ClassyMatriApi\\UploadFileApi\\", "");
                    else
                        basepath = HttpContext.Current.Server.MapPath("App_Data\\UserPhotos\\").Replace("api\\ClassyMatriApi\\UploadFileApi\\", "");

                    var extension = Path.GetExtension(postedFile.FileName);
                    var name = "MAT000" + id + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";
                    var filepath = Path.Combine(basepath, name);
                    var savefile = Path.Combine("..\\..\\UserPhotos", name);

                    //return Json(new { success = false, responseText = "File not Uploaded", filepath = filepath, userId = userid, genderText = gender }, JsonRequestBehavior.AllowGet);
                    try
                    {
                        if (!Directory.Exists(basepath))
                        {
                            DirectoryInfo di = Directory.CreateDirectory(basepath);
                        }

                        postedFile.SaveAs(filepath);
                    }
                    catch (Exception ex)
                    {
                    }

                    if (!string.IsNullOrWhiteSpace(filepath))
                    {
                        var entity = new UserPhotoDetail
                        {
                            FileName = fileName,
                            Path = savefile,
                            CreatedBy = Convert.ToString(id),
                            CreatedOn = DateTime.Now,
                            UserDetailId = id,
                            IsDefault = true,
                        };

                        this._dbcontext.UserPhotos.Add(entity);
                        this._dbcontext.SaveChanges();
                    }
                }
            }
            return response;
        }



        [Route("api/ClassyMatriApi/ManageUsers")]
        [HttpGet]
        [HttpPost]
        public IHttpActionResult ManageUsers([FromBody]UserModel model)
        {
            var validationErrors1 = string.Join(",", ModelState.Values.Where(e => e.Errors.Count > 0).SelectMany(e => e.Errors).Select(e => e.ErrorMessage).ToArray());

            if (validationErrors1 == "")
            {//[FromBody],
                //UserModel model = new UserModel();
                var result = 1;
                var dob = string.IsNullOrWhiteSpace(model.DateOfBirthstr) == true ? new DateTime() : Convert.ToDateTime(model.DateOfBirthstr);
                try
                {

                    var udetails = _dbcontext.UserDetails.FirstOrDefault(x => x.UserDetailsID == model.UserDetailsID);
                    if (udetails != null)
                    {
                        udetails.FirstName = model.FirstName;
                        udetails.LastName = model.LastName;
                        udetails.Address = model.Address;
                        udetails.Email = model.Email;
                        udetails.Gender = model.Gender;
                        udetails.DateOfBirth = dob;
                        udetails.BodyType = model.BodyType;
                        udetails.Complexion = model.Complexion;
                        udetails.PhysicalStatus = model.PhysicalStatus;
                        udetails.Height = model.Height;
                        udetails.Weight = model.Weight;
                        udetails.MaritalStatus = model.MaritalStatus;
                        udetails.NoofChildren = model.NoofChildren;
                        udetails.MotherTongueId = model.MotherTongueId;
                        udetails.LanguagesKnown = model.LanguagesKnown;
                        udetails.ResidingCountryId = model.ResidingCountryId;
                        udetails.ResidingState = model.ResidingState;
                        udetails.ResidingCity = model.ResidingCity;
                        udetails.CitizenshipId = model.CitizenshipId;
                        udetails.CreatedBy = model.CreatedBy;
                        udetails.CreatedOn = model.CreatedOn;
                        udetails.IsUpgraded = model.IsUpgraded;
                        udetails.Role = "User";
                        udetails.UpdatedBy = "Admin";
                        udetails.UpdatedOn = DateTime.Now;

                        using (var dbCtx = new ClassyMatrimonyDBEntities())
                        {
                            dbCtx.Entry(udetails).State = System.Data.Entity.EntityState.Modified;
                            result = dbCtx.SaveChanges();
                        }


                        var pdetails = _dbcontext.PersonalDetails.FirstOrDefault(x => x.UserDetailsID == model.UserDetailsID);

                        if (pdetails != null)
                        {
                            pdetails.Religion = model.Religion;
                            pdetails.Caste = model.Caste;
                            pdetails.SubCaste = model.SubCaste;
                            pdetails.EatingHabit = model.EatingHabit;
                            pdetails.Smoking = model.Smoking;
                            pdetails.DrinkType = model.DrinkType;
                            pdetails.LifeStyle = model.LifeStyle;
                            pdetails.Hobbies = model.Hobbies;
                            pdetails.EducationalQualificationId = model.EducationalQualificationId;
                            pdetails.NameoftheInstitution = model.NameoftheInstitution;
                            pdetails.Occupation = model.Occupation;
                            pdetails.Designation = model.Designation;

                            pdetails.Income = model.Income;
                            pdetails.BusinessType = model.BusinessType;
                            pdetails.Aboutmyself = model.Aboutmyself;
                            pdetails.FamilyType = model.FamilyType;
                            pdetails.FamilyValues = model.FamilyValues;
                            pdetails.FamilyStatus = model.FamilyStatus;
                            pdetails.FamilyOrigin = model.FamilyOrigin;
                            pdetails.NoofSiblings = model.NoofSiblings;
                            pdetails.NOofmarriedSiblings = model.NOofmarriedSiblings;
                            pdetails.Fathersoccupation = model.Fathersoccupation;

                            pdetails.Mothersoccupation = model.Mothersoccupation;
                            pdetails.FamilyBusinesstype = model.FamilyBusinesstype;
                            pdetails.FamilyBusinessTenure = model.FamilyBusinessTenure;
                            pdetails.AboutmyFamily = model.AboutmyFamily;
                            pdetails.CreatedBy = model.CreatedBy;
                            pdetails.CreatedOn = model.CreatedOn;
                            pdetails.Manglik = model.Manglik;
                            pdetails.HoroscopeAvailable = model.HoroscopeAvailable;

                            pdetails.Raasi = model.Raasi;
                            pdetails.Star = model.Star;
                            pdetails.Gothram = model.Gothram;
                            pdetails.Turnover = model.Turnover;


                            pdetails.UpdatedBy = "Admin";
                            pdetails.UpdatedOn = DateTime.Now;
                            pdetails.PersonDetailsID = pdetails.PersonDetailsID;
                            pdetails.UserDetailsID = udetails.UserDetailsID;

                            using (var dbCtx = new ClassyMatrimonyDBEntities())
                            {
                                dbCtx.Entry(pdetails).State = System.Data.Entity.EntityState.Modified;
                                result = dbCtx.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        Mapper.Initialize(cfg =>
                        {
                            cfg.CreateMap<UserModel, UserDetail>();
                            cfg.CreateMap<UserModel, PersonalDetail>();
                        });

                        var userdetail = Mapper.Map<UserModel, UserDetail>(model);
                        userdetail.FirstName = model.FirstName;
                        userdetail.LastName = model.LastName;
                        userdetail.Address = model.Address;
                        userdetail.Email = model.Email;
                        userdetail.Gender = model.Gender;
                        userdetail.MobileNo = model.MobileNo;
                        userdetail.CreatedBy = "Admin";
                        userdetail.CreatedOn = DateTime.Now;
                        userdetail.IsActive = true;
                        userdetail.DateOfBirth = dob;
                        userdetail.Role = "User";
                        userdetail.MatrimonyId = "MAT000" + Convert.ToString((_dbcontext.UserDetails.Max(x => x.UserDetailsID)) + 1);

                        using (var dbCtx = new ClassyMatrimonyDBEntities())
                        {
                            dbCtx.UserDetails.Add(userdetail);
                            result = dbCtx.SaveChanges();
                        }


                        model.UserDetailsID = userdetail.UserDetailsID;

                        var personalDetail = Mapper.Map<UserModel, PersonalDetail>(model);
                        personalDetail.CreatedBy = "Admin";
                        personalDetail.CreatedOn = DateTime.Now;

                        if (result > 0)
                        {
                            using (var dbCtx = new ClassyMatrimonyDBEntities())
                            {

                                personalDetail.UserDetailsID = userdetail.UserDetailsID;
                                dbCtx.PersonalDetails.Add(personalDetail);
                                result = dbCtx.SaveChanges();
                            }
                        }
                        //userid = personalDetail.UserDetailsID;
                    }

                }
                catch (Exception ex)
                {
                    // return Json(new { success = false, responseText = "", UserId = userid, UserName = model.FirstName, Email = model.Email, role = model.Role }, JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    var _model = new EnqueryUserModel();
                    _model.name = model.FirstName + " " + model.LastName;
                    _model.lookingfor = "";
                    _model.mobileno = model.MobileNo;
                    _model.emailid = model.Email;
                    _model.city = model.ResidingCity;
                    SendNotification(_model);
                }
            }
            else
            {
                var validationErrors = string.Join(",", ModelState.Values.Where(e => e.Errors.Count > 0).SelectMany(e => e.Errors).Select(e => e.ErrorMessage).ToArray());
                //return Json(new { success = false, responseText = validationErrors }, JsonRequestBehavior.AllowGet);
            }
            return Ok(new { success = true, responseText = "", UserId = model.UserDetailsID, UserName = model.FirstName, Email = model.Email, role = model.Role });
        }


        private void SendNotification(EnqueryUserModel user)
        {
            if (user != null && !string.IsNullOrWhiteSpace(user.name) && !string.IsNullOrWhiteSpace(user.lookingfor) &&
                    !string.IsNullOrWhiteSpace(user.mobileno) && !string.IsNullOrWhiteSpace(user.emailid) && !string.IsNullOrWhiteSpace(user.city))
            {
                //var url = string.Format("{0}://{1}/", System.Web.HttpContext.Current.Request.Url.Scheme, System.Web.HttpContext.Current.Request.Url.Authority);
                Task.Factory.StartNew(() =>
                {
                    var subject = "New Register for www.ClassyMatrimony.com";
                    var body = "User Name : " + user.name + "</br>" +
                               "</br>Looking For : " + user.lookingfor + "</br>" +
                               "</br>Mobile Number : " + user.mobileno + "</br>" +
                               "</br>EmailID : " + user.emailid + "</br>" +
                               "</br>City : " + user.city + "</br>";


                    var isvalid = common.SendClientMail.SendMail(subject, body);

                    SaveRegisterdata(user);
                });
            }
        }

        [Route("api/ClassyMatriApi/GetUserProfile/{id}")]
        [HttpGet]
        public IEnumerable<GetUserProfileDetails_Result> GetUserProfile(int id)
        {
            return _dbcontext.GetUserProfileDetails(id).ToList();
        }

    }

    public class SpecStatuses
    {
        public int specstatusid { get; set; }
        public string specstatusname { get; set; }
    }
    public class EnqueryUserModel
    {
        public string name { get; set; }
        public string lookingfor { get; set; }
        public string mobileno { get; set; }
        public string emailid { get; set; }
        public string city { get; set; }
    }
    public class Employee
    {
        public int empid { get; set; }
        public string empname { get; set; }
        public string emppassword { get; set; }
        public string status { get; set; }
    }

}