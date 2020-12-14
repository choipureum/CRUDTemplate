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
                                userId = reader["userId"].ToString(),
                                recommend = (int)reader["recommend"],
                                filePath = reader["filePath"].ToString(),
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
            sqlCmd.Parameters.Add("@board_title", SqlDbType.VarChar).Value = info.boardTitle;
            sqlCmd.Parameters.Add("@board_content", SqlDbType.VarChar).Value = info.boardContent;
            sqlCmd.Parameters.Add("@user_id", SqlDbType.VarChar).Value = info.userId;
            if (info.filePath == null) { info.filePath = ""; }
            sqlCmd.Parameters.Add("@filePath", SqlDbType.VarChar, 255).Value = info.filePath;

            return SQLHelper.ExecuteNonQuery(sqlCmd);
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

            if (reader != null)
            {
                if (reader.Read())
                {
                    info=new BoardInfo
                    {
                        boardId = (int)reader["boardId"],
                        boardTitle = reader["boardTitle"].ToString(),
                        boardContent = reader["boardContent"].ToString(),
                        regDate = (DateTime)reader["regDate"],
                        viewCount = (int)reader["viewCount"],
                        userId = reader["userId"].ToString(),
                        recommend = (int)reader["recommend"],
                        filePath = reader["filePath"].ToString(),
                    };                  
                }
            }

            return info;
        }
        /// <summary>
        /// 게시물 삭제
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
            sqlCmd.Parameters.Add("@board_title", SqlDbType.VarChar).Value = info.boardTitle;
            sqlCmd.Parameters.Add("@board_content", SqlDbType.VarChar).Value = info.boardContent;
            if (info.filePath == null) { info.filePath = ""; }
            sqlCmd.Parameters.Add("@filePath", SqlDbType.VarChar, 255).Value = info.filePath;
            return SQLHelper.ExecuteNonQuery(sqlCmd);
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
        /// <summary>
        /// 추천상승
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public bool UpdateRecommendUp(int boardId)
        {
            SqlCommand sqlCmd = new SqlCommand
            {
                CommandText = "Exercise@UpdateRecommendUp",
                CommandType = CommandType.StoredProcedure
            };
            sqlCmd.Parameters.Add("@board_id", SqlDbType.Int).Value = boardId;

            return SQLHelper.ExecuteNonQuery(sqlCmd);
        }

        //댓글======
        /// <summary>
        /// 게시물 당 댓글 조회
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public List<BoardComment> RetrieveBoardComment(int boardId)
        {
            SqlCommand sqlCmd = new SqlCommand
            {
                CommandText = "Exercise@RetrieveBoardComment",
                CommandType = CommandType.StoredProcedure
            };
            sqlCmd.Parameters.Add("@board_id", SqlDbType.Int).Value = boardId;
            SqlDataReader reader = SQLHelper.ExecuteReader(sqlCmd);
        
            List<BoardComment> list = new List<BoardComment>();

            if (reader!=null)
            {
                while (reader.Read())
                {
                    list.Add(new BoardComment
                    {
                        commentId = (int)reader["commentId"],
                        boardId = (int)reader["boardId"],
                        userId = reader["userId"].ToString(),
                        content = reader["content"].ToString(),
                        regDate = (DateTime)reader["regDate"]
                    });
                }
            }           
            return list;
        }

        public bool RegisterBoardComment(BoardComment info)
        {
            SqlCommand sqlCmd = new SqlCommand
            {
                CommandText = "Exercise@RegisterBoardComment",
                CommandType = CommandType.StoredProcedure
            };
            sqlCmd.Parameters.Add("@board_id", SqlDbType.Int).Value = info.boardId;
            sqlCmd.Parameters.Add("@user_id", SqlDbType.VarChar).Value = info.userId;
            sqlCmd.Parameters.Add("@content", SqlDbType.Text).Value = info.content;

            return SQLHelper.ExecuteNonQuery(sqlCmd);
        }






    }
}