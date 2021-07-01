using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace React610SignalRUserTasks.data
{
    public class UserRepository
    {

        private string _conn;

        public UserRepository(string connection)
        {
            _conn = connection;
        }


        public void AddUser(User u)
        {
            using (var context = new ChoreContext(_conn))
            {

                //add only if userName doesn't already exist
                var user = GetUserByEmail(u.Email);

                if (user is null)
                {
                    u.Password = BCrypt.Net.BCrypt.HashPassword(u.Password);//encrypts the password value passed in
                    context.Users.Add(u);
                    context.SaveChanges();
                }
            }
        }

        public User GetUserByEmail(string email)
        {
            using (var context = new ChoreContext(_conn))
            {
                return context.Users.FirstOrDefault(u => u.Email == email);
            }
        }

        public User Login(User u)
        {
            var user = GetUserByEmail(u.Email);

            if (user is null)
            {
                return null;
            }

            var isValidPass = BCrypt.Net.BCrypt.Verify(u.Password, user.Password);

            if (!isValidPass)
            {
                return null;
            }

            return user;
        }

    }
}
