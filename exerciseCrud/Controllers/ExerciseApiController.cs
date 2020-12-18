using exerciseCrud.Blls;
using exerciseCrud.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Results;


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
        //비동기식으로 api
        
        [HttpPost]
        [Route("Exercise/WriteUpload")]
        public async Task<HttpResponseMessage> WriteUpload()
        {
            // 요청이 multipart/form-data를 담고 있는지 확인합니다.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }


            string fullPath = ConfigurationManager.AppSettings["FilePath"].ToString();
            var provider = new CustomMultipartFormDataStreamProvider(fullPath);

                try
                {
                    await Request.Content.ReadAsMultipartAsync(provider);
                    
                     BoardInfo info = new BoardInfo();
                     info.filePath = provider.FileData[1].LocalFileName;                                            
                     info.userId = provider.FormData.GetValues(provider.FormData.AllKeys[2]).SingleOrDefault();
                     info.boardTitle = provider.FormData.GetValues(provider.FormData.AllKeys[3]).SingleOrDefault();
                     info.boardContent = provider.FormData.GetValues(provider.FormData.AllKeys[4]).SingleOrDefault();

                    //삽입
                    if (new BoardBiz().RegisterBoardInfo(info))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                    return Request.CreateResponse(HttpStatusCode.NotModified);
                    
                }
                catch(System.Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                }

        }
        

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
    //파일저장때 파일명 내맘대로 - 오버라이드
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            string fileName = Guid.NewGuid().ToString() +headers.ContentDisposition.FileName.Replace("\"", string.Empty).ToString();
                            // + Path.GetExtension(headers.ContentDisposition.FileName.Replace("\"", string.Empty));
            return fileName;
        }
        
    }



}

