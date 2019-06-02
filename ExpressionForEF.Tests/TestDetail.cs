using System;
#if NET
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Objects;
#elif NETSTANDARD
using Microsoft.EntityFrameworkCore;
#endif


namespace ExpressionForEF.Tests
{

    public class TestDetail
    {
        public Guid Id { get; set; }
        public decimal Decimal { get; set; }
        public float Float { get; set; }
        public double Double { get; set; }
        public bool Bool { get; set; }
        public short Short { get; set; }
        public ushort UShort { get; set; }
        public long Long { get; set; }
        public ulong ULong { get; set; }
        public byte Byte { get; set; }
    }
}
