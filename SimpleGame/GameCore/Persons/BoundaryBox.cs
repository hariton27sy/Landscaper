#nullable enable
using System.Collections.Generic;
using OpenTK;

namespace SimpleGame.GameCore.Persons
{
    public struct BoundaryBox
    {
        public Vector3 Start;
        public Vector3 End;

        public override bool Equals(object? obj)
        {
            return obj is BoundaryBox b && b.Start == Start && b.End == End;
        }

        public static BoundaryBox operator +(BoundaryBox box, Vector3 vector)
        {
            return new BoundaryBox{Start = box.Start + vector, End = box.End + vector};
        }
        
        public IEnumerable<Vector3> GetVertices()
        {
            yield return new Vector3(Start);
            yield return new Vector3(Start.X, Start.Y, End.Z);
            yield return new Vector3(Start.X, End.Y, Start.Z);
            yield return new Vector3(Start.X, End.Y, End.Z);
            yield return new Vector3(End.X, Start.Y, Start.Z);
            yield return new Vector3(End.X, Start.Y, End.Z);
            yield return new Vector3(End.X, End.Y, Start.Z);
            yield return new Vector3(End.X, End.Y, End.Z);
        }

        public Vector3 Center => (Start + End) / 2;

        public static BoundaryBox IntBoxFromVector(Vector3 pos)
        {
            var start = new Vector3((int) pos.X, (int) pos.Y, (int) pos.Z);
            var end = start + new Vector3(1, 1, 1);
            return new BoundaryBox
            {
                Start = start,
                End = end
            };
        }

        public float DistanceTo(BoundaryBox other)
        {
            return (other.Center - Center).Length;
        }
    }
}