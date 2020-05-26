#nullable enable
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
    }
}