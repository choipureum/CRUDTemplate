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

        /// <summary>
        /// 리스트 호출 API
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Exercise/List/{page?}")]
        public BoardList List(int page = 1)
        {
            int size = 10;
            BoardList list = _BoardBiz.RetrieveBoardList(page, size);
            return list;
        }



        /// <summary>
        /// 작성 비동기API
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Exercise/WriteUpload")]
        public async Task<HttpResponseMessage> WriteUpload()
        {
            // 요청이 multipart/form-data를 담고 있는지 확인
            if (!Request.Content.IsMimeMultipartContent())
            {
                //아니라면 에러
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string fullPath = ConfigurationManager.AppSettings["FilePath"].ToString();
            var provider = new CustomMultipartFormDataStreamProvider(fullPath); 
            //- > 파일데이터 생성 - (MultipartFormDataStreamProvider 오버라이드 후 커스텀적용)

            try
            {
                //비동기 적용
                await Request.Content.ReadAsMultipartAsync(provider);

                //폼데이터
                BoardInfo info = new BoardInfo();
                info.userId = provider.FormData.GetValues("userId").SingleOrDefault();
                info.boardTitle = provider.FormData.GetValues("boardTItle").SingleOrDefault();
                info.boardContent = provider.FormData.GetValues("boardContent").SingleOrDefault();
                List<Boardfile>list = new List<Boardfile>();
                
                //다중파일데이터 
                for(int i = 0; i < provider.FileData.Count; i++)
                {
                    Boardfile file = new Boardfile
                    {
                         fileName = provider.FileData[i].Headers.ContentDisposition.FileName.Replace("\"", string.Empty).ToString()
                        ,filePath = provider.FileData[i].LocalFileName
                        ,fileGuid = provider.FileData[i].LocalFileName.Substring(provider.FileData[i].LocalFileName.IndexOf("Upload") + 7)
                        
                    };
                    list.Add(file);
                }
                info.BoardFileList = list;

                //DB INSERT
                if (_BoardBiz.RegisterBoardInfo(info))
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                return Request.CreateResponse(HttpStatusCode.NotModified);

            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }

        }

        /// <summary>
        /// 수정 비동기 API
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Exercise/UpdateUpload")]
        public async Task<HttpResponseMessage> UpdateUpload()
        {
            BoardInfo info = new BoardInfo();

            // 요청이 multipart/form-data를 담고 있는지 확인
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            //업로드 경로생성
            string fullPath = ConfigurationManager.AppSettings["FilePath"].ToString();
            var provider = new CustomMultipartFormDataStreamProvider(fullPath);

            List<Boardfile> list = new List<Boardfile>();
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                //정보입력
                info.boardId = Int32.Parse(provider.FormData.GetValues("boardId").SingleOrDefault());
                info.userId = provider.FormData.GetValues("userId").SingleOrDefault();
                info.boardTitle = provider.FormData.GetValues("boardTitle").SingleOrDefault();
                info.boardContent = provider.FormData.GetValues("boardContent").SingleOrDefault();
                
                //먼저 신규파일있을시 파일 info담기
                for (int i = 0; i < provider.FileData.Count; i++)
                {
                    Boardfile file = new Boardfile
                    {
                        fileName = provider.FileData[i].Headers.ContentDisposition.FileName.Replace("\"", string.Empty).ToString(),
                        filePath = provider.FileData[i].LocalFileName,
                        fileGuid = provider.FileData[i].LocalFileName.Substring(provider.FileData[i].LocalFileName.IndexOf("Upload") + 7)
                    };
                    list.Add(file);
                }
                info.BoardFileList = list;
            }         
            catch (Exception) { }

            // 기존 파일처리(삭제 데이터)
            //cnt = 기존 파일데이터 삭제 개수
            int cnt=Int32.Parse(provider.FormData.GetValues("dFileCnt").SingleOrDefault());

            //삭제 파일있을시
            if (cnt > 0)
            {
                //FileId를 기준으로 삭제
                for(int i = 0; i < cnt; i++)
                {
                    int dFile = Int32.Parse(provider.FormData.GetValues("dFile"+(i+1)).SingleOrDefault());
                    _BoardBiz.DeleteBoardFile(dFile); // DB,서버 동시삭제
                }
            } //파일처리 END
            

            //업데이트 처리
            //정보 업데이트(신규파일 등록 && 정보 업데이트)
            if ( (_BoardBiz.UpdateBoardInfo(info)) && (_BoardBiz.RegisterBoardFile(list, info.boardId)))
           {

                return Request.CreateResponse(HttpStatusCode.OK);
                
            }
            return Request.CreateResponse(HttpStatusCode.NotModified);           
           
        }
        
        /// <summary>
        /// Detail View 조회 API
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Exercise/Detail/{boardId?}")]
        public BoardInfo Detail(int boardId = 0)
        {
            return _BoardBiz.RetrieveBoardInfo(boardId);
        }

        /// <summary>
        /// 삭제처리 API
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Exercise/Delete/{boardId?}")]
        public string Delete(int boardId)
        {
            //삭제, 파일DB삭제, 파일서버삭제
            if (_BoardBiz.DeleteBoardInfo(boardId))
            {
                return "삭제성공";
            }
            return "삭제실패";
        }

        /// <summary>
        /// 수정페이지 뷰  API
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Exercise/UpdateView/{boardId?}")]
        public BoardInfo UpdateView(int boardId)
        {
            return _BoardBiz.RetrieveBoardInfo(boardId);
        }

        
    }
    /// <summary>
    /// MultipartFormDataStreamProvider 커스텀 
    ///     GetLocalFileName : 저장 파일명 변경
    /// </summary>
    //파일저장때 파일명은 내맘대로 바꾸기 -GUID형식해보기
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path) : base(path) { }
        
        
        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(headers.ContentDisposition.FileName.Replace("\"", string.Empty));
            return fileName;
        }
        
     
    }



}

//버려진 단순 코드
/*
[HttpPost]
[Route("Exercise/Update")]
public string Update(BoardInfo info)
{
    if (_BoardBiz.UpdateBoardInfo(info))
    {
        return "업데이트 성공";
    }
    return "업데이트 실패";
}
*/
