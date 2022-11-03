using CronJob.DomainClasses;
using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CronJob
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IList<GitHubUsers> userlist = new List<GitHubUsers>();
            string mailBody = string.Empty;
            using (var isession = NHibernateHelper.CreateSessionFactory())
            {
                ISession session = isession.OpenSession();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        var icriteria = session.CreateCriteria(typeof(GitHubUsers));
                        userlist = icriteria.List<GitHubUsers>().ToList();

                        foreach (GitHubUsers user in userlist)
                        {
                            GitHubUsersLog log = new GitHubUsersLog
                            {
                                UserId = user.UserId,
                                PublicRepoCount = user.PublicRepoCount,
                                PublicGistCount = user.PublicGistCount,
                                CreateDate = DateTime.Now,
                                CreateUserId = 1
                            };
                            session.Save(log);

                            Users gitUser = SearchGitHubUsers(user.UserName);
                            if (gitUser != null)
                            {
                                user.PublicRepoCount = gitUser.public_repos;
                                user.PublicGistCount = gitUser.public_gists;
                                user.UpdateDate = DateTime.Now;
                                session.SaveOrUpdate(user);
                                if (gitUser.public_repos > log.PublicRepoCount || gitUser.public_gists > log.PublicGistCount)
                                {
                                    mailBody += "<tr><th>"+ gitUser.login+" </th><th>"+ gitUser.public_repos.ToString() +"</th><th>"+ gitUser.public_gists.ToString() +"</th></tr>";
                                }
                            }
                        }

                        transaction.Commit();
                        if (!string.IsNullOrEmpty(mailBody))
                        {
                            string body = @"<!doctype html><html><head><meta name=""viewport"" content=""width=device-width,initial-scale=1""><meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8""></head><body><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""body""><tr><td>&nbsp;</td><td class=""container""><div class=""content""><table role=""presentation"" class=""main""><tr><td class=""wrapper""><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td><p>Hi,</p><p>Below is the list of GitHub violators.</p><table role=""presentation"" border=""1"" cellpadding=""0"" cellspacing=""0"" class=""btn btn-primary""><thead><tr><th style=""text-align:center"">User Name</th><th style=""text-align:center"">Public Repo Count</th><th style=""text-align:center"">Public Gist Count</th></tr></thead><tbody>" + mailBody + @"</tbody></table></td></tr></table></td></tr></table><div class=""footer""><table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0""><tr><td class=""content-block powered-by""><p>&copy; 2022 - GitHub Tracker</p></td></tr></table></div></div></td><td>&nbsp;</td></tr></table></body></html>";

                            SendMail("List of GitHub Violators", body);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine(ex.Message);
                    }                    
                }
            }
        }

        private static Users SearchGitHubUsers(string userName)
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

        private static void SendMail(string subject,string body)
        {
            try
            {               

                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("harshad.parab.cci@gmail.com","CronJob");
                mail.Bcc.Add("harshad.parab.cci@gmail.com");
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential("harshad.parab.cci@gmail.com", "fdgacjpwpzbqaudy");
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
    }
}
