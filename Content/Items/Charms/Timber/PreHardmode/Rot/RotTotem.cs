using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode.Rot
{
    public class RotTotem : BaseCharm
    {
        public override int CharismaReward => 7;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Green;
        }

        public override void ApplyCharmEffects(Player player)
        {
            player.endurance += 0.03f;
            player.GetModPlayer<EbonwoodCharmPlayer>().ebonwoodCharmEquipped = true;

            player.runAcceleration += 0.07f;
            player.GetDamage(DamageClass.Generic) += 0.03f;
            player.GetModPlayer<ShadewoodCharmPlayer>().shadewoodCharmEquipped = true;

            player.GetArmorPenetration(DamageClass.Generic) += 3f;
            player.GetModPlayer<AshWoodCharmPlayer>().ashWoodCharmEquipped = true;

            if (player.ZoneCorrupt)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }
            if (player.ZoneCrimson)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }
            if (player.ZoneUnderworldHeight)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AshWoodCharm>())
                .AddIngredient(ModContent.ItemType<ShadewoodCharm>())
                .AddIngredient(ModContent.ItemType<EbonwoodCharm>())
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}