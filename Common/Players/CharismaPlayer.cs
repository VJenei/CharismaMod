using Charisma.Common.Systems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Charisma.Common.Players
{
    public class CharismaPlayer : ModPlayer
    {
        public int UnlockedCharmSlots = 0;
        public int VisualCharmSlotLimit = 3;
        public float luckBonusAccumulator = 0f;

        public override void ResetEffects()
        {
            luckBonusAccumulator = 0f;
        }

        public override void SaveData(TagCompound tag)
        {
            tag["UnlockedCharmSlots"] = UnlockedCharmSlots;
            tag["VisualCharmSlotLimit"] = VisualCharmSlotLimit;
        }

        public override void LoadData(TagCompound tag)
        {
            UnlockedCharmSlots = tag.GetInt("UnlockedCharmSlots");

            if (tag.ContainsKey("VisualCharmSlotLimit"))
            {
                VisualCharmSlotLimit = tag.GetInt("VisualCharmSlotLimit");
            }
            else
            {
                VisualCharmSlotLimit = 3;
            }
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write(Charisma.Packet_SyncPlayer);
            packet.Write((byte)Player.whoAmI);
            packet.Write(UnlockedCharmSlots);
            packet.Send(toWho, fromWho);
        }
        public override void PostUpdateMiscEffects()
        {
            int npcCount = 0;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.townNPC && !npc.isLikeATownNPC && Player.Distance(npc.Center) < 2200f)
                {
                    npcCount++;
                }
            }

            float scale = Math.Min(npcCount, 3) / 3f;

            if (scale > 0)
            {
                float bonus = CharismaWorldSystem.WorldCharismaCount * 0.002f * scale;
                Player.moveSpeed += bonus;
                Player.accRunSpeed += bonus;
            }
        }

        public override void ModifyLuck(ref float luck)
        {
            luck += luckBonusAccumulator / 2;

            int npcCount = 0;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.townNPC && Player.Distance(npc.Center) < 2200f) npcCount++;
            }

            float scale = Math.Min(npcCount, 3) / 3f;

            if (scale > 0)
            {
                float bonus = CharismaWorldSystem.WorldCharismaCount * 0.0005f * scale;
                luck += bonus;
            }
        }
    }
}