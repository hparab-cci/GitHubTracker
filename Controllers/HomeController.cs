using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using GitHubTracker.DataAccessLayer;
using GitHubTracker.Models;
using GitHubTracker.NHibernateMapping;
using Newtonsoft.Json;
using GitHubTracker.ViewModels;
using System.IO;
using System.Web.Security;

namespace GitHubTracker.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(VmUser user)
        {
            string message = string.Empty;
            if (user.Username == "Harshad" && user.Password == "pass")
            {
                FormsAuthentication.SetAuthCookie(user.Username, false);
                Session["UserId"] = 1;
                return RedirectToAction("Index");
            }
            else
            {
                message = "Username and/or password is incorrect.";
            }
            ViewBag.Message = message;
            return View(user);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["UserId"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult Index()
        {
            GitHubUsersDaoImpl gitHubUsersDaoImpl = new GitHubUsersDaoImpl();

            List<GitHubUsers> users = gitHubUsersDaoImpl.GetUsers();            
            VmDashboard vmDashboard = new VmDashboard
            {
                UserCount = users.Count,
                PublicRepoCount = users.Select(x => x.PublicRepoCount).Sum(),
                PublicGistCount = users.Select(x => x.PublicGistCount).Sum()
            };
            return View(vmDashboard);
        }

        public ActionResult AddUser()
        {
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(VmAddUser request)
        {
            try
            {
                GitHubUsersImpl gitHubUsers = new GitHubUsersImpl
                {
                    UserName = request.UserName,
                    Name = request.Name,
                    PublicRepoCount = request.PublicRepoCount,
                    PublicGistCount = request.PublicGistCount,
                    CreateDate = DateTime.Now,
                    CreateUserId = (int)Session["UserId"],
                    UpdateDate = null
                };

                GitHubUsersDaoImpl gitHubUsersDaoImpl = new GitHubUsersDaoImpl();
                gitHubUsersDaoImpl.CreateUser(gitHubUsers);
                TempData["SuccessMsg"] = "Saved Successfully";
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return RedirectToAction("UserDetails");
        }

        public ActionResult SearchUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchUser(VmAddUser request)
        {
            VmAddUser gitHubUsers = new VmAddUser();
            GitHubUsersDaoImpl gitHubUser = new GitHubUsersDaoImpl();
            List<GitHubUsers> users = gitHubUser.GetUsers();
            try
            {
                Users rep = SearchGitHubUsers(request.UserName);
                if (rep != null)
                {
                    gitHubUsers.UserName = rep.login;
                    gitHubUsers.Name = rep.name ?? "No Name";
                    gitHubUsers.PublicRepoCount = rep.public_repos;
                    gitHubUsers.PublicGistCount = rep.public_gists;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            if (gitHubUsers.UserName == null)
            {
                TempData["ErrorMsg"] = "User not found";
                return View();
            }
            else if (users.Any(x => x.UserName == request.UserName))
            {
                TempData["ErrorMsg"] = "User already exists";
                return View();
            }
            else
                return View("AddUser", gitHubUsers);
        }

        public ActionResult UserDetails()
        {
            GitHubUsersDaoImpl gitHubUsersDaoImpl = new GitHubUsersDaoImpl();
            VmUserDetails vmUserDetails = new VmUserDetails
            {
                Users = gitHubUsersDaoImpl.GetUsers().OrderByDescending(x => x.PublicRepoCount).ToList()
            };
            return View(vmUserDetails);
        }

        public ActionResult DeleteUser(int userId)
        {
            GitHubUsersDaoImpl gitHubUsersDaoImpl = new GitHubUsersDaoImpl();
            GitHubUsers user = gitHubUsersDaoImpl.GetUserById(userId);
            if (user != null)
            {
                gitHubUsersDaoImpl.Delete((GitHubUsersImpl)user);
                TempData["DeleteMsg"] = "Deleted Successfully";
            }

            return RedirectToAction("UserDetails");
        }

        public ActionResult CSVUpload()
        {
            return View(new List<GitHubUsers>());
        }

        [HttpPost]
        public ActionResult CSVUpload(HttpPostedFileBase postedFile)
        {
            List<GitHubUsers> users = new List<GitHubUsers>();
            string filePath = string.Empty;
            if (postedFile != null && postedFile.ContentType== "application/vnd.ms-excel")
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                //Read the contents of CSV file.
                string csvData = System.IO.File.ReadAllText(filePath);

                if (csvData.Replace("\r", "").Length > 1)
                {
                    GitHubUsersDaoImpl gitHubUser = new GitHubUsersDaoImpl();
                    List<GitHubUsers> userlst = gitHubUser.GetUsers();
                    string remarks = string.Empty;
                    //Execute a loop over the rows.
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            Users rep = SearchGitHubUsers(row.Split(',')[0]);
                            if (userlst.Any(x => x.UserName.Equals(row.Split(',')[0])))
                            {
                                remarks = "User already exists";
                            }
                            else if (rep == null)
                            {
                                remarks = "User not found";
                            }
                            else
                            {
                                GitHubUsersImpl gitHubUsers = new GitHubUsersImpl
                                {
                                    UserName = rep.login,
                                    Name = rep.name ?? "No Name",
                                    PublicRepoCount = rep.public_repos,
                                    PublicGistCount = rep.public_gists,
                                    CreateDate = DateTime.Now,
                                    CreateUserId = (int)Session["UserId"],
                                    UpdateDate = null
                                };
                                gitHubUser.CreateUser(gitHubUsers);
                                remarks = "User added successfully";
                            }
                            users.Add(new GitHubUsersImpl
                            {
                                UserName = row.Split(',')[0],
                                Name = row.Split(',')[1],
                                Remarks = remarks
                            });
                        }
                    }
                }
                else
                {
                    TempData["ErrorMsg"] = "Empty file uploaded";
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Incorrect file format";
            }
            return View(users);
        }

        public ActionResult UpdateCount()
        {
            GitHubUsersDaoImpl userList = new GitHubUsersDaoImpl();
            List<GitHubUsers> users = userList.GetUsers();
            foreach (var user in users)
            {
                Users rep = SearchGitHubUsers(user.UserName);
                user.PublicRepoCount = rep.public_repos;
                user.PublicGistCount=rep.public_gists;
                user.UpdateDate = DateTime.Now;
                userList.Update((GitHubUsersImpl)user);
            }
            return RedirectToAction("Index");
        }

        private Users SearchGitHubUsers(string userName)
        {
            string url = System.Configuration.ConfigurationManager.AppSettings["GitHubURL"].ToString() + userName;
            string token = System.Configuration.ConfigurationManager.AppSettings["Token"].ToString();
            
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Bearer " + token);
                var response = client.GetAsync(url).Result;
                Users rep = JsonConvert.DeserializeObject<Users>(response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : string.Empty);
                return rep;
            }
        }

    }
}