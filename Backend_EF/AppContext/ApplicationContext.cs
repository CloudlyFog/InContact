using Backend_EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace Backend_EF.AppContext
{
    public class ApplicationContext : DbContext
    {
        public const string QUERYCONNECTION = "Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=Users;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";
        public const string ADMINEMAIL = "maximkirichenk0.06@gmail.com";

        public DbSet<UserModel> Usersdata { get; set; }
        public DbSet<MessageModel> Messagedata { get; set; }
        public DbSet<NoteModel> Notes { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=Users;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");
        }


        //handle user data

        /// <summary>
        /// adds user`s data in context
        /// needed properties: ID, Name, Email, Password
        /// </summary>
        /// <param name="receivedUser"></param>
        /// <returns><see langword="true"/> if user has been added correctly</returns>
        public bool AddUser([Bind] UserModel receivedUser)
        {
            if (IsAuthanticated(receivedUser))//if user isn`t exist method will send false
                return false;
            receivedUser.Authenticated = true;
            Usersdata.Add(receivedUser);
            SaveChanges();
            return true;
        }

        /// <summary>
        /// changes user`s data
        /// needed properties: last & new Name, last & new Email, last & new Password
        /// changeableUser is an instance of UserModel which will be change to data of inputUser
        /// </summary>
        /// <param name="changeableUser"></param>
        /// <param name="inputUser"></param>
        public void EditUserData(UserModel changeableUser, UserModel inputUser)
        {
            Usersdata.Remove(changeableUser);
            Usersdata.Add(inputUser);
            SaveChanges();
        }

        /// <summary>
        /// determines whether user is authanticated in the database
        /// needed properties: Email, Password
        /// </summary>
        /// <param name="receivedUser"></param>
        /// <returns><see langword="true"/> if user was authenticate</returns>
        public bool IsAuthanticated(UserModel receivedUser) => Usersdata.Any(user => user.Email == receivedUser.Email && user.Password == receivedUser.Password && user.Authenticated);

        /// <summary>
        /// determines whether user is authanticated in the database
        /// </summary>
        /// <param name="ID"></param>
        /// <returns><see langword="true"/> if user was authenticate</returns>
        public bool IsAuthanticated(Guid ID) => Usersdata.Any(user => user.ID == ID && user.Authenticated);

        /// <summary>
        /// returns true if user has an access to admin panel
        /// needed properties: Email, Password
        /// </summary>
        /// <param name="receivedUser"></param>
        /// <returns><see langword="true"/> if user has an access to admin panel</returns>
        public bool IsHasAccess(UserModel receivedUser) => Usersdata.Any(user => user.Email == receivedUser.Email && user.Password == receivedUser.Password && user.Access);

        /// <summary>
        /// determines whether any user's data of sequence satisfies a condition
        /// needed properties: Email, Password
        /// </summary>
        /// <param name="receivedUser"></param>
        /// <returns>returns <see langword="true"/> if user exists</returns>
        public bool IsExist(UserModel receivedUser) => Usersdata.Any(user => user.Email == receivedUser.Email && user.Password == receivedUser.Password);

        /// <summary>
        /// determines whether any user's data of sequence satisfies a condition
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns>returns <see langword="true"/> if user exist</returns>
        public bool IsExist(string name, string email) => Usersdata.Any(user => user.Name == name && user.Email == email);

        /// <summary>
        /// determines whether any user's data of sequence satisfies a condition
        /// </summary>
        /// <param name="email"></param>
        /// <returns>returns <see langword="true"/> if user exists</returns>
        public bool IsExist(string email) => Usersdata.Any(user => user.Email == email);

        /// <summary>
        /// delete accounts where IncognitoMode is true
        /// </summary>
        public void Clear()
        {
            Usersdata.RemoveRange(Usersdata.Where(user => user.IncognitoMode));
            SaveChanges();
        }

        /// <summary>
        /// gets user's password with definite ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string GetPassword(Guid ID) => Usersdata.FirstOrDefault(user => user.ID == ID).Password;

        /// <summary>
        /// determines whether user's data satisfied a condition
        /// needed properties: Email, Password
        /// </summary>
        /// <param name="user"></param>
        /// <returns>return ID of specified user</returns>
        public Guid GetID(UserModel user) => Usersdata.Any(x => x.Name == GetUserProp(user, "Name") && x.Email == user.Email && x.Password == user.Password)
            ? Usersdata.FirstOrDefault(x => x.Name == GetUserProp(user, "Name") && x.Email == user.Email && x.Password == user.Password).ID : new();

        /// <summary>
        /// can return: ID, Name, Email, Password, Autenticated, Access
        /// properties NoteModel and MessageModel are not include in context and the method can`t return it
        /// needed properties: Email and Password, for searching definite property
        /// </summary>
        /// <param name="receivedUser"></param>
        /// <param name="userProp"></param>
        /// <returns>
        /// returns any existing property of user`s model.
        /// can return: ID, Name, Email, Password, Autenticated, Access
        /// </returns>
        public string GetUserProp(UserModel receivedUser, string userProp)
        {
            string name;
            string queryStringGetName = $"SELECT {userProp} FROM Usersdata WHERE Email LIKE '{receivedUser.Email}' AND Password LIKE '{receivedUser.Password}'";
            SqlConnection connection = new(QUERYCONNECTION);
            SqlCommand command = new(queryStringGetName, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
                name = reader.GetString(0);
            else
                name = "not found";
            reader.Close();
            connection.Close();
            return name;
        }

        /// <summary>
        /// returns any existing property of user`s model
        /// can return: PostID, Name, Email, Password, Autenticated, Access
        /// properties NoteModel and MessageModel are not include in context and the method can`t return it
        /// needed properties: ID, for searching definite property
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="userProp"></param>
        /// <returns></returns>
        public string GetUserProp(Guid ID, string userProp)
        {
            string name;
            string queryStringGetName = $"SELECT {userProp} FROM Usersdata WHERE ID LIKE '{ID}'";
            SqlConnection connection = new(QUERYCONNECTION);
            SqlCommand command = new(queryStringGetName, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
                name = reader.GetString(0);
            else
                name = "not found";
            reader.Close();
            connection.Close();
            return name;
        }

        /// <summary>
        /// encrypt user's id when AccountController is passing data in HomeController
        /// </summary>
        /// <param name="id"></param>
        /// <returns>encrypted user's ID</returns>
        public string EncryptID(Guid id)
        {
            string encrypted = "";
            byte[] secretkeyByte = Encoding.UTF8.GetBytes("87654321");
            byte[] publickeybyte = Encoding.UTF8.GetBytes("12345678");
            byte[] inputbyteArray = Encoding.UTF8.GetBytes(id.ToString());
            using (DESCryptoServiceProvider des = new())
            {
                MemoryStream ms = new();
                CryptoStream cs = new(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                encrypted = Convert.ToBase64String(ms.ToArray());
            }
            return encrypted;
        }

        /// <summary>
        /// Decrypt user's id when AccountController is passing data in HomeController
        /// </summary>
        /// <param name="id"></param>
        /// <returns>decrypted user's ID</returns>
        public Guid DecryptID(string id)
        {
            if (id is null)
                return new Guid();
            string decrypted = "";
            byte[] privatekeyByte = Encoding.UTF8.GetBytes("87654321");
            byte[] publickeybyte = Encoding.UTF8.GetBytes("12345678");
            byte[] inputbyteArray = new byte[id.Replace(" ", "+").Length];
            inputbyteArray = Convert.FromBase64String(id.ToString().Replace(" ", "+"));
            using (DESCryptoServiceProvider des = new())
            {
                MemoryStream ms = new();
                CryptoStream cs = new(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                decrypted = encoding.GetString(ms.ToArray());
            }

            return new Guid(decrypted);
        }


        //handle message data

        /// <summary>
        /// sends message from user`s name to administration
        /// needed properties: ToEmail, Email
        /// not to email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="messageModel"></param>
        /// <returns>returns string exception or that message was sent sussessfully</returns>
        public string SendMessage([Bind] UserModel user, MessageModel messageModel)
        {
            if (!IsExist(user.Email))
                return "email is wrong";
            if (ManyMessageToOneUser(messageModel))
                return "You have already sent message. Please wait while admin will read your message and answer you. Sincerely, administration";
            Messagedata.Add(messageModel);
            SaveChanges();
            return "Message was sent succesfully";
        }

        /// <summary>
        /// gets any message which exist in database
        /// needed properties for search: Name, Email, Message
        /// searchs by ToEmail's value
        /// </summary>
        /// <param name="messageModel"></param>
        /// <returns>returns any message</returns>
        public string GetMessage([Bind] MessageModel messageModel)
        {
            MessageModel message = Messagedata.FirstOrDefault(msg => msg.ToEmail == ADMINEMAIL);
            if (message is null)
                return "Message not found";
            return $"Message: {message.Message}\n Email: {message.Email}";
        }

        /// <summary>
        /// delete message from user in the context if it necessary
        /// </summary>
        /// <param name="id"></param>
        public void DeleteMessage(Guid id)
        {
            Messagedata.Remove(Messagedata.FirstOrDefault(message => message.ID == id));
            SaveChanges();
        }

        /// <summary>
        /// sends message from administration name
        /// needed properties: ToEmail, Email
        /// </summary>
        /// <param name="messageModel"></param>
        /// <returns>returns string exception or that message was sent sussessfully</returns>
        public string SendMessageFromAdmin(MessageModel messageModel)
        {
            if (!IsExist(messageModel.ToEmail))
                return "Name or email is wrong";
            if (ManyMessageToOneUser(messageModel))
                return "you sent more than 1 message to user, please delete your message when 3 days are went";
            Messagedata.Add(messageModel);
            SaveChanges();
            return "Message was successfully sent";

        }

        /// <summary>
        /// gets message only if it from administration. this method using for getting message to user
        /// </summary>
        /// <param name="messageModel"></param>
        /// <returns>message only if it from administration. this method using for getting message to user</returns>\
        public string GetMessageFromAdmin(MessageModel messageModel)
        {
            MessageModel messageFromAdmin = Messagedata.FirstOrDefault(msg => msg.Email == ADMINEMAIL && msg.ToEmail == messageModel.Email);
            if (messageFromAdmin is null)
                return "you haven't any message from administration yet.";
            return messageFromAdmin.Message;
        }

        /// <summary>
        /// gets all messages from database
        /// not need any properties
        /// </summary>
        /// <returns>all messages which contains in the database</returns>
        public string GetAllMessages()
        {
            string allMessages = string.Empty;
            foreach (var msg in Messagedata)
                allMessages += $"UserID: {msg.UserID}\nID: {msg.ID}\nEmail: {msg.Email}\nToEmail: {msg.ToEmail}\nMessage: {msg.Message}\n\n";
            return allMessages;
        }

        /// <summary>
        /// gets all info about specific user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>returns string with all user's properties</returns>
        public string GetUser([Bind] UserModel user)
        {
            UserModel userData = Usersdata.FirstOrDefault(usr => usr.Email == user.Email);
            string userInfo = string.Empty;
            if (userData is null)
                return "User not found";
            return $"UserID: {userData.ID}\nName: {userData.Name}\nEmail: {userData.Email}\nPassword: {userData.Password}\n\n";
        }

        /// <summary>
        /// sends message to user`s email
        /// work when user was autenticated
        /// </summary>
        /// <param name="messageModel"></param>
        public async void SendMail([Bind] MessageModel messageModel)
        {
            string password = GetPassword(new Guid(""));
            MailAddress to = new($"faqblogms@gmail.com");//куда приходит сообщение
            MailAddress from = new($"{messageModel.Email}", $"");//откуда будут отправляться сообщения
            MailMessage userMessage = new(from, to);//создаем образ сообщения
            userMessage.Body = messageModel.Message;
            SmtpClient client = new("smtp.gmail.com", 44352)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("faqblogms@gmail.com", password)
            }; //создаем экземпляр клиента 
            await client.SendMailAsync(userMessage);//отправляем сообщение
        }

        /// <summary>
        /// determines count of messages to user
        /// needed properties: Email
        /// </summary>
        /// <param name="messageModel"></param>
        /// <returns><see langword="true"/> if there are more then 1 message to one user</returns>
        private bool ManyMessageToOneUser([Bind] MessageModel messageModel) => Messagedata.Where(msg => msg.ToEmail == ADMINEMAIL && msg.Email == messageModel.Email).Any();

        //handle notes

        /// <summary>
        /// adds note to database
        /// needed properties: IdNote, ID, Title, Body
        /// </summary>
        /// <param name="insertNoteModel"></param>
        /// <param name="user"></param>
        public void CreateNote(NoteModel insertNoteModel, UserModel user)
        {
            insertNoteModel.ID = user.ID;
            Notes.Add(insertNoteModel);
            SaveChanges();
        }

        /// <summary>
        /// update note in database
        /// needed properties: IdNote, ID, Title, Body
        /// </summary>
        /// <param name="insertNoteModel"></param>
        /// <param name="deletedNoteId"></param>
        /// <param name="user"></param>
        public void EditNote(NoteModel insertNoteModel, Guid deletedNoteId, UserModel user)
        {
            NoteModel resultNote = Notes.FirstOrDefault(note => note.IdNote == deletedNoteId);
            if (resultNote is not null)
            {
                resultNote.IdNote = insertNoteModel.IdNote;
                resultNote.ID = user.ID;
                resultNote.Title = insertNoteModel.Title;
                resultNote.Body = insertNoteModel.Body;
                SaveChanges();
            }
        }

        /// <summary>
        /// deletes specyfied note from database
        /// </summary>
        /// <param name="IdNote"></param>
        /// <returns></returns>
        public void DeleteNote(Guid IdNote)
        {
            Notes.Remove(Notes.FirstOrDefault(note => note.IdNote == IdNote));
            SaveChanges();
        }
    }
}
