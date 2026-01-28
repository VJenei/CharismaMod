using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode.Ecotone
{
    public class BorealCharm : BaseCharm
    {
        public override int CharismaReward => 3;

        public override void ApplyCharmEffects(Player player)
        {
            player.GetCritChance(DamageClass.Generic) += 3f;

            if (player.ZoneSnow)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }

            player.GetModPlayer<BorealCharmPlayer>().borealCharmEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BorealWoodHelmet)
                .AddIngredient(ItemID.BorealWoodBreastplate)
                .AddIngredient(ItemID.BorealWoodGreaves)
                .AddIngredient(ItemID.BorealWoodSword)
                .AddIngredient(ItemID.Shiverthorn, 3)
                .AddIngredient(ItemID.Cherry)
                .AddIngredient(ItemID.Plum)
                .AddIngredient(ItemID.Penguin)
                .AddIngredient(ItemID.Acorn, 11)
                .AddIngredient(ItemID.BorealWood, 110)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Trees)
                .Register();
        }
    }

    public class BorealCharmPlayer : ModPlayer
    {
        public bool borealCharmEquipped;

        public override void ResetEffects()
        {
            borealCharmEquipped = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (borealCharmEquipped && Main.rand.NextFloat() < 0.11f)
            {
                target.AddBuff(BuffID.Frostburn, 180);
            }
            if (borealCharmEquipped && hit.Crit && Main.rand.NextFloat() < 0.07f)
            {
                target.AddBuff(BuffID.Frostburn, 180);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (borealCharmEquipped && Main.rand.NextFloat() < 0.07f)
            {
                target.AddBuff(BuffID.Frostburn, 180);
            }
            if (borealCharmEquipped && hit.Crit && Main.rand.NextFloat() < 0.05f)
            {
                target.AddBuff(BuffID.Frostburn, 180);
            }
        }
    }
}