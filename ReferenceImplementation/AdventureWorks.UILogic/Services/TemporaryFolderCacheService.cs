

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.Storage;

namespace AdventureWorks.UILogic.Services
{
    public class TemporaryFolderCacheService : ICacheService
    {
        private static readonly StorageFolder _cacheFolder = ApplicationData.Current.TemporaryFolder;
        private static TimeSpan _expirationPolicy = new TimeSpan(0, 5, 0); // 5 minutes

        private Dictionary<string,Task> _cacheKeyPreviousTask = new Dictionary<string, Task>();

        public async Task<T> GetDataAsync<T>(string cacheKey)
        {
            await CacheKeyPreviousTask(cacheKey);
            var result = GetDataAsyncInternal<T>(cacheKey);
            SetCacheKeyPreviousTask(cacheKey, result);
            return await result;
        }
        
        private async Task<T> GetDataAsyncInternal<T>(string cacheKey)
        {
            StorageFile file = await _cacheFolder.GetFileAsync(cacheKey);
            if (file == null) throw new FileNotFoundException("File does not exist");
            
            var fileBasicProperties = await file.GetBasicPropertiesAsync();
            var expirationDate = fileBasicProperties.DateModified.Add(_expirationPolicy).DateTime;
            bool fileIsValid = DateTime.Now.CompareTo(expirationDate) < 0;
            if (!fileIsValid) throw new FileNotFoundException("Cache entry has expired.");

            string text = await FileIO.ReadTextAsync(file);
            var toReturn = Deserialize<T>(text);

            return toReturn;
        }

        public async Task SaveDataAsync<T>(string cacheKey, T content)
        {
            await CacheKeyPreviousTask(cacheKey);
            var result = SaveDataAsyncInternal<T>(cacheKey, content);
            SetCacheKeyPreviousTask(cacheKey, result);
            await result;
        }

        private async Task SaveDataAsyncInternal<T>(string cacheKey, T content)
        {
            StorageFile file = await _cacheFolder.CreateFileAsync(cacheKey, CreationCollisionOption.ReplaceExisting);

            var textContent = Serialize<T>(content);
            await FileIO.WriteTextAsync(file, textContent);
        }
        
        private async Task CacheKeyPreviousTask(string cacheKey)
        {
           if (_cacheKeyPreviousTask.ContainsKey(cacheKey))
           {
               Task previousTask = null;
               while (_cacheKeyPreviousTask[cacheKey] != previousTask)
               {
                   previousTask = _cacheKeyPreviousTask[cacheKey];
                   try
                   {
                       await previousTask;
                   }
                   catch (Exception)
                   {
                   }
               }                  
           }
        }

        private void SetCacheKeyPreviousTask(string cacheKey, Task task)
        {
            if (_cacheKeyPreviousTask.ContainsKey(cacheKey))
            {
                _cacheKeyPreviousTask[cacheKey] = task;
            }
            else
            {
                _cacheKeyPreviousTask.Add(cacheKey, task);
            }
        }
        
        private static T Deserialize<T>(string json)
        {
            var jsonBytes = Encoding.Unicode.GetBytes(json);
            using (var jsonStream = new MemoryStream(jsonBytes))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                var toReturn = (T)serializer.ReadObject(jsonStream);

                return toReturn;
            }
        }

        private static string Serialize<T>(T entity)
        {
            var stream = new MemoryStream();
            StreamReader streamReader = null;
            try
            {
                var serializer = new DataContractJsonSerializer(entity.GetType());
                serializer.WriteObject(stream, entity);

                stream.Seek(0, SeekOrigin.Begin);

                streamReader = new StreamReader(stream);
                return streamReader.ReadToEnd();
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Dispose();
                }
                else
                {
                    stream.Dispose();
                }
            }
        }
    }
}
