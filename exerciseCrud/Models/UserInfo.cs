using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exerciseCrud.Models
{
    public class UserInfo
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public DateTime regDate { get; set; }
    }
    public class UserList
    {
        public int TotalCnt { get; set; }

        public List<UserInfo> List { get; set; }
    }
}