using System.Linq;
using System;
using ChessChallenge.API;

public class MyBot : IChessBot
{
    int queensGambit = 0;

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
            board.MakeMove(move);
            if (board.IsInCheck())
            {
                board.UndoMove(move);
                return move;
            }
            board.UndoMove(move);

            if (move.IsCapture)
            {
                return move;
            }
        }

        return legalMoves[rng.Next(legalMoves.Length)];
    }
}
