using Charisma.Common.Players;
using Charisma.Common.Systems;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma
{
    public class Charisma : Mod
    {
        internal const byte Packet_UnlockCharm = 0;
        internal const byte Packet_SyncPlayer = 1;

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            byte msgType = reader.ReadByte();
            switch (msgType)
            {
                case Packet_UnlockCharm:
                    int itemType = reader.ReadInt32();
                    int charismaReward = reader.ReadInt32();

                    if (Main.netMode == NetmodeID.Server)
                    {
                        CharismaWorldSystem.TryUnlockCharm(itemType, charismaReward);
                    }
                    break;

                case Packet_SyncPlayer:
                    byte playernumber = reader.ReadByte();
                    var p = Main.player[playernumber].GetModPlayer<CharismaPlayer>();

                    p.UnlockedCharmSlots = reader.ReadInt32();

                    if (Main.netMode == NetmodeID.Server)
                    {
                        p.SyncPlayer(-1, whoAmI, false);
                    }
                    break;
            }
        }
    }
}