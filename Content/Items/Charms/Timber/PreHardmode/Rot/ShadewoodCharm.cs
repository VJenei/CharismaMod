using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode.Rot
{
    public class ShadewoodCharm : BaseCharm
    {
        public override int CharismaReward => 3;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<EbonwoodCharm>();
        }

        public override void ApplyCharmEffects(Player player)
        {
            player.runAcceleration += 0.07f;

            player.GetDamage(DamageClass.Generic) += 0.03f;

            if (player.ZoneCrimson)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }

            player.GetModPlayer<ShadewoodCharmPlayer>().shadewoodCharmEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ShadewoodHelmet)
                .AddIngredient(ItemID.ShadewoodBreastplate)
                .AddIngredient(ItemID.ShadewoodGreaves)
                .AddIngredient(ItemID.ShadewoodSword)
                .AddIngredient(ItemID.Rambutan)
                .AddIngredient(ItemID.BloodOrange)
                .AddIngredient(ItemID.ViciousMushroom, 3)
                .AddIngredient(ItemID.Acorn, 11)
                .AddIngredient(ItemID.Deathweed, 3)
                .AddIngredient(ItemID.Shadewood, 110)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Trees)
                .Register();
        }
    }

    public class ShadewoodCharmPlayer : ModPlayer
    {
        public bool shadewoodCharmEquipped;
        public int lifeStealCooldown = 0;

        public override void ResetEffects()
        {
            shadewoodCharmEquipped = false;
        }

        public override void PostUpdateMiscEffects()
        {
            if (lifeStealCooldown > 0)
            {
                lifeStealCooldown--;
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            ApplyLifeSteal();
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            ApplyLifeSteal();
        }

        private void ApplyLifeSteal()
        {
            if (!shadewoodCharmEquipped) return;
            if (lifeStealCooldown > 0) return;

            if (Main.rand.NextFloat() < 0.07f)
            {
                float rawBase = Player.statDefense * 0.22f;

                int baseAmount = (int)MathHelper.Clamp(rawBase, 2f, 22f);

                int finalAmount = baseAmount + Main.rand.Next(1, 3);

                Player.Heal(finalAmount);

                lifeStealCooldown = 180;
            }
        }
    }
}