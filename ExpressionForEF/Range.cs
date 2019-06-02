using System;
using System.Collections;

namespace ExpressionForEF
{
    public struct Range
    {
        public Range((object from, object to) range, bool hasFrom = true, bool hasTo = true)
        {
            From = range.from;
            To = range.to;
            HasFrom = hasFrom;
            HasTo = hasTo;
        }
        public Range(object obj, bool hasFrom = true, bool hasTo = true)
        {
            From = obj;
            To = obj;
            HasFrom = hasFrom;
            HasTo = hasTo;
        }
        public object From;
        public object To;
        public bool HasFrom;
        public bool HasTo;
        public bool IsExactlyArray => typeof(IEnumerable).IsAssignableFrom(From.GetType());
        public bool IsComputable => IsNumberOrDateTime(From.GetType());
        public bool IsSingleValue => !IsComputable || From == To;
        public bool IsExcept => To is null || (IsComputable && Convert.ToDecimal(To) > Convert.ToDecimal(From));

        public static implicit operator Range(Array array)
            => new Range(array);
        public static implicit operator Range(ArrayList array)
            => new Range(array);
        public static implicit operator Range(short obj)
            => new Range(obj);
        public static implicit operator Range(int obj)
            => new Range(obj);
        public static implicit operator Range(long obj)
            => new Range(obj);
        public static implicit operator Range(float obj)
            => new Range(obj);
        public static implicit operator Range(double obj)
            => new Range(obj);
        public static implicit operator Range(decimal obj)
            => new Range(obj);
        public static implicit operator Range(ushort obj)
            => new Range(obj);
        public static implicit operator Range(uint obj)
            => new Range(obj);
        public static implicit operator Range(ulong obj)
            => new Range(obj);
        public static implicit operator Range(DateTime obj)
            => new Range(obj);
        public static implicit operator Range(string obj)
            => new Range(obj);
        public static implicit operator Range(TimeSpan obj)
            => new Range(obj);
        public static implicit operator Range(DateTimeOffset obj)
            => new Range(obj);
        public static implicit operator Range((object from, object to) range)
            => new Range(range);
        /*
                public static implicit operator Range((short from, short? to) range)
                    => new Range(range);
                public static implicit operator Range((int from, int? to) range)
                    => new Range(range);
                public static implicit operator Range((long from, long? to) range)
                    => new Range(range);
                public static implicit operator Range((float from, float? to) range)
                    => new Range(range);
                public static implicit operator Range((double from, double? to) range)
                    => new Range(range);
                public static implicit operator Range((decimal from, decimal? to) range)
                    => new Range(range);
                public static implicit operator Range((ushort from, ushort? to) range)
                    => new Range(range);
                public static implicit operator Range((uint from, uint? to) range)
                    => new Range(range);
                public static implicit operator Range((ulong from, ulong? to) range)
                    => new Range(range);

                public static implicit operator Range((DateTime from, DateTime? to) range)
                    => new Range(range);
                public static implicit operator Range((DateTimeOffset from, DateTimeOffset? to) range)
                    => new Range(range);
                public static implicit operator Range((TimeSpan from, TimeSpan? to) range)
                    => new Range(range);*/

        private static bool IsNumberOrDateTime(Type type)
        {
            var code = Type.GetTypeCode(type);
            return TypeCode.Int16 <= code && code <= TypeCode.DateTime;
        }
    }
}