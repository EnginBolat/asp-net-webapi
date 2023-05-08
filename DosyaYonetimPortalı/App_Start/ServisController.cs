using DosyaYonetimPortalı.Models;
using DosyaYonetimPortalı.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DosyaYonetimPortalı.App_Start
{
    public class ServisController : ApiController
    {
        DosyaYonetimPortaliEntities1 db = new DosyaYonetimPortaliEntities1();
        SonucModel sonuc = new SonucModel();

        #region User

        [HttpGet]
        [Route("api/allUsers")]
        public List<UserModel> allUsers()
        {
            List<UserModel> userList = db.users.Select(x => new UserModel()
            {
                id = x.Id,
                name = x.name,
                surname = x.surname,
                email = x.email,
                pass = x.password,
                authId = x.authorityId,
                groupId = x.groupId,

            }).ToList();

            return userList;
        }

        [HttpGet]
        [Route("api/userById/{id}")]
        public UserModel userById(int id)
        {
            UserModel user = db.users.Where(x => x.Id == id).Select(y => new UserModel()
            {

                id = y.Id,
                name = y.name,
                surname = y.surname,
                email = y.email,
                pass = y.password,
                authId = y.authorityId,
                groupId = y.groupId,

            }).SingleOrDefault();

            return user;
        }

        [HttpPost]
        [Route("api/addUser")]
        public SonucModel addUser(UserModel user)
        {
            if (db.users.Count(c => c.name == user.name) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen İsim Kayıtlıdır!";
                return sonuc;
            }

            user record = new user();
            record.Id = user.id;
            record.name = user.name;
            record.surname = user.surname;
            record.email = user.email;
            record.password = user.pass;
            record.authorityId = user.authId;
            record.groupId = user.groupId;

            db.users.Add(record);
            db.SaveChanges();



            sonuc.islem = true;
            sonuc.mesaj = "Kullanıcı Eklendi";
            return sonuc;
        }

        [HttpPost]
        [Route("api/updateUser")]
        public SonucModel updateUser(UserModel user)
        {
            user userObject = db.users.Where(x => x.Id == user.id).SingleOrDefault();

            if (userObject != null)
            {
                userObject.Id = user.id;
                userObject.name = user.name;
                userObject.surname = user.surname;
                userObject.email = user.email;
                userObject.password = user.pass;
                userObject.groupId = user.groupId;
                userObject.authorityId = user.authId;

                db.SaveChangesAsync();

                sonuc.islem = true;
                sonuc.mesaj = "Kullanıcı Güncellendi";

                return sonuc;
            }

            sonuc.islem = false;
            sonuc.mesaj = "Böyle Bir Kullanıcı Bulunamadı!";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/deleteUser/{id}")]
        public SonucModel deleteUser(int id)
        {
            user record = db.users.Where(x => x.Id == id).SingleOrDefault();

            if (record == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı";
                return sonuc;
            }
            if (db.files.Count(c => c.uploadedId == id) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Kullanıcı Daha Önceden Dosya Yüklediği İçin Silinemez";
                return sonuc;
            }

            db.users.Remove(record);
            db.SaveChangesAsync();

            sonuc.islem = true;
            sonuc.mesaj = "Kayıt Silindi";
            return sonuc;
        }

        [HttpPost]
        [Route("api/changeUserAuth")]
        public SonucModel changeUserAuth(int userId, int authId)
        {
            user userObject = db.users.Where(x => x.Id == userId).SingleOrDefault();
            authority authObject = db.authorities.Where(x => x.Id == authId).SingleOrDefault();

            if (userObject != null)
            {
                if (authObject != null)
                {
                    user record = new user();
                    record.Id = userObject.Id;
                    record.name = userObject.name;
                    record.surname = userObject.surname;
                    record.email = userObject.email;
                    record.password = userObject.password;
                    record.authorityId = authId;
                    record.groupId = userObject.groupId;

                    db.SaveChangesAsync();

                    sonuc.islem = true;
                    sonuc.mesaj = "Kayıt Güncellendi";
                    return sonuc;
                }
                else
                {
                    sonuc.islem = false;
                    sonuc.mesaj = "Böyle Bir Yetki Mevcut Değil";
                    return sonuc;
                }
            }
            sonuc.islem = false;
            sonuc.mesaj = "Böyle Bir Kullanıcı Mevcut Değil";
            return sonuc;
        }

        [HttpPost]
        [Route("api/changeUserGroup")]
        public SonucModel changeUserGroup(int userId, int groupId)
        {
            user userObject = db.users.Where(x => x.Id == userId).SingleOrDefault();
            group groupObject = db.groups.Where(x => x.Id == groupId).SingleOrDefault();

            if (userObject != null)
            {
                if (groupObject != null)
                {
                    user record = new user();
                    record.Id = userObject.Id;
                    record.name = userObject.name;
                    record.surname = userObject.surname;
                    record.email = userObject.email;
                    record.password = userObject.password;
                    record.authorityId = userObject.authorityId;
                    record.groupId = groupId;

                    db.SaveChangesAsync();


                    sonuc.islem = true;
                    sonuc.mesaj = "Kayıt Güncellendi";
                    return sonuc;
                }
                else
                {
                    sonuc.islem = false;
                    sonuc.mesaj = "Böyle Bir Grup Mevcut Değil";
                    return sonuc;
                }
            }
            sonuc.islem = false;
            sonuc.mesaj = "Böyle Bir Kullanıcı Mevcut Değil";
            return sonuc;
        }

        #endregion

        #region Group

        [HttpGet]
        [Route("api/groupList")]
        public List<GroupModel> groupList()
        {
            List<GroupModel> groupList = db.groups.Select(x => new GroupModel()
            {
                id = x.Id,
                name = x.name,
            }).ToList();

            return groupList;
        }

        [HttpGet]
        [Route("api/groupById/{id}")]
        public GroupModel groupbyId(int id)
        {
            GroupModel group = db.groups.Where(x => x.Id == id).Select(n => new GroupModel()
            {
                id = n.Id,
                name = n.name,
            }).SingleOrDefault();
            return group;
        }

        [HttpPost]
        [Route("api/addGroup")]
        public SonucModel addGroup(String name)
        {
            group groupObject = db.groups.Where(x => x.name == name).SingleOrDefault();

            if (groupObject != null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Grup Daha Önceden Sisteme Eklenmiş!";

                return sonuc;
            }


            group record = new group();
            record.name = name;

            db.groups.Add(record);
            db.SaveChangesAsync();

            sonuc.islem = true;
            sonuc.mesaj = "Grup Başarıyla Eklendi";

            return sonuc;
        }

        [HttpPost]
        [Route("api/updateGroup")]
        public SonucModel updateGroup(GroupModel group)
        {
            group groupObject = db.groups.Where(x => x.Id == group.id).SingleOrDefault();
            if (groupObject != null)
            {
                groupObject.Id = group.id;
                groupObject.name = group.name;

                db.SaveChangesAsync();

                sonuc.islem = true;
                sonuc.mesaj = "Grup İsmi Güncellendi";
                return sonuc;

            }

            sonuc.islem = false;
            sonuc.mesaj = "Böyle Bir Grup Bulunamadı!";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/deleteGroup/{groupId}")]
        public SonucModel deleteGroup(int groupId)
        {
            group groupObject = db.groups.Where(x => x.Id == groupId).SingleOrDefault();
            file fileObject = db.files.Where(x => x.groupId == groupId).SingleOrDefault();
            user userObject = db.users.Where(x => x.groupId == groupId).SingleOrDefault();

            if (groupObject != null)
            {

                if (fileObject != null || userObject != null)
                {
                    sonuc.islem = false;
                    sonuc.mesaj = "Grup Bir Dosya veya Kullanıcıya Bağlı Olduğu İçin Silinemez!";
                    return sonuc;
                }
                else
                {
                    db.groups.Remove(groupObject);
                    db.SaveChangesAsync();

                    sonuc.islem = false;
                    sonuc.mesaj = "Grup Başarılı Bir Şekilde Silindi!";
                    return sonuc;
                }

            }

            sonuc.islem = false;
            sonuc.mesaj = "Böyle Bir Grup Bulunamadı!";
            return sonuc;
        }


        #endregion

        #region Authority

        [HttpGet]
        [Route("api/authList")]
        public List<AuthModel> authList()
        {
            List<AuthModel> authList = db.authorities.Select(x => new AuthModel()
            {
                id = x.Id,
                name = x.name,
            }).ToList();

            return authList;
        }

        [HttpGet]
        [Route("api/authById/{id}")]
        public AuthModel authById(int id)
        {
            AuthModel auth = db.groups.Where(x => x.Id == id).Select(n => new AuthModel()
            {
                id = n.Id,
                name = n.name,
            }).SingleOrDefault();

            return auth;
        }

        [HttpPost]
        [Route("api/addAuth")]
        public SonucModel addAuth(String name)
        {
            authority authObject = db.authorities.Where(x => x.name == name).SingleOrDefault();

            if (authObject != null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Yetki Daha Önceden Sisteme Eklenmiş!";

                return sonuc;
            }


            authority record = new authority();
            record.name = name;

            db.authorities.Add(record);
            db.SaveChangesAsync();

            sonuc.islem = true;
            sonuc.mesaj = "Yetki Başarıyla Eklendi";

            return sonuc;
        }

        [HttpPost]
        [Route("api/updateAuth")]
        public SonucModel updateAuth(AuthModel auth)
        {
            authority authObject = db.groups.Where(x => x.Id == authority.id).SingleOrDefault();
            if (authObject != null)
            {
                authObject.Id = auth.id;
                authObject.name = auth.name;

                db.SaveChangesAsync();

                sonuc.islem = true;
                sonuc.mesaj = "Yetki İsmi Güncellendi";
                return sonuc;

            }

            sonuc.islem = false;
            sonuc.mesaj = "Böyle Bir Yetki Bulunamadı!";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/deleteAuth/{authId}")]
        public SonucModel deleteAuth(int authId)
        {
            authority authObject = db.authorities.Where(x => x.Id == authId).SingleOrDefault();
            user userObject = db.users.Where(x => x.authorityId == authId).SingleOrDefault();

            if (authObject != null)
            {

                if (userObject != null)
                {
                    sonuc.islem = false;
                    sonuc.mesaj = "Yetki Bir Kullanıcıya Bağlı Olduğu İçin Silinemez!";
                    return sonuc;
                }
                else
                {
                    db.authorities.Remove(authObject);
                    db.SaveChangesAsync();

                    sonuc.islem = false;
                    sonuc.mesaj = "Yetki Başarılı Bir Şekilde Silindi!";
                    return sonuc;
                }

            }

            sonuc.islem = false;
            sonuc.mesaj = "Yetki Bir Grup Bulunamadı!";
            return sonuc;
        }
        #endregion

        #region FileType

        [HttpGet]
        [Route("api/fileTypeList")]
        public List<FileType> fileTypeList()
        {
            List<FileType> fileTypeList = db.filetypes.Select(x => new FileType()
            {
                id = x.Id,
                name = x.name,
            }).ToList();

            return fileTypeList;
        }

        [HttpGet]
        [Route("api/fileTypeById/{id}")]
        public FileType fileTypeById(int id)
        {
            FileType fileType = db.filetypes.Where(x => x.Id == id).Select(n => new FileType()
            {
                id = n.Id,
                name = n.name,
            }).SingleOrDefault();

            return fileType;
        }

        [HttpPost]
        [Route("api/addFileType")]
        public SonucModel addFileType(String name)
        {
            filetype fileTypeObject = db.filetypes.Where(x => x.name == name).SingleOrDefault();

            if (fileTypeObject != null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu Dosya Türü Daha Önceden Sisteme Eklenmiş!";

                return sonuc;
            }


            filetype record = new filetype();
            record.name = name;

            db.filetypes.Add(record);
            db.SaveChangesAsync();

            sonuc.islem = true;
            sonuc.mesaj = "Dosya Türü Başarıyla Eklendi";

            return sonuc;
        }

        [HttpPost]
        [Route("api/updateFileType")]
        public SonucModel updateFileType(FileType fileType)
        {
            filetype fileTypeObject = db.groups.Where(x => x.Id == filetype.id).SingleOrDefault();
            if (fileTypeObject != null)
            {
                fileTypeObject.Id = fileType.id;
                fileTypeObject.name = fileType.name;

                db.SaveChangesAsync();

                sonuc.islem = true;
                sonuc.mesaj = "Dosya Türü İsmi Güncellendi";
                return sonuc;

            }

            sonuc.islem = false;
            sonuc.mesaj = "Böyle Bir Dosya Türü Bulunamadı!";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/deleteAuth/{fileTypeId}")]
        public SonucModel deleteFileType(int fileTypeId)
        {
            filetype fileTypeObject = db.filetypes.Where(x => x.Id == fileTypeId).SingleOrDefault();
            file fileObject = db.files.Where(x => x.typeId == fileTypeId).SingleOrDefault();

            if (fileTypeObject != null)
            {

                if (fileObject != null)
                {
                    sonuc.islem = false;
                    sonuc.mesaj = "Dosya Türü Bir Dosyaya Bağlı Olduğu İçin Silinemez!";
                    return sonuc;
                }
                else
                {
                    db.filetypes.Remove(fileTypeObject);
                    db.SaveChangesAsync();

                    sonuc.islem = false;
                    sonuc.mesaj = "Dosya Türü Başarılı Bir Şekilde Silindi!";
                    return sonuc;
                }

            }

            sonuc.islem = false;
            sonuc.mesaj = "Dosya Türü Bir Grup Bulunamadı!";
            return sonuc;
        }
        #endregion

        #region File
        #endregion
    }
}
