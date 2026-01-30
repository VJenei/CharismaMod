using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Personalities;
using Charisma.Content.NPCs.TownNPCs;

namespace Charisma.Common.GlobalNPCs
{
    public class HappinessGlobalNPC : GlobalNPC
    {
        public override void SetStaticDefaults()
        {
            int salesmanType = ModContent.NPCType<Salesman>();

            for (int i = 0; i < NPCID.Count; i++)
            {
                if (ContentSamples.NpcsByNetId.TryGetValue(i, out NPC npc))
                {
                    if (npc.townNPC && !NPCID.Sets.IsTownPet[i])
                    {
                        if (i == NPCID.Merchant || i == NPCID.Dryad || i == NPCID.Truffle || i == NPCID.Pirate)
                        {
                            NPCHappiness.Get(i).SetNPCAffection(salesmanType, AffectionLevel.Dislike);
                        }
                        else if (i == NPCID.TaxCollector)
                        {
                            NPCHappiness.Get(i).SetNPCAffection(salesmanType, AffectionLevel.Hate);
                        }
                        else
                        {
                            NPCHappiness.Get(i).SetNPCAffection(salesmanType, AffectionLevel.Like);
                        }
                    }
                }
            }
        }
    }
}