using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace TextIdVisualiser
{
    public class ResourceManagerHelper
    {

        /// <summary>
        /// Holds the list of text values.
        /// </summary>
        /// <owner>Anton Patron</owner>
        private static Dictionary<string, string> textValues;

        private static Dictionary<string, string> GetValues()
        {
            var list = ResourceManagerHelper.GetTextvalues(ResourceManager.CreateFileBasedResourceManager(Settings.Default.Filename, Settings.Default.Directory_Path, null), new CultureInfo(Settings.Default.Launguage_Id), true);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var textItem in list)
                dic[textItem.textId.ToString()] = textItem.text;

            return dic;
        }

        /// <summary>
        /// Returns a sequence of text cluster id values for texts that match the search words.
        /// </summary>
        /// <owner>Ragnar Roos</owner>
        /// <param name="language">The <see cref="CultureInfo"/> specifying the language to search.</param>
        /// <param name="includeParentCultures">The value indicating if the result should include the parent languages.</param>
        /// <returns>A sequence of text cluster id values for texts that match the search words.</returns>
        private static IEnumerable<(int textId, string text)> GetTextvalues(ResourceManager resourceManager, CultureInfo language, bool includeParentCultures)
        {
            //
            // Include parent cultures if specified.
            //
            List<(int textId, string text)> result = new List<(int textId, string text)>();
            if (includeParentCultures && language.Parent != null && !language.IsNeutralCulture)
                //
                // Call this method recursively to access text clusters registered on parent cultures.
                //
                result.AddRange(ResourceManagerHelper.GetTextvalues(resourceManager, language.Parent, true));

            //
            // Get all the text clusters registered on this culture.
            //
            ResourceSet resources = resourceManager.GetResourceSet(language, true, false);
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
                // Replace delimiters with space.
                //
                text = text.Replace(".", " ");
                text = text.Replace(",", " ");
                text = text.Replace(":", " ");
                text = text.Replace(";", " ");
                text = text.Replace("'", " ");
                text = text.Replace("-", " ");

                //
                // remove hotkey character.
                //
                text = text.Replace("&", string.Empty).Trim();
                result.Add((Convert.ToInt32(id.Replace("ID", string.Empty)), text));
            }

            return result.Distinct();
        }

        public static Dictionary<string, string> TextValues
        {
            get
            {
                if (ResourceManagerHelper.textValues == null)
                    ResourceManagerHelper.textValues = GetValues();

                return ResourceManagerHelper.textValues;
            }
        }
    }
}
