using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text.Adornments;
using TextIdVisualiser.Elements;
using TextIdVisualiser.Options;
using TextIdVisualiser.Translators.Interfaces;

namespace TextIdVisualiser.Translators
{
    [Export(typeof(ITranslatorProvider))]
    internal sealed class TranslatorProviderTextId : ITranslatorProvider
    {
        public void Dispose()
        {
        }

        /// <summary>
        /// Returns a sequence of text cluster id values for texts that match the search words.
        /// </summary>
        /// <owner>Anton Patron</owner>
        /// <param name="language">The <see cref="CultureInfo"/> specifying the language to search.</param>
        /// <param name="includeParentCultures">The value indicating if the result should include the parent languages.</param>
        /// <returns>A sequence of text cluster id values for texts that match the search words.</returns>
        private static List<(int textId, string text)> GetTextvalues(ResourceManager resourceManager, CultureInfo language, bool includeParentCultures)
        {
            //
            // Include parent cultures if specified.
            //
            List<(int textId, string text)> result = new List<(int textId, string text)>();
            if (includeParentCultures && language.Parent != null && !language.IsNeutralCulture)
                //
                // Call this method recursively to access text clusters registered on parent cultures.
                //
                result.AddRange(TranslatorProviderTextId.GetTextvalues(resourceManager, language.Parent, true));

            //
            // Get all the text clusters registered on this culture.
            //
            using (ResourceSet resources = resourceManager.GetResourceSet(language, true, false))
            {
                if (resources == null)
                    return result;

                //
                // Find matches to the search words.
                //
                foreach (DictionaryEntry item in resources)
                {
                    string text = item.Value as string;
                    if (!(item.Key is string id) || text == null)
                        continue;

                    //
                    // remove hotkey character.
                    //
                    text = text.Replace("&", string.Empty).Trim();
                    result.Add((Convert.ToInt32(id.Replace("ID", string.Empty)), text));
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <owner>Anton Patron</owner>
        /// <returns>The text values.</returns>
        private async Task<Dictionary<string, string>> GetResourceFileValuesAsync()
        {
            var optionsInstance = await GeneralOptions.GetLiveInstanceAsync().ConfigureAwait(false);
            var manager = ResourceManager.CreateFileBasedResourceManager(optionsInstance.OptionResourceFile, optionsInstance.OptionResourceDirectoryPath, null);
            return TranslatorProviderTextId.GetTextvalues(manager, new CultureInfo(optionsInstance.LaunguageId), true).
                   GroupBy(textItem => textItem.textId).
                   ToDictionary(textItem => textItem.Key.ToString(), textItem => textItem.First().text);
        }

        public Task<(bool, TooltipElement)> GetTooltipElementAsync(string text)
        {
            if (this.TextValues.TryGetValue(text, out string value))
                return Task.FromResult((true, new TooltipElement(string.Empty, value, ContainerElementStyle.Wrapped)));

            return Task.FromResult<(bool, TooltipElement)>((false, null));

        }

        /// <summary>
        /// Gets the text values.
        /// </summary>
        /// <owner>Anton Patron</owner>
        /// <value>The text values.</value>
        private Dictionary<string, string> TextValues { get; }

        public TranslatorProviderTextId()
        {
            this.TextValues = Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.Run(this.GetResourceFileValuesAsync);
        }
    }
}
