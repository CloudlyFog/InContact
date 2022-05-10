using Backend_EF.AppContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Backend_EF.Handlers
{
    public class GroupsHandler
    {
        static readonly GroupsContext groupsContext = new(new DbContextOptions<GroupsContext>());

        /// <summary>
        /// get list of definite GroupModel's property
        /// </summary>
        /// <param name="nameGroupPart"></param>
        /// <returns>list of: ID, OwnerID, UserID, Name</returns>
        public static List<object> GetGroupsPartList(string nameGroupPart)
        {
            List<object> groupParts = new();
            string queryString = $"SELECT {nameGroupPart} FROM Groupsdata";
            SqlConnection connection = new(GroupsContext.QUERYCONNECTION);
            SqlCommand command = new(queryString, connection);
            connection.Open();
            command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader();
            for (int i = 0; reader.Read(); i++)
                groupParts.Add(reader.GetValue(0));
            reader.Close();
            connection.Close();
            return groupParts;
        }

        /// <summary>
        /// determines count of Groupsdata table's rows
        /// </summary>
        /// <returns>returns count of table's rows</returns>
        public static int GetRowsCount() => groupsContext.Groupsdata.Count();
    }
}
