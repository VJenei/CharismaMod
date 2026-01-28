using Terraria;
using Terraria.ModLoader;
using Charisma.Common.Players;
using Charisma.Content.Items.Charms.Base;

namespace Charisma.Content.UI
{
    public class CharmSlot1 : ModAccessorySlot
    {
        public override string Name => "Charm Slot 1";
        public override string FunctionalTexture => "Charisma/Assets/Textures/UI/CharmSlot_Icon";
        public override bool IsEnabled()
        {
            return Player.GetModPlayer<CharismaPlayer>().UnlockedCharmSlots >= 1;
        }
        public override bool IsHidden()
        {
            return Player.GetModPlayer<CharismaPlayer>().VisualCharmSlotLimit < 1;
        }
        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
        {
            return checkItem.ModItem is BaseCharm;
        }
    }

    public class CharmSlot2 : ModAccessorySlot
    {
        public override string Name => "Charm Slot 2";
        public override string FunctionalTexture => "Charisma/Assets/Textures/UI/CharmSlot_Icon";
        public override bool IsEnabled()
        {
            return Player.GetModPlayer<CharismaPlayer>().UnlockedCharmSlots >= 2;
        }

        public override bool IsHidden()
        {
            return Player.GetModPlayer<CharismaPlayer>().VisualCharmSlotLimit < 2;
        }

        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
        {
            return checkItem.ModItem is BaseCharm;
        }
    }

    public class CharmSlot3 : ModAccessorySlot
    {
        public override string Name => "Charm Slot 3";
        public override string FunctionalTexture => "Charisma/Assets/Textures/UI/CharmSlot_Icon";
        public override bool IsEnabled()
        {
            return Player.GetModPlayer<CharismaPlayer>().UnlockedCharmSlots >= 3;
        }
        public override bool IsHidden()
        {
            return Player.GetModPlayer<CharismaPlayer>().VisualCharmSlotLimit < 3;
        }
        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
        {
            return checkItem.ModItem is BaseCharm;
        }
    }
}