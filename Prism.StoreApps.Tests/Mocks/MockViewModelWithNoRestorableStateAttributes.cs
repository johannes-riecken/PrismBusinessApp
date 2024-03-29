

using System.Collections.Generic;

namespace Microsoft.Practices.Prism.StoreApps.Tests.Mocks
{
    public class MockViewModelWithNoRestorableStateAttributes : ViewModel
    {
        private string title;
        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }

        }

        private string description;
        public string Description
        {
            get { return this.description; }
            set { this.SetProperty(ref this.description, value); }

        }

        private ICollection<ViewModel> childViewModels;

        public ICollection<ViewModel> ChildViewModels
        {
            get { return this.childViewModels; }
            set { this.SetProperty(ref this.childViewModels, value); }

        }
    }
}
