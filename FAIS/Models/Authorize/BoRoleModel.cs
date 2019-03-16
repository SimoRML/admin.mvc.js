using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FAIS.Models.Authorize
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string status { get; set; }
    }
    public class BoRoleModel
    {
        public IQueryable<IdentityRole> GetRoles()
        {
            using (var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext())))
            {
                return rm.Roles;
            }
        }

        public IQueryable<UserModel> GetUsers()
        {
            ApplicationDbContext userContext = new ApplicationDbContext();
            using (var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext)))
            {
                return um.Users.Select<ApplicationUser, UserModel>(u => new UserModel { Email = u.Email, Id = u.Id, status = "old" });
            }
        }
        public IQueryable<UserModel> GetUsers(string roleId)
        {
            ApplicationDbContext userContext = new ApplicationDbContext();
            using (var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext)))
            {
                return um.Users.Where(u => u.Roles.Count(r => r.RoleId == roleId) > 0).Select<ApplicationUser, UserModel>(u => new UserModel { Email = u.Email, Id = u.Id, status = "old" });
            }
        }

        public DataTable GetBoRoles(string roleId)
        {
            var parametres = new Dictionary<string, object>();
            parametres.Add("ROLE_ID", roleId);
            return new SGBD().Cmd(@"
select bo.META_BO_ID, bo.META_BO_ID, bo.BO_NAME, borole.*,
case when BO_ROLE_ID is null then 'new' else 'old' end as lineStatus
from META_BO bo
left outer join BO_ROLE borole  on bo.META_BO_ID = borole.META_BO_ID AND borole.ROLE_ID = @ROLE_ID 
where bo.STATUS != '-1'
", parametres);
        }

        public async System.Threading.Tasks.Task<bool> SetUsersRolesAsync(string roleName, List<UserModel> users)
        {
            ApplicationDbContext userContext = new ApplicationDbContext();
            using (var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext)))
            {
                foreach (var user in users)
                {
                    if(user.status == "delete")
                        await um.RemoveFromRoleAsync(user.Id, roleName);
                    else if (user.status == "new")
                        await um.AddToRoleAsync(user.Id, roleName);
                }
                return true;
            }
        }

        public async System.Threading.Tasks.Task<bool> AddRoleAsync(string roleName)
        {
            using (var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext())))
            {
                if (!rm.RoleExists(roleName))
                {
                    await rm.CreateAsync(new IdentityRole(roleName));
                    return true;
                }
            }
            return false;
        }

    }
}