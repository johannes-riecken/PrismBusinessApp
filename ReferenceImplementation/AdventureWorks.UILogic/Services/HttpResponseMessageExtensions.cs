

using AdventureWorks.UILogic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace AdventureWorks.UILogic.Services
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task EnsureSuccessWithValidationSupportAsync(this HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ModelValidationResult result = null;
                try
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ModelValidationResult>(responseContent);
                }
                catch { } // Fall through logic will take care of it
                if (result != null) throw new ModelValidationException(result);

            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new SecurityException();

            response.EnsureSuccessStatusCode(); // Will throw for any other service call errors
        }
    }
}
