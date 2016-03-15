﻿#region Copyright
/********************************************************************************
 * Aadhaar API for .NET
 * Copyright © 2015 Souvik Dey Chowdhury
 * 
 * This file is part of Aadhaar API for .NET.
 * 
 * Aadhaar API for .NET is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version.
 * 
 * Aadhaar API for .NET is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License
 * for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with Aadhaar API for .NET. If not, see http://www.gnu.org/licenses.
 ********************************************************************************/
#endregion

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Uidai.Aadhaar.Device;
using Xunit;

namespace Uidai.AadhaarTests.Device
{
    public class OtpContextTest
    {
        [Fact]
        public void FromXmlTest()
        {
            /*
            Assume:     ToXml(string) is correct.
            */
            var otpContext = new OtpContext();
            var xml = XElement.Parse(File.ReadAllText(Data.OtpContextXml)).Elements().ToArray();

            // Validate null argument.
            Assert.Throws<ArgumentNullException>("element", () => otpContext.FromXml(null));

            // XML must be same after loading and deserializing it.
            foreach (var element in xml)
            {
                otpContext.FromXml(element);
                Assert.True(XNode.DeepEquals(element, otpContext.ToXml("Otp")));
            }
        }

        [Fact]
        public void ToXmlTest()
        {
            var otpContext = Data.OtpContext;
            var xml = XElement.Parse(File.ReadAllText(Data.OtpContextXml)).Elements().ToArray();

            // Set: All
            Assert.True(XNode.DeepEquals(xml[0], otpContext.ToXml("Otp")));

            // Set: RequestType = AadhaarNumber
            otpContext.RequestType = OtpRequestType.AadhaarNumber;
            Assert.Throws<ArgumentException>(nameof(OtpContext.AadhaarOrMobileNumber), () => otpContext.ToXml("Otp"));

            // Set: AadhaarOrMobileNumber = 999999999999
            otpContext.AadhaarOrMobileNumber = "999999999999";
            Assert.True(XNode.DeepEquals(xml[1], otpContext.ToXml("Otp")));

            // Set: Channel = SmsAndEmail
            otpContext.Channel = OtpChannel.SmsAndEmail;
            Assert.True(XNode.DeepEquals(xml[2], otpContext.ToXml("Otp")));

            // Remove: AadhaarOrMobileNumber
            otpContext.AadhaarOrMobileNumber = null;
            Assert.Throws<ArgumentException>(nameof(OtpContext.AadhaarOrMobileNumber), () => otpContext.ToXml("Otp"));

            // Remove: Terminal
            otpContext.Terminal = null;
            Assert.Throws<ArgumentException>(nameof(OtpContext.Terminal), () => otpContext.ToXml("Otp"));
        }
    }
}