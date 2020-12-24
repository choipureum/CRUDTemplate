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

        /// <summary>
        /// 파일 DB삭제시 서버에 있는 파일도 제거
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public static bool DeleteFile(int fileId)
        {

            Boardfile file = new BoardDals().RetrieveBoardFileInfo(fileId);

            string filePath = file.filePath;

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


    }
}
