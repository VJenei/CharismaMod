using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Charisma.Common.Players;

namespace Charisma.Content.Items.Consumables
{
    public class CharmSlotUnlocker : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 5);
            Item.UseSound = SoundID.Item4;
        }

        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<CharismaPlayer>().UnlockedCharmSlots < 3;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                var modPlayer = player.GetModPlayer<CharismaPlayer>();

                modPlayer.UnlockedCharmSlots++;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    modPlayer.SyncPlayer(-1, -1, false);
                }

                Main.NewText($"Charm Slot Unlocked! ({modPlayer.UnlockedCharmSlots}/3)", 175, 75, 255);
                SoundEngine.PlaySound(SoundID.Item29, player.position);
            }
            return true;
        }
    }
}