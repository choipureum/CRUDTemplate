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
        public string userId{ get; set; }
        public int recommend { get; set; }
        public string filePath { get; set; }
        public HttpPostedFileBase pUpload { get; set; }

    }
    public class BoardComment
    {
        public int commentId { get; set; }
        public int boardId { get; set; }
        public string userId { get; set; }
        public string content { get; set; }
        public DateTime regDate { get; set; }
    }
    public class UploadFilesResult
    {
        public string NewName { get; set; }
        public string OrgName { get; set; }
        public bool Success { get; set; }
    }
    public class BoardList
    {   
        public int TotalCnt { get; set; }
        public List<BoardInfo> list { get; set; }     
        
    }
}