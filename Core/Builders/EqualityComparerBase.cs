using System;
using System.Collections.Generic;

namespace Core.Builders
{
    public static class EqualityComparerBuilder
    {
        public static EqualityComparerBase<T> New<T>()
        {
            return new EqualityComparerBase<T>();
        }
    }
    
    public class EqualityComparerBase<T> : IEqualityComparer<T>
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public static Func<T, T, bool> EqualsFunc { get; }

        // ReSharper disable once MemberCanBePrivate.Global
        public static Func<T, int> HashCodeFunc { get; }

        /// <summary>
        /// Static constructor
        /// </summary>
        static EqualityComparerBase()
        {
            EqualsFunc = new EqualsBuilder().BuildFunc<T>();
            HashCodeFunc = new HashCodeBuilder().BuildFunc<T>();
        }
        
        public bool Equals(T x, T y)
        {
            return EqualsFunc(x, y);
        }

        public int GetHashCode(T obj)
        {
            return HashCodeFunc(obj);
        }
    }
}