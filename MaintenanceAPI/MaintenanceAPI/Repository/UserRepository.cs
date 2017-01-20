using Dapper;
using MaintenanceAPI.Models;
using MaintenanceAPI.Repository.Interface;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using MaintenanceAPI.Utilities;

namespace MaintenanceAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private static readonly string m_BaseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();

        private static string m_ConnectionString;
        public UserRepository()
        {
            m_ConnectionString = ConfigurationManager.ConnectionStrings["MaintenanceDB"].ConnectionString;
        }

        public MaintenanceUser Login(string username, string password)
        {
            MaintenanceUser user = null;

            using(var connection = new SqlConnection(m_ConnectionString))
            {
                var users = connection.Query<MaintenanceUser>(
                    "SELECT * FROM [Maintenance].[dbo].[Users] WHERE Username = @Username AND Password = @Password",
                    new { Username = username, Password = password });

                if(users != null && users.Count() > 0)
                {
                    user = users.First();
                }
            }

            return user;
        }

        public SystemReply UserRegistration(string username, string password, string email)
        {
            var reply = new SystemReply() { Success = false };
            Guid userRegistration = Guid.NewGuid();

            using (var connection = new SqlConnection(m_ConnectionString))
            {
                var registrations = connection.Query<UserRegistration>(@"SELECT * FROM [dbo].[UserRegistration]
                    WHERE Email = @Email", new { Email = email });

                if (registrations != null && registrations.Any())
                {
                    reply.Success = false;
                    if (registrations.First().Validated)
                    {
                        reply.Message = "This e-mail has already been registered";
                    }
                    else
                    {
                        reply.Message = "This e-mail address is awaiting validation";
                    }
                }
                else
                {
                    connection.Execute(@"INSERT INTO [Maintenance].[dbo].[UserRegistration]
                                       ([RegistrationID]
                                       ,[Username]
                                       ,[Password]
                                       ,[Email]
                                       ,[Validated])
                                    VALUES
                                       (@RegistrationID
                                       ,@Username
                                       ,@Password
                                       ,@Email
                                       ,0)",
                                           new { RegistrationID = userRegistration, Username = username, Password = password, Email = email });

                    var registrationUrl = string.Format("{0}{1}{2}", m_BaseUrl, "/User/UserValidation/", userRegistration.ToString());
                    var emailBody = string.Format("Welcome! Please register your account by clicking on this link: {0}", registrationUrl);

                    reply.Success = EmailHelper.SendEmail(email, "Stander Maintenance Registration", emailBody);
                    if (!reply.Success)
                    {
                        reply.Message = "Failed to send e-mail";
                    }
                }
            }
            return reply;
        }

        public bool UserValidation(Guid registrationID)
        {
            var response = false;

            using (var connection = new SqlConnection(m_ConnectionString))
            {
                var registrations = connection.Query<UserRegistration>(@"SELECT [RegistrationID]
                                                                          ,[Username]
                                                                          ,[Password]
                                                                          ,[Email]
                                                                          ,[Validated]
                                                                      FROM [Maintenance].[dbo].[UserRegistration]
                                                                      WHERE RegistrationID = @RegistrationID
                                                                      AND Validated = 0",
                                                                      new { RegistrationID = registrationID });
                if(registrations != null && registrations.Count() > 0)
                {
                    var userReg = registrations.First();

                    //Move user to the Users table
                    connection.Execute(@"INSERT INTO [Maintenance].[dbo].[Users]
                                       ([Username]
                                       ,[Password]
                                       ,[Email])
                                     VALUES
                                       (@Username
                                       ,@Password
                                       ,@Email)", new { Username = userReg.Username, Password = userReg.Password, Email = userReg.Email });

                    //Mark registration as validated
                    connection.Execute(@"UPDATE [Maintenance].[dbo].[UserRegistration]
                                       SET [Validated] = 1
                                       WHERE RegistrationID = @RegistrationID",
                                       new { RegistrationID = registrationID });

                    response = true;
                }
            }

            return response;
        }
    }
}