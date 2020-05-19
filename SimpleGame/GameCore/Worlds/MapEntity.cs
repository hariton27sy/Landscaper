using System;

namespace SimpleGame.GameCore.Map
{
    public abstract class MapEntity
    {
        // todo hitboxes 
        public virtual bool DeadInConflict(MapEntity enemy)
        {
            return false;
        }

        public virtual string TextureName => this.GetType().Name;
    }
}