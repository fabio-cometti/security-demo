using FC.SecurityDemo.SQLInjection.Example05.Infrastructure;
using FC.SecurityDemo.SQLInjection.Example05.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FC.SecurityDemo.SQLInjection.Example05.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBManager db;

        public HomeController(ILogger<HomeController> logger, DBManager db)
        {
            _logger = logger;
            this.db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = GetUsers("");
            //var result = GetUsersSafe("");
            ViewBag.Users = result.Users;
            ViewBag.Query = result.Query;
            ViewBag.Filter = "";
            return View();
        }

        [HttpPost]
        public IActionResult Index([FromForm]string filter)
        {
            var result = GetUsers(filter);
            //var result = GetUsersSafe(filter);
            ViewBag.Users = result.Users;
            ViewBag.Query = result.Query;
            ViewBag.Filter = filter;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private (IEnumerable<User> Users, string Query) GetUsers(string filter = null)
        {
            List<User> users;            
            var connection = db.GetConnection();
            string where = string.IsNullOrEmpty(filter) ? "" : $"WHERE Username LIKE '%{filter}%' COLLATE NOCASE";
            string query = $"SELECT Id,Username,Email from Users {where};";

            using (var command = connection.CreateCommand())
            {

                command.CommandText = query;
                users = ReadUsers(command);
            }

            return (users, query);
        }

        private (IEnumerable<User> Users, string Query) GetUsersSafe(string filter = null)
        {
            List<User> users;
            var connection = db.GetConnection();
            string where = string.IsNullOrEmpty(filter) ? "" : $"WHERE Username LIKE '%' || @Filter || '%' COLLATE NOCASE";
            string query = $"SELECT Id,Username,Email from Users {where};";

            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@Filter", filter);
                users = ReadUsers(command);
            }

            return (users, query);
        }



        private List<User> ReadUsers(SqliteCommand command)
        {
            List<User> users = new List<User>();
            var result = command.ExecuteReader();
            while (result.Read())
            {
                User user = new User();
                user.Id = result.GetInt32(0);
                user.Username = result.GetString(1);
                user.Email = result.GetString(2);
                users.Add(user);

            }
            return users;
        }
    }
}
