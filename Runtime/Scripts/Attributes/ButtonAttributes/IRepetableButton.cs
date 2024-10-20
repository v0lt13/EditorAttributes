namespace EditorAttributes
{
	public interface IRepetableButton
    {
		public bool IsRepetable { get; }
		public long PressDelay { get; }
		public long RepetitionInterval { get; }
	}
}
