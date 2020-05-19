namespace SimpleGame.GraphicEngine
{
    public struct MouseArgs
    {
        public readonly int DeltaX;
        public readonly int DeltaY;
        public readonly bool LeftClick;
        public readonly bool RightClick;

        public MouseArgs(int deltaX, int deltaY, bool leftClick, bool rightClick)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
            LeftClick = leftClick;
            RightClick = rightClick;
        }
    }
}