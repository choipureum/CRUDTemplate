using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using exerciseCrud.Blls;
using exerciseCrud.Models;
using IMBC.FW.DB;

namespace exerciseCrud.Dals
{
    public class BoardDals 
    {
        /// <summary>
        /// 게시판 목록 조회
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public BoardList RetrieveBoardList(int page, int size)
        {
            BoardList boardList = new BoardList();
            SqlCommand sqlCmd = new SqlCommand
            {
                CommandText = "Exercise@RetrieveBoardList",
                CommandType = CommandType.StoredProcedure
            };
            sqlCmd.Parameters.Add("@page", SqlDbType.Int).Value = page;
            sqlCmd.Parameters.Add("@size", SqlDbType.Int).Value = size;

            SqlDataReader reader = SQLHelper.ExecuteReader(sqlCmd);

            try
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        boardList.TotalCnt = (int)reader["total_count"];
                    }

                    List<BoardInfo> list = new List<BoardInfo>();

                    if (reader.NextResult())
                    { 
                        while (reader.Read())
                        {
                            list.Add(new BoardInfo
                            {
                                boardId = (int)reader["boardId"],
                                boardTitle = reader["boardTitle"].ToString(),
                                boardContent = reader["boardContent"].ToString(),
                                regDate = (DateTime)reader["regDate"],                               
                                viewCount = (int)reader["viewCount"],
                                userId = reader["userId"].ToString()
                            });
                        }                      
                    }
                    boardList.list = list;
                }
            }
            catch (Exception) { }
            finally
            {
                reader.Close();
            }

            return boardList;
        }
        /// <summary>
        /// 글쓰기
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool RegisterBoardInfo(BoardInfo info)
        {
                
            SqlCommand sqlCmd = new SqlCommand
            {
                CommandText = "Exercise@RegisterBoardInfo",
                CommandType = CommandType.StoredProcedure
            };
            sqlCmd.Parameters.Add("@board_title", SqlDbType.VarChar,200).Value = info.boardTitle;
            sqlCmd.Parameters.Add("@board_content", SqlDbType.Text).Value = info.boardContent;
            sqlCmd.Parameters.Add("@user_id", SqlDbType.VarChar,200).Value = info.userId;
                   
            List<Boardfile> file = info.BoardFileList;
            int boardId = SQLHelper.ExecuteScalarRetInt(sqlCmd);

            if (RegisterBoardFile(file, boardId))
            {
                return true;
            }
            

            return false ;
        }
        /// <summary>
        /// 파일등록
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool RegisterBoardFile(List<Boardfile> file,int boardId)
        {
            
            for(int i = 0; i < file.Count; i++)
            {
                SqlCommand sqlCmd = new SqlCommand
                {
                    CommandText = "Exercise@RegisterBoardFile",
                    CommandType = CommandType.StoredProcedure
                };
                sqlCmd.Parameters.Add("@file_path", SqlDbType.VarChar,200).Value = file[i].filePath;
                sqlCmd.Parameters.Add("@file_name", SqlDbType.VarChar,200).Value = file[i].fileName;
                sqlCmd.Parameters.Add("@file_guid", SqlDbType.VarChar,200).Value = file[i].fileGuid;
                sqlCmd.Parameters.Add("@board_id", SqlDbType.Int).Value = boardId;
                SQLHelper.ExecuteNonQuery(sqlCmd);
            }
            return true;
            
        }



        /// <summary>
        /// 게시물 하나조회
        /// </summary>
        /// <param name="BoardId"></param>
        /// <returns></returns>
        public BoardInfo RetrieveBoardInfo(int boardId)
        {
            SqlCommand sqlCmd = new SqlCommand
            {
                CommandText = "Exercise@RetrieveBoardInfo",
                CommandType = CommandType.StoredProcedure
            };
            sqlCmd.Parameters.Add("@board_id", SqlDbType.Int).Value = boardId;
            SqlDataReader reader = SQLHelper.ExecuteReader(sqlCmd);

            BoardInfo info = null;
            try
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        info = new BoardInfo
                        {
                            boardId = (int)reader["boardId"],
                            boardTitle = reader["boardTitle"].ToString(),
                            boardContent = reader["boardContent"].ToString(),
                            regDate = (DateTime)reader["regDate"],
                            viewCount = (int)reader["viewCount"],
                            userId = reader["userId"].ToString()                          
                        };
                        info.BoardFileList = RetrieveBoardFile(boardId);
                    }
                }
            }
            catch (Exception e) { }
            finally
            {
                reader.Close();
            }
           


            return info;
        }
        /// <summary>
        /// 파일조회
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public List<Boardfile> RetrieveBoardFile(int boardId)
        {
            SqlCommand sqlCmd = new SqlCommand
            {
                CommandText = "Exercise@RetrieveBoardFile",
                CommandType = CommandType.StoredProcedure
            };
            sqlCmd.Parameters.Add("@board_id", SqlDbType.Int).Value = boardId;
            SqlDataReader reader = SQLHelper.ExecuteReader(sqlCmd);

            List<Boardfile> list = new List<Boardfile>();
            Boardfile file = null;
            try
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {                       
                        file = new Boardfile
                        {
                            boardId = (int)reader["boardId"],
                            fileId = (int)reader["fileId"],
                            fileName = reader["fileName"].ToString(),
                            filePath = reader["filePath"].ToString(),
                            fileGuid = reader["fileGuid"].ToString()
                        };
                        list.Add(file);
                    }
                }
            }
            catch (Exception) { }
            finally
            {
                reader.Close();
            }
            return list;
        }



        /// <summary>
        /// 게시물 삭제- 파일도
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public bool DeleteBoardInfo(int boardId)
        {
            SqlCommand sqlCmd = new SqlCommand
            {
                CommandText = "Exercise@DeleteBoardInfo",
                CommandType = CommandType.StoredProcedure
            };
            sqlCmd.Parameters.Add("@board_id", SqlDbType.Int).Value = boardId;
            return SQLHelper.ExecuteNonQuery(sqlCmd);
        }
        /// <summary>
        /// 파일 1개제거
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public bool DeleteBoardFile(int fileId)
        {
            SqlCommand sqlCmd = new SqlCommand
            {
                CommandText = "Exercise@DeleteBoardFile",
                CommandType = CommandType.StoredProcedure
            };
            sqlCmd.Parameters.Add("@file_id", SqlDbType.Int).Value = fileId;
            return SQLHelper.ExecuteNonQuery(sqlCmd);
        }




        /// <summary>
        /// 글 수정하기
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool UpdateBoardInfo(BoardInfo info)
        {
            SqlCommand sqlCmd = new SqlCommand
            {
                CommandText = "Exercise@UpdateBoardInfo",
                CommandType = CommandType.StoredProcedure
            };
            sqlCmd.Parameters.Add("@board_id", SqlDbType.Int).Value = info.boardId;
            sqlCmd.Parameters.Add("@board_title", SqlDbType.VarChar,200).Value = info.boardTitle;
            sqlCmd.Parameters.Add("@board_content", SqlDbType.VarChar,200).Value = info.boardContent;
            sqlCmd.Parameters.Add("@user_id", SqlDbType.VarChar,200).Value = info.userId;


            return SQLHelper.ExecuteNonQuery(sqlCmd);
        }

        /// <summary>
        /// 파일 정보조회
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public Boardfile RetrieveBoardFileInfo(int fileId)
        {
            SqlCommand sqlCmd = new SqlCommand
            {
                CommandText = "Exercise@RetrieveBoardFileInfo",
                CommandType = CommandType.StoredProcedure
            };
            sqlCmd.Parameters.Add("@file_id", SqlDbType.Int).Value = fileId;
            SqlDataReader reader = SQLHelper.ExecuteReader(sqlCmd);

            Boardfile file = null;
            try
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        file = new Boardfile
                        {
                            boardId = (int)reader["boardId"],
                            fileId = (int)reader["fileId"],
                            fileName = reader["fileName"].ToString(),
                            fileGuid = reader["fileGuid"].ToString(),
                            filePath = reader["filePath"].ToString()
                        };
                        
                    }
                }
            }
            catch (Exception e) { }
            finally
            {
                reader.Close();
            }
            return file;
        }


        /// <summary>
        /// 조회수 상승
        /// </summary>
        /// <returns></returns>
        public bool ViewCountUp(int boardId)
        {
            SqlCommand sqlCmd = new SqlCommand
            {
                CommandText = "Exercise@UpdateViewCountUp",
                CommandType = CommandType.StoredProcedure
            };
            sqlCmd.Parameters.Add("@board_id", SqlDbType.Int).Value = boardId;
            
            return SQLHelper.ExecuteNonQuery(sqlCmd);
        }
       



    }
}