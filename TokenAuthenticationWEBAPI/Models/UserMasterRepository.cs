using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TokenAuthenticationWEBAPI.Models
{
    public class UserMaster
    {
        public UserMaster(int id, string username, string password, string roles, string email)
        {
            this.UserID = id;
            this.UserName = username;
            this.UserPassword = password;
            this.UserRoles = roles;
            this.UserEmailID = email;
        }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserRoles { get; set; }
        public string UserEmailID { get; set; }
    }
    public class UserMasterRepository : IDisposable
    {
        //// SECURITY_DBEntities it is your context class
        //SECURITY_DBEntities context = new SECURITY_DBEntities();
        private List<UserMaster> _userMasterList = new List<UserMaster>()
        {
            new UserMaster(101, "Alberto", "123456","Admin","alberto@cheneso.com"),
            new UserMaster(102, "Bernardo", "abcdef","User","bernardo@cheneso.com"),
            new UserMaster(103, "Consuelo", "123pqr","SuperAdmin","consuelo@cheneso.com"),
            new UserMaster(104, "Diana", "abc123","Admin, User","diana@cheneso.com"),
        };

        //This method is used to check and validate the user credentials
        public UserMaster ValidateUser(string username, string password)
        {
            return this._userMasterList.FirstOrDefault(user =>
                        user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
                        && user.UserPassword == password);
        }
        public UserMaster ValidateUser(string username)
        {
            return this._userMasterList.FirstOrDefault(user =>
                        user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
        public void Dispose()
        {
            //context.Dispose();
        }
    }
}