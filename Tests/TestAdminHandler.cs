/*
* FILE          :   TestAdminHandler.cs
* PROJECT       :   SENG2020 - Omnicorp project
* PROGRAMMERS   :   - Ali Anwar - 8765779
*                   - Bruno Borges Russian - 8717542
*                   - Dhruvkumar Patel - 8777164
*                   - Thalys Baiao Lopes - 8760875
* FIRST VERSION :   Nov, 19, 2022
* DESCRIPTION   :   The file is used to  declare the TestAdminHandler Class
*/

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
    /*
    * CLASS NAME	:   TestAdminHandler
    * DESCRIPTION	:   The purpose of this class is to test the functionality of the admin class 
    *
    */
    internal class TestAdminHandler
    {
        private AdminHandler handler = new AdminHandler();


        [Test]
        /*
        * METHOD		:  ValidatePositiveAmountNegativeFar
        * DESCRIPTION	:   try to validate rates amount if negative 
        * PARAMETERS    : None
        *                  
        * RETURNS       :
        *                   None
        */
        public void ValidatePositiveAmountNegativeFar()
        {
            decimal amount = -1;
            Assert.Throws<ArgumentException>(
                () => handler.ValidatePositiveAmount(amount)
            );
        }

        [Test]
        /*
        * METHOD		:  ValidatePositiveAmountNegativeBoundary
        * DESCRIPTION	:   try to validate rates amount with boundary
        * PARAMETERS    : None
        *                  
        * RETURNS       :
        *                   None
        */
        public void ValidatePositiveAmountNegativeBoundary()
        {
            decimal amount = decimal.Parse("-0.0000000000001");
            Assert.Throws<ArgumentException>(
                () => handler.ValidatePositiveAmount(amount)
            );
        }

        [Test]
        /*
       * METHOD		:  ValidatePositiveAmountPositiveFar
       * DESCRIPTION	:   try to validate rates amount if positive
       * PARAMETERS    : None
       *                  
       * RETURNS       :
       *                   None
       */
        public void ValidatePositiveAmountPositiveFar()
        {
            decimal amount = 1;
            Assert.DoesNotThrow(
                () => handler.ValidatePositiveAmount(amount)
            );
        }

        [Test]

        /*
       * METHOD		:  ValidatePositiveAmountPositiveBoundary
       * DESCRIPTION	:   try to validate rates amount with positive boundary
       * PARAMETERS    : None
       *                  
       * RETURNS       :
       *                   None
       */
        public void ValidatePositiveAmountPositiveBoundary()
        {
            decimal amount = decimal.Parse("0.0000000000001");
            Assert.DoesNotThrow(
                () => handler.ValidatePositiveAmount(amount)
            );
        }
    }
}
