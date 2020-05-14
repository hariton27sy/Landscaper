namespace SimpleGame.GameCore.GameModels
{
    public class Texture
    {
        public int TextureId { get; }

        private readonly int width, height, elemWidth, elemHeight;

        private int CountByHorizontal => width / elemWidth;
        private int CountByVertical => height / elemHeight;

        /// <summary>
        /// Allows to get verticesCoordinates be number
        /// </summary>
        /// <param name="textureId">TextureId from GraphicEngine.Loader.LoadTexture</param>
        /// <param name="width">Texture width</param>
        /// <param name="height">Texture height</param>
        /// <param name="elemWidth">Width for one element</param>
        /// <param name="elemHeight">Height for one element</param>
        public Texture(int textureId, int width, int height, int elemWidth, int elemHeight)
        {
            TextureId = textureId;
            this.width = width;
            this.height = height;
            this.elemWidth = elemWidth;
            this.elemHeight = elemHeight;
        }

        public float[] this[int number]
        {
            get
            {
                var x = number % CountByHorizontal;
                var y = number / CountByHorizontal;
                return new[]
                {
                    (float)x * elemWidth / width, (float) (y + 1) * elemHeight / height,
                    (float)(x + 1) * elemWidth / width, (float) (y + 1) * elemHeight / height,
                    (float)(x + 1) * elemWidth / width, (float) (y) * elemHeight / height,
                    (float)x * elemWidth / width, (float) (y) * elemHeight / height,
                };
            }
        }
    }
}