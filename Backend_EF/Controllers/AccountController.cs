using Backend_EF.AppContext;
using Backend_EF.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend_EF.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext applicationContext;
        public AccountController(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        [Route("SignUp")]
        public IActionResult SignUp() => View();

        [Route("SignIn")]
        public IActionResult SignIn() => View();

        [Route("Privacy")]
        public IActionResult Privacy() => View();

        [Route("Admin-enter")]
        public IActionResult EnterAs() => View();

        [Route("ChangeUserData")]
        public IActionResult ChangeUserData(UserModel userModel, string[] args)
        {
            userModel.ID = new Guid(HttpContext.Session.GetString("userID"));
            return View(userModel);
        }

        [Route("RecoveringPassword")]
        public IActionResult RecoveringPassword() => View();

        [Route("Recovery")]
        public IActionResult Recovery() => View();

        [Route("FailedAuth")]
        public IActionResult FailedAuth() => View();

        [HttpPost, Route("SignIn")]
        public async Task<IActionResult> LoginUser([Bind] UserModel receivedUser)
        {
            UserModel user = new()
            {
                Name = applicationContext.GetUserProp(receivedUser, "Name"),
                Email = receivedUser.Email,
                Password = receivedUser.Password,
                Authenticated = applicationContext.IsAuthanticated(receivedUser),
                Access = applicationContext.IsHasAccess(receivedUser),
            };
            //setting user`s name and email into session
            HttpContext.Session.SetString("userEmail", user.Email);
            HttpContext.Session.SetString("userPassword", user.Password);
            user.ID = applicationContext.GetID(user);
            HttpContext.Session.SetString("userEncryptedId", applicationContext.EncryptID(user.ID));
            try
            {
                if (user.Access)
                    return RedirectToAction("EnterAs");
                else if (user.Authenticated)
                    return RedirectToAction("HomePage", "Home");
                else
                    return RedirectToAction("FailedAuth");
            }
            catch (Exception ex)
            {
                return Content(ex.Message.ToString());
            }
        }

        [HttpPost, Route("SignUp")]
        public async Task<IActionResult> AddUser([Bind] UserModel receivedUser)
        {
            if (applicationContext.AddUser(receivedUser))
                return RedirectToAction("SignIn");
            else
                return RedirectToAction("FailedAuth");
        }

        [Route("Incognito")]
        public IActionResult IncognitoMode()
        {
            UserModel incognitoUser = new()
            {
                ID = Guid.NewGuid(),
                IncognitoMode = true
            };
            applicationContext.Clear();
            applicationContext.AddUser(incognitoUser);
            HttpContext.Session.SetString("userID", incognitoUser.ID.ToString());
            HttpContext.Session.SetString("incognitoMode", incognitoUser.IncognitoMode.ToString());
            return RedirectToAction("HomePage", "Home");
        }

        [HttpPost, Route("ChangeUserData")]
        public async Task<IActionResult> ChangeUserData(UserModel receivedUser)
        {
            UserModel changeableUser = new()
            {
                Email = HttpContext.Session.GetString("userEmail"),
                Password = HttpContext.Session.GetString("userPassword")
            };
            changeableUser.Name = applicationContext.GetUserProp(changeableUser, "Name");
            changeableUser.ID = applicationContext.GetID(changeableUser);
            receivedUser.ID = changeableUser.ID;
            applicationContext.EditUserData(changeableUser, receivedUser);
            return RedirectToAction("SignIn");
        }

        [HttpPost, Route("Recovery")]
        public async Task<IActionResult> Recovery([Bind] UserModel receivedUser)
        {
            if (applicationContext.IsExist(
                applicationContext.GetUserProp(receivedUser.ID, "Name"),
                receivedUser.Email))
            {
                receivedUser.Password = applicationContext.GetPassword(receivedUser.ID);
                return View(receivedUser);
            }
            else
                return Content("password not found.");

        }
    }
}
