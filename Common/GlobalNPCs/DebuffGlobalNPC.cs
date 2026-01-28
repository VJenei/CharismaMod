using Terraria;
using Terraria.ModLoader;
using Charisma.Content.Buffs;

namespace Charisma.Common.GlobalNPCs
{
    public class DebuffGlobalNPC : GlobalNPC
    {
        private static int SplintersBuffID;

        public override void SetStaticDefaults()
        {
            SplintersBuffID = ModContent.BuffType<SplintersDebuff>();
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasBuff(SplintersBuffID))
            {
                modifiers.ArmorPenetration += 3;
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (npc.HasBuff(SplintersBuffID))
            {
                modifiers.ArmorPenetration += 3;
            }
        }
    }
}