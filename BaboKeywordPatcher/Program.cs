using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Aspects;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using Noggog;
using System.Linq;

namespace BaboKeywordPatcher
{
    public class Program
    {
        static Lazy<Settings> _settings = null!;
        public static Settings Settings => _settings.Value;

        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetAutogeneratedSettings(
                    nickname: "Settings",
                    path: "settings.json",
                    out _settings)
                .SetTypicalOpen(GameRelease.SkyrimSE, "BaboKeywords.esp")
                .Run(args);
        }

        public static IKeywordGetter? SLA_ArmorBondage;

        public static IKeywordGetter? EroticArmor;

        public static IKeywordGetter? SLA_ArmorRubber;

        public static IKeywordGetter? SLA_ArmorHarness;

        public static IKeywordGetter? SLA_ArmorSpendex;

        public static IKeywordGetter? SLA_ArmorTransparent;

        public static IKeywordGetter? SLA_BootsHeels;
        public static IKeywordGetter? SLA_KillerHeels;

        public static IKeywordGetter? SLA_VaginalBeads;
        public static IKeywordGetter? SLA_VaginalDildo;

        public static IKeywordGetter? SLA_AnalPlugTail;
        public static IKeywordGetter? SLA_AnalBeads;
        public static IKeywordGetter? SLA_AnalPlug;

        public static IKeywordGetter? SLA_PiercingClit;

        public static IKeywordGetter? SLA_PiercingNipple;

        public static IKeywordGetter? SLA_BraArmor;
        public static IKeywordGetter? SLA_ArmorCurtain;

        public static IKeywordGetter? SLA_ArmorHalfNakedBikini;

        public static IKeywordGetter? SLA_MicroHotpants;
        public static IKeywordGetter? SLA_PantiesNormal;
        public static IKeywordGetter? SLA_PastiesCrotch;
        public static IKeywordGetter? SLA_ThongCString;
        public static IKeywordGetter? SLA_ThongGstring;
        public static IKeywordGetter? SLA_ThongLowleg;
        public static IKeywordGetter? SLA_ThongT;

        public static IKeywordGetter? SLA_HasStockings;
        public static IKeywordGetter? SLA_HasLeggings;

        public static IKeywordGetter? SLA_MiniSkirt;
        public static IKeywordGetter? SLA_PelvicCurtain;

        public static IKeywordGetter? SLA_ArmorPretty;

        public static IKeywordGetter? SOS_Revealing;

        public static IKeywordGetter LoadKeyword(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, String kwd)
        {
            state.LinkCache.TryResolve<IKeywordGetter>(kwd, out var ReturnKwd);
            if (ReturnKwd == null)
            {
                throw new Exception("Failed to load keyword " + kwd);
            }
            return ReturnKwd;
        }

        public static void LoadKeywords(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            SLA_ArmorBondage = LoadKeyword(state, "SLA_ArmorBondage");

            EroticArmor = LoadKeyword(state, "EroticArmor");

            SLA_ArmorRubber = LoadKeyword(state, "SLA_ArmorRubber");

            SLA_ArmorHarness = LoadKeyword(state, "SLA_ArmorHarness");

            try // SLAX and SLA Babo spell this keyword differently. Check for both.
            {
                SLA_ArmorSpendex = LoadKeyword(state, "SLA_ArmorSpendex");
            }
            catch
            {
                SLA_ArmorSpendex = LoadKeyword(state, "SLA_ArmorSpandex");
            }

            SLA_ArmorTransparent = LoadKeyword(state, "SLA_ArmorTransparent");

            SLA_BootsHeels = LoadKeyword(state, "SLA_BootsHeels");
            SLA_KillerHeels = LoadKeyword(state, "SLA_KillerHeels");

            SLA_VaginalBeads = LoadKeyword(state, "SLA_VaginalBeads");
            SLA_VaginalDildo = LoadKeyword(state, "SLA_VaginalDildo");

            SLA_AnalPlugTail = LoadKeyword(state, "SLA_AnalPlugTail");
            SLA_AnalBeads = LoadKeyword(state, "SLA_AnalPlugBeads");
            SLA_AnalPlug = LoadKeyword(state, "SLA_AnalPlug");

            SLA_PiercingClit = LoadKeyword(state, "SLA_PiercingClit");

            SLA_PiercingNipple = LoadKeyword(state, "SLA_PiercingNipple");

            try
            {
                SLA_BraArmor = LoadKeyword(state, "SLA_BraArmor");
            }
            catch
            {
                SLA_BraArmor = LoadKeyword(state, "SLA_Brabikini");
            }
            SLA_ArmorCurtain = LoadKeyword(state, "SLA_ArmorCurtain");

            SLA_ArmorHalfNakedBikini = LoadKeyword(state, "SLA_ArmorHalfNakedBikini");

            SLA_MicroHotpants = LoadKeyword(state, "SLA_MicroHotpants");
            SLA_PantiesNormal = LoadKeyword(state, "SLA_PantiesNormal");
            SLA_PastiesCrotch = LoadKeyword(state, "SLA_PastiesCrotch");
            SLA_ThongCString = LoadKeyword(state, "SLA_ThongCString");
            SLA_ThongGstring = LoadKeyword(state, "SLA_ThongGstring");
            SLA_ThongLowleg = LoadKeyword(state, "SLA_ThongLowleg");
            SLA_ThongT = LoadKeyword(state, "SLA_ThongT");

            SLA_HasStockings = LoadKeyword(state, "SLA_HasStockings");
            SLA_HasLeggings = LoadKeyword(state, "SLA_HasLeggings");

            SLA_MiniSkirt = LoadKeyword(state, "SLA_MiniSkirt");
            SLA_PelvicCurtain = LoadKeyword(state, "SLA_PelvicCurtain");

            SLA_ArmorPretty = LoadKeyword(state, "SLA_ArmorPretty");

            SOS_Revealing = LoadKeyword(state, "SOS_Revealing");
        }

        public static bool StrMatch(String name, String comparator, bool isPart)
        {
            bool matched;

            if(isPart)
            {
                matched = (name.IndexOf(comparator, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            else
            {
                matched = Regex.IsMatch(name, (@"\b" + comparator + @"\b"), RegexOptions.IgnoreCase);
            }

            return matched;
        }

        public static bool StrMatchCS(String name, String comparator)
        {
            return (name.IndexOf(comparator) >= 0);
        }

        public static bool IsDeviousRenderedItem(String name)
        {
            return (StrMatch(name, "scriptinstance", false) || StrMatch(name, "rendered", false));
        }

        private static bool HasTag(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, Armor armorEditObj, IKeywordGetter tag)
        {
            if (armorEditObj.Keywords != null)
            {
                foreach (var kywd in armorEditObj.Keywords)
                {
                    kywd.TryResolve(state.LinkCache, out var kywdFormKey);
                    // We do a string comparison here because mods can include the same keyword name but for a different form ID, which means contains() won't work
                    if (kywdFormKey != null && kywdFormKey.EditorID != null && tag.EditorID != null && kywdFormKey.EditorID.ToUpper().Equals(tag.EditorID.ToUpper()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static void AddTag(Armor armorEditObj, IKeywordGetter tag)
        {
            Console.WriteLine("Adding keyword " + tag.ToString() + " to armor " + armorEditObj.Name);
            if (armorEditObj.Keywords != null && !armorEditObj.Keywords.Contains(tag))
            {
                armorEditObj.Keywords!.Add(tag);
            }
        }

        private static void RemoveTag(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, Armor armorEditObj, IKeywordGetter tag)
        {
            Console.WriteLine("Removing keyword " + tag.ToString() + " on armor " + armorEditObj.Name);
            if (armorEditObj.Keywords != null)
            {
                // Have to bend over backwards here to create new form keys based on the found keywords (because loss of scope?) and can't remove while iterating
                FormKey kywdForm = new FormKey();
                foreach (var kywd in armorEditObj.Keywords)
                {
                    kywd.TryResolve(state.LinkCache, out var kywdFormKey);
                    if (kywdFormKey != null && kywdFormKey.EditorID != null && tag.EditorID != null && kywdFormKey.EditorID.ToUpper().Equals(tag.EditorID.ToUpper()))
                    {
                        kywdForm = new FormKey(new ModKey(kywd.FormKey.ModKey.Name, kywd.FormKey.ModKey.Type), kywd.FormKey.ID);
                    }
                }
                armorEditObj.Keywords!.Remove(kywdForm);
            }
        }

        // Keywords are static / nullabe, but are initialized on runtime. Ignore warning.
#pragma warning disable CS8604 // Possible null reference argument.
        public static void ParseName(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, IArmorGetter armor, String name, bool isPart)
        {
            bool patched = false;

            var armorEditObj = armor.DeepCopy();

            if (armorEditObj == null)
            {
                Console.WriteLine("Armor is null for " + name);
                return;
            }

            // SLA_ArmorBondage
            if (StrMatch(name, "harness", isPart)
                    || StrMatch(name, "corset", isPart)
                    || StrMatch(name, "StraitJacket", isPart)
                    || StrMatch(name, "hobble", isPart)
                    || StrMatch(name, "tentacles", isPart)
                    || StrMatch(name, "slave", isPart)
                    || StrMatch(name, "chastity", isPart)
                    || StrMatch(name, "cuff", isPart)
                    || StrMatch(name, "binder", isPart)
                    || StrMatch(name, "yoke", isPart)
                    || StrMatch(name, "mitten", isPart)
                )
            {
                if (SLA_ArmorBondage != null && !HasTag(state, armorEditObj, SLA_ArmorBondage))
                {
                    patched = true;
                    AddTag(armorEditObj, SLA_ArmorBondage);
                }
            }
            
            // EroticArmor
            if (StrMatch(name, "suit", isPart)
                || StrMatch(name, "latex", isPart)
                || StrMatch(name, "rubber", isPart)
                || StrMatch(name, "ebonite", isPart)
                || StrMatch(name, "slut", isPart)
                || StrMatch(name, "lingerie", isPart)
                || (StrMatch(name, "dress", isPart) && Settings.EroticDresses)
                )
            {
                if (EroticArmor != null && !HasTag(state, armorEditObj, EroticArmor))
                {
                    patched = true;
                    AddTag(armorEditObj, EroticArmor);
                }
            }
            
            // SLA_ArmorRubber
            if (StrMatch(name, "rubber", isPart))
            {
                if (SLA_ArmorRubber != null && !HasTag(state, armorEditObj, SLA_ArmorRubber))
                {
                    patched = true;
                    AddTag(armorEditObj, SLA_ArmorRubber);
                }
            }
            
            // SLA_ArmorHarness
            if (StrMatch(name, "harness", isPart))
            {
                if (SLA_ArmorRubber != null && !HasTag(state, armorEditObj, SLA_ArmorRubber))
                {
                    patched = true;
                    AddTag(armorEditObj, SLA_ArmorHarness);
                }
            }
            
            // SLA_ArmorSpendex
            if (StrMatch(name, "suit", isPart)
                || StrMatch(name, "spandex", isPart)
                || StrMatch(name, "spendex", isPart)
                || StrMatch(name, "ebonite", isPart))
            {
                if (SLA_ArmorSpendex != null && !HasTag(state, armorEditObj, SLA_ArmorSpendex))
                {
                    patched = true;
                    AddTag(armorEditObj, SLA_ArmorSpendex);
                }
            }
            
            // SLA_ArmorTransparent
            if (StrMatch(name, "transparent", isPart)
                || StrMatchCS(name, "TR"))
            {
                if (SLA_ArmorTransparent != null && !HasTag(state, armorEditObj, SLA_ArmorTransparent))
                {
                    patched = true;
                    AddTag(armorEditObj, SLA_ArmorTransparent);
                }
            }
            
            // SLA_BootsHeels
            // Don't apply if already has heels type keyword
            IBodyTemplateGetter? bodyTemplate = armor.BodyTemplate;
            if ((IsDeviousRenderedItem(name) && StrMatch(name, "boots", isPart))
                || (StrMatch(name, "heels", isPart) && !StrMatch(name, "wheel", isPart) && bodyTemplate != null && bodyTemplate.FirstPersonFlags.HasFlag(BipedObjectFlag.Feet)))
            {
                if ((SLA_BootsHeels != null && !HasTag(state, armorEditObj, SLA_BootsHeels))
                    && (SLA_KillerHeels != null && !HasTag(state, armorEditObj, SLA_KillerHeels)))
                {
                    patched = true;
                    AddTag(armorEditObj, SLA_BootsHeels);
                }
            }
            
            // SLA_VaginalDildo
            if ((StrMatch(name, "plug", isPart) && StrMatch(name, "vag", isPart))
                || StrMatch(name, "vaginal", isPart)
                || StrMatch(name, "vibrator", isPart))
            {
                if (StrMatch(name, "beads", isPart))
                {
                    if (SLA_VaginalBeads != null && !HasTag(state, armorEditObj, SLA_VaginalBeads))
                    {
                        patched = true;
                        AddTag(armorEditObj, SLA_VaginalBeads);
                    }
                }
                else
                {
                    if (SLA_VaginalBeads != null && !HasTag(state, armorEditObj, SLA_VaginalBeads))
                    {
                        patched = true;
                        AddTag(armorEditObj, SLA_VaginalDildo);
                    }
                }
            }
            
            // SLA_AnalPlug
            if (StrMatch(name, "anal", isPart)
                || StrMatch(name, "buttplug", isPart)
                || StrMatch(name, "vibrator", isPart))
            {
                if (StrMatch(name, "tail", isPart))
                {
                    if (SLA_AnalPlugTail != null && !HasTag(state, armorEditObj, SLA_AnalPlugTail))
                    {
                        patched = true;
                        AddTag(armorEditObj, SLA_AnalPlugTail);
                    }
                }
                else if (StrMatch(name, "beads", isPart))
                {
                    if (SLA_AnalBeads != null && !HasTag(state, armorEditObj, SLA_AnalBeads))
                    {
                        patched = true;
                        AddTag(armorEditObj, SLA_AnalBeads);
                    }
                }
                else
                {
                    if (SLA_AnalPlug != null && !HasTag(state, armorEditObj, SLA_AnalPlug))
                    {
                        patched = true;
                        AddTag(armorEditObj, SLA_AnalPlug);
                    }
                }
            }
            
            // SLA_PiercingClit
            if (StrMatch(name, "piercingv", isPart)
                || StrMatch(name, "vpiercing", isPart))
            {
                if (SLA_PiercingClit != null && !HasTag(state, armorEditObj, SLA_PiercingClit))
                {
                    patched = true;
                    AddTag(armorEditObj, SLA_PiercingClit);
                }
            }
            
            // SLA_PiercingNipple
            if (StrMatch(name, "piercingn", isPart)
                || StrMatch(name, "npiercing", isPart))
            {
                if (SLA_PiercingNipple != null && !HasTag(state, armorEditObj, SLA_PiercingNipple))
                {
                    patched = true;
                    AddTag(armorEditObj, SLA_PiercingNipple);
                }
            }
            
            // SLA_BraArmor
            // Don't apply if already has top armor type keyword
            if (!StrMatch(name, "bracer", isPart) && !StrMatch(name, "brawn", isPart) && (StrMatch(name, "bra", isPart) || StrMatch(name, "bikini top", isPart) || (StrMatch(name, "undergarment", isPart) && StrMatch(name, "upper", isPart))))
            {
                if ((SLA_BraArmor != null && !HasTag(state, armorEditObj, SLA_BraArmor))
                    && (SLA_ArmorCurtain != null && !HasTag(state, armorEditObj, SLA_ArmorCurtain)))
                {
                    patched = true;
                    AddTag(armorEditObj, SLA_BraArmor);
                }
            }
            
            // SLA_ArmorHalfNakedBikini
            if (StrMatch(name, "bikini", isPart) && !(Settings.NoBikiniForBra && HasTag(state, armorEditObj, SOS_Revealing)))
            {
                if (SLA_ArmorHalfNakedBikini != null && !HasTag(state, armorEditObj, SLA_ArmorHalfNakedBikini))
                {
                    patched = true;
                    AddTag(armorEditObj, SLA_ArmorHalfNakedBikini);
                }
            }
            
            // SLA_ThongT
            // Don't apply if already has a panties type keyword
            if (StrMatch(name, "thong", isPart)
                || StrMatch(name, "bottom", isPart))
            {
                if ((SLA_MicroHotpants != null && !HasTag(state, armorEditObj, SLA_MicroHotpants))
                    && (SLA_PantiesNormal != null && !HasTag(state, armorEditObj, SLA_PantiesNormal))
                    && (SLA_PastiesCrotch != null && !HasTag(state, armorEditObj, SLA_PastiesCrotch))
                    && (SLA_ThongCString != null && !HasTag(state, armorEditObj, SLA_ThongCString))
                    && (SLA_ThongGstring != null && !HasTag(state, armorEditObj, SLA_ThongGstring))
                    && (SLA_ThongLowleg != null && !HasTag(state, armorEditObj, SLA_ThongLowleg))
                    && (SLA_ThongT != null && !HasTag(state, armorEditObj, SLA_ThongT)))
                {
                    patched = true;
                    AddTag(armorEditObj, SLA_ThongT);
                }
            }
            
            // SLA_PantiesNormal
            // Don't apply if already has a panties type keyword
            if (StrMatch(name, "panties", isPart)
                || StrMatch(name, "panty", isPart)
                || StrMatch(name, "underwear", isPart)
                || StrMatch(name, "binkini bot", isPart)
                || (StrMatch(name, "undergarment", isPart)  && StrMatch(name, "lower", isPart)))
            {
                if ((SLA_MicroHotpants != null && !HasTag(state, armorEditObj, SLA_MicroHotpants))
                    && (SLA_PantiesNormal != null && !HasTag(state, armorEditObj, SLA_PantiesNormal))
                    && (SLA_PastiesCrotch != null && !HasTag(state, armorEditObj, SLA_PastiesCrotch))
                    && (SLA_ThongCString != null && !HasTag(state, armorEditObj, SLA_ThongCString))
                    && (SLA_ThongGstring != null && !HasTag(state, armorEditObj, SLA_ThongGstring))
                    && (SLA_ThongLowleg != null && !HasTag(state, armorEditObj, SLA_ThongLowleg))
                    && (SLA_ThongT != null && !HasTag(state, armorEditObj, SLA_ThongT)))
                {
                    patched = true;
                    AddTag(armorEditObj, SLA_PantiesNormal);
                }
            }
            
            // SLA_HasStockings
            // Don't apply if already has a stockings/leggings keyword
            if (StrMatch(name, "stockings", isPart))
            {
                if ((SLA_HasStockings != null && !HasTag(state, armorEditObj, SLA_HasStockings))
                    && (SLA_HasLeggings != null && !HasTag(state, armorEditObj, SLA_HasLeggings)))
                {
                    patched = true;
                    AddTag(armorEditObj, SLA_HasStockings);
                }
            }
            
            // SLA_HasLeggings
            // Don't apply if already has a stockings/leggings keyword
            if (StrMatch(name, "leggings", isPart))
            {
                if ((SLA_HasStockings != null && !HasTag(state, armorEditObj, SLA_HasStockings))
                    && (SLA_HasLeggings != null && !HasTag(state, armorEditObj, SLA_HasLeggings)))
                {
                    patched = true;
                    AddTag(armorEditObj, SLA_HasLeggings);
                }
            }
            
            // SLA_MiniSkirt
            // Don't apply if already has skirt type keyword
            if (StrMatch(name, "skirt", isPart))
            {
                if ((SLA_MiniSkirt != null && !HasTag(state, armorEditObj, SLA_MiniSkirt))
                    && (SLA_PelvicCurtain != null && !HasTag(state, armorEditObj, SLA_PelvicCurtain)))
                {
                    patched = true;
                    AddTag(armorEditObj, SLA_MiniSkirt);
                }
            }
            
            // Pretty/Erotic - All vanilla armors
            // I use a skimpy armor replacer (But not to the level of bikini). Having ArmorPretty on all armors is appropriate.
            if (Settings.ArmorPrettyDefault && !patched && (StrMatch(name, "armor", true) || StrMatch(name, "cuiras", true) || StrMatch(name, "robes", true)))
            {
                if (SLA_ArmorPretty != null && !HasTag(state, armorEditObj, SLA_ArmorPretty))
                {
                    patched = true;
                    AddTag(armorEditObj, SLA_ArmorPretty);
                }
            }
            else if (Settings.ArmorEroticDefault && !patched && (StrMatch(name, "armor", true) || StrMatch(name, "cuiras", true) || StrMatch(name, "robes", true)))
            {
                if (EroticArmor != null && !HasTag(state, armorEditObj, EroticArmor))
                {
                    patched = true;
                    AddTag(armorEditObj, EroticArmor);
                }
            }
            
            // If set, remove SLA_ArmorHalfNakedBikini on armors tagged SOS_Revealing
            // Had to switch to strings because mods like having their own copy of SOS_Revealing (to avoid more masters) which is a problem for this purpose
            if (Settings.NoBikiniForBra && HasTag(state, armorEditObj, SOS_Revealing) && HasTag(state, armorEditObj, SLA_ArmorHalfNakedBikini))
            {
                patched = true;
                RemoveTag(state, armorEditObj, SLA_ArmorHalfNakedBikini);
            }
            
            // Armor was patched, add to state
            if (patched)
            {
                // Console.WriteLine("Matched: " + name);
                state.PatchMod.Armors.Set(armorEditObj);
            }
        }
#pragma warning restore CS8604 // Possible null reference argument.

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            LoadKeywords(state);
            // state.ExtraSettingsDataPath.
            foreach (var armorGetter in state.LoadOrder.PriorityOrder.WinningOverrides<IArmorGetter>())
            {
                try
                {
                    // skip blacklisted items
                    if (Settings.ItemBlacklist.Contains(armorGetter))
                        continue;

                    // check if item has keywords
                    if (armorGetter is not IKeywordedGetter itemKeywords || itemKeywords.Keywords == null)
                        continue;

                    // skip if item has blacklisted keywords
                    if (itemKeywords.Keywords.Any(e => Settings.KywdBlacklist.Contains(e)))
                        continue;

                    // skip armor with non-default race
                    if (armorGetter.Race != null)
                    {
                        armorGetter.Race.TryResolve<IRaceGetter>(state.LinkCache, out var race);
                        if (race != null && race.EditorID != "DefaultRace") continue;
                    }

                    // skip armor that is non-playable or a shield
                    if (armorGetter.MajorFlags.HasFlag(Armor.MajorFlag.NonPlayable)) continue;
                    if (armorGetter.MajorFlags.HasFlag(Armor.MajorFlag.Shield)) continue;

                    // skip armor that is head, hair, circlet, rings, or amulets
                    if (armorGetter.BodyTemplate != null)
                    {
                        if (armorGetter.BodyTemplate.FirstPersonFlags.HasFlag(BipedObjectFlag.Head)) continue;
                        if (armorGetter.BodyTemplate.FirstPersonFlags.HasFlag(BipedObjectFlag.Hair)) continue;
                        if (armorGetter.BodyTemplate.FirstPersonFlags.HasFlag(BipedObjectFlag.Circlet)) continue;
                        if (armorGetter.BodyTemplate.FirstPersonFlags.HasFlag(BipedObjectFlag.Ring)) continue;
                        if (armorGetter.BodyTemplate.FirstPersonFlags.HasFlag(BipedObjectFlag.Amulet)) continue;
                    }

                    // skip items with no name
                    if (armorGetter.Name == null) continue;

                    string? v = armorGetter.Name.ToString();

                    // skip items with empty name
                    if (v == null || v.Length == 0) continue;

                    ParseName(state, armorGetter, v, false);
                }
                // MoreNastyCritters breaks the patching process. Ignore it.
                catch (Exception e)
                {
                    Console.WriteLine("Caught exception: " + e);
                }
            }
            Console.WriteLine("Done.");
        }
    }
}
