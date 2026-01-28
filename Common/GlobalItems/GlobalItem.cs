using Charisma.Common.Systems;
using Charisma.Content.Items.Charms.Base;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Charisma.Common.GlobalItems
{
    public class CharismaGlobalItem : GlobalItem
    {
        public override void OnCreated(Item item, ItemCreationContext context)
        {
            if (item.ModItem is BaseCharm charm)
            {
                CharismaWorldSystem.TryUnlockCharm(item.type, charm.CharismaReward);
            }
        }

        public override bool OnPickup(Item item, Player player)
        {
            if (item.ModItem is BaseCharm charm)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    CharismaWorldSystem.TryUnlockCharm(item.type, charm.CharismaReward);
                }
            }
            return base.OnPickup(item, player);
        }
    }
}