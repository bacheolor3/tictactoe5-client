using UnityEngine;

public static class TicTacToeAI
{
    public static (int row, int col)? GetBestMove(Constants.PlayerType[,] board)
    {
        float bestScore = -1000;
        (int row, int col) movePosition = (-1, -1);

        for(var row = 0; row < board.GetLength(0); row++)
        {
            for (var col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == Constants.PlayerType.None)
                {
                    board[row, col] = Constants.PlayerType.PlayerB;
                    var score = TicTacToeAI.DoMinMix(board, 0, false);
                    board[row, col] = Constants.PlayerType.None;
                    if(score > bestScore)
                    {
                        bestScore = score;
                        movePosition = (row, col);
                    }
                }
            }
        }

        if(movePosition != (row:-1, col: -1))
        {
            return (movePosition.row, movePosition.col);
        }



        return null;
    }

    private static float DoMinMix(Constants.PlayerType[,] board, int depth, bool isMaximizing)
    {
        if(CheckGameWin(Constants.PlayerType.PlayerA, board))
        {
            return -10 + depth;
        }
        if (CheckGameWin(Constants.PlayerType.PlayerB, board))
        {
            return 10 - depth;
        }
        if (CheckGameDraw(board))
        {
            return 0;
        }

        if (isMaximizing)
        {
            var bestScore = float.MinValue;
            for(var row = 0; row < board.GetLength(dimension:0); row++)
            {
                for(var col = 0; col < board.GetLength(dimension: 1); col++)
                {
                    if (board[row, col] == Constants.PlayerType.None)
                    {
                        board[row, col] = Constants.PlayerType.PlayerB;
                        var score = DoMinMix(board, depth + 1, false);
                        board[row, col] = Constants.PlayerType.None;
                        bestScore = Mathf.Max(score, bestScore);
                    }
                }
            }
            return bestScore;
            
        }
        else
        {
            var bestScore = float.MaxValue;
            for (var row = 0; row < board.GetLength(dimension:0); row++)
            {
                for (var col = 0; col < board.GetLength(dimension: 1); col++)
                {
                    if (board[row, col] == Constants.PlayerType.None)
                    {
                        board[row, col] = Constants.PlayerType.PlayerB;
                        var score = DoMinMix(board, depth + 1, true);
                        board[row, col] = Constants.PlayerType.None;
                        bestScore = Mathf.Min(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
    }


    public static bool CheckGameDraw(Constants.PlayerType[,] board)
    {
        for (var row = 0; row < board.GetLength(0); row++)
        {
            for (var col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == Constants.PlayerType.None)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool CheckGameWin(Constants.PlayerType playerType, Constants.PlayerType[,] board)
    {
        for (var row = 0; row < board.GetLength(0); row++)
        {
            if (board[row, 0] == playerType &&
                board[row, 1] == playerType &&
                board[row, 2] == playerType)
            {
                return true;
            }
        }

        for (var col = 0; col < board.GetLength(1); col++)
        {
            if (board[0, col] == playerType &&
                board[1, col] == playerType &&
                board[2, col] == playerType)
            {
                return true;
            }
        }

        if (board[0, 0] == playerType &&
            board[1, 1] == playerType &&
            board[2, 2] == playerType)
        {
            return true;
        }

        if (board[0, 2] == playerType &&
            board[1, 1] == playerType &&
            board[2, 0] == playerType)
        {
            return true;
        }
        return false;
    }

    public static void Test()
    {
        int i = 30;

        Test();

        i = 20;
    }
}
