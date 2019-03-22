using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
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

        public List<string> GetUserRoles(IPrincipal user)
        {
            var roles = ((ClaimsIdentity)user.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value).ToList();
            return roles;
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

        public DataTable GetBoRolesByName(List<string> roleNames)
        {
            var parametres = new Dictionary<string, object>();
            // parametres.Add("ROLE_ID", roleId);
            return new SGBD().Cmd(@"
                        select bo.META_BO_ID, bo.META_BO_ID, bo.BO_NAME, borole.*
                        from META_BO bo
                        left outer join BO_ROLE borole  on bo.META_BO_ID = borole.META_BO_ID AND borole.ROLE_ID in (select roles.Id from AspNetRoles roles where Name in ('"+ string.Join("','", roleNames) +"'))");  
        }

        public async System.Threading.Tasks.Task<bool> SetUsersRolesAsync(string roleName, List<UserModel> users)
        {
            ApplicationDbContext userContext = new ApplicationDbContext();
            using (var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext)))
            {
                foreach (var user in users)
                {
                    if (user.status == "delete")
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
        
        public async System.Threading.Tasks.Task<DataTable> SaveBoRolesAsync(string roleId, string userName, List<BoRoleLine> boRoleLines, FAISEntities db)
        {
            foreach (var line in boRoleLines)
            {
                var boRole = new BO_ROLE()
                {
                    META_BO_ID = line.META_BO_ID,
                    ROLE_ID = line.ROLE_ID,
                    CAN_READ = line.CAN_READ,
                    CAN_WRITE = line.CAN_WRITE,
                    STATUS = "ACTIVE",
                };

                if (line.lineStatus == "new")
                {
                    boRole.CREATED_BY = userName;
                    boRole.CREATED_DATE = DateTime.Now;
                    boRole.ROLE_ID = roleId;

                    db.Entry(boRole).State = System.Data.Entity.EntityState.Added;
                }
                else
                {
                    boRole = await db.BO_ROLE.FindAsync(line.BO_ROLE_ID);

                    boRole.CAN_READ = line.CAN_READ;
                    boRole.CAN_WRITE = line.CAN_WRITE;
                    boRole.UPDATED_BY = userName;
                    boRole.UPDATED_DATE = DateTime.Now;

                    db.Entry(boRole).State = System.Data.Entity.EntityState.Modified;
                }
            }

            await db.SaveChangesAsync();
            return GetBoRoles(roleId);
        }
    }

    public class BoRoleLine
    {
        public int META_BO_ID { get; set; }
        public string ROLE_ID { get; set; }
        public long BO_ROLE_ID { get; set; }
        public bool CAN_READ { get; set; }
        public bool CAN_WRITE { get; set; }
        public string lineStatus { get; set; }
    }

}