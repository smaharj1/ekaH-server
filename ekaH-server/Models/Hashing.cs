using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BCrypt.Net;

namespace ekaH_server.Models
{
    /// <summary>
    /// This class handles the hashing for passwords.
    /// </summary>
    public class Hashing
    {
        /// <summary>
        /// This function generates a random salt.
        /// </summary>
        /// <returns>Returns the random salt.</returns>
        private static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }

        /// <summary>
        /// This function hashes the regular password using Bcrypt.
        /// </summary>
        /// <param name="a_password">It holds the original password that needs to be hashed.</param>
        /// <returns>Returns the hashed passwords.</returns>
        public static string HashPassword(string a_password)
        {
            return BCrypt.Net.BCrypt.HashPassword(a_password, GetRandomSalt());
        }

        /// <summary>
        /// This function validates the password.
        /// </summary>
        /// <param name="a_password">It holds the password.</param>
        /// <param name="a_correctHash">It holds the correct hash.</param>
        /// <returns></returns>
        public static bool ValidatePassword(string a_password, string a_correctHash)
        {
            return BCrypt.Net.BCrypt.Verify(a_password, a_correctHash);
        }
    }
}