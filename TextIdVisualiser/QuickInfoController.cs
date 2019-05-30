using System.Collections.Generic;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace TextIdVisualiser
{
    /// <summary>
    /// The control for quick info.
    /// </summary>
    /// <owner>Anton Patron</owner>
    /// <seealso cref="IIntellisenseController" />
    internal class QuickInfoController : IIntellisenseController
    {
        private ITextView m_textView;
        private readonly IList<ITextBuffer> m_subjectBuffers;
        private readonly IIntellisenseControllerProvider m_provider;

        /// <summary>
        /// Called when a new subject <see cref="T:Microsoft.VisualStudio.Text.ITextBuffer" /> appears in the graph of buffers associated with the <see cref="T:Microsoft.VisualStudio.Text.Editor.ITextView" />, due to a change in projection or content type.
        /// </summary>
        /// <owner>Anton Patron</owner>
        /// <param name="subjectBuffer">The newly-connected text buffer.</param>
        public void ConnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
        }

        /// <summary>
        /// Called when a subject <see cref="T:Microsoft.VisualStudio.Text.ITextBuffer" /> is removed from the graph of buffers associated with the <see cref="T:Microsoft.VisualStudio.Text.Editor.ITextView" />, due to a change in projection or content type.
        /// </summary>
        /// <owner>Anton Patron</owner>
        /// <param name="subjectBuffer">The disconnected text buffer.</param>
        public void DisconnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
        }

        /// <summary>
        /// Detaches the controller from the specified <see cref="T:Microsoft.VisualStudio.Text.Editor.ITextView" />.
        /// </summary>
        /// <owner>Anton Patron</owner>
        /// <param name="textView">The <see cref="T:Microsoft.VisualStudio.Text.Editor.ITextView" /> from which the controller should detach.</param>
        public void Detach(ITextView textView)
        {
            if (this.m_textView == textView)
                this.m_textView = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickInfoController"/> class.
        /// </summary>
        /// <owner>Anton Patron</owner>
        /// <param name="textView">The text view.</param>
        /// <param name="subjectBuffers">The subject buffers.</param>
        /// <param name="provider">The provider.</param>
        internal QuickInfoController(ITextView textView, IList<ITextBuffer> subjectBuffers, IIntellisenseControllerProvider provider)
        {
            this.m_textView = textView;
            this.m_subjectBuffers = subjectBuffers;
            this.m_provider = provider;
        }
    }
}