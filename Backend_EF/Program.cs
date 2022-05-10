using Backend_EF.AppContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("ConnectionToDbUsers");
string connectionToGroupsdb = builder.Configuration.GetConnectionString("ConnectionToDbGroups");
string ConnectionToBindNewsGroup = builder.Configuration.GetConnectionString("ConnectionToBindNewsGroup");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<NewsContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<GroupsContext>(options => options.UseSqlServer(connectionToGroupsdb));
builder.Services.AddDbContext<BindContext>(options => options.UseSqlServer(ConnectionToBindNewsGroup));
builder.Services.AddSession();

var app = builder.Build();

app.UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=HomePage}/{id?}");

app.Run();
