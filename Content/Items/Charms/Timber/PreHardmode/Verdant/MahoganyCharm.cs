using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode.Verdant
{
    public class MahoganyCharm : BaseCharm
    {
        public override int CharismaReward => 3;

        public override void ApplyCharmEffects(Player player)
        {
            player.jumpSpeedBoost += 1.0f;

            if (player.ZoneJungle)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }

            player.GetModPlayer<MahoganyCharmPlayer>().mahoganyCharmEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.RichMahoganyHelmet)
                .AddIngredient(ItemID.RichMahoganyBreastplate)
                .AddIngredient(ItemID.RichMahoganyGreaves)
                .AddIngredient(ItemID.RichMahoganySword)
                .AddIngredient(ItemID.Mango)
                .AddIngredient(ItemID.Pineapple)
                .AddIngredient(ItemID.Grubby)
                .AddIngredient(ItemID.Acorn, 11)
                .AddIngredient(ItemID.Moonglow, 3)
                .AddIngredient(ItemID.RichMahogany, 110)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Trees)
                .Register();
        }
    }

    public class MahoganyCharmPlayer : ModPlayer
    {
        public bool mahoganyCharmEquipped;

        public override void ResetEffects()
        {
            mahoganyCharmEquipped = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (mahoganyCharmEquipped && Main.rand.NextFloat() < 0.11f)
            {
                target.AddBuff(BuffID.Poisoned, 360);
            }
            if (mahoganyCharmEquipped && hit.Crit && Main.rand.NextFloat() < 0.07f)
            {
                target.AddBuff(BuffID.Poisoned, 360);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (mahoganyCharmEquipped && Main.rand.NextFloat() < 0.07f)
            {
                target.AddBuff(BuffID.Poisoned, 360);
            }
            if (mahoganyCharmEquipped && hit.Crit && Main.rand.NextFloat() < 0.05f)
            {
                target.AddBuff(BuffID.Poisoned, 360);
            }
        }
    }

    public class MahoganyHookGlobalProjectile : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.aiStyle == ProjAIStyleID.Hook && source is EntitySource_ItemUse itemSource)
            {
                Player player = itemSource.Player;
                if (player != null && player.GetModPlayer<MahoganyCharmPlayer>().mahoganyCharmEquipped)
                {
                    projectile.velocity *= 1.22f;
                }
            }
        }
    }
}