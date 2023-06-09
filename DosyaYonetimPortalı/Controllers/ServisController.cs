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
    public class ServisController : ApiController
    {
        Db01Entities1 db = new Db01Entities1();
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
        public UserModel userById(int userId)
        {
            UserModel record = db.Users.Where(x => x.Id == userId).Select(y => new UserModel()
            {
                Id = y.Id,
                userNameSurname = y.userNameSurname,
                userEmail = y.userEmail,
                userPassword = y.userPassword,
                userAuthorityId = y.userAuthorityId,
                userGroupId = y.userGroupId ?? 1,
            }).FirstOrDefault();

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
        public ResponseModel addUser(UserModel user)
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
            record.userAuthorityId = user.userAuthorityId;
            record.userGroupId = user.userGroupId;
            db.Users.Add(record);
            db.SaveChanges();
            response.process = true;
            response.message = "Kullanıcı Eklendi";
            return response;
        }

        [HttpPut]
        [Route("api/user/updateUser")]
        public ResponseModel updateUser(UserModel user)
        {
            User record = db.Users.Where(x => x.Id == user.Id).SingleOrDefault();

            if (record != null)
            {
                record.userNameSurname = user.userNameSurname;
                record.userEmail = user.userEmail;
                record.userPassword = user.userPassword;
                record.userAuthorityId = user.userAuthorityId;
                record.userGroupId = user.userGroupId;

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

        #region File

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

                    // Hata durumunu döndürün
                    response.process = true;
                    response.message = "Dosya Yüklendi";
                    return response;
                }

                // Dosya gönderilmediyse hata durumunu döndürün
                response.process = false;
                response.message = "Dosya Yüklenemedi";
                return response;
            }
            catch (Exception ex)
            {
                // Hata durumunu döndürün
                response.process = false;
                response.message = ex.Message;
                return response;
            }
        }

        [HttpGet]
        [Route("api/download/{fileName}")]
        public IHttpActionResult DownloadFile(string fileName)
        {
            try
            {
                string uploadsFolder = HttpContext.Current.Server.MapPath("~/uploads/");
                string filePath = Path.Combine(uploadsFolder, fileName);

                // Dosya var mı diye kontrol edin
                if (System.IO.File.Exists(filePath))
                {
                    // Dosyayı bir byte dizisine okuyun
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    // Yanıtı bir StreamContent nesnesi olarak oluşturun
                    var responseContent = new StreamContent(new MemoryStream(fileBytes));

                    // İndirilecek dosyanın MIME türünü ayarlayın
                    responseContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // İndirilecek dosyanın adını ayarlayın
                    responseContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = fileName
                    };

                    // Dosyayı içeren yanıtı döndürün
                    return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = responseContent
                    });
                }

                // Dosya bulunamadıysa hata durumunu döndürün
                return NotFound();
            }
            catch (Exception ex)
            {
                // Hata durumunu döndürün
                return InternalServerError(ex);
            }
        }
        #endregion
    }
}
