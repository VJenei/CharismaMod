using Charisma.Common.Players;
using Charisma.Common.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Base
{
    public abstract class BaseCharm : ModItem
    {
        public virtual int CharismaReward => 5;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(gold: 1);

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item29;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)
        {
            CharismaWorldSystem.TryUnlockCharm(Type, CharismaReward);
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return !CharismaWorldSystem.FoundCharms.Contains(Type);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ApplyCharmEffects(player);
        }

        public virtual void ApplyCharmEffects(Player player) { }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (!CharismaWorldSystem.FoundCharms.Contains(Type))
            {
                var line = new TooltipLine(Mod, "CharismaReward", $"[c/FFD700:Permanently increases charisma by {CharismaReward}]");
                tooltips.Add(line);
            }
        }
    }
}