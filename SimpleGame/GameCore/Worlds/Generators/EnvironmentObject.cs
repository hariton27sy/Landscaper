using System.Collections.Generic;
using OpenTK;

namespace SimpleGame.GameCore.Worlds
{
    public class EnvironmentObject
    {
        public List<(Vector3, int)> Parts;

        public EnvironmentObject()
        {
            Parts = new List<(Vector3, int)>();
        }

        public EnvironmentObject((Vector3, int) part) : this()
        {
            Parts.Add(part);
        }
    }
}