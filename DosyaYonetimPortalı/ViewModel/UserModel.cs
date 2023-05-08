using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DosyaYonetimPortalı.ViewModel
{
    public class UserModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string pass { get; set; }
        public int authId { get; set; }
        public int groupId { get; set; }
    }
}