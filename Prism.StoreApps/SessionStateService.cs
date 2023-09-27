

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.ApplicationModel.Resources;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Practices.Prism.StoreApps
{
    
    public class SessionStateService : ISessionStateService
    {
        private Dictionary<string, object> _sessionState = new Dictionary<string, object>();
        private List<Type> _knownTypes = new List<Type>();

        public Dictionary<string, object> SessionState
        {
            get { return _sessionState; }
        }

        public void RegisterKnownType(Type type)
        {
            _knownTypes.Add(type);
        }

        public async Task SaveAsync()
        {
            try
            {
                foreach (var weakFrameReference in _registeredFrames)
                {
                    IFrameFacade frame;
                    if (weakFrameReference.TryGetTarget(out frame))
                    {
                        SaveFrameNavigationState(frame);
                    }
                }

                MemoryStream sessionData = new MemoryStream();
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, object>), _knownTypes);
                serializer.WriteObject(sessionData, _sessionState);

                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(Constants.SessionStateFileName, CreationCollisionOption.ReplaceExisting);
                using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    sessionData.Seek(0, SeekOrigin.Begin);
                    var provider = new DataProtectionProvider("LOCAL=user");

                    await provider.ProtectStreamAsync(sessionData.AsInputStream(), fileStream);
                    await fileStream.FlushAsync();
                }
            }
            catch (Exception e)
            {
                throw new SessionStateServiceException(e);
            }
        }

        public async Task RestoreSessionStateAsync()
        {
            _sessionState = new Dictionary<String, Object>();

            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(Constants.SessionStateFileName);
                using (IInputStream inStream = await file.OpenSequentialReadAsync())
                {
                    var memoryStream = new MemoryStream();
                    var provider = new DataProtectionProvider("LOCAL=user");

                    await provider.UnprotectStreamAsync(inStream, memoryStream.AsOutputStream());
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, object>),
                                                                                   _knownTypes);
                    _sessionState = (Dictionary<string, object>)serializer.ReadObject(memoryStream);
                }
            }
            catch (Exception e)
            {
                throw new SessionStateServiceException(e);
            }
        }


        public void RestoreFrameState()
        {
            try
            {
                foreach (var weakFrameReference in _registeredFrames)
                {
                    IFrameFacade frame;
                    if (weakFrameReference.TryGetTarget(out frame))
                    {
                        frame.ClearValue(FrameSessionStateProperty);
                        RestoreFrameNavigationState(frame);
                    }
                }
            }
            catch (Exception e)
            {
                throw new SessionStateServiceException(e);
            }
        }

        private static DependencyProperty FrameSessionStateKeyProperty =
            DependencyProperty.RegisterAttached("_FrameSessionStateKey", typeof(String), typeof(SessionStateService), null);
        private static DependencyProperty FrameSessionStateProperty =
            DependencyProperty.RegisterAttached("_FrameSessionState", typeof(Dictionary<String, Object>), typeof(SessionStateService), null);
        private static List<WeakReference<IFrameFacade>> _registeredFrames = new List<WeakReference<IFrameFacade>>();

        public void RegisterFrame(IFrameFacade frame, String sessionStateKey)
        {
            if (frame == null) throw new ArgumentNullException("frame");

            var resourceLoader = ResourceLoader.GetForCurrentView(Constants.StoreAppsInfrastructureResourceMapId);

            if (frame.GetValue(FrameSessionStateKeyProperty) != null)
            {
                var errorString = resourceLoader.GetString("FrameAlreadyRegisteredWithKey");
                throw new InvalidOperationException(errorString);
            }

            if (frame.GetValue(FrameSessionStateProperty) != null)
            {
                var errorString = resourceLoader.GetString("FrameRegistrationRequirement");
                throw new InvalidOperationException(errorString);
            }

            frame.SetValue(FrameSessionStateKeyProperty, sessionStateKey);
            _registeredFrames.Add(new WeakReference<IFrameFacade>(frame));

            RestoreFrameNavigationState(frame);
        }

        public void UnregisterFrame(IFrameFacade frame)
        {
            SessionState.Remove((String)frame.GetValue(FrameSessionStateKeyProperty));
            _registeredFrames.RemoveAll((weakFrameReference) =>
            {
                IFrameFacade testFrame;
                return !weakFrameReference.TryGetTarget(out testFrame) || testFrame == frame;
            });
        }

        public Dictionary<String, Object> GetSessionStateForFrame(IFrameFacade frame)
        {
            if (frame == null) throw new ArgumentNullException("frame");

            var frameState = (Dictionary<String, Object>)frame.GetValue(FrameSessionStateProperty);

            if (frameState == null)
            {
                var frameSessionKey = (String)frame.GetValue(FrameSessionStateKeyProperty);
                if (frameSessionKey != null)
                {
                    if (!_sessionState.ContainsKey(frameSessionKey))
                    {
                        _sessionState[frameSessionKey] = new Dictionary<String, Object>();
                    }
                    frameState = (Dictionary<String, Object>)_sessionState[frameSessionKey];
                }
                else
                {
                    frameState = new Dictionary<String, Object>();
                }
                frame.SetValue(FrameSessionStateProperty, frameState);
            }
            return frameState;
        }

        private void RestoreFrameNavigationState(IFrameFacade frame)
        {
            var frameState = GetSessionStateForFrame(frame);
            if (frameState.ContainsKey("Navigation"))
            {
                frame.SetNavigationState((String)frameState["Navigation"]);
            }
        }

        private void SaveFrameNavigationState(IFrameFacade frame)
        {
            var frameState = GetSessionStateForFrame(frame);
            frameState["Navigation"] = frame.GetNavigationState();
        }
    }
    public class SessionStateServiceException : Exception
    {
        public SessionStateServiceException() : base((ResourceLoader.GetForCurrentView(Constants.StoreAppsInfrastructureResourceMapId)).GetString("SessionStateServiceFailed"))
        {
        }

        public SessionStateServiceException(string message) : base(message)
        {
        }

        public SessionStateServiceException(Exception exception)
            : base((ResourceLoader.GetForCurrentView(Constants.StoreAppsInfrastructureResourceMapId)).GetString("SessionStateServiceFailed"), exception)
        {
        }

        public SessionStateServiceException(string message, Exception innerException):base(message, innerException)
        {
        }
    }
}
