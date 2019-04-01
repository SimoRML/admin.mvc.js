using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace FAIS.Models.Authorize
{
    public sealed class UserRoleManager
    {
        private static readonly object padlock = new object();

        public static UserRoleManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (SessionVar.Get<UserRoleManager>("UserRoleManager") == null)
                    {
                        SessionVar.Set("UserRoleManager", new UserRoleManager());
                    }
                    return SessionVar.Get<UserRoleManager>("UserRoleManager");
                }
            }
            set
            {
                SessionVar.Set("UserRoleManager", value);
            }
        }

        public static void Clear()
        {
            //SessionVar.Remove("UserRoleManager");
        }

        private Dictionary<string, UserBoRole> boRoles;
        public Dictionary<string, UserBoRole> BoRoles { get { return boRoles; } }
        UserRoleManager()
        {
            IPrincipal user = HttpContext.Current.User;

            //UserRoleManager instance = UserRoleManager.Instance;
            this.boRoles = new Dictionary<string, UserBoRole>();

            BoRoleModel boRoleModel = new BoRoleModel();
            DataTable userBoRoles = boRoleModel.GetBoRolesByName(boRoleModel.GetUserRoles(user));

            foreach (DataRow row in userBoRoles.Rows)
            {
                UserBoRole currentBoRole;
                if (this.boRoles.ContainsKey(row["BO_DB_NAME"].ToString()))
                {
                    currentBoRole = this.boRoles[row["BO_DB_NAME"].ToString()];

                    currentBoRole.CAN_READ = user.IsInRole("admin") || currentBoRole.CAN_READ || (row["CAN_READ"].ToString() == "" ? false : (bool)row["CAN_READ"]);
                    currentBoRole.CAN_WRITE = user.IsInRole("admin") || currentBoRole.CAN_WRITE || (row["CAN_WRITE"].ToString() == "" ? false : (bool)row["CAN_WRITE"]);
                    currentBoRole.CAN_ACCESS = user.IsInRole("admin") || currentBoRole.CAN_WRITE || currentBoRole.CAN_READ;
                }
                else
                {
                    currentBoRole = new UserBoRole()
                    {
                        META_BO_ID = int.Parse(row["META_BO_ID"].ToString()),
                        BO_NAME = row["BO_NAME"].ToString(),
                        BO_DB_NAME = row["BO_DB_NAME"].ToString(),
                        BO_ROLE_ID = row["BO_ROLE_ID"].ToString() == "" ? -1 : int.Parse(row["BO_ROLE_ID"].ToString()),
                        ROLE_ID = row["ROLE_ID"].ToString(),
                        CAN_READ = user.IsInRole("admin") || (row["CAN_READ"].ToString() == "" ? false : (bool)row["CAN_READ"]),
                        CAN_WRITE = user.IsInRole("admin") || (row["CAN_WRITE"].ToString() == "" ? false : (bool)row["CAN_WRITE"]),
                    };
                    currentBoRole.CAN_ACCESS = user.IsInRole("admin") || currentBoRole.CAN_WRITE || currentBoRole.CAN_READ;
                    this.boRoles.Add(row["BO_DB_NAME"].ToString(), currentBoRole);
                }
            }
        }


        public void Verify(string boName, char accessType)
        {
            if (!this.boRoles.ContainsKey(boName)) throw new UnauthorizedAccessException("Unauthorized Access To " + boName);
            switch (accessType)
            {
                case 'r':
                    if (!this.boRoles[boName].CAN_READ) throw new UnauthorizedAccessException("Unauthorized Read Access On " + boName);
                    break;
                case 'w':
                    if (!this.boRoles[boName].CAN_WRITE) throw new UnauthorizedAccessException("Unauthorized Write Access On " + boName);
                    break;
                case 'a':
                    if (!this.boRoles[boName].CAN_ACCESS) throw new UnauthorizedAccessException("Unauthorized Access To " + boName);
                    break;
            }
        }
        public void VerifyRead(string boName)
        {
            this.Verify(boName, 'r');
        }
        public void VerifyWrite(string boName)
        {
            this.Verify(boName, 'w');
        }
        public void VerifyAccess(string boName)
        {
            this.Verify(boName, 'a');
        }
    }


    public class UserBoRole
    {
        public int META_BO_ID { get; set; }
        public string BO_NAME { get; set; }
        public string BO_DB_NAME { get; set; }
        public int BO_ROLE_ID { get; set; }
        public string ROLE_ID { get; set; }
        public bool CAN_READ { get; set; }
        public bool CAN_WRITE { get; set; }
        public bool CAN_ACCESS { get; set; }
    }

}