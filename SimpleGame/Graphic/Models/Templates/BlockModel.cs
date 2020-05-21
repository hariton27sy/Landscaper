using System;
using OpenTK;

namespace SimpleGame.Graphic.Models.Templates
{
    /// <summary>
    /// Вспомогательный класс для моеделей чанков
    /// </summary>
    public class BlockModel
    {
        private static readonly Vector3[] vertices =
        {
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
        };

        public static readonly Vector3[] UpVertices =
        {
            vertices[3], vertices[2], vertices[6], vertices[7]
        };
        
        public static readonly Vector3[] TopVertices =
        {
            vertices[0], vertices[1], vertices[2], vertices[3]
        };
        
        public static readonly Vector3[] RightVertices =
        {
            vertices[1], vertices[5], vertices[6], vertices[2]
        };
        
        public static readonly Vector3[] BackVertices =
        {
            vertices[5], vertices[4], vertices[7], vertices[6]
        };
        
        public static readonly Vector3[] LeftVertices =
        {
            vertices[4], vertices[0], vertices[3], vertices[7]
        };
        
        public static readonly Vector3[] BottomVertices =
        {
            vertices[0], vertices[1], vertices[5], vertices[4]
        };

        public static Vector3[] GetEdge(BlockEdge edge)
        {
            switch (edge)
            {
                case BlockEdge.Top:
                    return TopVertices;
                case BlockEdge.Right:
                    return RightVertices;
                case BlockEdge.Back:
                    return BackVertices;
                case BlockEdge.Left:
                    return LeftVertices;
                case BlockEdge.Up:
                    return UpVertices;
                case BlockEdge.Bottom:
                    return BottomVertices;
            }

            throw new ArgumentException("Unknown sruface");
        }
    }
}