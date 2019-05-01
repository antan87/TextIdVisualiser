using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System.Collections.Generic;

namespace TextIdVisualiser
{
    internal class TestQuickInfoController : IIntellisenseController
    {
        private ITextView m_textView;
        private readonly IList<ITextBuffer> m_subjectBuffers;
        private readonly TestQuickInfoControllerProvider m_provider;
        private IAsyncQuickInfoSession m_session;

        internal TestQuickInfoController(ITextView textView, IList<ITextBuffer> subjectBuffers, TestQuickInfoControllerProvider provider)
        {
            this.m_textView = textView;
            this.m_subjectBuffers = subjectBuffers;
            this.m_provider = provider;
            this.m_textView.MouseHover += this.OnTextViewMouseHover;
        }

        private async void OnTextViewMouseHover(object sender, MouseHoverEventArgs e)
        {
            //find the mouse position by mapping down to the subject buffer
            SnapshotPoint? point = this.m_textView.BufferGraph.MapDownToFirstMatch(new SnapshotPoint(this.m_textView.TextSnapshot, e.Position), PointTrackingMode.Positive,
                snapshot => this.m_subjectBuffers.Contains(snapshot.TextBuffer), PositionAffinity.Predecessor);
            if (point != null)
            {
                ITrackingPoint triggerPoint = point.Value.Snapshot.CreateTrackingPoint(point.Value.Position,
                PointTrackingMode.Positive);

                if (!this.m_provider.QuickInfoBroker.IsQuickInfoActive(this.m_textView))
                    this.m_session = await this.m_provider.QuickInfoBroker.TriggerQuickInfoAsync(this.m_textView, triggerPoint);
            }
        }

        public void Detach(ITextView textView)
        {
            if (this.m_textView == textView)
            {
                this.m_textView.MouseHover -= this.OnTextViewMouseHover;
                this.m_textView = null;
            }
        }

        public void ConnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
        }

        public void DisconnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
        }
    }
}