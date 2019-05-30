using System.Collections.Generic;
using System.Threading.Tasks;
using TextIdVisualiser.Elements;

namespace TextIdVisualiser.Translators.Interfaces
{
    internal interface ITranslatorService
    {
        Task<IEnumerable<TooltipElement>> GetTooltipElementsAsync(string text);
        IEnumerable<ITranslatorProvider> Translators { get; set; }
    }
}
