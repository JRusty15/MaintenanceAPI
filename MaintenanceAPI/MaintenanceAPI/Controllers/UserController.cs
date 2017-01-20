using MaintenanceAPI.Models;
using MaintenanceAPI.Models.InputModels;
using MaintenanceAPI.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MaintenanceAPI.Controllers
{
    [RoutePrefix("User")]
    public class UserController : ApiController
    {
        private IUserRepository m_UserRep { get; set; }
        public UserController(IUserRepository userRepo)
        {
            m_UserRep = userRepo;
        }

        [HttpGet]
        [Route("Login")]
        public bool UserLogin()
        {
            var user = (User as MaintenanceUser);
            return user != null;
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route("UserRegistration/")]
        public SystemReply UserRegistration([FromBody] UserRegistrationInput regInput)
        {
            return m_UserRep.UserRegistration(regInput.Username, regInput.Password, regInput.Email);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("UserValidation/{registrationID}")]
        public bool UserValidation(Guid registrationID)
        {
            return m_UserRep.UserValidation(registrationID);
        }
    }
}
