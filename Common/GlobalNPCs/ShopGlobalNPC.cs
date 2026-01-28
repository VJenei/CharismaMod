using Terraria;
using Terraria.ModLoader;
using Charisma.Common.Systems;

namespace Charisma.Common.GlobalNPCs
{
    public class ShopGlobalNPC : GlobalNPC
    {
        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            float discount = CharismaWorldSystem.WorldCharismaCount * 0.0005f;
            if (discount > 0.5f)
            {
                discount = 0.5f;
            }

            if (discount > 0f)
            {
                foreach (Item item in items)
                {
                    if (item != null && !item.IsAir)
                    {
                        int price = item.shopCustomPrice ?? item.value;
                        item.shopCustomPrice = (int)(price * (1f - discount));
                    }
                }
            }
        }
    }
}