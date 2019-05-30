using System.ComponentModel;

namespace TextIdVisualiser.Options
{
    internal class GeneralOptions : BaseOptionModel<GeneralOptions>
    {
        [Category("Settings")]
        [DisplayName("Language Id")]
        [Description("The language id to be used.")]
        public int LaunguageId { get; set; }

        [Category("Settings")]
        [DisplayName("Resource directory path")]
        [Description("The path to the directory where the resource file is located.")]
        public string OptionResourceDirectoryPath { get; set; }

        [Category("Settings")]
        [DisplayName("Resource file")]
        [Description("The resource file that should be loaded.")]
        public string OptionResourceFile { get; set; }
    }
}
