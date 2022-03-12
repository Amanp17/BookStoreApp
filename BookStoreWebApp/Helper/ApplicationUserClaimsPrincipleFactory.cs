using BookStoreWebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStoreWebApp.Helper
{
    public class ApplicationUserClaimsPrincipleFactory:UserClaimsPrincipalFactory<AccountUser,IdentityRole>
    {
        //The IOptions service is used to bind strongly types options class to configuration 
        //section and registers it to the Asp.Net Core Dependency Injection Service Container as singleton lifetime.
        public ApplicationUserClaimsPrincipleFactory(UserManager<AccountUser> userManager,
            RoleManager<IdentityRole> identityRole,IOptions<IdentityOptions> option):base(userManager,identityRole,option)
        {

        }
        
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AccountUser user)     //Generate the claims for a user
        {
            var Identity = await base.GenerateClaimsAsync(user);
            //Adding Claims to Identity
            Identity.AddClaim(new Claim("UserFirstName", user.FirstName ?? ""));
            Identity.AddClaim(new Claim("UserLastName", user.LastName ?? ""));
            return Identity;

            
        }
    }
}
