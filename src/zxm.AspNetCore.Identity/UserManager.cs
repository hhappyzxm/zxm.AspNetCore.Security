using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using zxm.AspNetCore.Identity.Abstractions;

namespace zxm.AspNetCore.Identity
{
    public class UserManager<TIdentityUser> : IUserManager<TIdentityUser> where TIdentityUser : class
    {
        public Task AddUser(string token, TIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUser(string token)
        {
            throw new NotImplementedException();
        }
    }
}
