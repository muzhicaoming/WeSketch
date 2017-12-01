using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using WeSketchSharedDataModels;

namespace WeSketchAPI
{
    public class WeSketchSecurity
    {
        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public User CreateUser(string user, string password, string email)
        {
            using (var db = new WeSketchDataContext())
            {
                if(db.Users.Any(existingUser => existingUser.UserName == user))
                {
                    throw new Exception($"User {user} already exists.");
                }
                
                byte[] salt = GetSalt();
                var userId = Guid.NewGuid();
                var boardId = Guid.NewGuid();

                var newUser = new User()
                {
                    UserID = userId,
                    UserName = user,
                    Email = email,
                    Password = GetComputedHash(password, salt),
                    SeaSalt = Convert.ToBase64String(salt),
                    Disabled = false,
                    UserBoard = new UserBoard()
                    {
                        UserID = userId,
                        BoardID = boardId,
                        BoardOwner = true,
                        CanSketch = true
                    }
                };
                db.Users.InsertOnSubmit(newUser);
                db.SubmitChanges();
                return db.Users.Single(usr => usr.UserName == user);
            }
        }

        /// <summary>
        /// Logins the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public User Login(string user, string password)
        {
            string errorMessage = "Invalid credentials";
            using (var db = new WeSketchDataContext())
            {
                db.DeferredLoadingEnabled = false;
                var existingUser = db.Users.SingleOrDefault(eUser => eUser.UserName == user);
                
                if (existingUser == null)
                {
                    db.SubmitChanges();
                    throw new Exception(errorMessage);
                }
                existingUser.LastLoginAttempt = DateTime.Now;

                HashAlgorithm algorithm = new SHA256Managed();
                if (password == string.Empty)
                {
                    throw new Exception(errorMessage);
                }
                byte[] attemptBytes = GetSeasonedPasswordBytes(password, 
                    Convert.FromBase64String(existingUser.SeaSalt), 
                    Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["pepper"]));
                byte[] attemptHash = algorithm.ComputeHash(attemptBytes);
                byte[] existingHash = Convert.FromBase64String(existingUser.Password);
                
                if(!attemptHash.SequenceEqual(existingHash))
                {
                    db.SubmitChanges();
                    throw new Exception(errorMessage);
                }

                existingUser.LastLogin = DateTime.Now;
                db.SubmitChanges();
                existingUser.UserBoard = db.UserBoards.Single(brd => brd.UserID == existingUser.UserID);
                return existingUser;
            }
        }

        /// <summary>
        /// Gets the computed hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        private string GetComputedHash(string password, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] pepper = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["pepper"]);
            byte[] saltyPwBytes = GetSeasonedPasswordBytes(password, salt, pepper);
            byte[] computedHash = algorithm.ComputeHash(saltyPwBytes);
            return Convert.ToBase64String(computedHash);
        }

        /// <summary>
        /// Gets the seasoned password bytes.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="pepper">The pepper.</param>
        /// <returns></returns>
        private byte[] GetSeasonedPasswordBytes(string password, byte[] salt, byte[] pepper)
        {
            byte[] pwBytes = Encoding.UTF8.GetBytes(password);
            byte[] seasonedPwBytes = new byte[salt.Length + pwBytes.Length + pepper.Length];
            pwBytes.CopyTo(seasonedPwBytes, 0);
            salt.CopyTo(seasonedPwBytes, pwBytes.Length - 1);
            pepper.CopyTo(seasonedPwBytes, pwBytes.Length + salt.Length - 1);
            return seasonedPwBytes;
        }

        /// <summary>
        /// Gets the salt.
        /// </summary>
        /// <returns>A byte array populated with random values.</returns>
        private byte[] GetSalt()
        {
            using (var hasher = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[32];
                hasher.GetNonZeroBytes(salt);
                return salt;
            }
        }
    }
}