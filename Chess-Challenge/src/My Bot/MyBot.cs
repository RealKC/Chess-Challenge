using System.Linq;
using System;
using ChessChallenge.API;
using System.Collections.Generic;

public class MyBot : IChessBot
{
    int queensGambit = 0;
    int[] pieceValues = { 0, 100, 300, 300, 500, 900, 10000 };
    public Move Think(Board board, Timer timer)
    {
        var legalMoves = board.GetLegalMoves();
        System.Random rng = new();

        if (timer.MillisecondsRemaining < 10_000)
        {
            return legalMoves[rng.Next(legalMoves.Length)];
        }

        if (timer.MillisecondsRemaining < 5_000)
        {
            return legalMoves[0];
        }

        // queen's gambit is forced.
        if (board.IsWhiteToMove)
        {
            if (queensGambit == 0)
            {
                Move move = new("d2d4", board);
                if (legalMoves.Contains(move))
                {
                    return move;
                }
                else
                {
                    queensGambit = 69;
                }
            }
            else if (queensGambit == 1)
            {
                return new("c2c4", board);
            }
        }

        foreach (var move in legalMoves)
        {
            if (move.ToString().EndsWith("q")) //force the promotions to be queens
                return move;
            if(MoveIsCheckmate(board, move))
                return move;
            if(MoveIsSafeCheck(board, move))
                return move;
            if(MoveIsUnsafeCheck(board, move)) 
                return move;
            if (MoveIsSafeCapture(board, move))
                return move;
            //if (MoveIsInSafeSquare(board, move)) //very bad, leads to repetition
            //    return move;
        }

        return legalMoves[rng.Next(legalMoves.Length)];
    }

    bool MoveIsCheckmate(Board board, Move move)
    {
        board.MakeMove(move);
        bool isMate = board.IsInCheckmate();
        board.UndoMove(move);
        return isMate;
    }

    bool MoveIsUnsafeCheck(Board board, Move move)
    {
        board.MakeMove(move);
        bool isCheck = board.IsInCheck();
        board.UndoMove(move);
        return isCheck;
    }
    bool MoveIsSafeCheck(Board board, Move move) //safe means that a piece can't take our piece in the new position
    {
        if (MoveIsUnsafeCheck(board, move))
        {
            Square targetSquare = move.TargetSquare;
            board.MakeMove(move);
            Move[] enemyMoves = board.GetLegalMoves();
            board.UndoMove(move);
            foreach (Move enemyMove in enemyMoves)
                if (enemyMove.TargetSquare == targetSquare)
                    return false;

            return true;
        }

        return false;
    }
    /*bool MoveIsInSafeSquare(Board board, Move move)
    {
        Square targetSquare = move.TargetSquare;
        board.MakeMove(move);
        Move[] enemyMoves = board.GetLegalMoves();
        board.UndoMove(move);
        foreach (Move enemyMove in enemyMoves)
            if (enemyMove.TargetSquare == targetSquare)
                return false;

        return true;
    }*/

    bool MoveIsSafeCapture(Board board, Move move) //safe means that a piece can't take our piece in the new position
    {
        if (move.IsCapture)
        {
            Square targetSquare = move.TargetSquare;
            board.MakeMove(move);
            Move[] enemyMoves = board.GetLegalMoves();
            board.UndoMove(move);
            foreach (Move enemyMove in enemyMoves)  
                if(enemyMove.TargetSquare == targetSquare) 
                    return false;
         
            return true;
        }
            
        return false;
    }
}
