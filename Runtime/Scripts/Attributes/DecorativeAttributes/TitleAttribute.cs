using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to draw a fully customizable header
	/// </summary>
	public class TitleAttribute : PropertyAttribute, IDynamicStringAttribute
    {
	    public string Title { get; private set; }
        public int TitleSize { get; private set; }
        public float TitleSpace { get; private set; }
        public float LineThickness { get; private set; }
        public bool DrawLine { get; private set; }

        public TextAnchor Alignment { get; private set; }
        public StringInputMode StringInputMode { get; private set; }

		/// <summary>
		/// Attribute to draw a fully customizable header
		/// </summary>
		/// <param name="title">The text of the title</param>
		/// <param name="titleSize">The size of the title font</param>
		/// <param name="titleSpace">The space between the title and field</param>
		/// <param name="drawLine">Draw a line under the title</param>
		/// <param name="lineThickness">The thickness of the line in pixels</param>
		/// <param name="alignment">The alignment of the title</param>
		/// <param name="stringInputMode">Set if the string input is set trough a constant or dynamically trough another member</param>
		public TitleAttribute(string title, int titleSize = 20, float titleSpace = 10f, bool drawLine = true, float lineThickness = 2f, TextAnchor alignment = TextAnchor.MiddleLeft, StringInputMode stringInputMode = StringInputMode.Constant)
#if UNITY_2023_3_OR_NEWER
        : base(true)
#endif
		{
			Title = title;
            TitleSize = titleSize;
            TitleSpace = titleSpace;
            DrawLine = drawLine;
			LineThickness = lineThickness;
			Alignment = alignment;
            StringInputMode = stringInputMode;
        }
	}
}
