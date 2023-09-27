

using System;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using System.Security;
using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace AdventureWorks.UILogic.Services
{
    public class AccountService : IAccountService
    {
        public const string SignedInUserKey = "AccountService_SignedInUser";
        public const string PasswordVaultResourceName = "AdventureWorksShopper";
        private const string UserNameKey = "AccountService_UserName";
        private const string PasswordKey = "AccountService_Password";
        

        private readonly IIdentityService _identityService;
        private readonly ISessionStateService _sessionStateService;
        private readonly ICredentialStore _credentialStore;
        private UserInfo _signedInUser;
        private string _userName;
        private string _password;

        public AccountService(IIdentityService identityService, ISessionStateService sessionStateService, ICredentialStore credentialStore)
        {
            _identityService = identityService;
            _sessionStateService = sessionStateService;
            _credentialStore = credentialStore;
            if (_sessionStateService != null)
            {
                if (_sessionStateService.SessionState.ContainsKey(SignedInUserKey))
                {
                    _signedInUser = _sessionStateService.SessionState[SignedInUserKey] as UserInfo;
                }
                if (_sessionStateService.SessionState.ContainsKey(UserNameKey))
                {
                    _userName = _sessionStateService.SessionState[UserNameKey].ToString();
                }
                if (_sessionStateService.SessionState.ContainsKey(PasswordKey))
                {
                    _password = _sessionStateService.SessionState[PasswordKey].ToString();
                }
            }
        }

        public UserInfo SignedInUser { get { return _signedInUser; } }

        public async Task<UserInfo> VerifyUserAuthenticationAsync()
        {
            try
            {
                if (_signedInUser != null && await _identityService.VerifyActiveSessionAsync(_signedInUser.UserName))
                {
                    return _signedInUser;
                }
            }
            catch (SecurityException)
            {
            }

            if (_userName != null && _password != null)
            {
                if (await SignInUserAsync(_userName, _password, false))
                {
                    return _signedInUser;
                }
            }

            return await VerifySavedCredentialsAsync();
        }

        public async Task<UserInfo> VerifySavedCredentialsAsync()
        {
            var savedCredentials = _credentialStore.GetSavedCredentials(PasswordVaultResourceName);
            if (savedCredentials != null)
            {
                savedCredentials.RetrievePassword();
                if (await SignInUserAsync(savedCredentials.UserName, savedCredentials.Password, false))
                {
                    return _signedInUser;
                }
            }
            return null;
        }

        public async Task<bool> SignInUserAsync(string userName, string password, bool useCredentialStore)
        {
            var result = await _identityService.LogOnAsync(userName, password);

            UserInfo previousUser = _signedInUser;
            _signedInUser = result.UserInfo;

            _sessionStateService.SessionState[SignedInUserKey] = _signedInUser;

            _userName = userName;
            _password = password;
            _sessionStateService.SessionState[UserNameKey] = userName;
            _sessionStateService.SessionState[PasswordKey] = password;

            if (useCredentialStore)
            {
                _credentialStore.SaveCredentials(PasswordVaultResourceName, userName, password);

            }

            if (previousUser == null)
            {
                RaiseUserChanged(_signedInUser, previousUser);
            }
            else if (_signedInUser != null && _signedInUser.UserName != previousUser.UserName)
            {   
                RaiseUserChanged(_signedInUser, previousUser);
            }
            return true;
        }

        public event EventHandler<UserChangedEventArgs> UserChanged;

        private void RaiseUserChanged(UserInfo newUserInfo, UserInfo oldUserInfo)
        {
            var handler = UserChanged;
            if (handler != null)
            {
                handler(this, new UserChangedEventArgs(newUserInfo, oldUserInfo));
            }
        }

        public void SignOut()
        {
            var previousUser = _signedInUser;
            _signedInUser = null;
            _userName = null;
            _password = null;

            _sessionStateService.SessionState.Remove(SignedInUserKey);
            _sessionStateService.SessionState.Remove(UserNameKey);
            _sessionStateService.SessionState.Remove(PasswordKey);

            _credentialStore.RemoveSavedCredentials(PasswordVaultResourceName);


            RaiseUserChanged(_signedInUser, previousUser);
        }
    }
}
