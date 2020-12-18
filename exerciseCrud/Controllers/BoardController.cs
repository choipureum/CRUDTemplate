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
        
        
        
        
        
        [HttpPost]
        [ValidateInput(false)]
        public string WriteUpload()
        {
            BoardInfo info = new BoardInfo();
            bool isSavedSuccessfully = true;
            int fileCnt = 0;
            fileCnt = Request.Files.Count;
            try
            {
                if (fileCnt > 0)
                {
                    var file = Request.Files[1];
                    string FileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string FileExtension = Path.GetExtension(file.FileName);
                    FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
                    string UploadPath = ConfigurationManager.AppSettings["FilePath"].ToString();
                    info.filePath = UploadPath + FileName;
                    file.SaveAs(info.filePath);
                }
            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }
           
            info.boardTitle = Request.Form["boardTitle"];           
            info.userId = Request.Form["userId"];
            info.boardContent = Request.Form["boardContent"];


            if (isSavedSuccessfully && new BoardBiz().RegisterBoardInfo(info))
            {
                return "성공!";
            }
            else
            {
                return "실패!";
            }

        }
       


        [HttpPost]
        [ValidateInput(false)]
        public string Update()
        {
           
            BoardInfo info = new BoardInfo();
            int fileCnt = 0;
            fileCnt = Request.Files.Count;
            string filePath = Request.Form["filePath"];
            try
            {
                info.boardTitle = Request.Form["boardTitle"];
                info.userId = Request.Form["userId"];
                info.boardContent = Request.Form["boardContent"];
                info.boardId = Int32.Parse( Request.Form["boardId"]);

                //파일존재
                if (fileCnt > 1 && !(filePath=="" || filePath==null ))
                {
                    var file = Request.Files[1];
                    string FileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string FileExtension = Path.GetExtension(file.FileName);
                    FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
                    string UploadPath = ConfigurationManager.AppSettings["FilePath"].ToString();
                    info.filePath = UploadPath + FileName;
                    file.SaveAs(info.filePath);
                }
                //파일없을때
                //파일고대로
                else if( !(filePath == "" || filePath == null))
                {
                    info.filePath = ConfigurationManager.AppSettings["FilePath"].ToString()+filePath;
                }
                //파일 제거만했을때
                else 
                {
                    if (Util.DeleteFile(info.boardId))
                    {
                        info.filePath = "";
                    }
                }
            }

            catch (Exception){}

            if (new BoardBiz().UpdateBoardInfo(info))
            {
               
                info=new BoardBiz().RetrieveBoardInfo(info.boardId);
                return "/Board/Detail?boardId=" + info.boardId;
            }
            return "/Board/List";
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