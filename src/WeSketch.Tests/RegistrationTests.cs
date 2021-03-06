﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Threading;

namespace WeSketch.Tests
{

    /// <summary>
    /// Testing registration - CreateUser
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    class RegistrationTests
    {

        /// <summary>
        /// Method with test cases for creating a user name, email, and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        [Test]
        [TestCase("", "", "")]
        [TestCase("", "somepassword", "")]
        [TestCase("someusername", "", "")]
        [TestCase("someusername", "", "someemail")]
        [TestCase("", "", "someemail")]
        [TestCase("someusername", "somepassword", "someemail")]
        [TestCase("someusername", "somepassword", "")]
        [TestCase("", "somepassword", "someemail")]
        public void IsInvalidCreate_User(string userName, string email, string password)
        {
            Registration reg = new Registration();
            reg.Dispatcher.Invoke(() =>
            {
                var ex = Assert.Catch<Exception>(() => reg.CreateUser(userName, email, password));
                StringAssert.Contains("Error", ex.Message);
            });

        }
    }
}
