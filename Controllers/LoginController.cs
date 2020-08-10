using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace DotNetCoreSqlDb.Controllers
{
    public class LoginController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;


            // InitializeUsers();
        }

        /*public async Task InitializeUsers()
        {
            var adminUser = new IdentityUser
            {
                UserName = "admin"
            };
            await _userManager.CreateAsync(adminUser, "AdminPassw0rd!");

            var studentUser = new IdentityUser
            {
                UserName = "student"
            };
            await _userManager.CreateAsync(studentUser, "StudentPassw0rd!");
        }*/


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(string usertype, string password)
        {
            Console.WriteLine("usertype");
            Console.WriteLine(usertype);
            Console.WriteLine(password);
            
            // var user = await _userManager.FindByNameAsync(usertype);


            /*if (user != null)
            {
                Console.WriteLine("found user");
                var signInResult = await _signInManager.PasswordSignInAsync(usertype, password, false, false);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Inventory");
                } else
                {
                    return RedirectToAction("Index", "Todos");
                }
            }*/


            Console.WriteLine("user is null");

            var user = new IdentityUser
            {
                UserName = usertype,
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (signInResult.Succeeded)
                {
                    Console.WriteLine("Account created");
                    return RedirectToAction("Index", "Inventory");
                }
            }

            return RedirectToAction("Index");
            

            /*
            if (usertype == "Admin" && password == "AdminPassword")
            {
                return RedirectToAction("Index", "Inventory");

            } else if (usertype == "Student" && password == "StudentPassword")
            {
                return RedirectToAction("Index", "Inventory");
            } else
            {
                return RedirectToAction("Index");
            }
            */
        }
    }
}
