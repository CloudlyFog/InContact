using Backend_EF.AppContext;
using Backend_EF.Handlers;
using Backend_EF.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend_EF.Controllers
{
    public class NotesController : Controller
    {
        private readonly ApplicationContext applicationContext;
        public NotesController(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        [Route("Your-notes")]
        public IActionResult Index(UserModel user)
        {
            //sets user by session
            //
            if (HttpContext.Session.GetString("incognitoMode") == true.ToString())
            {
                user.ID = new Guid(HttpContext.Session.GetString("userID"));
                return View(user);
            }
            //
            user = new UserModel()
            {
                Email = HttpContext.Session.GetString("userEmail"),
                Password = HttpContext.Session.GetString("userPassword")
            };
            user.Name = applicationContext.GetUserProp(user, "Name");
            user.ID = applicationContext.GetID(user);
            return View(user);
        }

        [HttpGet, Route("CreateNote")]
        public IActionResult CreateNote(UserModel user)
        {
            if (HttpContext.Session.GetString("incognitoMode") == true.ToString())
            {
                user.ID = new Guid(HttpContext.Session.GetString("userID"));
                return View(user);
            }
            user = new UserModel()
            {
                Email = HttpContext.Session.GetString("userEmail"),
                Password = HttpContext.Session.GetString("userPassword")
            };
            user.ID = applicationContext.GetID(user);
            return View(user);
        }

        [HttpGet, Route("EditNote")]
        public IActionResult EditNote(UserModel user, Guid IdNote)
        {
            user = new UserModel()
            {
                Email = HttpContext.Session.GetString("userEmail"),
                Password = HttpContext.Session.GetString("userPassword")
            };
            user.ID = applicationContext.GetID(user);

            user.NoteModel = new NoteModel()
            {
                Title = NoteHandler.GetNotePart(IdNote, "Title"),
                Body = NoteHandler.GetNotePart(IdNote, "Body"),
                ID = user.ID
            };
            user.NoteModel.IdNote = IdNote;
            return View(user);
        }

        [HttpPost, Route("EditNote")]
        public async Task<IActionResult> EditNote(UserModel user, NoteModel insertNoteModel, Guid IdNote, string[] args)
        {
            user.Email = HttpContext.Session.GetString("userEmail");
            user.Password = HttpContext.Session.GetString("userPassword");
            user.Name = applicationContext.GetUserProp(user, "Name");
            user.ID = applicationContext.GetID(user);
            user.NoteModel = new NoteModel()
            {
                Title = NoteHandler.GetNotePart(IdNote, "Title"),
                Body = NoteHandler.GetNotePart(IdNote, "Body"),
            };
            applicationContext.EditNote(insertNoteModel, IdNote, user);
            return RedirectToAction("Index");
        }

        [HttpPost, Route("CreateNote")]
        public async Task<IActionResult> CreateNote(UserModel user, string[] args)
        {
            user.Email = HttpContext.Session.GetString("userEmail");
            user.Password = HttpContext.Session.GetString("userPassword");
            user.Name = applicationContext.GetUserProp(user, "Name");
            user.ID = applicationContext.GetID(user);
            applicationContext.CreateNote(user.NoteModel, user);
            return RedirectToAction("Index");
        }

        [HttpPost, Route("DeleteNote")]
        public async Task<IActionResult> DeleteNote(Guid IdNote)
        {
            applicationContext.DeleteNote(IdNote);
            return RedirectToAction("Index");
        }
    }
}
