using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DosyaYonetimPortalı.ViewModel
{
    public class FileModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int fileTypeId { get; set; }
        public int uploadedUserId { get; set; }
        public int groupId { get; set; }
    }
}