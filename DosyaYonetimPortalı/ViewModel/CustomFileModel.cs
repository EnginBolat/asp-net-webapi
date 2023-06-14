using DosyaYonetimPortalı.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DosyaYonetimPortalı.ViewModel
{
    public class CustomFileModel
    {
        public int Id { get; set; }
        public string fileName { get; set; }
        public string fileOriginalName { get; set; }
        public string fileType { get; set; }
        public GroupModel fileGroupId { get; set; }
        public CustomModel fileUploaderId { get; set; }
    }
}