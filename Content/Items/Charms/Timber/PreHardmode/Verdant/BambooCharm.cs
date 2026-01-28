using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode.Verdant
{
    public class BambooCharm : BaseCharm
    {
        public override int CharismaReward => 3;

        public override void ApplyCharmEffects(Player player)
        {
            player.statLifeMax2 += 10;

            player.blockRange += 1;

            if (player.ZoneJungle)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BambooBlock, 110)
                .AddIngredient(ItemID.LargeBambooBlock, 110)
                .AddIngredient(ItemID.Frog)
                .AddIngredient(ItemID.Moonglow, 3)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Bamboo)
                .Register();
        }
    }
}