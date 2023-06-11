using DosyaYonetimPortalı.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DosyaYonetimPortalı.Models
{
    public class CustomModel
    {
        public int Id { get; set; }
        public string userNameSurname { get; set; }
        public string userEmail { get; set; }
        public string userPassword { get; set; }
        public AuthorityModel userAuthority { get; set; }
        public GroupModel userGroup { get; set; }

    }
}