using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Operations;
using TextIdVisualiser.Elements;
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

        private static ContainerElement ComposeContainerElement(IEnumerable<TooltipElement> elements)
        {
            if (!elements.Any())
                return null;

            var element = elements.First();
            return new ContainerElement(element.Style, element.Value);
        }

        public void Dispose()
        {
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
                return null;

            ITextSnapshot currentSnapshot = subjectTriggerPoint.Value.Snapshot;
            ITextStructureNavigator navigator = this.m_provider.NavigatorService.GetTextStructureNavigator(this.m_subjectBuffer);
            TextExtent extent = navigator.GetExtentOfWord(subjectTriggerPoint.Value);
            string searchText = extent.Span.GetText();
            var elements = await this.m_provider.TranslatorService.GetTooltipElementsAsync(searchText).ConfigureAwait(false);
            if (!elements.Any())
                return null;

            var applicableToSpan = currentSnapshot.CreateTrackingSpan(extent.Span.Start, searchText.Length, SpanTrackingMode.EdgeInclusive);
            var element = QuickInfoSource.ComposeContainerElement(elements);

            return new QuickInfoItem(applicableToSpan, element);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickInfoSource"/> class.
        /// </summary>
        /// <owner>Anton Patron</owner>
        /// <param name="values">The values.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="subjectBuffer">The subject buffer.</param>
        public QuickInfoSource(QuickInfoSourceProvider provider, ITextBuffer subjectBuffer)
        {
            this.m_provider = provider;
            this.m_subjectBuffer = subjectBuffer;
        }
    }
}