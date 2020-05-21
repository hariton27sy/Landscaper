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
            new Vector3(-0.5f, -0.5f, 0.5f),  // left bottom front corner [0]
                                                        //     ^ y
                                                        // •--------•
                                                        // |\      /|
                                                        // | •----• |
                                                        // | | *z | | ---> x
                                                        // | •----• |
                                                        // |/      \|
                                                        // ■--------•
            new Vector3(0.5f, -0.5f, 0.5f),   // right bottom front corner [1]
                                                        //     ^ y
                                                        // •--------•
                                                        // |\      /|
                                                        // | •----• |
                                                        // | | *z | | ---> x
                                                        // | •----• |
                                                        // |/      \|
                                                        // •--------■
            new Vector3(0.5f, 0.5f, 0.5f),    // right upper front corner [2]
                                                        //     ^ y
                                                        // •--------■
                                                        // |\      /|
                                                        // | •----• |
                                                        // | | *z | | ---> x
                                                        // | •----• |
                                                        // |/      \|
                                                        // •--------•
            new Vector3(-0.5f, 0.5f, 0.5f),   // left upper front corner [3]
                                                        //     ^ y
                                                        // ■--------•
                                                        // |\      /|
                                                        // | •----• |
                                                        // | | *z | | ---> x
                                                        // | •----• |
                                                        // |/      \|
                                                        // •--------•
            new Vector3(-0.5f, -0.5f, -0.5f), // left bottom back corner [4]
                                                        //     ^ y
                                                        // •--------•
                                                        // |\      /|
                                                        // | •----• |
                                                        // | | *z | | ---> x
                                                        // | ■----• |
                                                        // |/      \|
                                                        // •--------•
            new Vector3(0.5f, -0.5f, -0.5f),  // right bottom back corner [5]
                                                        //     ^ y
                                                        // •--------•
                                                        // |\      /|
                                                        // | •----• |
                                                        // | | *z | | ---> x
                                                        // | •----■ |
                                                        // |/      \|
                                                        // •--------•
            new Vector3(0.5f, 0.5f, -0.5f),   // right upper back corner [6]
                                                        //     ^ y
                                                        // •--------•
                                                        // |\      /|
                                                        // | •----■ |
                                                        // | | *z | | ---> x
                                                        // | •----• |
                                                        // |/      \|
                                                        // •--------•
            new Vector3(-0.5f, 0.5f, -0.5f),  // left upper back corner [7]
                                                        //     ^ y
                                                        // •--------•
                                                        // |\      /|
                                                        // | ■----• |
                                                        // | | *z | | ---> x
                                                        // | •----• |
                                                        // |/      \|
                                                        // •--------•
        };

        public static readonly Vector3[] TopVertices =
        {
            vertices[3], vertices[2], vertices[6], vertices[7]
            //     ^ y
            // •--------•
            // |\ ■■■■ /|
            // | •----• |
            // | | *z | | ---> x
            // | •----• |
            // |/      \|
            // •--------•
        };
        
        public static readonly Vector3[] FrontVertices =
        {
            vertices[0], vertices[1], vertices[2], vertices[3]
            //     ^ y
            // •--------•
            // |■■■■■■■■|
            // |■■■■■■■■|
            // |■■■■■■■■| ---> x
            // |■■■■■■■■|
            // |■■■■■■■■|
            // •--------•
        };
        
        public static readonly Vector3[] RightVertices =
        {
            vertices[1], vertices[5], vertices[6], vertices[2]
            //     ^ y
            // •--------•
            // |\      /|
            // | •----•■|
            // | | *z |■| ---> x
            // | •----•■|
            // |/      \|
            // •--------•
        };
        
        public static readonly Vector3[] BackVertices =
        {
            vertices[5], vertices[4], vertices[7], vertices[6]
            //     ^ y
            // •--------•
            // |\      /|
            // | •----• |
            // | | ■■ | | ---> x
            // | •----• |
            // |/      \|
            // •--------•
        };
        
        public static readonly Vector3[] LeftVertices =
        {
            vertices[4], vertices[0], vertices[3], vertices[7]
            //     ^ y
            // •--------•
            // |\      /|
            // |■•----• |
            // |■| *z | | ---> x
            // |■•----• |
            // |/      \|
            // •--------•
        };
        
        public static readonly Vector3[] BottomVertices =
        {
            vertices[0], vertices[1], vertices[5], vertices[4]
            //     ^ y
            // •--------•
            // |\      /|
            // | •----• |
            // | | *z | | ---> x
            // | •----• |
            // |/ ■■■■ \|
            // •--------•
        };

        public static Vector3[] GetEdge(BlockEdge edge)
        {
            switch (edge)
            {
                case BlockEdge.Front:
                    return FrontVertices;
                case BlockEdge.Right:
                    return RightVertices;
                case BlockEdge.Back:
                    return BackVertices;
                case BlockEdge.Left:
                    return LeftVertices;
                case BlockEdge.Top:
                    return TopVertices;
                case BlockEdge.Bottom:
                    return BottomVertices;
            }

            throw new ArgumentException("Unknown surface");
        }
    }
}