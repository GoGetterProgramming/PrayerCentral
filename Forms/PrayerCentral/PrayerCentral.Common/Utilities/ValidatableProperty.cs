using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace PrayerCentral.Common.Utilities
{
    public class TestClass2
    {
        public TestClass2()
        {
            ValidatableProperty<int> property = 3;

            property.AddError("error");
        }
    }

    public class ValidatableProperty<T> : NotifyPropertyChangedBase
    {
        private ConcurrentQueue<string> _ErrorMessages = new ConcurrentQueue<string>();
        private T _Value;

        public bool HasErrors => _ErrorMessages.Any();

        public ValidatableProperty(T value)
        {
            _Value = value;
        }

        public static implicit operator T(ValidatableProperty<T> validatableProperty) => validatableProperty._Value;

        public static implicit operator ValidatableProperty<T>(T value) => new ValidatableProperty<T>(value);

        public void AddError(string errorMessage, [CallerMemberName] string propertyName = "")
        {
            _ErrorMessages.Enqueue(errorMessage);

            NotifyPropertyChanged(propertyName);
        }

        public static void LinkProperties(params ValidatableProperty<object>[] properties)
        {

        }
    }
}
