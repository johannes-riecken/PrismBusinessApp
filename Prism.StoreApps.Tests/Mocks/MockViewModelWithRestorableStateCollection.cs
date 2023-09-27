

using System.Collections.Generic;

namespace Microsoft.Practices.Prism.StoreApps.Tests.Mocks
{
    public class MockViewModelWithRestorableStateCollection : ViewModel
    {
        private string _title;
        private string _description;
        private ICollection<BindableBase> _childViewModels;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }

        }

        [RestorableState]
        public ICollection<BindableBase> ChildViewModels
        {
            get { return _childViewModels; }
            set { SetProperty(ref _childViewModels, value); }
        }
    }
}
