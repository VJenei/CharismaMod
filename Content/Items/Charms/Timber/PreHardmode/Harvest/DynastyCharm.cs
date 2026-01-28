using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode.Harvest
{
    public class DynastyCharm : BaseCharm
    {
        public override int CharismaReward => 3;

        public override void ApplyCharmEffects(Player player)
        {
            player.statDefense -= 2;
            player.GetDamage(DamageClass.Generic) += 0.03f;
            player.GetCritChance(DamageClass.Generic) += 3f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.05f;

            player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.005f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.RedDynastyShingles, 33)
                .AddIngredient(ItemID.BlueDynastyShingles, 33)
                .AddIngredient(ItemID.Acorn, 11)
                .AddIngredient(ItemID.Sake, 3)
                .AddIngredient(ItemID.DynastyWood, 110)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Bamboo)
                .Register();
        }
    }
}