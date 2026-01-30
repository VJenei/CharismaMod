using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode.Verdant
{
    public class VerdantTotem : BaseCharm
    {
        public override int CharismaReward => 7;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Green;
        }

        public override void ApplyCharmEffects(Player player)
        {
            player.jumpSpeedBoost += 1.0f;
            player.GetModPlayer<MahoganyCharmPlayer>().mahoganyCharmEquipped = true;

            player.statLifeMax2 += 10;
            player.blockRange += 1;

            player.potionDelayTime = (int)(player.potionDelayTime * 0.89f);
            player.restorationDelayTime = (int)(player.restorationDelayTime * 0.89f);

            Lighting.AddLight(player.Center, 0, .22f, 0.33f);

            if (player.ZoneJungle)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.07f;
            }
            if (player.ZoneGlowshroom)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GlowingMushroomCharm>())
                .AddIngredient(ModContent.ItemType<BambooCharm>())
                .AddIngredient(ModContent.ItemType<MahoganyCharm>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}