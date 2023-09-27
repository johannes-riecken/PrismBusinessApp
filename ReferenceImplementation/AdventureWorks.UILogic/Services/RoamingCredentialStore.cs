

using System;
using System.Linq;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.Security.Credentials;

namespace AdventureWorks.UILogic.Services
{
    public class RoamingCredentialStore : ICredentialStore
    {
        public void SaveCredentials(string resource, string userName, string password)
        {
            var vault = new PasswordVault();

            RemoveAllCredentialsByResource(resource, vault);

            var passwordCredential = new PasswordCredential(resource, userName, password);
            vault.Add(passwordCredential); 
        }

        private static void RemoveAllCredentialsByResource(string resource, PasswordVault vault)
        {
            try
            {
                var oldCredentials = vault.FindAllByResource(resource);
                foreach (var oldCredential in oldCredentials)
                {
                    vault.Remove(oldCredential);
                }
            }
            catch (Exception)
            {
            } // FindAllByResource throws Exception if nothing stored for that resource
        }

        public PasswordCredential GetSavedCredentials(string resource)
        {
            try
            {
                var vault = new PasswordVault();
                var credentials = vault.FindAllByResource(resource);
                var cred = credentials.FirstOrDefault();
                if (cred != null)
                    return vault.Retrieve(resource, cred.UserName);
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void RemoveSavedCredentials(string resource)
        {
            var vault = new PasswordVault();
            RemoveAllCredentialsByResource(resource, vault);
        }
    }
}
