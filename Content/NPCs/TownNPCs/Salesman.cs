using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Charisma.Common.Systems;
using Charisma.Content.Items.Charms.Timber.PreHardmode.Ecotone;
using Charisma.Content.Items.Consumables;

namespace Charisma.Content.NPCs.TownNPCs
{
    [AutoloadHead]
    public class Salesman : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25;
            NPCID.Sets.ExtraFramesCount[Type] = 9;
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 700;
            NPCID.Sets.AttackType[Type] = 0;
            NPCID.Sets.AttackTime[Type] = 90;
            NPCID.Sets.AttackAverageChance[Type] = 30;
            NPCID.Sets.HatOffsetY[Type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            AnimationType = NPCID.Guide;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return CharismaWorldSystem.WorldCharismaCount > 0;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>() {
                "Casanova",
                "Valentino",
                "Rico",
                "Fabio"
            };
        }

        public override string GetChat()
        {
            return Language.GetTextValue("Mods.Charisma.Dialogue.Salesman.Standard");
        }

        public override void AddShops()
        {
            var shop = new NPCShop(Type);

            var conditionWoodCharm = new Condition("Mods.Charisma.Conditions.CraftedWood", () => CharismaWorldSystem.FoundCharms.Contains(ModContent.ItemType<WoodCharm>())); shop.Add(ModContent.ItemType<WoodCharm>(), conditionWoodCharm);

            shop.Add(ModContent.ItemType<CharmSlotUnlocker>(), Condition.InHallow);

            shop.Register();
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.WoodenArrowFriendly;
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }
    }
}