﻿using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
using System;
using System.ComponentModel.Composition;

namespace TextIdVisualiser
{
    [Export(typeof(IAsyncQuickInfoSourceProvider))]
    [Name("ToolTip QuickInfo Source")]
    [Order(Before = "Default Quick Info Presenter")]
    [ContentType("text")]
    internal class TestQuickInfoSourceProvider : IAsyncQuickInfoSourceProvider
    {

        [Import]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        [Import]
        internal ITextBufferFactoryService TextBufferFactoryService { get; set; }

        //public IAsyncQuickInfoSource TryCreateQuickInfoSource(ITextBuffer textBuffer)
        //{
        //    return new TestQuickInfoSource(ResourceManagerHelper.TextValues, this, textBuffer);
        //}

        IAsyncQuickInfoSource IAsyncQuickInfoSourceProvider.TryCreateQuickInfoSource(ITextBuffer textBuffer)
        {
            return new TestQuickInfoSource(ResourceManagerHelper.TextValues, this, textBuffer);
        }
    }
}