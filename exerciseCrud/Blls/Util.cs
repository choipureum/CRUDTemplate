using exerciseCrud.Dals;
using exerciseCrud.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        /// <summary>
        /// 크로스사이트 스크립트 체크
        /// </summary>
        /// <param name="pStr"></param>
        /// <returns></returns>
        public static string ReplaceXss(string pStr)
        {
            if (string.IsNullOrEmpty(pStr)) { return pStr; }
            string ls_conv = pStr;
            string ls_regExp1 = "<\\s*\\/??\\s*[sS][cC][rR][iI][pP][tT].*?>";
            string ls_regExp2 = "<\\s*\\/??\\s*[iI][fF][rR][aA][mM][eE].*?>";
            string ls_regExp3 = "<\\s*\\/??\\s*[fF][rR][aA][mM][eE].*?>";
            string ls_regExp4 = "<\\s*\\/??\\s*[mM][eE][tT][aA].*?>";
            string ls_regExp5 = "<\\s*\\/??\\s*[bB][aA][sS][eE].*?>";
            string ls_regExp6 = "<\\s*\\/??\\s*[lL][iI][nN][kK].*?>";
            string ls_regExp7 = "<\\s*\\/??\\s*[xX][sS][sS].*?>";
            string ls_regExp8 = "[dD][oO][cC][uU][mM][eE][nN][tT].[cC][oO][oO][kK][iI][eE]";
            string ls_regExp8_1 = "&#[xX]64;&#[xX]6[fF];&#[xX]63;&#[xX]75;&#[xX]6[dD];&#[xX]65;&#[xX]6[eE];&#[xX]74;&#[xX]2[eE];&#[xX]63;&#[xX]6[fF];&#[xX]6[fF];&#[xX]6[bB];&#[xX]69;&#[xX]65;";
            string ls_regExp8_2 = "&#100&#111&#99&#117&#109&#101&#110&#116&#46&#99&#111&#111&#107&#105&#101";

            ls_conv = Regex.Replace(ls_conv, ls_regExp1, "");
            ls_conv = Regex.Replace(ls_conv, ls_regExp2, "");
            ls_conv = Regex.Replace(ls_conv, ls_regExp3, "");
            ls_conv = Regex.Replace(ls_conv, ls_regExp4, "");
            ls_conv = Regex.Replace(ls_conv, ls_regExp5, "");
            ls_conv = Regex.Replace(ls_conv, ls_regExp6, "");
            ls_conv = Regex.Replace(ls_conv, ls_regExp7, "");
            ls_conv = Regex.Replace(ls_conv, ls_regExp8, "");
            ls_conv = Regex.Replace(ls_conv, ls_regExp8_1, "");
            ls_conv = Regex.Replace(ls_conv, ls_regExp8_2, "");

            ls_conv = RemoveXssAttributes(ls_conv);
            return ls_conv;// ls_conv.Replace("<", "&lt;").Replace(">", "&gt;");

        }

        /// <summary>
        /// Xss 제거
        /// </summary>
        public static string RemoveXssAttributes(string pStr)
        {
            string ls_conv = pStr;
            // removing unwanted tags     
            ls_conv = Regex.Replace(ls_conv, @"<[/]?(form|xml|del|ins|[ovwxp]:\w+)[^>]*?>", "", RegexOptions.IgnoreCase);
            // removing unwanted attributes     
            ls_conv = Regex.Replace(ls_conv, @"<([^>]*)(?:onmouseover|onblur|onfocus|onload|onselect|onsubmit|onunload|[ovwxp]:\w+)=(?:'[^']*'|""[^""]*""|[^\s>]+)([^>]*)>", "<$1$2>", RegexOptions.IgnoreCase);
            ls_conv = Regex.Replace(ls_conv, @"<([^>]*)(?:onabort|onerror|onmouseout|onreset|onclick|ondbclick|ondragdrop|onkeydown|[ovwxp]:\w+)=(?:'[^']*'|""[^""]*""|[^\s>]+)([^>]*)>", "<$1$2>", RegexOptions.IgnoreCase);
            ls_conv = Regex.Replace(ls_conv, @"<([^>]*)(?:onkeypress|onkeyup|onmousedown|onmousemove|onmouseup|onmove|onresize|[ovwxp]:\w+)=(?:'[^']*'|""[^""]*""|[^\s>]+)([^>]*)>", "<$1$2>", RegexOptions.IgnoreCase);

            return ls_conv;
        }
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
