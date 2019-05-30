using System;
using System.Threading.Tasks;
using TextIdVisualiser.Elements;

namespace TextIdVisualiser.Translators.Interfaces
{
    internal interface ITranslatorProvider : IDisposable
    {
        Task<(bool, TooltipElement)> GetTooltipElementAsync(string text);

    }
}
