using Backend_EF.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_EF.AppContext
{

    public class NewsContext : DbContext
    {
        public const string QUERYCONNECTION = "Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=Users;Integrated Security=True;" +
            "Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";
        public DbSet<NewsModel> Newsdata { get; set; }
        public DbSet<UserNodeLike> UserNodeLike { get; set; }
        public DbSet<GroupModel> Groupsdata { get; set; }
        public NewsContext(DbContextOptions<NewsContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(
                @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=Users;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");

        /// <summary>
        /// adds a new post to the database
        /// needed properties: PostID, UserID, GroupName, Body
        /// </summary>
        /// <param name="newsModel"></param>
        public void AddNewsPost(NewsModel newsModel)
        {
            Newsdata.Add(newsModel);
            SaveChanges();
        }

        /// <summary>
        /// delete a definite post from the database
        /// needed properties: PostID, UserID, GroupName, Body
        /// </summary>
        /// <param name="newsModel"></param>
        public void DeleteNewsPost(NewsModel newsModel)
        {
            Newsdata.Remove(newsModel);
            SaveChanges();
        }

        /// <summary>
        /// set like to post with definite user's data
        /// </summary>
        /// <param name="newsModel"></param>
        public void LikePost(NewsModel newsModel)
        {
            NewsModel result = Newsdata.FirstOrDefault(news => news.PostID == newsModel.PostID);
            if (result is not null)
            {
                result.Likes++;
                SaveChanges();
            }
        }

        /// <summary>
        /// unset like to post with definite user's data
        /// </summary>
        /// <param name="newsModel"></param>
        public void UnLikePost(NewsModel newsModel)
        {
            if (!IsLike(newsModel.PostID, newsModel.UserID))
                return;
            NewsModel result = Newsdata.FirstOrDefault(news => news.PostID == newsModel.PostID);
            if (result is not null)
            {
                UserNodeLike node = UserNodeLike.FirstOrDefault(node => node.PostNewsID == newsModel.PostID && node.UserID == newsModel.UserID);
                UserNodeLike.Remove(node);
                result.Likes--;
                SaveChanges();
            }

        }

        /// <summary>
        /// check if user was likes post
        /// if user wasn't likes post method adds node for tracking likes
        /// needed properties: postNewsID, userID
        /// </summary>
        /// <param name="postNewsId"></param>
        /// <param name="userID"></param>
        /// <returns>returns <see langword="true"/> if specyfied post has been liked by definite user</returns>
        public bool IsLike(Guid postNewsId, Guid userID)
        {
            if (UserNodeLike.Any(node => node.PostNewsID == postNewsId && node.UserID == userID))
                return true;
            UserNodeLike.Add(new UserNodeLike() { PostNewsID = postNewsId, UserID = userID });
            SaveChanges();
            return false;
        }
    }
}
