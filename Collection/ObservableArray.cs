using System;
using System.Collections.Generic;

namespace PUnity
{
    public class ObservableArray<T>
    {
        private T[] _array;
        public ObservableArray(int capacity, Func<T, bool> validationChecker, Action<int, T> doOnItemChanged)
        {
            _array = new T[capacity];
            _doOnItemChanged = doOnItemChanged;
            _validationChecker = validationChecker;

        }

        public T this[int i]
        {
            get
            {
                return _array[i];
            }
            set
            {
                if (_validationChecker != null && !_validationChecker.Invoke(value))
                    return;

                if (!EqualityComparer<T>.Default.Equals(_array[i], value))
                {
                    _array[i] = value;
                    _doOnItemChanged?.Invoke(i, _array[i]);
                }
            }
        }

        public int Length { get { return _array.Length; } }

        private Func<T, bool> _validationChecker;
        private Action<int, T> _doOnItemChanged;
    } 
}