using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode.Rot
{
    public class EbonwoodCharm : BaseCharm
    {
        public override int CharismaReward => 3;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<ShadewoodCharm>();
        }

        public override void ApplyCharmEffects(Player player)
        {
            player.endurance += 0.03f;

            if (player.ZoneCorrupt)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }

            player.GetModPlayer<EbonwoodCharmPlayer>().ebonwoodCharmEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.EbonwoodHelmet)
                .AddIngredient(ItemID.EbonwoodBreastplate)
                .AddIngredient(ItemID.EbonwoodGreaves)
                .AddIngredient(ItemID.EbonwoodSword)
                .AddIngredient(ItemID.Acorn, 11)
                .AddIngredient(ItemID.Elderberry)
                .AddIngredient(ItemID.BlackCurrant)
                .AddIngredient(ItemID.VileMushroom, 3)
                .AddIngredient(ItemID.Deathweed, 3)
                .AddIngredient(ItemID.Ebonwood, 110)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Trees)
                .Register();
        }
    }

    public class EbonwoodCharmPlayer : ModPlayer
    {
        public bool ebonwoodCharmEquipped;

        public override void ResetEffects()
        {
            ebonwoodCharmEquipped = false;
        }

        public override void ModifyItemScale(Item item, ref float scale)
        {
            if (ebonwoodCharmEquipped && item.damage > 0)
            {
                scale *= 1.22f;
            }
        }
    }

    public class EbonwoodCharmGlobalProjectile : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse itemSource)
            {
                Player player = itemSource.Player;
                if (player != null && player.GetModPlayer<EbonwoodCharmPlayer>().ebonwoodCharmEquipped)
                {
                    projectile.scale *= 1.22f;

                    projectile.width = (int)(projectile.width * 1.22f);
                    projectile.height = (int)(projectile.height * 1.22f);
                }
            }
        }
    }
}