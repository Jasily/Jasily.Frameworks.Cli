using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Frameworks.Cli.Tests
{
    [TestClass]
    public class TestParameterTypeConvert : BaseTest
    {
        public class CommandClass
        {
            public bool Boolean(bool value) => value;

            public char Char(char value) => value;

            public sbyte SByte(sbyte value) => value;

            public byte Byte(byte value) => value;

            public short Int16(short value) => value;

            public ushort UInt16(ushort value) => value;

            public int Int32(int value) => value;

            public uint UInt32(uint value) => value;

            public long Int64(long value) => value;

            public ulong UInt64(ulong value) => value;

            public float Single(float value) => value;

            public double Double(double value) => value;

            public decimal Decimal(decimal value) => value;

            public DateTime DateTime(DateTime value) => value;

            public string String(string value) => value;

            public string[] Array(string[] value) => value;

            public string[] ParamsArray(params string[] value) => value;
        }

        [TestMethod]
        public void TypeShouldConvertCorrect()
        {
            var engine = this.Build(new CommandClass(), out var sb);
            Assert.AreEqual(5, engine.Execute(new[] { nameof(CommandClass.Int32), "5" }));
            Assert.AreEqual((uint)5, engine.Execute(new[] { nameof(CommandClass.UInt32), "5" }));
            Assert.AreEqual((long)5, engine.Execute(new[] { nameof(CommandClass.Int64), "5" }));
            Assert.AreEqual((ulong)5, engine.Execute(new[] { nameof(CommandClass.UInt64), "5" }));
            Assert.AreEqual((float)5, engine.Execute(new[] { nameof(CommandClass.Single), "5" }));
            Assert.AreEqual((double)5, engine.Execute(new[] { nameof(CommandClass.Double), "5" }));
        }

        [TestMethod]
        public void TypeConvertWrong()
        {
            var engine = this.Build(new CommandClass(), out var sb);
            Assert.AreEqual(null, engine.Execute(new[] { nameof(CommandClass.Int32), "5.2" }));
            //Assert.AreEqual((uint)5, engine.Execute(new[] { nameof(CommandClass.UInt32), "5" }));
            //Assert.AreEqual((long)5, engine.Execute(new[] { nameof(CommandClass.Int64), "5" }));
            //Assert.AreEqual((ulong)5, engine.Execute(new[] { nameof(CommandClass.UInt64), "5" }));
            //Assert.AreEqual((float)5, engine.Execute(new[] { nameof(CommandClass.Single), "5" }));
            //Assert.AreEqual((double)5, engine.Execute(new[] { nameof(CommandClass.Double), "5" }));
        }
    }
}