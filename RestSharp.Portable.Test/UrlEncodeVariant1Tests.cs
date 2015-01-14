﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web;

namespace RestSharp.Portable.Test
{
    [TestClass]
    public class UrlEncodeVariant1Tests
    {
        private readonly UrlEscapeUtility s_utility = new UrlEscapeUtility(false);

        [TestMethod]
        public void TestEscapeDataStringComplianceASCII()
        {
            var chars = new string(Enumerable.Range(32, 95).Select(x => (char)x).ToArray());
            var expected = Uri.EscapeDataString(chars);
            var test = s_utility.Escape(chars, UrlEscapeFlags.BuilderVariantListByte);
            Assert.AreEqual(expected, test);
        }

        [TestMethod]
        public void TestEscapeDataStringComplianceUmlaut()
        {
            var chars = "äöüßÄÖÜ\u007F";
            var expected = Uri.EscapeDataString(chars);
            var test = s_utility.Escape(chars, UrlEscapeFlags.BuilderVariantListByte);
            Assert.AreEqual(expected, test);
        }

        [TestMethod]
        public void TestUrlEncodeComplianceASCII()
        {
            var chars = new string(Enumerable.Range(32, 95).Select(x => (char)x).ToArray());
            var expected = HttpUtility.UrlEncode(chars);
            var test = s_utility.Escape(chars, UrlEscapeFlags.LikeUrlEncode | UrlEscapeFlags.BuilderVariantListByte);
            Assert.AreEqual(expected, test);
        }

        [TestMethod]
        public void TestUrlEncodeComplianceUmlaut()
        {
            var chars = "äöüßÄÖÜ\u007F";
            var expected = HttpUtility.UrlEncode(chars);
            var test = s_utility.Escape(chars, UrlEscapeFlags.LikeUrlEncode | UrlEscapeFlags.BuilderVariantListByte);
            Assert.AreEqual(expected, test);
        }
    }
}
