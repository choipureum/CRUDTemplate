using exerciseCrud.Blls;
using exerciseCrud.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace exerciseCrud.Controllers
{
    public class BoardController : Controller
    {
        
        public ActionResult List(int page=1)
        {
            //받을 객체생성
            BoardList list= new BoardList();
            //객체 JSON받아오기
            try
            {                           
                HttpWebRequest objWRequest = (HttpWebRequest)System.Net.WebRequest.Create("http://exerciseCrud.com/Exercise/List/" + page);
                HttpWebResponse objWResponse = (HttpWebResponse)objWRequest.GetResponse();
                Stream stream = objWResponse.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string result = reader.ReadToEnd();
                stream.Close();
                objWResponse.Close();
                //역직렬화
                list = JsonConvert.DeserializeObject<BoardList>(result);               
            }
            catch(Exception) {}
            ViewBag.page = page;        
            return View(list);
        }

        [HttpGet]
        public ActionResult Write()
        {
            return View();
        }

        
        [HttpGet]
        public ActionResult Detail(int boardId=0)
        {
            ViewBag.boardId = boardId;
            return View();       
        }
        [HttpGet]
        public ActionResult UpdateView(int boardId)
        {
            ViewBag.boardId = boardId;
            return View();
        }
        /*

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Write(BoardInfo info)
        {
                 
            if (info.pUpload != null)
            {
                string FileName = Path.GetFileNameWithoutExtension(info.pUpload.FileName);
                string FileExtension = Path.GetExtension(info.pUpload.FileName);
                FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
                string UploadPath = ConfigurationManager.AppSettings["FilePath"].ToString();
                info.filePath = UploadPath + FileName;
                info.pUpload.SaveAs(info.filePath);
            }

            if (new BoardBiz().RegisterBoardInfo(info))
            {
                ViewBag.message = "글작성 성공";
                return RedirectToAction("List", "Board");
            }
            return RedirectToAction("Write", "Board");
        }
        */
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult WriteUpload(BoardInfo info)
        {
           
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                HttpPostedFileBase file = Request.Files["files"];
                //Save file content goes here
                fName = file.FileName;


                if (file != null)
                {                   
                    string FileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string FileExtension = Path.GetExtension(file.FileName);
                    FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
                    string UploadPath = ConfigurationManager.AppSettings["FilePath"].ToString();
                    info.filePath= UploadPath + FileName;
                    info.pUpload.SaveAs(info.filePath);
                }
            }

            catch(Exception)
            {
                isSavedSuccessfully = false;
            }
            info.boardTitle = Request.Form["boardTitle"];
            info.boardContent = Request.Form["boardContent"];
            info.userId = Request.Form["userId"];

            
            if (isSavedSuccessfully && new BoardBiz().RegisterBoardInfo(info))
            {
                return Json(new { msg = "OK" });
            }
            else
            {
                return Json(new { msg = "Error in saving file" });
            }

        }
       



       

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(BoardInfo info)
        {
            string fileName=Request.Form["fileName"];

            //값이 존재할때(보통)
            if (info.pUpload != null && !(fileName =="" || fileName==null))
            {
                //기존 파일삭제 해야함
                if (Util.DeleteFile(info.boardId))
                {

                    string FileName = Path.GetFileNameWithoutExtension(info.pUpload.FileName);
                    string FileExtension = Path.GetExtension(info.pUpload.FileName);
                    FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
                    string UploadPath = ConfigurationManager.AppSettings["FilePath"].ToString();
                    info.filePath = UploadPath + FileName;
                    info.pUpload.SaveAs(info.filePath);

                }
            }

            //파일 고대로 보존
            else if(info.pUpload == null && !(fileName == "" || fileName == null))
            {
                //기존파일 삭제하지 않음
                info.filePath = fileName;
            }
            //파일 삭제
            else if (fileName == "" || fileName == null)
            {
                //기존 파일삭제 해야함
                if (Util.DeleteFile(info.boardId))
                {
                    info.filePath = "";
                }
  
            }

            if (new BoardBiz().UpdateBoardInfo(info))
            {
                ViewBag.Message = "업데이트성공";
                info=new BoardBiz().RetrieveBoardInfo(info.boardId);
                return RedirectToAction("List", "Board");
            }
            return RedirectToAction("List", "Board");
        }
        /*
           public ActionResult Delete(int boardId=0)
           {
               if(new BoardBiz().DeleteBoardInfo(boardId))
               {
                   ViewBag.Message = "삭제성공";
                   return View();
               }
               return RedirectToRoute("/Board/Detail?" + boardId);
           }





           

           [HttpGet]
           public int UpdateRecommendUp(int boardId)
           {
               int result = 0;
               if (new BoardBiz().UpdateRecommendUp(boardId))
               {             
                   result = new BoardBiz().RetrieveBoardInfo(boardId).recommend;
                   return result;
               }
               return result;
           }

           //댓글등록
           public ActionResult RegisterBoardComment(BoardComment info)
           {
               if(new BoardBiz().RegisterBoardComment(info))
               {
                   return Redirect("/Board/Detail?boardId=" + info.boardId);
               }
               return Redirect("/Board/List");
           }
      */


    }
}