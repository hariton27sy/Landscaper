using System;
using OpenTK;

namespace SimpleGame.Graphic.Models.Templates
{
    public class HollowCrossModel
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

        public static readonly Vector3[] CollateralDiagonalVertices =
        {
            vertices[0], vertices[5], vertices[6], vertices[3]
        };
        
        public static readonly Vector3[] MainDiagonalVertices =
        {
            vertices[4], vertices[1], vertices[2], vertices[7]
        };

        public static Vector3[] GetEdge(BlockEdge edge)
        {
            switch (edge)
            {
                case BlockEdge.Front:
                case BlockEdge.Right:
                case BlockEdge.Back:
                    return MainDiagonalVertices;
                case BlockEdge.Left:
                case BlockEdge.Top:
                case BlockEdge.Bottom:
                    return CollateralDiagonalVertices;
            }

            throw new ArgumentException("Unknown surface");
        }
    }
}