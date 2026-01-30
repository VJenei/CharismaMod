using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;
using Charisma.Content.Items.Charms.Timber.PreHardmode.Ecotone;
using Charisma.Content.Items.Charms.Timber.PreHardmode.Harvest;
using Charisma.Content.Items.Charms.Timber.PreHardmode.Rot;
using Charisma.Content.Items.Charms.Timber.PreHardmode.Verdant;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode
{
    public class TimberTalisman : BaseCharm
    {
        public override int CharismaReward => 21;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
        }

        public override void ApplyCharmEffects(Player player)
        {
            player.statDefense += 4;
            player.GetCritChance(DamageClass.Generic) += 7f;
            player.moveSpeed += 0.05f;
            player.fishingSkill += 5;
            player.GetModPlayer<WoodCharmPlayer>().woodCharmEquipped = true;
            player.GetModPlayer<BorealCharmPlayer>().borealCharmEquipped = true;

            player.GetDamage(DamageClass.Generic) += 0.07f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
            player.GetModPlayer<CactusCharmPlayer>().cactusCharmEquipped = true;
            player.GetModPlayer<PumpkinCharmPlayer>().pumpkinCharmEquipped = true;

            player.jumpSpeedBoost += 1.0f;
            player.statLifeMax2 += 10;
            player.blockRange += 1;
            player.potionDelayTime = (int)(player.potionDelayTime * 0.9f);
            player.restorationDelayTime = (int)(player.restorationDelayTime * 0.9f);
            player.GetModPlayer<MahoganyCharmPlayer>().mahoganyCharmEquipped = true;

            player.endurance += 0.03f;
            player.runAcceleration += 0.07f;
            player.GetArmorPenetration(DamageClass.Generic) += 3f;
            player.GetModPlayer<EbonwoodCharmPlayer>().ebonwoodCharmEquipped = true;
            player.GetModPlayer<ShadewoodCharmPlayer>().shadewoodCharmEquipped = true;
            player.GetModPlayer<AshWoodCharmPlayer>().ashWoodCharmEquipped = true;

            var charismaPlayer = player.GetModPlayer<CharismaPlayer>();
            charismaPlayer.luckBonusAccumulator += 0.11f;

            if (player.ZoneForest || player.ZoneSnow || player.ZoneBeach ||
                player.ZoneDesert || player.ZoneJungle || player.ZoneGlowshroom ||
                player.ZoneCorrupt || player.ZoneCrimson || player.ZoneUnderworldHeight)
            {
                charismaPlayer.luckBonusAccumulator += 0.07f;
            }

            Lighting.AddLight(player.Center, .11f, .22f, 0.33f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<EcotoneTotem>())
                .AddIngredient(ModContent.ItemType<HarvestTotem>())
                .AddIngredient(ModContent.ItemType<VerdantTotem>())
                .AddIngredient(ModContent.ItemType<RotTotem>())
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}