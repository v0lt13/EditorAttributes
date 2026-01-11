namespace EditorAttributes
{
    public interface IMinMaxAxisValueAttribute
    {
        public float MinValueX { get; }
        public float MaxValueX { get; }

        public float MinValueY { get; }
        public float MaxValueY { get; }

        public float MinValueZ { get; }
        public float MaxValueZ { get; }

        public float MinValueW { get; }
        public float MaxValueW { get; }
    }
}
