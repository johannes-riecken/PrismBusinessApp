

using System;
using System.Globalization;
using Microsoft.Practices.Prism.StoreApps;

namespace AdventureWorks.UILogic.ViewModels
{
    public class CheckoutDataViewModel
    {
        public string EntityId { get; set; }

        public string Title { get; set; }

        public string FirstLine { get; set; }

        public string SecondLine { get; set; }

        public string BottomLine { get; set; }

        public Uri LogoUri { get; set; }

        public string DataType { get; set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}, {3}, {4}", DataType, Title, FirstLine, SecondLine, BottomLine);
        }
    }
}
