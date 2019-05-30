using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using TextIdVisualiser.Options;

namespace TextIdVisualiser
{
    /// <summary>
    /// The quick info control provider.
    /// </summary>
    /// <owner>Anton Patron</owner>
    /// <seealso cref="Microsoft.VisualStudio.Language.Intellisense.IIntellisenseControllerProvider" />
    [Export(typeof(IIntellisenseControllerProvider))]
    [Name("ToolTip QuickInfo Controller")]
    [ContentType("CSharp")]
    internal class QuickInfoControllerProvider : IIntellisenseControllerProvider
    {
        /// <summary>
        /// Gets or sets the quick information broker.
        /// </summary>
        /// <owner>Anton Patron</owner>
        /// <value>The quick information broker.</value>
        [Import]
        internal IAsyncQuickInfoBroker QuickInfoBroker { get; set; }

        /// <summary>
        /// Attempts to create an IntelliSense controller for a specific text view.
        /// </summary>
        /// <owner>Anton Patron</owner>
        /// <param name="textView">The text view for which a controller should be created.</param>
        /// <param name="subjectBuffers">The set of text buffers with matching content types that are potentially visible in the view.</param>
        /// <returns>A valid IntelliSense controller, or null if none could be created.</returns>
        public IIntellisenseController TryCreateIntellisenseController(ITextView textView, IList<ITextBuffer> subjectBuffers)
        {
            return textView.Properties.GetOrCreateSingletonProperty(() => new QuickInfoController(textView, subjectBuffers, this));
        }
    }
}