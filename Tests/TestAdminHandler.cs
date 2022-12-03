using NUnit.Framework;
using Omnicorp.Exceptions;
using Omnicorp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omnicorp.Admin;

namespace Tests
{
    [TestFixture]
    internal class TestAdminHandler
    {
        private AdminHandler handler = new AdminHandler(false);


        [Test]
        public void ValidatePositiveAmountNegativeFar()
        {
            decimal amount = -1;
            Assert.Throws<ArgumentException>(
                () => handler.ValidatePositiveAmount(amount)
            );
        }

        [Test]
        public void ValidatePositiveAmountNegativeBoundary()
        {
            decimal amount = decimal.Parse("-0.0000000000001");
            Assert.Throws<ArgumentException>(
                () => handler.ValidatePositiveAmount(amount)
            );
        }

        [Test]
        public void ValidatePositiveAmountPositiveFar()
        {
            decimal amount = 1;
            Assert.DoesNotThrow(
                () => handler.ValidatePositiveAmount(amount)
            );
        }

        [Test]
        public void ValidatePositiveAmountPositiveBoundary()
        {
            decimal amount = decimal.Parse("0.0000000000001");
            Assert.DoesNotThrow(
                () => handler.ValidatePositiveAmount(amount)
            );
        }
    }
}
