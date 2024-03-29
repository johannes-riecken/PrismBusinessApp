

using System;
using System.ComponentModel.DataAnnotations;
using AdventureWorks.WebServices.Strings;

namespace AdventureWorks.WebServices.Models
{
    public class ShippingMethod
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string EstimatedTime { get; set; }

        public double Cost { get; set; }
    }
}
