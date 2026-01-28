using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Content.Items.Charms.Timber.PreHardmode.Harvest
{
    public class CactusCharm : BaseCharm
    {
        public override int CharismaReward => 3;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<PumpkinCharm>();
        }

        public override void ApplyCharmEffects(Player player)
        {
            player.statDefense += 2;

            if (player.ZoneDesert)
            {
                player.GetModPlayer<CharismaPlayer>().luckBonusAccumulator += 0.03f;
            }

            player.GetModPlayer<CactusCharmPlayer>().cactusCharmEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CactusHelmet)
                .AddIngredient(ItemID.CactusBreastplate)
                .AddIngredient(ItemID.CactusLeggings)
                .AddIngredient(ItemID.CactusSword)
                .AddIngredient(ItemID.PinkPricklyPear)
                .AddIngredient(ItemID.Scorpion)
                .AddIngredient(ItemID.Waterleaf, 3)
                .AddIngredient(ItemID.Cactus, 110)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Cactus)
                .Register();
        }
    }

    public class CactusCharmPlayer : ModPlayer
    {
        public bool cactusCharmEquipped;

        public override void ResetEffects()
        {
            cactusCharmEquipped = false;
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (cactusCharmEquipped && info.DamageSource.SourceNPCIndex >= 0)
            {
                int npcIndex = info.DamageSource.SourceNPCIndex;
                if (Main.npc.IndexInRange(npcIndex))
                {
                    NPC target = Main.npc[npcIndex];
                    int defense = Player.statDefense;
                    int initialBase = (int)(defense * 0.5f);

                    int baseDamage = initialBase switch
                    {
                        < 3 => 3,
                        < 7 => 3 + (int)(defense * 0.33f),
                        < 22 => 7 + (int)(defense * 0.22f),
                        _ => 22 + (int)(defense * 0.11f)
                    };

                    int totalDamage = baseDamage + Main.rand.Next(-2, 3);

                    if (Main.rand.NextBool(11))
                    {
                        totalDamage *= 2;
                    }

                    Player.ApplyDamageToNPC(target, 11 + totalDamage, 0f, 0, false);
                }
            }
        }
    }
}