using System.Linq;
using System;
using ChessChallenge.API;
using System.Collections.Generic;

public class MyBot : IChessBot
{
    int[] pieceValues = { 0, 100, 300, 300, 500, 900, 10000 };
    Move bestMove;
    public Move Think(Board board, Timer timer)
    {
        bestMove = board.GetLegalMoves()[1];
        return Tumefiere(board, timer);
    }

    public Move Tumefiere(Board board, Timer timer, int depth = 0, bool enemy = false)
    {
        if(depth == 6)
        {
            return bestMove;
        }

        if(enemy)
        {
            var enemyMoves = board.GetLegalMoves();
            foreach (var move in enemyMoves)
            {
                board.MakeMove(move);
                return Tumefiere(board, timer, depth + 1, true);
                board.UndoMove(move);
            }
        }
        else
        {
            var ourMoves = board.GetLegalMoves();
            foreach(var move in ourMoves)
            {
                board.MakeMove(move);
                return Tumefiere(board, timer, depth + 1, true);
                board.UndoMove(move);
            }
        }
        return bestMove;
    }

}
