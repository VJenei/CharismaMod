using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode.Ecotone
{
    public class EcotoneTotem : BaseCharm
    {
        public override int CharismaReward => 7;

        public override void ApplyCharmEffects(Player player)
        {
            player.statDefense += 2;
            player.GetModPlayer<WoodCharmPlayer>().woodCharmEquipped = true;

            player.GetCritChance(DamageClass.Generic) += 3f;
            player.GetModPlayer<BorealCharmPlayer>().borealCharmEquipped = true;

            player.moveSpeed += 0.05f;
            player.fishingSkill += 5;

            player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.07f;
            if (player.ZoneForest)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }
            if (player.ZoneSnow)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }
            if (player.ZoneBeach)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<WoodCharm>())
                .AddIngredient(ModContent.ItemType<PalmCharm>())
                .AddIngredient(ModContent.ItemType<BorealCharm>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}