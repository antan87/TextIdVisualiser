using Microsoft.VisualStudio.Text.Adornments;

namespace TextIdVisualiser.Elements
{
    public sealed class TooltipElement
    {
        public string Label { get; }
        public ContainerElementStyle Style { get; }
        public object Value { get; }

        public TooltipElement(string label, object value, ContainerElementStyle style)
        {
            this.Label = label;
            this.Value = value;
            this.Style = style;
        }
    }
}
