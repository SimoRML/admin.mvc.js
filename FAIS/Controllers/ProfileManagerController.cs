using FAIS.Models;
using FAIS.Models.Authorize;
using FAIS.Models.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace FAIS.Controllers
{
    [Authorize]
    [RoutePrefix("api/Profile")]
    public class ProfileManagerController : ApiController
    {
        FAISEntities db = new FAISEntities();
        private BoRoleModel boRoleModel = new BoRoleModel();

        // GET: api/ProfileManager
        [HttpGet]
        [Route("Menu")]
        public async Task<IHttpActionResult> Menu()
        {
            var metas = await new MetaBoRepo().GetMetaBoExAsync(@" WHERE CREATED_BY <> 'admin' AND TYPE='form' 
            AND (
META_BO_ID in (select META_BO_ID from BO_ROLE WHERE (CAN_READ = 1 OR CAN_WRITE = 1) AND ROLE_ID in (select roles.Id from AspNetRoles roles where Name in ('" + string.Join("','", boRoleModel.GetUserRoles(User)) + "'))) or 'admin' in ('" + string.Join("','", boRoleModel.GetUserRoles(User)) + "') ) ");

            Dictionary<string, object> menu = new Dictionary<string, object>();
            Dictionary<string, object> menuAdmin = new Dictionary<string, object>();

            Dictionary<string, object> menu_new = new Dictionary<string, object>();
            if (User.IsInRole("admin"))
            {

                menuAdmin.Add("Admin", new
                {
                    icon = "build",
                    text = "Administration",
                    MenuPrincipal = "Admin",
                    href = "home",
                    User = User.Identity.Name,
                    parent = true,
                    childs = new[] {
                                       new MenuFields { icon = "extension", text = "MetaBO", href = "router.metabo" },
                                       new MenuFields { icon = "dashboard", text = "Workflow", href = "bo.admin.workflow" },
                                       new MenuFields { icon = "web", text = "Page", href = "bo.admin.page" },
                                       new MenuFields { icon = "pie_chart", text = "Reporting", href = "home.reporting" },
                                       new MenuFields { icon = "pie_chart", text = "Rôles", href = "admin.roles" },
                                   },
                    open = false,
                });

                menu_new.Add("Administration", new Dictionary<string, object>()
                         {
                             { "icon", "build" },
                             { "text","Administration" },
                             //{ "href", "home" },
                             { "User", User.Identity.Name },
                             //{"MenuPrincipal",meta.MENUPRINCIPAL },
                             { "parent", true },
                             { "childs", new List<object>() },
                             { "open", false }
                         });
                var mp = (List<object>)((Dictionary<string, object>)menu_new["Administration"])["childs"];

                mp.Add(menuAdmin["Admin"]);
            }



            foreach (var meta in metas)
            {

                if (meta.GROUPE == null) meta.GROUPE = "Objects";

                if (!menu.ContainsKey(meta.GROUPE))
                {
                    menu.Add(meta.GROUPE, new Dictionary<string, object>()
                         {
                             { "icon", meta.ICONEGROUPE },
                             { "text", meta.GROUPE },
                             { "href", "home" },
                             { "User", User.Identity.Name },
                             {"MenuPrincipal",meta.MENUPRINCIPAL },
                             {"IconeMenuPrincipal",meta.ICONEMENUPRINCIPAL },
                             { "parent", true },
                             { "childs", new List<object>() },
                             { "open", false }
                         });
                }
        ((List<object>)((Dictionary<string, object>)menu[meta.GROUPE])["childs"]).Add(new
        {
            icon = meta.ICONEBO,
            text = meta.BO_NAME,
            href = "bo.index." + meta.BO_DB_NAME.Replace("_BO_", ""),
            User = "",
            parent = false,
            childs = ""
        });
            }


            var pages = await new MetaBoRepo().GetPagesAsync(@" AND (
BO_ID in (select PAGE_ID from BO_ROLE WHERE (CAN_READ = 1 OR CAN_WRITE = 1) AND ROLE_ID in (select roles.Id from AspNetRoles roles where Name in ('" + string.Join("', '", boRoleModel.GetUserRoles(User)) + "'))) or 'admin' in ('" + string.Join("', '", boRoleModel.GetUserRoles(User)) + "') ) ");
            foreach (var page in pages)
            {
                if (!menu.ContainsKey(page.GROUPE))
                {
                    menu.Add(page.GROUPE, new Dictionary<string, object>()
                        {
                            { "icon", "web" },
                            { "text", page.GROUPE },
                            { "href", "home" },
                            { "User", User.Identity.Name },
                            {"MenuPrincipal", "Pages" },
                            { "parent", true },
                            { "childs", new List<object>() },
                            { "open", false }
                        });
                }
                ((List<object>)((Dictionary<string, object>)menu[page.GROUPE])["childs"]).Add(new
                {
                    icon = "",
                    text = page.TITLE,
                    href = "bo.page." + page.META_BO_ID,
                    User = "",
                    parent = false,
                    childs = ""
                });
            }


            foreach (var item in menu.Keys)
            {
                var oneGroupe = (Dictionary<string, object>)menu[item];
                //var childs = (List<object>)oneGroupe["childs"];
                var menuPrin = oneGroupe["MenuPrincipal"].ToString();


                if (menuPrin == "") menuPrin = "Menu";
                if (!menu_new.ContainsKey(menuPrin))
                {
                    menu_new.Add(menuPrin, new Dictionary<string, object>()
                         {
                             { "icon",oneGroupe.ContainsKey("IconeMenuPrincipal") ? oneGroupe["IconeMenuPrincipal"].ToString()  : ""},
                             { "text",menuPrin },
                             //{ "href", "home" },
                             { "User", User.Identity.Name },
                             //{"MenuPrincipal",meta.MENUPRINCIPAL },
                             { "parent", true },
                             { "childs", new List<object>() },
                             { "open", false }
                         });
                }
                var mp = (List<object>)((Dictionary<string, object>)menu_new[menuPrin])["childs"];

                mp.Add(oneGroupe);
            }


            //foreach (var meta in metas)
            //{
            //    if (meta.MENUPRINCIPAL == null) meta.MENUPRINCIPAL = "Objects";

            //    if (!menu.ContainsKey(meta.MENUPRINCIPAL))
            //    {
            //        menu.Add(meta.MENUPRINCIPAL, new Dictionary<string, object>()
            //                 {
            //                     { "icon", meta.ICONEMENUPRINCIPAL },
            //                     { "text", meta.MENUPRINCIPAL },
            //                     { "href", "home" },
            //                     { "User", User.Identity.Name },
            //                     { "parent", true },
            //                     { "childs",new Dictionary<string,object>() },
            //                     { "open", false }
            //                 });
            //    }

            //    if (meta.GROUPE == null) meta.GROUPE = "Objects";

            //    if (!((Dictionary<string, object>)((Dictionary<string, object>)menu[meta.MENUPRINCIPAL])["childs"]).ContainsKey(meta.GROUPE))
            //    {

            //        ((Dictionary<string, object>)((Dictionary<string, object>)menu[meta.MENUPRINCIPAL])["childs"]).Add(meta.GROUPE, new Dictionary<string, object>()
            //                 {
            //                     { "icon", meta.ICONEGROUPE },
            //                     { "text", meta.GROUPE },
            //                     { "href", "home" },
            //                     { "User", User.Identity.Name },
            //                     { "parent", true },
            //                     { "childs", new List<object>() },
            //                     { "open", false }
            //                 });
            //    }

            //         /* ((List<object>)((Dictionary<string, Dictionary<string, object>>)menu[meta.MENUPRINCIPAL])[meta.GROUPE]["childs"]).Add(new
            //           {
            //               icon = meta.ICONEBO,
            //               text = meta.BO_NAME,
            //               href = "bo.index." + meta.BO_DB_NAME.Replace("_BO_", ""),
            //               User = "",
            //               parent = false,
            //               childs = ""
            //           });*/
            //}
            return Ok(menu_new);
        }

        [HttpGet]
        [ResponseType(typeof(List<string>))]
        [Route("Validators")]
        public async Task<IHttpActionResult> GetValidators()
        {
            List<string> validators = await db.Database.SqlQuery<string>("SELECT [Email] FROM [AspNetUsers] order by Email").ToListAsync();
            return Ok(validators);
        }


        [HttpGet]
        [Route("roles")]
        public async Task<IHttpActionResult> GetRoles()
        {
            return Ok(boRoleModel.GetRoles().ToList());
        }
        [HttpGet]
        [Route("users")]
        public async Task<IHttpActionResult> GetUsers()
        {
            return Ok(boRoleModel.GetUsers().ToList());
        }
        [HttpGet]
        [Route("usersRoles")]
        public async Task<IHttpActionResult> GetUserRoles()
        {
            return Ok(boRoleModel.GetUserRoles(User));
        }
        [HttpGet]
        [Route("users/{roleId}")]
        public async Task<IHttpActionResult> GetUsers(string roleId)
        {
            return Ok(boRoleModel.GetUsers(roleId).ToList());
        }
        [HttpPost]
        [Route("addRole/{roleName}")]
        public async Task<IHttpActionResult> AddRole(string roleName)
        {
            return Ok(await boRoleModel.AddRoleAsync(roleName));
        }
        [HttpPut]
        [Route("usersRoles/{roleName}")]
        public async Task<IHttpActionResult> SetUsersRoles(string roleName, List<UserModel> users)
        {
            return Ok(await boRoleModel.SetUsersRolesAsync(roleName, users));
        }

        [HttpGet]
        [Route("boRoles/{roleId}")]
        public async Task<IHttpActionResult> GetBoRoles(string roleId)
        {
            return Ok(boRoleModel.GetBoRoles(roleId));
        }
        [HttpGet]
        [Route("access")]
        public async Task<IHttpActionResult> GetAccess()
        {
            return Ok(UserRoleManager.Instance.BoRoles);
        }
        [HttpPost]
        [Route("saveBoRoles/{roleId}")]
        public async Task<IHttpActionResult> SaveBoRoles(string roleId, List<BoRoleLine> boRoleLines)
        {
            return Ok(await boRoleModel.SaveBoRolesAsync(roleId, User.Identity.Name, boRoleLines, db));
        }
    }
}
