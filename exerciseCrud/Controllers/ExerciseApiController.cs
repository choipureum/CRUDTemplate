using exerciseCrud.Blls;
using exerciseCrud.Dals;
using exerciseCrud.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace exerciseCrud.Controllers
{
    public class ExerciseApiController : ApiController
    {
        private readonly BoardBiz _BoardBiz;
        public ExerciseApiController()
        {
            _BoardBiz = new BoardBiz();
        }

        [HttpGet]
        [Route("Exercise/List/{page?}")]
        public BoardList List(int page = 1)
        {
            int size = 10;
            BoardList list = _BoardBiz.RetrieveBoardList(page, size);                
            return list;
        }
        /*
        [HttpPost]
        [Route("Exercise/Write")]
        public string Write(BoardInfo info)
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
                return "글작성 성공";
            }
            return "글작성 실패";
        }
        */
        //비동기식으로 api

        



        [HttpGet]
        [Route("Exercise/Detail/{boardId?}")]
        public BoardInfo Detail(int boardId = 0)
        {
            return new BoardBiz().RetrieveBoardInfo(boardId);
        }

        [HttpGet]
        [Route("Exercise/Delete/{boardId?}")]
        public string Delete(int boardId)
        {
            if (new BoardBiz().DeleteBoardInfo(boardId))
            {
               return "삭제성공";
            }
            return "삭제실패";
        }
        [HttpGet]
        [Route("Exercise/UpdateView/{boardId?}")]
        public BoardInfo UpdateView(int boardId)
        {           
            return new BoardBiz().RetrieveBoardInfo(boardId);
        }
     
        [HttpPost]
        [Route("Exercise/Update")]
        public string Update(BoardInfo info)
        {
            if (new BoardBiz().UpdateBoardInfo(info))
            {
                return "업데이트 성공";
            }
            return "업데이트 실패";
        }




    }
}
