﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using SiliconStudio.Core.Diagnostics;
using SiliconStudio.Core.Storage;

namespace SiliconStudio.Core.Tests
{
    [TestFixture]
    public class TestObjectIdBuilder
    {
        [Test]
        public void TestFull()
        {
            var builder = new ObjectIdBuilder();
            var buffer = ASCIIEncoding.ASCII.GetBytes("test");
            builder.Write(buffer, 0, buffer.Length);
            var id1 = builder.ComputeHash();
            var id1_verified = new ObjectId(StringToByteArray("30ef026f687d0c55687d0c55687d0c55"));
            Assert.AreEqual(id1_verified, id1);

            builder = new ObjectIdBuilder();
            buffer = ASCIIEncoding.ASCII.GetBytes("this is a test of a longer string");
            builder.Write(buffer, 0, buffer.Length);
            var id2 = builder.ComputeHash();
            var id2_verified = new ObjectId(StringToByteArray("eb221d36fbd7e7bfd3ab4fd02fa6482b"));
            Assert.AreEqual(id2_verified, id2);
        }

        [Test]
        public void TestSimple()
        {
            var builder = new ObjectIdBuilder();
            var buffer = ASCIIEncoding.ASCII.GetBytes("0123456789ABCDEF"); // 16 bytes
            builder.Write(buffer, 0, buffer.Length);
            var id1 = builder.ComputeHash();

            // The ObjectIdSimpleBuilder must be identical to the ObjectIdBuilder when data length is module of 16 bytes
            var simpleBuilder = new ObjectIdSimpleBuilder();
            simpleBuilder.Write(BitConverter.ToInt32(ASCIIEncoding.ASCII.GetBytes("0123"), 0));
            simpleBuilder.Write(BitConverter.ToInt32(ASCIIEncoding.ASCII.GetBytes("4567"), 0));
            simpleBuilder.Write(BitConverter.ToInt32(ASCIIEncoding.ASCII.GetBytes("89AB"), 0));
            simpleBuilder.Write(BitConverter.ToInt32(ASCIIEncoding.ASCII.GetBytes("CDEF"), 0));
            var id2 = simpleBuilder.ComputeHash();
            Assert.AreEqual(id1, id2);
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
