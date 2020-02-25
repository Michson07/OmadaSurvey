using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Omada.Areas.Identity.Data;
using Omada.Models;

namespace Omada.Pages.ManageUser
{
    public class UserData : IUserData
    {
        private readonly OmadaContext db;
        public UserData(OmadaContext db)
        {
            this.db = db;
        }
        public OmadaUser Add(OmadaUser newUser)
        {
            db.Add(newUser);
            return newUser;
        }

        public OmadaUser Delete(string id)
        {
            OmadaUser user = GetUserById(id);
            if(user != null)
            {
                db.Remove(user);
            }
            return user;
        }

        public OmadaUser Edit(OmadaUser updatedUser)
        {
            var entity = db.Users.Attach(updatedUser);
            entity.State = EntityState.Modified;
            return updatedUser;
        }

        public OmadaUser GetUserById(string id)
        {
            return db.Users.Find(id);
        }
        public string GetUserEmail(OmadaUser user)
        {
            return db.Users.Find(user).Email;
        }
        public int Commit()
        {
            return db.SaveChanges();
        }
    }
}
