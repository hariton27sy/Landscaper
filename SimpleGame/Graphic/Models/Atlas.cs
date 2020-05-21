using System;

namespace SimpleGame.Graphic.Models
{
    public class Atlas
    {
        public readonly int GlAtlasId;
        private readonly int width;
        private readonly int height;
        private readonly int elemWidth;
        private readonly int elemHeight;

        public Atlas(int glAtlasId, int width, int height, int elemWidth, int elemHeight)
        {
            this.GlAtlasId = glAtlasId;
            this.width = width;
            this.height = height;
            this.elemWidth = elemWidth;
            this.elemHeight = elemHeight;
        }

        private int CountX => width / elemWidth;
        private int CountY => height / elemHeight;
        
        
        /// <summary>
        /// Returns vertex coordinates of texture in atlas
        /// </summary>
        /// <param name="textureNumber">Texture number. Indexing from 0</param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public float[] this[int textureNumber]
        {
            get
            {
                var x = textureNumber % CountX;
                var y = textureNumber / CountX;
                if (y >= CountY)
                    throw new IndexOutOfRangeException(
                        $"Trying to get texture with number {textureNumber}. Atlas contains only {CountX * CountY} textures.");
                
                return new float[]
                {
                    (float)x * elemWidth / width, (float)(y + 1) * elemHeight / height,
                    (float)(x + 1) * elemWidth / width, (float)(y + 1) * elemHeight / height,
                    (float)(x + 1) * elemWidth / width, (float)y * elemHeight / height,
                    (float)x * elemWidth / width, y * (float)elemHeight / height,
                };
            }
        }
    }
}