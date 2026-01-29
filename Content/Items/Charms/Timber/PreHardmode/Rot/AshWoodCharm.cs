using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode.Rot
{
    public class AshWoodCharm : BaseCharm
    {
        public override int CharismaReward => 3;

        public override void ApplyCharmEffects(Player player)
        {
            player.GetArmorPenetration(DamageClass.Generic) += 3f;

            if (player.ZoneUnderworldHeight)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }

            player.GetModPlayer<AshWoodCharmPlayer>().ashWoodCharmEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.AshWoodHelmet)
                .AddIngredient(ItemID.AshWoodBreastplate)
                .AddIngredient(ItemID.AshWoodGreaves)
                .AddIngredient(ItemID.AshWoodSword)
                .AddIngredient(ItemID.Acorn, 11)
                .AddIngredient(ItemID.SpicyPepper)
                .AddIngredient(ItemID.Pomegranate)
                .AddIngredient(ItemID.Fireblossom, 3)
                .AddIngredient(ItemID.AshWood, 110)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Trees)
                .Register();
        }
    }

    public class AshWoodCharmPlayer : ModPlayer
    {
        public bool ashWoodCharmEquipped;
        public int explosionCooldown = 0;

        public override void ResetEffects()
        {
            ashWoodCharmEquipped = false;
        }

        public override void PostUpdateMiscEffects()
        {
            if (explosionCooldown > 0)
            {
                explosionCooldown--;
            }
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            TryTriggerExplosion(target, damageDone, hit.Crit);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            TryTriggerExplosion(target, damageDone, hit.Crit);
        }

        private void TryTriggerExplosion(NPC target, int damage, bool isCrit)
        {
            if (!ashWoodCharmEquipped) return;
            if (explosionCooldown > 0) return;

            float chance = isCrit ? 0.11f : 0.07f;

            if (Main.rand.NextFloat() < chance)
            {
                TriggerExplosion(target, damage);
            }
        }

        private void TriggerExplosion(NPC target, int damage)
        {
            float rawDamage = damage * 1.33f;

            int baseDamage = (int)MathHelper.Clamp(rawDamage, 3f, 33f);

            int finalDamage = baseDamage + Main.rand.Next(1, 4);

            Projectile.NewProjectile(
                Player.GetSource_FromThis(),
                target.Center,
                Vector2.Zero,
                ModContent.ProjectileType<AshWoodExplosion>(),
                finalDamage,
                0f,
                Player.whoAmI
            );

            explosionCooldown = 420;
        }
    }

    public class AshWoodExplosion : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.SolarWhipSwordExplosion;

        public override void SetDefaults()
        {
            Projectile.width = 220;
            Projectile.height = 220;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 5;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                for (int i = 0; i < 20; i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                    Main.dust[dustIndex].velocity *= 1.4f;
                }
                for (int i = 0; i < 10; i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1.2f);
                    Main.dust[dustIndex].velocity *= 1.4f;
                }

                Projectile.localAI[0] = 1;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }
    }
}