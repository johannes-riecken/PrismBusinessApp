

using System;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Services
{
    public interface IAccountService
    {
        UserInfo SignedInUser { get; }

        Task<UserInfo> VerifyUserAuthenticationAsync();
        Task<UserInfo> VerifySavedCredentialsAsync();

        Task<bool> SignInUserAsync(string userName, string password, bool useCredentialStore);
        
        void SignOut();
        
        event EventHandler<UserChangedEventArgs> UserChanged;
    }
}
