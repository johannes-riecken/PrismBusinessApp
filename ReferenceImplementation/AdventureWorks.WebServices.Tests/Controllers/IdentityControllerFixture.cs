

using System;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AdventureWorks.WebServices.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http;
using System.Net;

namespace AdventureWorks.WebServices.Tests.Controllers
{
    [TestClass]
    public class IdentityControllerFixture
    {
        #region Simulation of Client Side Processing

        private static byte[] DecodeFromHexString(string hex)
        {
            var raw = new byte[hex.Length / 2];
            for (var i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        private static string EncodeToHexString(byte[] hexBytes)
        {
            var sb = new StringBuilder(hexBytes.Length * 2);
            foreach (var b in hexBytes)
            {
                sb.Append(b.ToString("x2", CultureInfo.InvariantCulture));
            }
            return sb.ToString();
        }

        private static string CreatePasswordHash(string password, string challengeString)
        {
            var passwordBuffer = Encoding.UTF8.GetBytes(password);

            var provider = new HMACSHA512(passwordBuffer);

            var challengeBuffer = DecodeFromHexString(challengeString);

            var hmacBytes = provider.ComputeHash(challengeBuffer);

            return EncodeToHexString(hmacBytes);
        }
        #endregion

        [TestMethod]
        public void ValidateUserNameValidPassword()
        {
            var controller = new IdentityController();

            const string requestId = "ec609a4f";
            var challengeString = controller.GetPasswordChallenge(requestId);
            Assert.IsFalse(string.IsNullOrEmpty(challengeString));

            var result = controller.GetIsValid("JohnDoe", requestId, CreatePasswordHash("pwd", challengeString));

            Assert.IsNotNull(result);
            Assert.AreEqual(result.UserName, "JohnDoe");
        }

        [TestMethod]
        public void ValidateUserNameInvalidPassword()
        {
            var sawException = false;
            var controller = new IdentityController();

            const string requestId = "ec609a4f";
            var challengeString = controller.GetPasswordChallenge(requestId);
            Assert.IsFalse(string.IsNullOrEmpty(challengeString));

            try
            {
                var result = controller.GetIsValid("JohnDoe", requestId, CreatePasswordHash("InvalidPassword", challengeString));
            }
            catch (HttpResponseException ex)
            {
                Assert.AreEqual(HttpStatusCode.Unauthorized, ex.Response.StatusCode);
                sawException = true;
            }

            Assert.IsTrue(sawException);
        }

        [TestMethod]
        public void ValidateUserNameInvalidUser()
        {
            var sawException = false;
            var controller = new IdentityController();

            const string requestId = "ec609a4f";
            var challengeString = controller.GetPasswordChallenge(requestId);
            Assert.IsFalse(string.IsNullOrEmpty(challengeString));

            try
            {
                var result = controller.GetIsValid("UnknownUser", requestId, CreatePasswordHash("pwd", challengeString));

            }
            catch (HttpResponseException ex)
            {
                Assert.AreEqual(HttpStatusCode.Unauthorized, ex.Response.StatusCode);
                sawException = true;
            }
            Assert.IsTrue(sawException);
        }
    }
}
