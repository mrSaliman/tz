using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            var mapSize = grid.Size;
            var chessUnit = grid.Get(from);

            // движения представлены в виде вектора (x, y, z), где x и y - координаты, а z - количество возможных ходов в данном направлении
            List<Vector3Int> chessUnitMoves = GetChessUnitMoves(unit, chessUnit.PieceModel.Color);

            // далее идёт поиск в ширину
            Queue<Vector2Int> queue = new();
            queue.Enqueue(from);

            Dictionary<Vector2Int, Vector2Int> cameFrom = new();
            Dictionary<Vector2Int, int> cost = new();
            cameFrom[from] = from;
            cost[from] = 0;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.Equals(to))
                {
                    break;
                }

                foreach (var move in chessUnitMoves)
                {
                    var newCost = cost[current] + 1;
                    var p = 1;
                    var next = current + ((Vector2Int)move) * p;
                    while (p <= move.z && Suits(next, mapSize, grid))
                    {
                        if (!cost.ContainsKey(next) || newCost < cost[next])
                        {
                            cost[next] = newCost;
                            queue.Enqueue(next);
                            cameFrom[next] = current;
                        }
                        p++;
                        next = current + ((Vector2Int)move) * p;
                    }
                }
            }

            if (!cameFrom.ContainsKey(to))
            {
                return null;
            }

            var path = new List<Vector2Int>();
            var cell = to;
            while (!cell.Equals(from))
            {
                path.Add(cell);
                cell = cameFrom[cell];
            }
            path.Reverse();

            return path;
        }

        private List<Vector3Int> GetChessUnitMoves(ChessUnitType unit, ChessUnitColor color)
        {
            List<Vector3Int> chessUnitMoves = new();
            switch (unit)
            {
                case ChessUnitType.Pon:
                    chessUnitMoves.Add(new(0, color == ChessUnitColor.White ? 1 : -1, 1));
                    break;
                case ChessUnitType.King:
                    for (int i = -1; i < 2; i++)
                        for (int j = -1; j < 2; j++)
                            if (i != j || i != 0) chessUnitMoves.Add(new(i, j, 1));
                    break;
                case ChessUnitType.Queen:
                    for (int i = -1; i < 2; i++)
                        for (int j = -1; j < 2; j++)
                            if (i != j || i != 0) chessUnitMoves.Add(new(i, j, int.MaxValue));
                    break;
                case ChessUnitType.Rook:
                    for (int i = -1; i < 2; i++)
                        for (int j = -1; j < 2; j++)
                            if ((i == 0 || j == 0) && i != j) chessUnitMoves.Add(new(i, j, int.MaxValue));
                    break;
                case ChessUnitType.Knight:
                    chessUnitMoves.Add(new(2, 1, 1));
                    chessUnitMoves.Add(new(2, -1, 1));
                    chessUnitMoves.Add(new(-2, 1, 1));
                    chessUnitMoves.Add(new(-2, -1, 1));
                    chessUnitMoves.Add(new(1, 2, 1));
                    chessUnitMoves.Add(new(1, -2, 1));
                    chessUnitMoves.Add(new(-1, 2, 1));
                    chessUnitMoves.Add(new(-1, -2, 1));
                    break;
                case ChessUnitType.Bishop:
                    for (int i = -1; i < 2; i++)
                        for (int j = -1; j < 2; j++)
                            if ((i == j || i == -j) && i != 0) chessUnitMoves.Add(new(i, j, int.MaxValue));
                    break;
                default:
                    break;
            }
            return chessUnitMoves;
        }

        //проверка на пустоту клетки и на выход за границу
        static private bool Suits(Vector2Int next, Vector2Int size, ChessGrid grid)
        {
            if (next.x >= 0 && next.y >= 0 && next.x < size.x && next.y < size.y)
            {
                var unit = grid.Get(next);
                return unit is null;
            }
            return false;
        }
    }
}