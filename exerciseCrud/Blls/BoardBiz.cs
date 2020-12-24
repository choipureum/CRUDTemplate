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

            // 상세정보 조회수+1
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
        public bool RegisterBoardFile(List<Boardfile> file, int boardId)
        {
            return _dal.RegisterBoardFile(file, boardId);
        }
        public int RetrieveBoardFileCount(int boardId)
        {
            return _dal.RetrieveBoardFile(boardId).Count;
        }
        public bool DeleteBoardFile(int fileId)
        {
            //DB && 서버 파일삭제
            if (Util.DeleteFile(fileId) && _dal.DeleteBoardFile(fileId))
            {
                return true;
            }
            return false;
        }


    }
}