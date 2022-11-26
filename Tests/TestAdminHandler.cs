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
            double amount = -1;
            Assert.Throws<ArgumentException>(
                () => handler.ValidateRateAmount(amount)
            );
        }

        [Test]
        public void ValidateRateAmountNegativeBoundary()
        {
            double amount = -0.0000000000001;
            Assert.Throws<ArgumentException>(
                () => handler.ValidateRateAmount(amount)
            );
        }

        [Test]
        public void ValidateRateAmountPositiveFar()
        {
            double amount = 1;
            Assert.DoesNotThrow(
                () => handler.ValidateRateAmount(amount)
            );
        }

        [Test]
        public void ValidateRateAmountPositiveBoundary()
        {
            double amount = 0.0000000000001;
            Assert.DoesNotThrow(
                () => handler.ValidateRateAmount(amount)
            );
        }
    }
}
