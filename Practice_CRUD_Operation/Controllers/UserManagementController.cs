using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Practice_CRUD_Operation.Models;

namespace Practice_CRUD_Operation.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly IUserManagement _userManagement;
        public UserManagementController(IUserManagement userManagement)
        {
            _userManagement = userManagement;
        }
        // GET: UserManagement
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(FormCollection collection)
        {
            var newUser = new User
            {
                Firstname = collection["firstname"],
                Lastname = collection["lastname"],
                Email = collection["email"],
                Password = collection["password"]
            };
            var isInserted = await _userManagement.InsertAsync(newUser);
            if (isInserted)
            {
                TempData["Inserted"] = "Account Created";
                return RedirectToAction("Create", "UserManagement");
            }
            return View();
        }

        public async Task<ActionResult> Read()
        {
            var users = await _userManagement.ReadAsync();
            //var user = from u in users select u;
            var userData = users.Select(u => new User { Firstname= u.Firstname, Lastname = u.Lastname, Email = u.Email });
            return View(userData);
        }

        public ActionResult Update()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        public ActionResult Login()
        {
            
            return View();
        }
        
        public async Task<ActionResult> LoginCredentials(string email, string password)
        {
            var data = new List<object>();
            if (await _userManagement.CheckEmailAsync(email))
            {
                var users = await _userManagement.ReadAsync();
                var user = users.FirstOrDefault(u => u.Email == email);
                if (await _userManagement.CheckPasswordAsync(user.Email, password))
                {
                    Session["UserID"] = user.Id;
                    Session["UserInfo"] = user;
                    data.Add(new { success = 1, email = email, password = password });
                }
                else
                {
                    data.Add(new { success = 2 });
                }
            }
            else data.Add(new { success = 3 });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}