using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader.Config;
using Charisma.Common.Configs;
using Charisma.Common.Players;
using System.Reflection;

namespace Charisma.Common.Systems
{
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind ToggleOverlayKeybind { get; private set; }
        public static ModKeybind CycleCharmSlotsKeybind { get; private set; }

        public override void Load()
        {
            ToggleOverlayKeybind = KeybindLoader.RegisterKeybind(Mod, "Toggle Charisma Overlay", "L");
            CycleCharmSlotsKeybind = KeybindLoader.RegisterKeybind(Mod, "Cycle Charm Slots", "K");
        }

        public override void Unload()
        {
            ToggleOverlayKeybind = null;
            CycleCharmSlotsKeybind = null;
        }
    }

    public class KeybindPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybindSystem.ToggleOverlayKeybind.JustPressed)
            {
                var config = ModContent.GetInstance<CharismaConfig>();

                config.ShowCharismaOverlay = !config.ShowCharismaOverlay;

                typeof(ConfigManager)
                    .GetMethod("Save", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    ?.Invoke(null, new object[] { config });

                if (config.ShowCharismaOverlay) SoundEngine.PlaySound(SoundID.MenuOpen);
                else SoundEngine.PlaySound(SoundID.MenuClose);
            }

            if (KeybindSystem.CycleCharmSlotsKeybind.JustPressed)
            {
                var modPlayer = Player.GetModPlayer<CharismaPlayer>();

                int maxSlots = modPlayer.UnlockedCharmSlots;

                modPlayer.VisualCharmSlotLimit++;

                if (modPlayer.VisualCharmSlotLimit > maxSlots)
                {
                    modPlayer.VisualCharmSlotLimit = 0;
                    Main.NewText("Charm Slots: Hidden", 175, 75, 255);
                }
                else
                {
                    Main.NewText($"Charm Slots: Showing {modPlayer.VisualCharmSlotLimit}", 175, 75, 255);
                }

                SoundEngine.PlaySound(SoundID.MenuTick);
            }
        }
    }
}