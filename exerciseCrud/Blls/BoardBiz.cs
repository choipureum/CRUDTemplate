using exerciseCrud.Dals;
using exerciseCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exerciseCrud.Blls
{
    public class BoardBiz
    {
        private readonly BoardDals _dal = new BoardDals();
        public BoardList RetrieveBoardList(int page, int size)
        {
            return _dal.RetrieveBoardList(page, size);
        }
        public bool RegisterBoardInfo(BoardInfo info)
        {
            return _dal.RegisterBoardInfo(info);
        }
        public BoardInfo RetrieveBoardInfo(int boardId)
        {

            // 조회수+1
            if (_dal.ViewCountUp(boardId))
            {
                return _dal.RetrieveBoardInfo(boardId);
            }
            return _dal.RetrieveBoardInfo(boardId);
        }
        public bool DeleteBoardInfo(int boardId)
        {
            return _dal.DeleteBoardInfo(boardId);
        }

        public bool UpdateBoardInfo(BoardInfo info)
        {
            return _dal.UpdateBoardInfo(info);
        }
        public bool UpdateRecommendUp(int boardId)
        {
            return _dal.UpdateRecommendUp(boardId);
        }
        public List<BoardComment> RetrieveBoardComment(int boardId)
        {
            return _dal.RetrieveBoardComment(boardId);
        }
        public bool RegisterBoardComment(BoardComment info)
        {
            return _dal.RegisterBoardComment(info);
        }
    }
}