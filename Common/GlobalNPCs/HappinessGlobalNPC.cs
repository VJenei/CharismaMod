using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Personalities; // Required for AffectionLevel
// Make sure to add 'using' for your specific mod namespace where the Salesman class is located
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
                // 1. Get the "dummy" instance of this NPC type to check its stats
                if (ContentSamples.NpcsByNetId.TryGetValue(i, out NPC npc))
                {
                    // 2. Check if it is a Town NPC and NOT a Town Pet (Cat/Dog/Bunny)
                    if (npc.townNPC && !NPCID.Sets.IsTownPet[i])
                    {
                        // 3. The Haters
                        if (i == NPCID.Merchant || i == NPCID.Dryad || i == NPCID.Truffle || i == NPCID.Pirate)
                        {
                            NPCHappiness.Get(i).SetNPCAffection(salesmanType, AffectionLevel.Dislike);
                        }
                        else if (i == NPCID.TaxCollector)
                        {
                            NPCHappiness.Get(i).SetNPCAffection(salesmanType, AffectionLevel.Hate);
                        }
                        // 4. Everyone Else (Fan Club)
                        else
                        {
                            // Ensure the Salesman doesn't try to like himself
                            if (i != salesmanType)
                            {
                                NPCHappiness.Get(i).SetNPCAffection(salesmanType, AffectionLevel.Like);
                            }
                        }
                    }
                }
            }
        }
    }
}