using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;

namespace TextIdVisualiser
{
    /// <summary>
    /// THe quick info source.
    /// </summary>
    /// <owner>Anton Patron</owner>
    /// <seealso cref="Microsoft.VisualStudio.Language.Intellisense.IAsyncQuickInfoSource" />
    internal class QuickInfoSource : IAsyncQuickInfoSource
    {
        private readonly QuickInfoSourceProvider m_provider;
        private readonly ITextBuffer m_subjectBuffer;
        private readonly Dictionary<string, string> m_dictionary;
        private bool m_isDisposed;

        public void Dispose()
        {
            if (!this.m_isDisposed)
            {
                GC.SuppressFinalize(this);
                this.m_isDisposed = true;
            }
        }

        /// <summary>
        /// Gets the quick information item asynchronous.
        /// </summary>
        /// <owner>Anton Patron</owner>
        /// <param name="session">The session.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="QuickInfoItem"/></returns>
        public async Task<QuickInfoItem> GetQuickInfoItemAsync(IAsyncQuickInfoSession session, CancellationToken cancellationToken)
        {
            SnapshotPoint? subjectTriggerPoint = session.GetTriggerPoint(this.m_subjectBuffer.CurrentSnapshot);
            if (!subjectTriggerPoint.HasValue)
                return await Task.FromResult(new QuickInfoItem(null, null)).ConfigureAwait(false);

            ITextSnapshot currentSnapshot = subjectTriggerPoint.Value.Snapshot;

            //look for occurrences of our QuickInfo words in the span
            ITextStructureNavigator navigator = this.m_provider.NavigatorService.GetTextStructureNavigator(this.m_subjectBuffer);
            TextExtent extent = navigator.GetExtentOfWord(subjectTriggerPoint.Value);
            string searchText = extent.Span.GetText();
            if (this.m_dictionary.TryGetValue(searchText, out string value))
            {

                var applicableToSpan = currentSnapshot.CreateTrackingSpan(extent.Span.Start, searchText.Length, SpanTrackingMode.EdgeInclusive);
                return await Task.FromResult(new QuickInfoItem(applicableToSpan, value)).ConfigureAwait(false);
            }
            else
                return await Task.FromResult(new QuickInfoItem(null, searchText)).ConfigureAwait(false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickInfoSource"/> class.
        /// </summary>
        /// <owner>Anton Patron</owner>
        /// <param name="values">The values.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="subjectBuffer">The subject buffer.</param>
        public QuickInfoSource(Dictionary<string, string> values, QuickInfoSourceProvider provider, ITextBuffer subjectBuffer)
        {
            this.m_provider = provider;
            this.m_subjectBuffer = subjectBuffer;

            // Indentifier names and their descriptions
            this.m_dictionary = values;
        }
    }
}