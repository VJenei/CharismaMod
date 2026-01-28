using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode.Ecotone
{
    public class PalmCharm : BaseCharm
    {
        public override int CharismaReward => 3;

        public override void ApplyCharmEffects(Player player)
        {
            player.moveSpeed += 0.05f;
            player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.07f;
            player.fishingSkill += 5;

            if (player.ZoneBeach)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PalmWoodHelmet)
                .AddIngredient(ItemID.PalmWoodBreastplate)
                .AddIngredient(ItemID.PalmWoodGreaves)
                .AddIngredient(ItemID.PalmWoodSword)
                .AddIngredient(ItemID.Coconut)
                .AddIngredient(ItemID.Banana)
                .AddIngredient(ItemID.Seagull)
                .AddIngredient(ItemID.LimeKelp)
                .AddIngredient(ItemID.ShellPileBlock, 7)
                .AddIngredient(ItemID.Acorn, 11)
                .AddIngredient(ItemID.PalmWood, 110)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Trees)
                .Register();
        }
    }
}