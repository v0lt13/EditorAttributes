using UnityEngine;

namespace EditorAttributes
{
    public class TitleAttribute : PropertyAttribute
    {
	    public string Title { get; private set; }
        public int TitleSize { get; private set; }
        public bool DrawLine { get; private set; }
        public TextAnchor Alignment { get; private set; }

        /// <summary>
        /// Attribute to draw a fully customizable header
        /// </summary>
        /// <param name="title">The text of the title</param>
        /// <param name="titleSize">The size of the title</param>
        /// <param name="drawLine">Draw a line under the title</param>
        /// <param name="alignment">The alignment of the title</param>
        public TitleAttribute(string title, int titleSize = 20, bool drawLine = true, TextAnchor alignment = TextAnchor.MiddleLeft)
        {
            Title = title;
            TitleSize = titleSize;
            DrawLine = drawLine;
            Alignment = alignment;
        }
    }
}
