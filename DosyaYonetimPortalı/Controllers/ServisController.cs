using DosyaYonetimPortalı.Models;
using DosyaYonetimPortalı.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace DosyaYonetimPortalı.App_Start
{
    [Authorize]
    public class ServisController : ApiController
    {
        Db01Entities4 db = new Db01Entities4();
        ResponseModel response = new ResponseModel();

        //Kullanıcı İşlemleri
        #region User

        [HttpGet]
        [Route("api/user/list")]
        public List<CustomModel> userList()
        {
            List<CustomModel> userList = db.Users.Select(x => new CustomModel()
            {
                Id = x.Id,
                userNameSurname = x.userNameSurname,
                userEmail = x.userEmail,
                userPassword = x.userPassword,
                userAuthority = db.Authorities.Where(y => y.Id == x.userAuthorityId).Select(y => new AuthorityModel()
                {
                    Id = y.Id,
                    authorityName = y.authorityName,
                }).FirstOrDefault(),
                userGroup = db.Groups.Where(g => g.Id == x.userGroupId).Select(g => new GroupModel()
                {
                    Id = g.Id,
                    groupName = g.groupName,
                }).FirstOrDefault(),
            }).ToList();

            return userList;
        }


        [HttpGet]
        [Route("api/user/userById/{userId}")]
        public CustomModel userById(int userId)
        {
            CustomModel record = db.Users.Where(x => x.Id == userId).Select(y => new CustomModel()
            {
                Id = y.Id,
                userNameSurname = y.userNameSurname,
                userEmail = y.userEmail,
                userPassword = y.userPassword,
                userAuthority = db.Authorities.Where(z => y.Id == y.userAuthorityId).Select(z => new AuthorityModel()
                {
                    Id = z.Id,
                    authorityName = z.authorityName,
                }).FirstOrDefault(),
                userGroup = db.Groups.Where(g => g.Id == y.userGroupId).Select(g => new GroupModel()
                {
                    Id = g.Id,
                    groupName = g.groupName,
                }).FirstOrDefault(),
            }).FirstOrDefault();

            return record;
        }

        [HttpGet]
        [Route("api/user/usersByGroupId/{groupId}")]
        public List<CustomModel> usersByGroupId(int groupId)
        {
            List<CustomModel> record = db.Users.Where(x => x.userGroupId == groupId).Select(y => new CustomModel()
            {
                Id = y.Id,
                userNameSurname = y.userNameSurname,
                userEmail = y.userEmail,
                userPassword = y.userPassword,
                userAuthority = db.Authorities.Where(z => y.Id == y.userAuthorityId).Select(z => new AuthorityModel()
                {
                    Id = z.Id,
                    authorityName = z.authorityName,
                }).FirstOrDefault(),
                userGroup = db.Groups.Where(g => g.Id == y.userGroupId).Select(g => new GroupModel()
                {
                    Id = g.Id,
                    groupName = g.groupName,
                }).FirstOrDefault(),

            }).ToList();

            return record;
        }

        [HttpDelete]
        [Route("api/user/deleteUser/{userId}")]
        public ResponseModel deleteUser(int userId)
        {
            User record = db.Users.Where(x => x.Id == userId).SingleOrDefault();
            if (record != null)
            {
                db.Users.Remove(record);
                db.SaveChangesAsync();
                response.process = true;
                response.message = "Kullanıcı Silindi";
                return response;
            }
            response.process = false;
            response.message = "Kullanıcı Bulunamadı";
            return response;
        }

        [HttpPost]
        [Route("api/user/addUser")]
        public ResponseModel addUser(CustomModel user)
        {
            if (db.Users.Count(c => c.userEmail == user.userEmail) > 0)
            {
                response.process = false;
                response.message = "Kullanıcı Daha Önceden Kayıtlıdır";
                return response;
            }
            User record = new User();

            record.userNameSurname = user.userNameSurname;
            record.userEmail = user.userEmail;
            record.userPassword = user.userPassword;
            record.userAuthorityId = user.userAuthority.Id;
            record.userGroupId = user.userGroup.Id;
            db.Users.Add(record);
            db.SaveChanges();
            response.process = true;
            response.message = "Kullanıcı Eklendi";
            return response;
        }

        [HttpPut]
        [Route("api/user/updateUser")]
        public ResponseModel updateUser(CustomModel user)
        {
            User record = db.Users.Where(x => x.Id == user.Id).SingleOrDefault();

            if (record != null)
            {
                record.userNameSurname = user.userNameSurname;
                record.userEmail = user.userEmail;
                record.userPassword = user.userPassword;
                record.userAuthorityId = user.userAuthority.Id;
                record.userGroupId = user.userGroup.Id;

                db.SaveChangesAsync();

                response.process = true;
                response.message = "Kullanıcı Güncellendi";
                return response;
            }
            response.process = false;
            response.message = "Kullanıcı Bulunamadı";
            return response;
        }

        #endregion // Kullanıcı İşlemleri

        //Grup İşlemleri
        #region Group 

        [HttpGet]
        [Route("api/group/list")]
        public List<GroupModel> groupList()
        {
            List<GroupModel> groupList = db.Groups.Select(x => new GroupModel()
            {
                Id = x.Id,
                groupName = x.groupName,
                totalUser = db.Users.Count(y => y.userGroupId == x.Id),
            }).ToList();

            return groupList;
        }

        [HttpGet]
        [Route("api/group/groupById/{groupId}")]
        public GroupModel groupById(int groupId)
        {
            GroupModel record = db.Groups.Where(x => x.Id == groupId).Select(y => new GroupModel()
            {
                Id = y.Id,
                groupName = y.groupName,
            }).FirstOrDefault();

            return record;
        }

        [HttpDelete]
        [Route("api/group/deleteGroup/{groupId}")]
        public ResponseModel deleteGroup(int groupId)
        {
            Group record = db.Groups.Where(x => x.Id == groupId).SingleOrDefault();
            if (record != null)
            {
                db.Groups.Remove(record);
                db.SaveChangesAsync();
                response.process = true;
                response.message = "Grup Silindi";
                return response;
            }
            response.process = false;
            response.message = "Grup Bulunamadı";
            return response;
        }

        [HttpPost]
        [Route("api/group/addGroup")]
        public ResponseModel addGroup(GroupModel group)
        {
            if (db.Groups.Count(c => c.groupName == group.groupName) > 0)
            {
                response.process = true;
                response.message = "Grup Daha Önceden Kayıtlıdır";
                return response;
            }
            Group record = new Group();

            record.groupName = group.groupName;
            db.Groups.Add(record);
            db.SaveChanges();
            response.process = true;
            response.message = "Grup Eklendi";
            return response;
        }

        [HttpPut]
        [Route("api/group/updateGroup")]
        public ResponseModel updateGroup(GroupModel group)
        {
            Group record = db.Groups.Where(x => x.Id == group.Id).SingleOrDefault();

            if (record != null)
            {
                record.Id = group.Id;
                record.groupName = group.groupName;
                db.SaveChangesAsync();

                response.process = true;
                response.message = "Grup GÜncellendi";
                return response;
            }
            response.process = false;
            response.message = "Grup Bulunamadı";
            return response;
        }

        #endregion

        //Yetki İşlemleri
        #region Authority

        [HttpGet]
        [Route("api/authority/list")]
        public List<AuthorityModel> authorityList()
        {
            List<AuthorityModel> authorityList = db.Authorities.Select(x => new AuthorityModel()
            {
                Id = x.Id,
                authorityName = x.authorityName,
            }).ToList();

            return authorityList;
        }

        [HttpGet]
        [Route("api/authority/authorityById/{authorityId}")]
        public AuthorityModel authorityById(int authorityId)
        {
            AuthorityModel record = db.Authorities.Where(x => x.Id == authorityId).Select(y => new AuthorityModel()
            {
                Id = y.Id,
                authorityName = y.authorityName,
            }).FirstOrDefault();

            return record;
        }

        [HttpDelete]
        [Route("api/authority/deleteAuthority/{authorityId}")]
        public ResponseModel deleteAuthority(int authorityId)
        {
            Authority record = db.Authorities.Where(x => x.Id == authorityId).SingleOrDefault();
            if (record != null)
            {
                db.Authorities.Remove(record);
                db.SaveChangesAsync();
                response.process = true;
                response.message = "Yetki Silindi";
                return response;
            }
            response.process = false;
            response.message = "Yetki Bulunamadı";
            return response;
        }

        [HttpPost]
        [Route("api/authority/addAuth")]
        public ResponseModel addAuth(AuthorityModel auth)
        {
            if (db.Authorities.Count(c => c.authorityName == auth.authorityName) > 0)
            {
                response.process = true;
                response.message = "Yetki Daha Önceden Kayıtlıdır";
                return response;
            }
            Authority record = new Authority();

            record.authorityName = auth.authorityName;
            db.Authorities.Add(record);
            db.SaveChanges();
            response.process = true;
            response.message = "Yetki Eklendi";
            return response;
        }

        [HttpPut]
        [Route("api/authority/updateAuthority")]
        public ResponseModel updateAuthority(AuthorityModel authority)
        {
            Authority record = db.Authorities.Where(x => x.Id == authority.Id).SingleOrDefault();

            if (record != null)
            {
                record.Id = authority.Id;
                record.authorityName = authority.authorityName;
                db.SaveChangesAsync();

                response.process = true;
                response.message = "Yetki GÜncellendi";
                return response;
            }
            response.process = false;
            response.message = "Yetki Bulunamadı";
            return response;
        }
        #endregion

        //Dosya İşlemleri
        #region File

        [HttpGet]
        [Route("api/file/list")]
        public List<ViewModel.FileModel> allFiles()
        {
            List<ViewModel.FileModel> fileList = db.Files.Select(x => new ViewModel.FileModel()
            {
                Id = x.Id,
                fileName = x.fileName,
                fileOriginalName = x.fileOriginalName,
                fileType = x.fileType,
                fileGroupId = db.Groups.Where(g => g.Id == x.fileGroupId).Select(group => new GroupModel()
                {
                    Id = group.Id,
                    groupName = group.groupName
                }).FirstOrDefault(),
                fileUploaderId = db.Users.Where(g => g.Id == x.fileUploaderId).Select(custom => new CustomModel()
                {
                    Id = custom.Id,
                    userNameSurname = custom.userNameSurname,
                    userEmail = custom.userEmail,
                    userPassword = custom.userPassword,
                    userAuthority = db.Authorities.Where(y => y.Id == custom.userAuthorityId).Select(auth => new AuthorityModel()
                    {
                        Id = auth.Id,
                        authorityName = auth.authorityName,
                    }).FirstOrDefault(),
                    userGroup = db.Groups.Where(g => g.Id == custom.userGroupId).Select(g => new GroupModel()
                    {
                        Id = g.Id,
                        groupName = g.groupName,
                    }).FirstOrDefault(),
                }).FirstOrDefault(),
            }).ToList();
            return fileList;
        }

        [HttpGet]
        [Route("api/file/listByGroupId/{groupId}")]
        public List<ViewModel.FileModel> allFilesByGroup(int groupId)
        {
            List<ViewModel.FileModel> fileList = db.Files.Where(p => p.fileGroupId == groupId).Select(x => new ViewModel.FileModel()
            {
                Id = x.Id,
                fileName = x.fileName,
                fileOriginalName = x.fileOriginalName,
                fileType = x.fileType,
                fileGroupId = db.Groups.Where(g => g.Id == x.fileGroupId).Select(group => new GroupModel()
                {
                    Id = group.Id,
                    groupName = group.groupName
                }).FirstOrDefault(),
                fileUploaderId = db.Users.Where(g => g.Id == x.fileUploaderId).Select(custom => new CustomModel()
                {
                    Id = custom.Id,
                    userNameSurname = custom.userNameSurname,
                    userEmail = custom.userEmail,
                    userPassword = custom.userPassword,
                    userAuthority = db.Authorities.Where(y => y.Id == custom.userAuthorityId).Select(auth => new AuthorityModel()
                    {
                        Id = auth.Id,
                        authorityName = auth.authorityName,
                    }).FirstOrDefault(),
                    userGroup = db.Groups.Where(g => g.Id == custom.userGroupId).Select(g => new GroupModel()
                    {
                        Id = g.Id,
                        groupName = g.groupName,
                    }).FirstOrDefault(),
                }).FirstOrDefault(),
            }).ToList();
            return fileList;
        }

        [HttpPost]
        [Route("api/file/addFile")]
        public ResponseModel AddFile(FileModel fileModel)
        {
            if (db.Files.Any(c => c.Id == fileModel.Id))
            {
                response.process = false;
                response.message = "Dosya Daha Önceden Kayıtlıdır";
                return response;
            }

            Models.File record = new Models.File();

            record.fileName = fileModel.fileName;
            record.fileGroupId = fileModel.fileGroupId.Id;
            record.fileUploaderId = fileModel.fileUploaderId.Id;
            record.fileOriginalName = fileModel.fileOriginalName;
            record.fileType = fileModel.fileType;

            db.Files.Add(record);
            db.SaveChanges();

            response.process = true;
            response.message = "Dosya Eklendi";

            return response;
        }



        [HttpGet]
        [Route("api/file/fileById/{fileId}")]
        public ViewModel.CustomFileModel fileById(int fileId)
        {
            ViewModel.CustomFileModel file = db.Files.Where(y => y.Id == fileId).Select(x => new ViewModel.CustomFileModel()
            {
                Id = x.Id,
                fileName = x.fileName,
                fileOriginalName = x.fileOriginalName,
                fileType = x.fileType,
                fileGroupId = db.Groups.Where(a => a.Id == x.fileGroupId).Select(g => new GroupModel()
                {
                    Id = g.Id,
                    groupName = g.groupName,
                }).SingleOrDefault(),
                fileUploaderId = db.Users.Where(u => u.Id == x.fileUploaderId).Select(us => new CustomModel()
                {
                    Id = us.Id,
                    userNameSurname = us.userNameSurname,
                    userEmail = us.userEmail,
                    userPassword = us.userPassword,
                    userAuthority = db.Authorities.Where(y => y.Id == us.userAuthorityId).Select(auth => new AuthorityModel()
                    {
                        Id = auth.Id,
                        authorityName = auth.authorityName,
                    }).FirstOrDefault(),
                    userGroup = db.Groups.Where(g => g.Id == us.userGroupId).Select(g => new GroupModel()
                    {
                        Id = g.Id,
                        groupName = g.groupName,
                    }).FirstOrDefault(),
                }).FirstOrDefault(),
            }).SingleOrDefault();

            return file;
        }


        [HttpGet]
        [Route("api/file/filesByUserId/{userId}")]
        public List<ViewModel.FileModel> filesByUserId(int userId)
        {
            List<ViewModel.FileModel> fileList = db.Files.Where(g => g.fileUploaderId == userId).Select(x => new ViewModel.FileModel()
            {
                Id = x.Id,
                fileName = x.fileName,
                fileOriginalName = x.fileOriginalName,
                fileType = x.fileType,
                fileGroupId = db.Groups.Where(g => g.Id == x.fileGroupId).Select(group => new GroupModel()
                {
                    Id = group.Id,
                    groupName = group.groupName
                }).FirstOrDefault(),
                fileUploaderId = db.Users.Where(g => g.Id == x.fileUploaderId).Select(custom => new CustomModel()
                {
                    Id = custom.Id,
                    userNameSurname = custom.userNameSurname,
                    userEmail = custom.userEmail,
                    userPassword = custom.userPassword,
                    userAuthority = db.Authorities.Where(y => y.Id == custom.userAuthorityId).Select(auth => new AuthorityModel()
                    {
                        Id = auth.Id,
                        authorityName = auth.authorityName,
                    }).FirstOrDefault(),
                    userGroup = db.Groups.Where(g => g.Id == custom.userGroupId).Select(g => new GroupModel()
                    {
                        Id = g.Id,
                        groupName = g.groupName,
                    }).FirstOrDefault(),
                }).FirstOrDefault(),
            }).ToList();

            return fileList;
        }

        [HttpPut]
        [Route("api/file/updateFile")]
        public ResponseModel updateFile(ViewModel.FileModel file)
        {
            Models.File record = db.Files.Where(x => x.Id == file.Id).SingleOrDefault();

            if (record != null)
            {
                record.fileName = file.fileName;
                record.fileGroupId = file.fileGroupId.Id;
                record.fileUploaderId = file.fileUploaderId.Id;
                record.fileType = file.fileType;

                db.SaveChangesAsync();

                response.process = true;
                response.message = "Dosya Güncellendi";
                return response;
            }
            response.process = false;
            response.message = "Dosya Bulunamadı";
            return response;
        }


        [HttpDelete]
        [Route("api/file/deleteFile/{fileId}")]
        public ResponseModel deleteFile(int fileId)
        {
            Models.File file = db.Files.Where(x => x.Id == fileId).SingleOrDefault();
            if (file != null)
            {
                db.Files.Remove(file);
                db.SaveChangesAsync();
                response.process = true;
                response.message = "Dosya Silindi";
                return response;
            }
            response.process = false;
            response.message = "Dosya Bulunamadı";
            return response;
        }


        [HttpGet]
        [Route("api/file/allFiles")]
        public IHttpActionResult GetFiles()
        {
            // "uploads" klasöründe bulunan dosyaların isimlerini al
            string[] fileNames = Directory.GetFiles(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/uploads")));

            // Sadece dosya adlarını döndür (yol bilgisi olmadan)
            for (int i = 0; i < fileNames.Length; i++)
            {
                fileNames[i] = Path.GetFileName(fileNames[i]);
            }

            return Ok(fileNames);
        }

        [HttpPost]
        [Route("api/file/uploadFile")]
        public ResponseModel UploadFile()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                // Kontrol edin, bir dosya gönderildi mi?
                if (httpRequest.Files.Count > 0)
                {
                    var postedFile = httpRequest.Files[0]; // İlk dosyayı alın

                    // İstediğiniz klasöre dosyayı kaydedin veya başka bir işlem yapın
                    var filePath = HttpContext.Current.Server.MapPath("~/uploads/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    // Başarılı durumunu döndürün
                    var response = new ResponseModel
                    {
                        process = true,
                        message = "Dosya Yüklendi"
                    };
                    return response;
                }

                // Dosya gönderilmediyse hata durumunu döndürün
                var errorResponse = new ResponseModel
                {
                    process = false,
                    message = "Dosya Yüklenemedi"
                };
                return errorResponse;
            }
            catch (Exception ex)
            {
                // Hata durumunu döndürün
                var errorResponse = new ResponseModel
                {
                    process = false,
                    message = ex.Message
                };
                return errorResponse;
            }
        }


        [HttpGet]
        [Route("api/file/downloadFile/{fileName}/{fileType}")]
        public HttpResponseMessage DownloadFile(string fileName, string fileType)
        {
            string fullFileName = fileName + "." + fileType;
            var filePath = Path.Combine("C:\\Users\\Engin Bolat\\source\\repos\\DosyaYonetimPortalı\\DosyaYonetimPortalı\\Uploads", fullFileName);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(fileBytes);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = fileName;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return response;
        }
        #endregion
    }
}
