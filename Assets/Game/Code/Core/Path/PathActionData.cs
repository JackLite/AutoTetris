namespace Core.Path
{
    public struct PathActionData
    {
        public PathAction action;
        public Direction direction;
        public PathActionData(PathAction action, Direction direction)
        {
            this.action = action;
            this.direction = direction;
        }
    }
}