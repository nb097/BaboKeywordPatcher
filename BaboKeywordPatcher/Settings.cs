using System.Collections.Generic;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis.Settings;

namespace BaboKeywordPatcher
{
    public class Settings
    {
        [SynthesisSettingName(
            "Mark vanilla outfits as pretty (not erotic)"
        )]
        [SynthesisDescription(
            "If you have a vanilla outfit replacer that 'prettifies' vanilla outfits"
        )]
        public bool ArmorPrettyDefault { get; set; } = false;

        [SynthesisSettingName(
            "Mark vanilla outfits as erotic (not pretty)"
        )]
        [SynthesisDescription(
            "If you have a vanilla outfit replacer that eroticizes vanilla outfits"
        )]
        public bool ArmorEroticDefault { get; set; } = false;

        [SynthesisSettingName(
            "Mark dress outfits as erotic"
        )]
        [SynthesisDescription(
            "Any outfit that has the word 'dress' in its name will be marked erotic"
        )]
        public bool EroticDresses { get; set; } = false;

        [SynthesisSettingName(
            "Remove SLA_ArmorHalfNakedBikini for outfits that have SOS_Revealing"
        )]
        [SynthesisDescription(
            "Check this if you are using a mod that assumes SLA_ArmorHalfNakedBikini is not naked"
        )]
        public bool NoBikiniForBra { get; set; } = false;

        [SynthesisSettingName("Blacklist items")]
        public List<IFormLinkGetter<IItemGetter>> ItemBlacklist { get; set; } = new();

        [SynthesisSettingName("Blacklist keywords")]
        public List<IFormLinkGetter<IKeywordGetter>> KywdBlacklist { get; set; } = new();
    }
}
