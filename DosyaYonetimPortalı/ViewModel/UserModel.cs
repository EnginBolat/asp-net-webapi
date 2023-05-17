using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DosyaYonetimPortalı.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string userNameSurname { get; set; }
        public string userEmail { get; set; }
        public string userPassword { get; set; }
        public int userAuthorityId { get; set; }
        public int userGroupId { get; set; }

    }
}