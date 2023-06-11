using DosyaYonetimPortalı.Models;
using DosyaYonetimPortalı.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DosyaYonetimPortalı.Auth
{
    public class MemberService
    {
        Db01Entities4 db = new Db01Entities4();
        public CustomModel MemberLogin(string email, string password)
        {
            CustomModel member = db.Users.Where(s => s.userEmail == email && s.userPassword == password).Select(x => new CustomModel()
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
            }).SingleOrDefault();

            return member;
        }
    }
}