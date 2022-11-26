using NUnit.Framework;
using Omnicorp;
using Omnicorp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    internal class TestLoginHandler
    {
        private LoginHandler handler = new LoginHandler();

        [Test]
        public void VerifyUserPasswordWithWrongPassword()
        {
            Assert.Throws<InvalidUserException>(
                () => handler.VerifyPassword("1234", "abcd")
            );
        }

        [Test]
        public void VerifyUserPasswordSuccess()
        {
            Assert.DoesNotThrow(() => handler.VerifyPassword("1234", "1234"));
        }
    }
}
