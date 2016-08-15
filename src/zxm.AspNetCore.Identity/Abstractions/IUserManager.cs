using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace zxm.AspNetCore.Identity.Abstractions
{
    public interface IUserManager<TIdentityUser> where TIdentityUser : class
    {
        Task<string> TrySignIn(string userName, string password);

        Task TrySignOut(string token);

        Task<TIdentityUser> GetUser(string token);
    }
}
