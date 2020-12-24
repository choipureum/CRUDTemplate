using exerciseCrud.Blls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace exerciseCrud.Models
{
    public class BoardInfo
    {
        public int boardId { get; set; }
        public string boardTitle { get; set; }
        [AllowHtml]
        public string boardContent { get; set; }
        public DateTime regDate { get; set; }
        public int viewCount { get; set; }
        public string userId { get; set; }
        public List <Boardfile> BoardFileList {get;set;}     

    }

    public class Boardfile
    {
        public int fileId { get; set; }
        public string filePath { get; set; }
        public string fileName { get; set; }
        public string fileGuid { get; set; }
        public int boardId { get; set; }

    }



    public class BoardList
    {   
        public int TotalCnt { get; set; }
        public List<BoardInfo> list { get; set; }     
        
    }
}