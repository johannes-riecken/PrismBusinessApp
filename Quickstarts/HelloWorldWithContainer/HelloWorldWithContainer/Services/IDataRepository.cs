

using System.Collections.Generic;

namespace HelloWorldWithContainer.Services
{
    public interface IDataRepository
    {
        List<string> GetFeatures();
        string GetUserEnteredData();
        void SetUserEnteredData(string data);
    }
}
