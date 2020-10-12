using System.Collections.Generic;

namespace DataStore
{
    public class GenericComparer<TItem> : IEqualityComparer<TItem>
    {
        public delegate dynamic Field(TItem obj);
        public Field GetComparableField { get; set; }


        public bool Equals(TItem x, TItem y)
        {
            return GetComparableField(x) == GetComparableField(y);
        }

        public int GetHashCode(TItem obj)
        {
            return GetComparableField(obj).GetHashCode();
        }
    }
}


