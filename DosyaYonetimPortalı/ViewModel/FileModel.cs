using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DosyaYonetimPortalı.ViewModel
{
    public class FileModel
    {
        public int Id { get; set; }
        public string fileName { get; set; }
        public int fileTypeId { get; set; }
        public int fileGroupId { get; set; }
        public int fileUploaderId { get; set; }
    }
}