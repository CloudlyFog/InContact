using Backend_EF.AppContext;
using Backend_EF.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Backend_EF.Controllers
{

    public class HomeController : Controller
    {
        private readonly ApplicationContext applicationContext;
        public HomeController(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public IActionResult HomePage(UserModel user)
        {
            //sets user`s data
            if (HttpContext.Session.GetString("incognitoMode") == true.ToString())
            {
                user.ID = new Guid(HttpContext.Session.GetString("userID"));
                user.IncognitoMode = true;
                return View(user);
            }
            user.Email = HttpContext.Session.GetString("userEmail");
            user.Password = HttpContext.Session.GetString("userPassword");
            user.Name = applicationContext.GetUserProp(user, "Name");
            if (user.ID != new Guid())
                user.ID = applicationContext.DecryptID(HttpContext.Session.GetString("userEncryptedId"));
            HttpContext.Session.SetString("userID", user.ID.ToString());
            return View(user);
        }

        [Route("Error")]
        public IActionResult Error() => View();

        [Route("WriteMessage")]
        public IActionResult WriteMessage(MessageModel messageModel)
        {
            messageModel.UserID = new Guid(HttpContext.Session.GetString("userID"));
            messageModel.Email = HttpContext.Session.GetString("userEmail");
            return View(messageModel);
        }

        [Route("GetAnswer")]
        public IActionResult GetAnswer(MessageModel messageModel)
        {
            messageModel.UserID = new Guid(HttpContext.Session.GetString("userID"));
            messageModel.Email = HttpContext.Session.GetString("userEmail");
            return View(messageModel);
        }

        [Route("SuccessfullySentMessage")]
        public IActionResult SuccessfullySentMessage() => View();

        [Route("SuccessfullyGotMessage")]
        public IActionResult SuccessfullyGotMessage(MessageModel messageModel)
        {
            messageModel.Email = HttpContext.Session.GetString("userEmail");
            return View(messageModel);
        }

        [Route("SignOut")]
        public async Task<IActionResult> Logout(UserModel user)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("HomePage");
        }

        //HttpPost

        [HttpPost, Route("Send")]
        public string Send([Bind] UserModel user, MessageModel messageModel)
        {
            messageModel.UserID = new Guid(HttpContext.Session.GetString("userID"));
            return applicationContext.SendMessage(user, messageModel);
        }

        [HttpPost, Route("SussessfullyGotMessage")]
        public IActionResult GetMessageAnswer(MessageModel messageModel)
        {
            messageModel.Message = applicationContext.GetMessageFromAdmin(messageModel);
            return View(messageModel);
        }

        [HttpPost, Route("SendEmail")]
        public void SendEmail(MessageModel messageModel) => applicationContext.SendMail(messageModel);
    }
}
