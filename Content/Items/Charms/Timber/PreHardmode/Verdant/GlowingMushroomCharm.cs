using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode.Verdant
{
    public class GlowingMushroomCharm : BaseCharm
    {
        public override int CharismaReward => 3;

        public override void ApplyCharmEffects(Player player)
        {
            player.potionDelayTime = (int)(player.potionDelayTime * 0.89f);
            player.restorationDelayTime = (int)(player.restorationDelayTime * 0.89f);

            Lighting.AddLight(player.Center, 0, 0, 0.33f);

            if (player.ZoneGlowshroom)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MushroomWorkBench)
                .AddIngredient(ItemID.GlowingSnail)
                .AddIngredient(ItemID.GlowingMushroom, 330)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.MushroomTrees)
                .Register();
        }
    }
}