using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace React610SignalRUserTasks.data
{
    public class ChoreRepository
    {
        private string _conn;

        public ChoreRepository(string conn)
        {
            _conn = conn;
        }

        public List<Chore> GetTasks ()
        {
            using (var context = new ChoreContext(_conn))
            {
                return context.Tasks.Where(t=> t.isClosed == false).Include(t=> t.User).ToList();
            }  
        }
        
        public void AddTask (Chore task)
        {
            using (var context = new ChoreContext(_conn))
            {
                context.Tasks.Add(task);
                context.SaveChanges();
            }
        }

        //public void UpdateTask(Chore task)
        //{
        //    using (var context = new ChoreContext(_conn))
        //    {
        //        context.Tasks.Update(task);
        //        context.SaveChanges();
        //    }
        //}

        public void AssignTask(int taskId,string userName)
        {
            using (var context = new ChoreContext(_conn))
            {
                var task = context.Tasks.FirstOrDefault(t => t.Id == taskId);
                var user = GetUserByEmail(userName);
                    
                    if (user != null)
                    {
                        task.UserId = user.Id;
                        task.isAssigned = true;
                    }                                             
                context.SaveChanges();
            }
        }

        public void UnassignTask(int taskId)
        {
            using (var context = new ChoreContext(_conn))
            {
                var task = context.Tasks.FirstOrDefault(t => t.Id == taskId);
                task.UserId = null;
                task.isAssigned = false;
                context.SaveChanges();
            }
        }

        public void CloseTask(int taskId)
        {
            using (var context = new ChoreContext(_conn))
            {
                var task = context.Tasks.FirstOrDefault(t => t.Id == taskId);
                task.isClosed = true;
                context.SaveChanges();
            }
        }


        public User GetUserByEmail(string email)
        {
            using (var context = new ChoreContext(_conn))
            {
                return context.Users.FirstOrDefault(u => u.Email == email);
            }
        }
    }
}
