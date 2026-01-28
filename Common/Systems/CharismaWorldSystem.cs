using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID; // Required for NetmodeID

namespace Charisma.Common.Systems
{
    public class CharismaWorldSystem : ModSystem
    {
        public static HashSet<int> FoundCharms = new HashSet<int>();
        public static int WorldCharismaCount = 0;

        public override void OnWorldLoad()
        {
            FoundCharms.Clear();
            WorldCharismaCount = 0;

            On_Main.UpdateTime_SpawnTownNPCs += CharismaTownSpawnDetour;
        }

        public override void OnWorldUnload()
        {
            FoundCharms.Clear();
            WorldCharismaCount = 0;

            On_Main.UpdateTime_SpawnTownNPCs -= CharismaTownSpawnDetour;
        }
        private void CharismaTownSpawnDetour(On_Main.orig_UpdateTime_SpawnTownNPCs original)
        {
            original();

            if (WorldCharismaCount >= 3)
            {
                var milestones = new System.Collections.Generic.HashSet<long>();
                milestones.Add(3);

                long p7 = 7;
                while (p7 <= WorldCharismaCount)
                {
                    long p3 = 1;
                    while (p7 * p3 <= WorldCharismaCount)
                    {
                        milestones.Add(p7 * p3);

                        if (p3 > long.MaxValue / 3) break;
                        p3 *= 3;
                    }

                    if (p7 > long.MaxValue / 7) break;
                    p7 *= 7;
                }

                int extraTicks = milestones.Count;

                if (extraTicks > 0)
                {

                    for (int i = 0; i < extraTicks; i++)
                    {
                        original();
                    }
                }
            }
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["FoundCharms"] = new List<int>(FoundCharms);
            tag["WorldCharismaCount"] = WorldCharismaCount;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            var list = tag.GetList<int>("FoundCharms");
            FoundCharms = new HashSet<int>(list);
            WorldCharismaCount = tag.GetInt("WorldCharismaCount");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(WorldCharismaCount);
            writer.Write(FoundCharms.Count);
            foreach (int id in FoundCharms)
            {
                writer.Write(id);
            }
        }

        public override void NetReceive(BinaryReader reader)
        {
            WorldCharismaCount = reader.ReadInt32();
            int count = reader.ReadInt32();
            FoundCharms.Clear();
            for (int i = 0; i < count; i++)
            {
                FoundCharms.Add(reader.ReadInt32());
            }
        }

        public static void TryUnlockCharm(int itemType, int charismaReward)
        {
            if (FoundCharms.Contains(itemType)) return;

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ModContent.GetInstance<Charisma>().GetPacket();
                packet.Write(Charisma.Packet_UnlockCharm);
                packet.Write(itemType);
                packet.Write(charismaReward);
                packet.Send();
                return;
            }

            FoundCharms.Add(itemType);
            WorldCharismaCount += charismaReward;

            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
            if (Main.netMode != NetmodeID.Server)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item29);
            }
        }
    }
}