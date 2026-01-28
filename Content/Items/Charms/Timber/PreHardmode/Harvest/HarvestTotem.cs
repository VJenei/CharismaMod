using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode.Harvest
{
    public class HarvestTotem : BaseCharm
    {
        public override int CharismaReward => 7;

        public override void ApplyCharmEffects(Player player)
        {
            player.statDefense += 2;

            player.GetModPlayer<CactusCharmPlayer>().cactusCharmEquipped = true;
            player.GetModPlayer<PumpkinCharmPlayer>().pumpkinCharmEquipped = true;

            player.GetDamage(DamageClass.Generic) += 0.03f;
            player.GetCritChance(DamageClass.Generic) += 3f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.05f;

            player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            if (player.ZoneDesert)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CactusCharm>())
                .AddIngredient(ModContent.ItemType<PumpkinCharm>())
                .AddIngredient(ModContent.ItemType<DynastyCharm>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}