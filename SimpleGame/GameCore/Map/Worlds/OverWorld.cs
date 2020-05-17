using System;
using System.Collections.Generic;
using OpenTK;
using SimpleGame.GameCore.Map.Blocks;

namespace SimpleGame.GameCore.Map.Worlds
{
    public class OverWorld : World
    {
        public OverWorld()
        {
            chunks = new Dictionary<Vector2, Chunk>();

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    var anchor = new Vector2(x, y);
                    var chunk = new Chunk(anchor);
                    chunk.Map[1, 1, 1] = new Wood();
                    
                    chunks.Add(anchor, chunk);
                }
            }
            
            // chunks[new Vector2(0, 0)].Mobs.Add(new Player());
            
            // todo world generation
        }
    }
}