

using System.Collections.Generic;

namespace AdventureWorks.UILogic.Models
{
    public class ModelValidationResult
    {
        public ModelValidationResult()
        {
            ModelState = new Dictionary<string, List<string>>();
        }

        public Dictionary<string,List<string>> ModelState { get; private set; }
    }
}
