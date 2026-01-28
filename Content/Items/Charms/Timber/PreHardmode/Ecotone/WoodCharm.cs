using Charisma.Common.Players;
using Charisma.Content.Buffs;
using Charisma.Content.Items.Charms.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode.Ecotone
{
    public class WoodCharm : BaseCharm
    {
        public override int CharismaReward => 3;

        public override void ApplyCharmEffects(Player player)
        {
            player.statDefense += 2;

            if (player.ZoneForest)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }

            player.GetModPlayer<WoodCharmPlayer>().woodCharmEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WoodHelmet)
                .AddIngredient(ItemID.WoodBreastplate)
                .AddIngredient(ItemID.WoodGreaves)
                .AddIngredient(ItemID.WoodenSword)
                .AddIngredient(ItemID.Apple)
                .AddIngredient(ItemID.Bunny)
                .AddIngredient(ItemID.Mushroom, 3)
                .AddIngredient(ItemID.Acorn, 11)
                .AddIngredient(ItemID.Daybloom, 3)
                .AddIngredient(ItemID.Wood, 110)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Trees)
                .Register();
        }
    }

    public class WoodCharmPlayer : ModPlayer
    {
        public bool woodCharmEquipped;

        public override void ResetEffects()
        {
            woodCharmEquipped = false;
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (woodCharmEquipped && info.DamageSource.SourceNPCIndex >= 0)
            {
                int npcIndex = info.DamageSource.SourceNPCIndex;
                if (Main.npc.IndexInRange(npcIndex))
                {
                    Main.npc[npcIndex].AddBuff(ModContent.BuffType<SplintersDebuff>(), 300);
                }
            }
        }
    }
}