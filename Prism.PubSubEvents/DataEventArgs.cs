

using System;

namespace Microsoft.Practices.Prism.PubSubEvents
{
    public class DataEventArgs<TData> : EventArgs
    {
        private readonly TData _value;

        public DataEventArgs(TData value)
        {
            _value = value;
        }

        public TData Value
        {
            get { return _value; }
        }
    }
}