namespace EditorAttributes
{
	public enum StringInputMode
	{
		Constant,
		Dynamic
	}

	public interface IDynamicStringAttribute
    {
		public StringInputMode StringInputMode { get; }
    }
}
