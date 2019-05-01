using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TextIdVisualiser
{
    internal class TestQuickInfoSource : IAsyncQuickInfoSource
    {
        private readonly TestQuickInfoSourceProvider m_provider;
        private readonly ITextBuffer m_subjectBuffer;
        private readonly Dictionary<string, string> m_dictionary;
        private bool m_isDisposed;

        public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> qiContent, out ITrackingSpan applicableToSpan)
        {
            // Map the trigger point down to our buffer.
            SnapshotPoint? subjectTriggerPoint = session.GetTriggerPoint(this.m_subjectBuffer.CurrentSnapshot);
            if (!subjectTriggerPoint.HasValue)
            {
                applicableToSpan = null;
                return;
            }

            ITextSnapshot currentSnapshot = subjectTriggerPoint.Value.Snapshot;

            //look for occurrences of our QuickInfo words in the span
            ITextStructureNavigator navigator = this.m_provider.NavigatorService.GetTextStructureNavigator(this.m_subjectBuffer);
            TextExtent extent = navigator.GetExtentOfWord(subjectTriggerPoint.Value);
            string searchText = extent.Span.GetText();
            if (this.m_dictionary.TryGetValue(searchText, out string value))
            {
                applicableToSpan = currentSnapshot.CreateTrackingSpan(extent.Span.Start, searchText.Length, SpanTrackingMode.EdgeInclusive);
                qiContent.Add(value);
            }
            else
                applicableToSpan = null;
        }

        public void Dispose()
        {
            if (!this.m_isDisposed)
            {
                GC.SuppressFinalize(this);
                this.m_isDisposed = true;
            }
        }


        public Task<QuickInfoItem> GetQuickInfoItemAsync(IAsyncQuickInfoSession session, CancellationToken cancellationToken)
        {
            SnapshotPoint? subjectTriggerPoint = session.GetTriggerPoint(this.m_subjectBuffer.CurrentSnapshot);
            if (!subjectTriggerPoint.HasValue)
                return Task.FromResult(new QuickInfoItem(null, null));

            ITextSnapshot currentSnapshot = subjectTriggerPoint.Value.Snapshot;

            //look for occurrences of our QuickInfo words in the span
            ITextStructureNavigator navigator = this.m_provider.NavigatorService.GetTextStructureNavigator(this.m_subjectBuffer);
            TextExtent extent = navigator.GetExtentOfWord(subjectTriggerPoint.Value);
            string searchText = extent.Span.GetText();
            if (this.m_dictionary.TryGetValue(searchText, out string value))
            {

                var applicableToSpan = currentSnapshot.CreateTrackingSpan(extent.Span.Start, searchText.Length, SpanTrackingMode.EdgeInclusive);
                return Task.FromResult(new QuickInfoItem(applicableToSpan, value));
            }
            else
                return Task.FromResult(new QuickInfoItem(null, searchText));
        }

        public TestQuickInfoSource(Dictionary<string, string> values, TestQuickInfoSourceProvider provider, ITextBuffer subjectBuffer)
        {
            this.m_provider = provider;
            this.m_subjectBuffer = subjectBuffer;

            // Indentifier names and their descriptions
            this.m_dictionary = values;
        }
    }
}