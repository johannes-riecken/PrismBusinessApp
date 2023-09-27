

using Windows.Security.Credentials;

namespace Microsoft.Practices.Prism.StoreApps.Interfaces
{
    public interface ICredentialStore
    {
        void SaveCredentials(string resource, string userName, string password);

        PasswordCredential GetSavedCredentials(string resource);

        void RemoveSavedCredentials(string resource);
    }
}
