using AspNet.Identity.Oracle;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Xapp.Models;
using Owin;
using Xapp.Db;
using Dextra.Database;
using System;
using DextraLib.GeneralDao;

[assembly: OwinStartupAttribute(typeof(Xapp.Startup))]
namespace Xapp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            ConfigureAuth(app);
            app.MapSignalR();
            try
            {
                createRolesandUsers();
            }
            catch
            {

            }

        }

        // In this method we will create default User roles and Admin user for login      
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {
                // first we create Admin rool   
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);

            }



            Dao<Dxusergroup> ugdao = new Dao<Dxusergroup>(MvcApplication.Adb);
            ugdao.SqlSelect("select * from Dxusergroup", new object[] { });
            if (ugdao.Result.GetSate(DaoResult.ResCountZero))
            {

                Dxusergroup ug = new Dxusergroup();
                ug.Groupname = "Admin";
                ug.Id = 0;
                ugdao.SqlInsert(ug);

                ug.Groupname = "Default";
                ug.Id = 1;
                ugdao.SqlInsert(ug);
            }



            Dao<Dxorggroup> ogdao = new Dao<Dxorggroup>(MvcApplication.Adb);
            ogdao.SqlSelect("select * from Dxorggroup ", new object[] { });
            if (ogdao.Result.GetSate(DaoResult.ResCountZero))
            {

                Dxorggroup og = new Dxorggroup();
                og.Groupname = "Admin";
                og.Id = 0;
                og.Groupvalue = 0;
                og.Descript = "Adminorggroup";
                ogdao.SqlInsert(og);

                og.Groupname = "Default";
                og.Id = 1;
                og.Groupvalue = 0;
                og.Descript = "Defaultorggroup";
                ogdao.SqlInsert(og);
            }

            Dao<Dxroles> rgdao = new Dao<Dxroles>(MvcApplication.Adb);
            rgdao.SqlSelect("select * from Dxroles ", new object[] { });
            if (rgdao.Result.GetSate(DaoResult.ResCountZero))
            {

                Dxroles rg = new Dxroles();
                rg.Name = "AdminRole";
                rg.Id = 0;
                rg.Descript = "AdminRole";
                rgdao.SqlInsert(rg);


                rg.Name = "DefaultRole";
                rg.Id = 1;
                rg.Descript = "DefaultRole";
                rgdao.SqlInsert(rg);

            }






            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (UserManager.FindByName("Gad") == null)
            {
                var user = new ApplicationUser() { UserName = "Gad" };
                user.Email = "Gad@mail.hu";
                var chkUser = UserManager.Create(user, "Abc123.Abc");
                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                    Persons p = new Persons();
                    Dao<Persons> pdao = new Dao<Persons>(MvcApplication.Adb);
                    p.Id_Persons = null;
                    p.Bid_Persons = "NEW";
                    p.Usedname = "GenaralAdministrator";
                    p.Birthfirstname = "General";
                    p.Birthlastname = "Adminisrator";
                    p.Userid = "Gad";
                    p.Assignment = "Hu";
                    p.Birthdate = DateTime.Now;
                    p.InitCommonFieldsForAdd(MvcApplication.Adb);
                    pdao.SqlInsert(p);
                    decimal id = pdao.Result.Lastid;


                    Dao<Dxxrefuserusergroup> xrefuu = new Dao<Dxxrefuserusergroup>(MvcApplication.Adb);
                    Dxxrefuserusergroup xref = new Dxxrefuserusergroup();
                    xref.Id = null;
                    xref.Groupname = "Admin";
                    xref.Username = "Gad";
                    xrefuu.SqlInsert(xref);
                    xref.Id = null;
                    xref.Groupname = "Default";
                    xrefuu.SqlInsert(xref);


                    Dao<Dxxrefuserorggroup> xrefou = new Dao<Dxxrefuserorggroup>(MvcApplication.Adb);
                    Dxxrefuserorggroup xrefo = new Dxxrefuserorggroup();
                    xrefo.Id = null;
                    xrefo.Groupname = "Admin";
                    xrefo.Username = "Gad";
                    xrefou.SqlInsert(xrefo);
                    xrefo.Id = null;
                    xrefo.Groupname = "Default";

                    Dao<Dxxrefusergrouprole> xrefrole = new Dao<Dxxrefusergrouprole>(MvcApplication.Adb);
                    Dxxrefusergrouprole xrole = new Dxxrefusergrouprole();
                    xrole.Id = null;
                    xrole.Rolename = "DefaultRole";
                    xrole.Groupname = "Default";
                    xrefrole.SqlInsert(xrole);

                    xrole.Id = null;
                    xrole.Rolename = "AdminRole";
                    xrole.Groupname = "Admin";





                }


            }


        }

    }
}
