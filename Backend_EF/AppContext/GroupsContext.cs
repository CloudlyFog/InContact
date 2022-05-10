using Backend_EF.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_EF.AppContext
{
    public class GroupsContext : DbContext
    {
        public const string QUERYCONNECTION = "Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=Groups;Integrated Security=True;" +
            "Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";
        public DbSet<GroupModel> Groupsdata { get; set; }
        public DbSet<GroupsNodeOwner> GroupsNodeOwner { get; set; }
        public DbSet<GroupsMembers> GroupsMembers { get; set; }
        public GroupsContext(DbContextOptions<GroupsContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.UseSqlServer(
                @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=Groups;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
        }

        /// <summary>
        /// creates a new group in the database
        /// need all properties of GroupModel and properties OwnerID and GroupID of GroupsNodeOwner
        /// </summary>
        /// <param name="groupModel"></param>
        public void CreateGroup(GroupModel groupModel)
        {
            AddOwner(new GroupsNodeOwner() { OwnerID = groupModel.OwnerID, GroupID = groupModel.ID });
            Groupsdata.Add(groupModel);
            SaveChanges();
        }

        /// <summary>
        /// delete definite group from database
        /// need all properties of GroupModel and GroupsNodeOwner
        /// </summary>
        /// <param name="groupModel"></param>
        /// <param name="groupsNodeOwner"></param>
        public void DeleteGroup(GroupModel groupModel, GroupsNodeOwner groupsNodeOwner)
        {
            DeleteOwner(groupsNodeOwner);
            Groupsdata.Remove(groupModel);
            SaveChanges();
        }

        /// <summary>
        /// adds an user in group's members by adding user's data in the table GroupsMembers in the database
        /// need all properties of GroupsMembers
        /// if user joined in the group some time ago it'll leaves from group
        /// </summary>
        /// <param name="groupMember"></param>
        public void Join(GroupsMembers groupMember)
        {
            if (IsUserJoinGroup(groupMember.UserID, groupMember.GroupID))
            {
                Leave(groupMember);
                return;
            }
            GroupsMembers.Add(groupMember);
            SaveChanges();
        }

        /// <summary>
        /// deletes an user from group's members and also deletes user's data from table GroupsMembers in the database
        /// </summary>
        /// <param name="groupMember"></param>
        private void Leave(GroupsMembers groupMember)
        {
            GroupsMembers.Remove(groupMember);
            SaveChanges();
        }

        /// <summary>
        /// adds group's owner in the database
        /// </summary>
        /// <param name="groupsNodeOwner"></param>
        private void AddOwner(GroupsNodeOwner groupsNodeOwner)
        {
            GroupsNodeOwner.Add(groupsNodeOwner);
            SaveChanges();
        }

        /// <summary>
        /// deletes group's owner from the database
        /// </summary>
        /// <param name="groupsNodeOwner"></param>
        private void DeleteOwner(GroupsNodeOwner groupsNodeOwner)
        {
            GroupsNodeOwner.Remove(groupsNodeOwner);
            SaveChanges();
        }

        /// <summary>
        /// determies whether user is owner of the definite group
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="GroupID"></param>
        /// <returns><see langword="true"/> if user is owner of the definite group, else <see langword="false"/></returns>
        public bool IsUserOwnerGroup(Guid UserID, Guid GroupID) => GroupsNodeOwner.Any(node => node.OwnerID == UserID && node.GroupID == GroupID);

        /// <summary>
        /// determies whether user is joined in the definite group ago
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <returns><see langword="true"/> if user joined in the definite group, else <see langword="false"/></returns>
        private bool IsUserJoinGroup(Guid userID, Guid groupID) => GroupsMembers.Any(groupMember => groupMember.UserID == userID && groupMember.GroupID == groupID);
    }
}
