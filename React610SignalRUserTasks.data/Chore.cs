using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace React610SignalRUserTasks.data
{
    public class Chore
    {
        public int Id { get; set; }
        public String Description { get; set; }
        public bool isClosed { get; set; }
        public bool isAssigned { get; set; } // UserId ==null ? false : true;
        public int? UserId { get; set; }

        [NotMapped]
        public String UserName => UserId != null ? User.Name : "";

       /* [NotMapped]
        public String UserEmail => UserId != null ? User.Email : "";*/

        [JsonIgnore]
        public User User { get; set; }
    }
}
