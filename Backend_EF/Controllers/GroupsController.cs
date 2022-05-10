using Backend_EF.AppContext;
using Backend_EF.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend_EF.Controllers
{
    public class GroupsController : Controller
    {
        private readonly GroupsContext groupsContext;
        private readonly ApplicationContext applicationContext;
        public GroupsController(GroupsContext context, ApplicationContext applicationContext)
        {
            this.groupsContext = context;
            this.applicationContext = applicationContext;
        }

        [Route("Groups")]
        public IActionResult AllGroups(GroupModel groupModel)
        {
            groupModel = new()
            {
                UserID = HttpContext.Session.GetString("userID") is not null
                    ? new(HttpContext.Session.GetString("userID")) : new(),
            };
            return View(groupModel);
        }

        [Route("Create")]
        public IActionResult CreateGroup(GroupModel groupModel)
        {
            groupModel = new()
            {
                UserID = HttpContext.Session.GetString("userID") is not null
                    ? new(HttpContext.Session.GetString("userID")) : new(),
            };
            return View(groupModel);
        }

        [Route($"GroupFeed")]
        public IActionResult GroupFeed(GroupModel groupModel, Guid groupID)
        {
            groupModel = new()
            {
                UserID = HttpContext.Session.GetString("userID") is not null
                    ? new(HttpContext.Session.GetString("userID")) : new(),
                ID = groupID,
                OwnerID = groupsContext.Groupsdata.Any(x => x.ID == groupID)
                    ? groupsContext.Groupsdata.First(x => x.ID == groupID).ID : new(),
                Name = groupsContext.Groupsdata.Any(x => x.ID == groupID)
                    ? groupsContext.Groupsdata.First(x => x.ID == groupID).Name : string.Empty,
                Description = groupsContext.Groupsdata.Any(x => x.ID == groupID)
                    ? groupsContext.Groupsdata.First(x => x.ID == groupID).Description : string.Empty
            };
            return View(groupModel);
        }

        [HttpPost, Route("Join")]
        public async Task<IActionResult> Join(GroupsMembers groupMember, Guid groupID, Guid groupMemberID)
        {
            groupMember = new()
            {
                UserID = HttpContext.Session.GetString("userID") is not null
                    ? new(HttpContext.Session.GetString("userID")) : new(),
                GroupID = groupID,
                ID = groupMemberID
            };

            if (!applicationContext.IsAuthanticated(groupMember.UserID))
                return RedirectToAction("AllGroups");

            groupsContext.Join(groupMember);
            return RedirectToAction("AllGroups");
        }

        [HttpPost, Route("Create-Group")]
        public async Task<IActionResult> CreateGroup(GroupModel groupModel, Guid ID)
        {
            groupModel.UserID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();
            groupModel.OwnerID = groupModel.UserID;

            if (!applicationContext.IsAuthanticated(groupModel.UserID))
                return RedirectToAction("AllGroups");

            groupsContext.CreateGroup(groupModel);
            return RedirectToAction("AllGroups");
        }

        [HttpPost, Route("Delete-Group")]
        public async Task<IActionResult> DeleteGroup(GroupModel groupModel, Guid nodeOwnerID, Guid ID, string Name, string Description)
        {
            groupModel.UserID = HttpContext.Session.GetString("userID") is not null
                ? new(HttpContext.Session.GetString("userID")) : new();
            groupModel.OwnerID = groupModel.UserID;
            groupModel.ID = ID;
            groupModel.Name = Name;
            groupModel.Description = Description;

            GroupsNodeOwner groupsNodeOwner = new()
            {
                ID = nodeOwnerID,
                OwnerID = groupModel.OwnerID,
                GroupID = groupModel.ID
            };

            if (!applicationContext.IsAuthanticated(groupModel.UserID))
                return RedirectToAction("AllGroups");

            groupsContext.DeleteGroup(groupModel, groupsNodeOwner);
            return RedirectToAction("AllGroups");
        }

    }
}
