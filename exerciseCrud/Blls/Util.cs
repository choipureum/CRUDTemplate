using exerciseCrud.Dals;
using exerciseCrud.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace exerciseCrud.Blls
{
    public class Util
    {

        public static bool IsNumeric(string s)
        {
            Int64 number = 0;
            return Int64.TryParse(s, out number);
        }
        /// <summary>
        /// 파일 변경 && 제거시 서버 파일도 제거
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public static bool DeleteFile(int boardId)
        {
            string filePath = new BoardDals().RetrieveBoardInfo(boardId).filePath;

            if (File.Exists(@filePath))
            {
                try
                {
                    File.Delete(@filePath);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            return true;
        }







        /*
        public class Pager
        {

            public Pager(int totalItems, int? page, int pageSize = 10)
            {
                // 페이징
                var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
                var currentPage = page != null ? (int)page : 1;
                var startPage = currentPage - 5;
                var endPage = currentPage + 4;
                if (startPage <= 0)
                {
                    endPage -= (startPage - 1);
                    startPage = 1;
                }
                if (endPage > totalPages)
                {
                    endPage = totalPages;
                    if (endPage > 10)
                    {
                        startPage = endPage - 9;
                    }
                }

                CurrentPage = currentPage;
                PageSize = pageSize;
                TotalPages = totalPages;
                StartPage = startPage;
                EndPage = endPage;

            }


            public int CurrentPage { get; private set; }
            public int PageSize { get; private set; }
            public int TotalPages { get; private set; }
            public int StartPage { get; private set; }
            public int EndPage { get; private set; }
            public List<int> Paging{ get; private set; }

        }
        */
    }
}