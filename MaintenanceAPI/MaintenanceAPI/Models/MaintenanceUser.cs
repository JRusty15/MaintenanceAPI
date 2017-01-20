using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace MaintenanceAPI.Models
{
    public class MaintenanceUser : IIdentity, IPrincipal
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public IIdentity Identity
        {
            get
            {
                return this;
            }
        }

        public string Name
        {
            get
            {
                return Username;
            }
        }

        public string AuthenticationType
        {
            get
            {
                 return "basic"; 
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}