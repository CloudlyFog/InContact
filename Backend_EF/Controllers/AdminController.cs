using Backend_EF.AppContext;
using Backend_EF.Models;
using Microsoft.AspNetCore.Mvc;


namespace Backend_EF.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationContext applicationContext;
        public AdminController(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        [Route("AdminPanel")]
        public IActionResult Index(MessageModel messageModel)
        {
            messageModel = new MessageModel()
            {
                Email = HttpContext.Session.GetString("userEmail")
            };
            return View(messageModel);
        }

        [HttpPost]
        public string GetAllMessages() => applicationContext.GetAllMessages();

        [HttpPost]
        public string Messages([Bind] MessageModel messageModel) => applicationContext.GetMessage(messageModel);

        [HttpPost]
        public string AllUsers([Bind] UserModel user) => applicationContext.GetUser(user);

        [HttpPost]
        public string DeleteMessage([Bind] MessageModel messageModel)
        {
            applicationContext.DeleteMessage(messageModel.ID);
            return $"message from \"{messageModel.Email}\" who wrote to \"{messageModel.ToEmail}\" was deleted.";
        }

        [HttpPost]
        public string SendMessageFromAdmin(MessageModel messageModel) => applicationContext.SendMessageFromAdmin(messageModel);
    }
}
