using Backend_EF.AppContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Backend_EF.Handlers
{
    public class NewsHandler
    {
        private static readonly NewsContext newsContext = new(new DbContextOptions<NewsContext>());
        private static readonly BindContext bindContext = new(new DbContextOptions<BindContext>());

        /// <summary>
        /// get list of definite NewsModel's property
        /// </summary>
        /// <param name="nameNewsPart"></param>
        /// <returns>list of: ID, PostID, UserID, GroupName, Body, Likes with definite value of GroupName</returns>
        public static List<object> GetNewsPartList(string nameNewsPart, string groupName)
        {
            List<object> newsParts = new();
            string queryString = $"SELECT {nameNewsPart} FROM Newsdata WHERE GroupName = '{groupName}'";
            SqlConnection connection = new(ApplicationContext.QUERYCONNECTION);
            SqlCommand command = new(queryString, connection);
            connection.Open();
            command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader();
            for (int i = 0; reader.Read(); i++)
                newsParts.Add(reader.GetValue(0));
            reader.Close();
            connection.Close();
            return newsParts;
        }

        /// <summary>
        /// get list of definite NewsModel's property
        /// </summary>
        /// <param name="nameNewsPart"></param>
        /// <returns>list of: ID, PostID, UserID, GroupName, Body, Likes</returns>
        public static List<object> GetNewsPartList(string nameNewsPart)
        {
            List<object> newsParts = new();
            string queryString = $"SELECT {nameNewsPart} FROM Newsdata";
            SqlConnection connection = new(ApplicationContext.QUERYCONNECTION);
            SqlCommand command = new(queryString, connection);
            connection.Open();
            command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader();
            for (int i = 0; reader.Read(); i++)
                newsParts.Add(reader.GetValue(0));
            reader.Close();
            connection.Close();
            return newsParts;
        }

        /// <summary>
        /// determines count of Newsdata table's rows
        /// </summary>
        /// <returns>returns count of table's rows</returns>
        public static int GetRowsCount() => newsContext.Newsdata.Count();

        /// <summary>
        /// determines count of Newsdata table's rows
        /// </summary>
        /// <returns>returns count of table's rows with definite value of GroupName</returns>
        public static int GetRowsCount(string groupName) => newsContext.Newsdata.Count(x => x.GroupName == groupName);

        /// <summary>
        /// determines whether elements of sequence satisfies a condition
        /// </summary>
        /// <param name="userID"></param>
        /// <returns><see langword="true"/> if exist post with defenite UserID</returns>
        public static bool IsExist(Guid userID) => newsContext.Newsdata.Any(news => news.UserID == userID);
    }
}
