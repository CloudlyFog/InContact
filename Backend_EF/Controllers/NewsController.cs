using Backend_EF.AppContext;
using Backend_EF.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend_EF.Controllers
{
    public class NewsController : Controller
    {
        private readonly NewsContext newsContext;
        private readonly ApplicationContext applicationContext;
        private readonly BindContext bindContext;
        private readonly GroupsContext groupsContext;
        public NewsController(NewsContext context, ApplicationContext applicationContext, BindContext bindContext, GroupsContext groupsContext)
        {
            this.newsContext = context;
            this.applicationContext = applicationContext;
            this.bindContext = bindContext;
            this.groupsContext = groupsContext;
        }

        [Route("Feed")]
        public ActionResult Feed(NewsModel newsModel)
        {
            newsModel.UserID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();
            return View(newsModel);
        }

        [Route("Create-Post")]
        public IActionResult CreatePost(NewsModel newsModel, string[] args)
        {
            newsModel.UserID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();
            return View(newsModel);
        }

        [HttpPost, Route("Like")]
        public async Task<IActionResult> LikePost(NewsModel newsModel)
        {

            if (!applicationContext.IsAuthanticated(newsModel.UserID))
                return RedirectToAction("Feed");


            if (newsContext.IsLike(newsModel.PostID, newsModel.UserID))//here happen adding data in UserNodeLike
            {
                newsContext.UnLikePost(newsModel);
                return RedirectToAction("Feed");
            }

            newsContext.LikePost(newsModel);
            return RedirectToAction("Feed");
        }

        [HttpPost, Route("Create-Post")]
        public async Task<IActionResult> CreatePost(NewsModel newsModel, string groupName)
        {
            newsModel.GroupName = groupName;
            newsModel.UserID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();

            BindNewsGroupModel bindModel = new()
            {
                PostID = newsModel.PostID,
                GroupID = groupsContext.Groupsdata.Any(x => x.UserID == newsModel.UserID)
                    ? groupsContext.Groupsdata.First(x => x.UserID == newsModel.UserID).ID : new()
            };

            if (!applicationContext.IsAuthanticated(newsModel.UserID))
                return Content("You ain't authanticated.");

            bindContext.AddBind(bindModel);
            newsContext.AddNewsPost(newsModel);
            return Content("Post has been created.");
        }

        [HttpPost, Route("Delete-Post")]
        public async Task<IActionResult> DeletePost(NewsModel newsModel)
        {
            if (!applicationContext.IsAuthanticated(newsModel.UserID))
                return RedirectToAction("Feed");

            BindNewsGroupModel bindModel = new()
            {
                ID = newsModel.BindID,

                PostID = newsModel.PostID,

                GroupID = groupsContext.Groupsdata.Any(x => x.UserID == newsModel.UserID)
                    ? groupsContext.Groupsdata.First(x => x.UserID == newsModel.UserID).ID : new()
            };

            bindContext.DeleteBind(bindModel);
            newsContext.DeleteNewsPost(newsModel);
            return RedirectToAction("Feed");
        }
    }
}
