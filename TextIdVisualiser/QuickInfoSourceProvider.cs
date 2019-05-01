using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;

namespace TextIdVisualiser
{
    /// <summary>
    /// The quick info source provider.
    /// </summary>
    /// <owner>Anton Patron</owner>
    /// <seealso cref="Microsoft.VisualStudio.Language.Intellisense.IAsyncQuickInfoSourceProvider" />
    [Export(typeof(IAsyncQuickInfoSourceProvider))]
    [Name("ToolTip QuickInfo Source")]
    [Order(Before = "Default Quick Info Presenter")]
    [ContentType("text")]
    internal class QuickInfoSourceProvider : IAsyncQuickInfoSourceProvider
    {

        /// <summary>
        /// Gets or sets the navigator service.
        /// </summary>
        /// <owner>Anton Patron</owner>
        /// <value>The navigator service.</value>
        [Import]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        /// <summary>
        /// Gets or sets the text buffer factory service.
        /// </summary>
        /// <owner>Anton Patron</owner>
        /// <value>The text buffer factory service.</value>
        [Import]
        internal ITextBufferFactoryService TextBufferFactoryService { get; set; }

        /// <summary>
        /// Tries the create quick information source.
        /// </summary>
        /// <owner>Anton Patron</owner>
        /// <param name="textBuffer">The text buffer.</param>
        /// <returns></returns>
        IAsyncQuickInfoSource IAsyncQuickInfoSourceProvider.TryCreateQuickInfoSource(ITextBuffer textBuffer)
        {
            return new QuickInfoSource(ResourceManagerHelper.TextValues, this, textBuffer);
        }
    }
}