namespace EditorAttributes
{
    public interface IColorAttribute
    {
		public float R { get; }
		public float G { get; }
		public float B { get; }
		public string HexColor { get; }

		public GUIColor GUIColor { get; }
	}
}
