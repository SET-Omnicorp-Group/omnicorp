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
        private AdminHandler handler = new AdminHandler();


        [Test]
        public void ValidateRateAmountNegativeFar()
        {
            decimal amount = -1;
            Assert.Throws<ArgumentException>(
                () => handler.ValidatePositiveAmount(amount)
            );
        }

        [Test]
        public void ValidateRateAmountNegativeBoundary()
        {
            decimal amount = decimal.Parse("-0.0000000000001");
            Assert.Throws<ArgumentException>(
                () => handler.ValidatePositiveAmount(amount)
            );
        }

        [Test]
        public void ValidateRateAmountPositiveFar()
        {
            decimal amount = 1;
            Assert.DoesNotThrow(
                () => handler.ValidatePositiveAmount(amount)
            );
        }

        [Test]
        public void ValidateRateAmountPositiveBoundary()
        {
            decimal amount = decimal.Parse("0.0000000000001");
            Assert.DoesNotThrow(
                () => handler.ValidatePositiveAmount(amount)
            );
        }
    }
}
