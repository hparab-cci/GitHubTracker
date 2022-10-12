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
            //string url = "https://api.github.com/users/hparab-cci";
            //string token = "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";            
            Session["UserId"] = 1;

            //using (var client = new HttpClient())
            //{
            //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Bearer " + token);
            //    var response = client.GetAsync(url).Result;
            //    Users rep = JsonConvert.DeserializeObject<Users>(response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : string.Empty);
            //    GitHubUsersImpl gitHubUsers = new GitHubUsersImpl{
            //        UserName = rep.login,
            //        EmailId = rep.name,
            //        PublicRepoCount=rep.public_repos,
            //        PublicGistCount=rep.public_gists,
            //        CreateDate=DateTime.Now,
            //        CreateUserId= (int)Session["UserId"],
            //        UpdateDate=null
            //        };               
            //}
            GitHubUsersDaoImpl gitHubUsersDaoImpl = new GitHubUsersDaoImpl();
            //gitHubUsersDaoImpl.CreateUser(gitHubUsers);
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
                    EmailId = request.EmailId,
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
    }
}