using Backend_EF.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_EF.AppContext
{
    public class BindContext : DbContext
    {
        public BindContext(DbContextOptions<BindContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(
                @"Server=localhost\\SQLEXPRESS;Data Source=maxim;Initial Catalog=BindNewsGroup;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False");

        public DbSet<BindNewsGroupModel> Binds { get; set; }

        /// <summary>
        /// adds a new bind in the database
        /// need all properties of BindNewsGroupModel
        /// </summary>
        /// <param name="bind"></param>
        public void AddBind(BindNewsGroupModel bind)
        {
            Binds.Add(bind);
            SaveChanges();
        }

        /// <summary>
        /// updates a new bind in the database
        /// need all properties of BindNewsGroupModel
        /// </summary>
        /// <param name="bind"></param>
        public void UpdateBind(BindNewsGroupModel bind)
        {
            Binds.Update(bind);
            SaveChanges();
        }

        /// <summary>
        /// deletes a bind from the database
        /// need all properties of BindNewsGroupModel
        /// </summary>
        /// <param name="bind"></param>
        public void DeleteBind(BindNewsGroupModel bind)
        {
            Binds.Remove(bind);
            SaveChanges();
        }

        /// <summary>
        /// determies whether a bind exists in the database
        /// needed properties: GroupID and PostID
        /// </summary>
        /// <param name="bind"></param>
        /// <returns><see langword="true"/> if bind is exist in the database, else <see langword="false"/></returns>
        public bool IsExistBind(BindNewsGroupModel bind) => Binds.Any(b => b.GroupID == bind.GroupID && b.PostID == bind.PostID);
    }
}
