using Backend_EF.AppContext;
using Backend_EF.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Backend_EF.Handlers
{
    public class NoteHandler
    {
        private static readonly ApplicationContext applicationContext = new(new DbContextOptions<ApplicationContext>());

        /// <summary>
        /// get list of definite NoteModel's property
        /// can return list of: IdNote, ID, Title, Body
        /// </summary>
        /// <param name="user"></param>
        /// <param name="nameNotePart"></param>
        /// <returns>list of definite NoteModel's property</returns>
        public static List<object> GetNotePartList(UserModel user, string nameNotePart)
        {
            List<object> noteParts = new();
            string queryString = $"SELECT {nameNotePart} FROM Notes WHERE ID LIKE '{user.ID}'";
            SqlConnection connection = new(ApplicationContext.QUERYCONNECTION);
            SqlCommand command = new(queryString, connection);
            connection.Open();
            command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader();
            for (int i = 0; reader.Read(); i++)
                noteParts.Add(reader.GetValue(0));
            reader.Close();
            connection.Close();
            return noteParts;
        }

        /// <summary>
        /// get definite NoteModel's property
        /// can return IdNote, ID, Title, Body
        /// </summary>
        /// <param name="IdNote"></param>
        /// <param name="nameNotePart"></param>
        /// <param name="result"></param>
        /// <returns>return IdNote, ID, Title, Body</returns>
        public static string GetNotePart(Guid IdNote, string nameNotePart, string result = "")
        {
            string queryString = $"SELECT {nameNotePart} FROM Notes WHERE IdNote LIKE '{IdNote}'";
            SqlConnection connection = new(ApplicationContext.QUERYCONNECTION);
            SqlCommand command = new(queryString, connection);
            connection.Open();
            command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
                result = reader.GetString(0);
            else
                result = null;
            reader.Close();
            connection.Close();
            return result;
        }

        /// <summary>
        /// determines count of Notes table's rows with definite property ID
        /// </summary>
        /// <param name="user"></param>
        /// <returns>returns count of table's rows</returns>
        public static int GetRowsCount(UserModel user) => applicationContext.Notes.Count(x => x.ID == user.ID);

        /// <summary>
        /// return IdNote 
        /// notePart is a variable by which will be searching idNote
        /// value - is a value of definite note's part
        /// </summary>
        /// <param name="notePart"></param>
        /// <param name="value">value of definite note's part</param>
        /// <param name="idNote"></param>
        /// <returns>returns note's ID</returns>
        public static Guid GetNoteId(string notePart, string value, Guid idNote = new Guid())
        {
            string queryString = $"SELECT IdNote FROM Notes WHERE {notePart} LIKE '{value}'";
            SqlConnection connection = new(ApplicationContext.QUERYCONNECTION);
            SqlCommand command = new(queryString, connection);
            connection.Open();
            command.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
                idNote = reader.GetGuid(0);
            else
                idNote = Guid.Empty;
            connection.Close();
            return idNote;
        }
    }
}
