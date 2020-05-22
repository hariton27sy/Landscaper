using OpenTK;

namespace SimpleGame.GameCore.Worlds
{
    public static class VectorExtensions
    {
        public static Vector2 ToChunkPosition(this Vector2 worldPosition)
        {
            return new Vector2((int)(worldPosition.X / Chunk.Width), (int)(worldPosition.Y / Chunk.Length));
        }

        public static Vector2 ToChunkPosition(this Vector3 worldPosition)
        {
            return ToChunkPosition(new Vector2(worldPosition.X, worldPosition.Z));
        }

        public static Vector2 InWorldPosition(this Vector2 inChunk, Vector2 chunkPosition)
        {
            return new Vector2(inChunk.X + chunkPosition.X * Chunk.Width, inChunk.Y + chunkPosition.Y * Chunk.Length);
        }

        public static Vector2 Round(this Vector2 position)
        {
            return new Vector2((int) position.X, (int) position.Y);
        }
    }
}