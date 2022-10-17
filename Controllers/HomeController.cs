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

namespace GitHubTracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["UserId"] = 1;
            GitHubUsersDaoImpl gitHubUsersDaoImpl = new GitHubUsersDaoImpl();
            GitHubUsers user = gitHubUsersDaoImpl.GetUserById(2);


            List<GitHubUsers> users = gitHubUsersDaoImpl.GetUsers();
            if (users.Any(x => x.Name == "Harshad"))
            {

            }
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
                ViewBag.Message = "Saved Successfully";
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SearchUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchUser(VmAddUser request)
        {
            VmAddUser gitHubUsers = new VmAddUser();
            try
            {
                string url = "https://api.github.com/users/" + request.UserName;
                string token = "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";

                using (var client = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Bearer " + token);
                    var response = client.GetAsync(url).Result;
                    Users rep = JsonConvert.DeserializeObject<Users>(response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : string.Empty);
                    gitHubUsers.UserName = rep.login;
                    gitHubUsers.Name = rep.name;
                    gitHubUsers.PublicRepoCount = rep.public_repos;
                    gitHubUsers.PublicGistCount = rep.public_gists;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }

            return View("AddUser", gitHubUsers);
        }

        public ActionResult UserDetails()
        {
            GitHubUsersDaoImpl gitHubUsersDaoImpl = new GitHubUsersDaoImpl();
            VmUserDetails vmUserDetails = new VmUserDetails
            {
                Users = gitHubUsersDaoImpl.GetUsers()
            };
            return View(vmUserDetails);
        }

    }
}