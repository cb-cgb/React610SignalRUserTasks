using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using React610SignalRUserTasks.data;


namespace React610SignalRUserTasks.web
{
    public class TaskHub : Hub
    {


        private string _conn;

        public TaskHub(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("ConStr");
        }

        public void SendTasks()
        {
            var tasks = GetTasks();
            Clients.All.SendAsync("showtasks", tasks);
        }

        public List<Chore> GetTasks()
        {
            var db = new ChoreRepository(_conn);
            return db.GetTasks();
        }

        public void AddTask (Chore task)
        {
            var db = new ChoreRepository(_conn);
            db.AddTask(task);
            SendTasks();
        }

        public void AssignTask(Chore task)
        {
            var userName = Context.User.Identity.Name;
            var db = new ChoreRepository(_conn);
            db.AssignTask(task.Id, userName);
            SendTasks();
        }

        public void UnassignTask(Chore task)
        {
            var db = new ChoreRepository(_conn);
            db.UnassignTask(task.Id);
            SendTasks();
        }

        public void CloseTask(Chore task)
        {
            var db = new ChoreRepository(_conn);
            db.CloseTask(task.Id);
            SendTasks();
        }




    }
}
