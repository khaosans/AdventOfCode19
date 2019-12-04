using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Day3V2
{
    public class Direction
    {
        public DirectionType DirectionType { get; set; }
        public int Distance { get; set; }

        public Direction(DirectionType directionType, int distance)
        {
            DirectionType = directionType;
            Distance = distance;
        }


        public static List<Direction> ParseInput(String input)
        {
            return input.Split(",").Select(
                x =>
                {
                    var type = DetermineDirectionType(x);
                    var distance = int.Parse(x.Substring(1));
                    return new Direction(type, distance);
                }
            ).ToList();
        }

        private static DirectionType DetermineDirectionType(string x)
        {
            DirectionType type = DirectionType.None;
            var enumType = x.First().ToString();
            switch (enumType)
            {
                case "D":
                    type = DirectionType.Down;
                    break;
                case "U":
                    type = DirectionType.Up;
                    break;
                case "L":
                    type = DirectionType.Left;
                    break;
                case "R":
                    type = DirectionType.Right;
                    break;
            }

            return type;
        }

        public static List<List<Direction>> GetPaths(String input)
        {
            return input.Split("\n")
                .Select(ParseInput)
                .ToList();
        }

        public static List<Tuple<int, Point>> GetPoints(List<Direction> directions)
        {
            var origin = new Point(0, 0);
            List<Tuple<int, Point>> path = new List<Tuple<int, Point>> {Tuple.Create(0, origin)};

            foreach (var direction in directions)
            {
                for (int i = 1; i <= direction.Distance; ++i)

                {
                    var tupleOfLast = path.Last();
                    var lastPoint = tupleOfLast.Item2;

                    switch (direction.DirectionType)
                    {
                        case DirectionType.Up:
                            path.Add(Tuple.Create(tupleOfLast.Item1 + 1, new Point(lastPoint.X, lastPoint.Y + 1)));
                            break;
                        case DirectionType.Down:
                            path.Add(Tuple.Create(tupleOfLast.Item1 + 1, new Point(lastPoint.X, lastPoint.Y - 1)));

                            break;
                        case DirectionType.Right:
                            path.Add(Tuple.Create(tupleOfLast.Item1 + 1, new Point(lastPoint.X + 1, lastPoint.Y)));

                            break;
                        case DirectionType.Left:
                            path.Add(Tuple.Create(tupleOfLast.Item1 + 1, new Point(lastPoint.X + -1, lastPoint.Y)));
                            break;
                        case DirectionType.None:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            path.RemoveAt(0);
            return path;
        }

        public static int MathDistance(Point finish)
        {
            var origin = new Point(0, 0);
            return Math.Abs(origin.X - finish.X) + Math.Abs(origin.Y - finish.Y);
        }

        public static int FindClosest(string test1)
        {
            var set1 = GetPointsForData(test1, out var set2);

            var intersect = Enumerable.Intersect(set1, set2).ToHashSet();

            var closest = intersect.Select(MathDistance).Min();
            return closest;
        }

        private static HashSet<Point> GetPointsForData(string test1, out HashSet<Point> set2)
        {
            List<List<Direction>> paths = GetPaths(test1);

            var set1 = GetPoints(paths[0]).Select(x => x.Item2).ToHashSet();
            set2 = GetPoints(paths[1]).Select(x => x.Item2).ToHashSet();
            return set1;
        }


        public static int GetSteps(string test1)
        {
            List<List<Direction>> paths = GetPaths(test1);

            var first = GetPoints(paths[0]).ToHashSet();
            var sec = GetPoints(paths[1]).ToHashSet();

            var intersections = Enumerable.Intersect(first.Select(x => x.Item2), sec.Select(x => x.Item2));

            //  distance, point, steps
            List<Tuple<int, Point, int>> metaDatas = new List<Tuple<int, Point, int>>();

            foreach (var intersection in intersections)
            {
                Tuple<int,Point> firstSingle = first.Where(x => x.Item2.Equals(intersection)).Single();
                Tuple<int,Point> secondSing = sec.Where(x => x.Item2.Equals(intersection)).Single();
                var min = Math.Min(firstSingle.Item1,secondSing.Item1);
                metaDatas.Add(Tuple.Create(MathDistance(intersection), intersection, min));
            }

            return metaDatas.Select(x => x.Item3).Sum();

        }
    }


    public enum DirectionType
    {
        Up = 'U',
        Down = 'D',
        Left = 'L',
        Right = 'R',
        None = 'N'
    }
}