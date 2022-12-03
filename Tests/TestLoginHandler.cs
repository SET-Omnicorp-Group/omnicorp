/*
* FILE          :   TestLoginHandler.cs
* PROJECT       :   SENG2020 - Omnicorp project
* PROGRAMMERS   :   - Ali Anwar - 8765779
*                   - Bruno Borges Russian - 8717542
*                   - Dhruvkumar Patel - 8777164
*                   - Thalys Baiao Lopes - 8760875
* FIRST VERSION :   Nov, 19, 2022
* DESCRIPTION   :   The file is used to  declare the TestLoginHandler Class
*/
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
    /*
    * CLASS NAME	:   TestLoginHandler
    * DESCRIPTION	:   The purpose of this class is to test the functionality of the Login class 
    *
    */
    internal class TestLoginHandler
    {
        private LoginHandler handler = new LoginHandler();

        [Test]
        /*
        * METHOD		:  VerifyUserPasswordWithWrongPassword
        * DESCRIPTION	:   try to validate the password with wrong password 
        * PARAMETERS    : None
        *                  
        * RETURNS       :
        *                   None
        */
        public void VerifyUserPasswordWithWrongPassword()
        {
            Assert.Throws<InvalidUserException>(
                () => handler.VerifyPassword("1234", "abcd")
            );
        }

        [Test]
     /*
     * METHOD		:  VerifyUserPasswordSuccess
     * DESCRIPTION	:   try to validate the password with correct password
     * PARAMETERS    : None
     *                  
     * RETURNS       :
     *                   None
     */
        public void VerifyUserPasswordSuccess()
        {
            Assert.DoesNotThrow(() => handler.VerifyPassword("1234", "1234"));
        }
    }
}
