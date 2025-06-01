

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microsoft.Practices.Prism.StoreApps.Interfaces
{
    public interface ISessionStateService
    {
        Dictionary<string, object> SessionState { get; }

        void RegisterKnownType(Type type);

        Task SaveAsync();

        Task RestoreSessionStateAsync();

        void RestoreFrameState();

        // void RegisterFrame(IFrameFacade frame, String sessionStateKey);

        // void UnregisterFrame(IFrameFacade frame);

        // Dictionary<String, Object> GetSessionStateForFrame(IFrameFacade frame);
    }
}
