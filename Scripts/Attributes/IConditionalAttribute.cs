namespace EditorAttributes
{
    public interface IConditionalAttribute
    {
        public string ConditionName { get; }
	    public int EnumValue { get; }
    }
}
