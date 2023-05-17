using DosyaYonetimPortalı.Models;
using DosyaYonetimPortalı.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public List<UserModel> userList()
        {
            List<UserModel> userList = db.Users.Select(x => new UserModel()
            {
                Id = x.Id,
                userNameSurname = x.userNameSurname,
                userEmail = x.userEmail,
                userPassword = x.userPassword,
                userAuthorityId = x.userAuthorityId,
                userGroupId = x.userGroupId,
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
                userGroupId = y.userGroupId,
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
                response.process = true;
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
                response.message = "Kullanıcı GÜncellendi";
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
        [Route("api/user/addGroup")]
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
        [Route("api/user/addAuth")]
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

        //Dosya Tipi
        #region FileType

        [HttpGet]
        [Route("api/filetype/list")]
        public List<FileTypeModel> fileTypeList()
        {
            List<FileTypeModel> fileTypeList = db.FileTypes.Select(x => new FileTypeModel()
            {
                Id = x.Id,
                typeName = x.typeName,
            }).ToList();

            return fileTypeList;
        }

        [HttpGet]
        [Route("api/filetype/filetypeById/{fileTypeId}")]
        public FileTypeModel fileTypeIdById(int fileTypeId)
        {
            FileTypeModel record = db.FileTypes.Where(x => x.Id == fileTypeId).Select(y => new FileTypeModel()
            {
                Id = y.Id,
                typeName = y.typeName,
            }).FirstOrDefault();

            return record;
        }

        [HttpDelete]
        [Route("api/fileType/deleteFileType/{fileTypeId}")]
        public ResponseModel deleteFileType(int fileTypeId)
        {
            FileType record = db.FileTypes.Where(x => x.Id == fileTypeId).SingleOrDefault();
            if (record != null)
            {
                db.FileTypes.Remove(record);
                db.SaveChangesAsync();
                response.process = true;
                response.message = "Dosya Türü Silindi";
                return response;
            }
            response.process = false;
            response.message = "Dosya Türü Bulunamadı";
            return response;
        }

        [HttpPut]
        [Route("api/filetype/updateFileType")]
        public ResponseModel updateFileType(FileTypeModel fileType)
        {
            FileType record = db.FileTypes.Where(x => x.Id == fileType.Id).SingleOrDefault();

            if (record != null)
            {
                record.Id = fileType.Id;
                record.typeName = fileType.typeName;
                db.SaveChangesAsync();

                response.process = true;
                response.message = "Dosya Türü GÜncellendi";
                return response;
            }
            response.process = false;
            response.message = "Dosya Türü Bulunamadı";
            return response;
        }
        #endregion

        [HttpPost]
        [Route("api/file/uploadFile")]
        public ResponseModel UploadFile(UserModel user)
        {
            String fileName = "";
            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/Uploads");
            var provider =
                new MultipartFormDataStreamProvider(root);

            try
            {
                Request.Content
                    .ReadAsMultipartAsync(provider);

                foreach (var file in provider.FileData)
                {
                    var name = file.Headers
                        .ContentDisposition
                        .FileName;

                    // remove double quotes from string.
                    name = name.Trim('"');

                    var localFileName = file.LocalFileName;
                    var filePath = Path.Combine(root, name);
                    fileName = localFileName;

                    System.IO.File.Move(localFileName, filePath);
                }
            }
            catch (Exception e)
            {
                response.process = false;
                response.message = e.Message;
                return response;
            }

            Models.File record = new Models.File();
            record.fileUploaderId = user.Id;
            record.fileName = fileName;
            record.fileGroupId = user.userGroupId;



            response.process = true;
            response.message = "Dosya Eklendi";
            return response;

        }
    }
}
