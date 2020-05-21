using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;

namespace SimpleGame.Graphic.Models
{
    public class Texture
    {
        public readonly int AtlasGlId;
        private float[] coordinates;
        public readonly string Name;
        private Texture[] states;

        public Texture(int atlasGlId, float[] coordinates)
        {
            AtlasGlId = atlasGlId;
            this.coordinates = coordinates;
        }

        public Texture(string name, Texture[] states)
        {
            this.states = states;
            AtlasGlId = states[0].AtlasGlId;
            coordinates = states[0].coordinates;
            Name = name;
        }

        public float[] Top => coordinates.Take(8).ToArray();
        public float[] Right => coordinates.Skip(8).Take(8).ToArray();
        public float[] Back => coordinates.Skip(16).Take(8).ToArray();
        public float[] Left => coordinates.Skip(24).Take(8).ToArray();
        public float[] Up => coordinates.Skip(32).Take(8).ToArray();
        public float[] Bottom => coordinates.Skip(40).Take(8).ToArray();

        public Texture this[int state]
        {
            get
            {
                if (states is null)
                    throw new ArgumentException("This texture don't support states");

                return states[state];
            }
        }
    }
}