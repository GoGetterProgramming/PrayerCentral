using System;
using System.Collections.Generic;
using System.Text;

namespace PrayerCentral.Common.Utilities
{
    public class Modified<T> : NotifyPropertyChangedBase
    {
        private T _Value;
        private bool _IsEdited;

        public T Value
        {
            get => _Value;
            set
            {
                SetProperty(ref _Value, value);
            }
        }

        public bool IsEditied => _IsEdited;

        public Modified(T value)
        {
            _Value = value;
        }

        public static implicit operator T(Modified<T> modified) => modified.Value;

        public static implicit operator Modified<T>(T value) => new Modified<T>(value);
    }

    public class TestClass
    {
        public void Drum()
        {
            Modified<int> modified = 3;

            modified = 4;
        }
    }
}
