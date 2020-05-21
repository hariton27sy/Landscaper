namespace SimpleGame.Graphic.Models.Templates
{
    /// <summary>
    /// Вспомогательный класс для моеделей чанков
    /// </summary>
    public class BlockModel
    {
        public static float[] UpVertices
        {
            get
            {
                return new[]
                {
                    -0.5f, 0.5f, 0.5f,
                    0.5f, 0.5f, 0.5f,
                    0.5f, 0.5f, -0.5f,
                    -0.5f, 0.5f, -0.5f
                };
            }
        }
        
        public static float[] TopVertices
        {
            get
            {
                return new[]
                {
                    -0.5f, -0.5f, 0.5f,
                    0.5f, -0.5f, 0.5f,
                    0.5f, 0.5f, 0.5f,
                    -0.5f, 0.5f, 0.5f
                };
            }
        }
        
        public static float[] RightVertices
        {
            get
            {
                return new[]
                {
                    0.5f, -0.5f, 0.5f,
                    0.5f, -0.5f, -0.5f,
                    0.5f, 0.5f, -0.5f,
                    0.5f, 0.5f, 0.5f
                };
            }
        }
        
        public static float[] BackVertices
        {
            get
            {
                return new[]
                {
                    0.5f, -0.5f, -0.5f,
                    -0.5f, -0.5f, -0.5f,
                    0.5f, 0.5f, -0.5f,
                    -0.5f, 0.5f, -0.5f
                };
            }
        }
        
        public static float[] LeftVertices
        {
            get
            {
                return new[]
                {
                    -0.5f, -0.5f, -0.5f,
                    -0.5f, -0.5f, 0.5f,
                    -0.5f, 0.5f, -0.5f,
                    -0.5f, 0.5f, 0.5f
                };
            }
        }
        
        public static float[] BottomVertices
        {
            get
            {
                return new[]
                {
                    -0.5f, -0.5f, 0.5f,
                    0.5f, -0.5f, 0.5f,
                    0.5f, -0.5f, -0.5f,
                    -0.5f, -0.5f, -0.5f
                };
            }
        }

        public static int[] Indices
        {
            get
            {
                return new[]
                {
                    0, 1, 2,
                    2, 3, 0
                };
            }
        }
    }
}