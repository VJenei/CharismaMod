using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode.Harvest
{
    public class PumpkinCharm : BaseCharm
    {
        public override int CharismaReward => 3;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<CactusCharm>();
        }

        public override void ApplyCharmEffects(Player player)
        {
            player.statDefense += 2;

            player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.015f;

            player.GetModPlayer<PumpkinCharmPlayer>().pumpkinCharmEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PumpkinHelmet)
                .AddIngredient(ItemID.PumpkinBreastplate)
                .AddIngredient(ItemID.PumpkinLeggings)
                .AddIngredient(ItemID.PumpkinPie)
                .AddIngredient(ItemID.JackOLantern, 3)
                .AddIngredient(ItemID.Worm)
                .AddIngredient(ItemID.Pumpkin, 110)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Pumpkins)
                .Register();
        }
    }
    public class PumpkinCharmPlayer : ModPlayer
    {
        public bool pumpkinCharmEquipped;

        public int pumpkinCharges = 0;
        public int pumpkinTimer = 0;
        private const int MaxCharges = 7;
        private const int CooldownTicks = 420;

        public override void ResetEffects()
        {
            pumpkinCharmEquipped = false;
        }

        public override void PostUpdate()
        {
            if (pumpkinTimer > 0)
            {
                pumpkinTimer--;

                if (pumpkinTimer <= 0)
                {
                    pumpkinCharges = 0;
                }
            }
        }

        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (pumpkinCharmEquipped)
            {
                ApplyPumpkinEffect(target, isItem: true);
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (pumpkinCharmEquipped)
            {
                ApplyPumpkinEffect(target, isItem: false);
            }
        }

        private void ApplyPumpkinEffect(NPC target, bool isItem)
        {
            if (pumpkinCharges >= MaxCharges)
                return;

            if (Main.rand.NextFloat() > 0.22f)
            {
                if (pumpkinCharges == 0)
                {
                    pumpkinTimer = CooldownTicks;
                }
                pumpkinCharges++;

                int defense = Player.statDefense;
                int initialBase = (int)(defense * 0.50f);

                int baseDamage = initialBase switch
                {
                    < 7 => 1,
                    < 22 => (1 + (int)(defense * 0.33f)) / 2,
                    < 40 => (3 + (int)(defense * 0.22f)) / 2,
                    _ => (5 + (int)(defense * 0.11f)) / 2
                };

                int variance = isItem ? Main.rand.Next(-1, 1) : Main.rand.Next(-2, 1);
                int totalDamage = baseDamage + variance;

                if (Main.rand.NextBool(66))
                {
                    totalDamage *= 2;
                }

                int finalDamage = totalDamage * 2;
                Player.ApplyDamageToNPC(target, finalDamage, 0f, 0, false);

                float visualChance = isItem ? 0.7f : 0.3f;
                if (Main.rand.NextFloat() < visualChance)
                {
                    SpawnPumpkinVisuals(target);
                }
            }
        }

        private void SpawnPumpkinVisuals(NPC target)
        {
            Vector2 direction = (target.Center - Player.Center).SafeNormalize(Vector2.UnitX);

            for (int i = 0; i < (1 + Main.rand.Next(2)); i++)
            {
                Vector2 velocity = direction.RotatedByRandom(MathHelper.ToRadians(33)) * Main.rand.NextFloat(1f, 3.3f);

                Projectile.NewProjectile(
                    Player.GetSource_FromThis(),
                    target.Center,
                    velocity,
                    ModContent.ProjectileType<PumpkinPieceVisual>(),
                    0, 0, Player.whoAmI
                );
            }
        }
    }
    public class PumpkinPieceVisual : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 110;
            Projectile.aiStyle = -1;
            Projectile.scale = 0.5f;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.11f;
            Projectile.velocity *= 0.89f;
            Projectile.rotation += Projectile.velocity.X * 0.11f;
            if (Projectile.scale >= 0.25f)
            {
                Projectile.scale *= 0.989f;
            }
            if (Projectile.Opacity > 0f)
            {
                Projectile.Opacity *= 0.989f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X * 0.6f;
            if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y * 0.6f;
            return false;
        }
    }
}