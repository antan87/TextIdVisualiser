using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using TextIdVisualiser.Elements;
using TextIdVisualiser.Translators.Interfaces;

namespace TextIdVisualiser.Translators
{
    [Export(typeof(ITranslatorService))]
    internal class TranslatorService : ITranslatorService
    {
        [ImportMany(typeof(ITranslatorProvider))]
        public IEnumerable<ITranslatorProvider> Translators { get; set; }

        public async Task<IEnumerable<TooltipElement>> GetTooltipElementsAsync(string text)
        {
            IEnumerable<Task<(bool, TooltipElement)>> tasks = this.Translators.Select(item => item.GetTooltipElementAsync(text));
            var tooltipElements = await Task.WhenAll<(bool, TooltipElement)>(tasks).ConfigureAwait(false);

            return tooltipElements.Where(item => item.Item1).Select(item => item.Item2);
        }
    }
}
