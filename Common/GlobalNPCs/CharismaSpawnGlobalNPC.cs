using Charisma.Common.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Charisma.Common.GlobalNPCs
{
    public class CharismaSpawnGlobalNPC : GlobalNPC
    {
        private static int _tickCounter;
        private static long _lastCharismaCount = -1;
        private static float _cachedMilestoneMultiplier = 1f;

        private const int MaxCharismaMilestoneIndex = 7;
        private const float SkeletonMerchantRarityMultiplier = 2f;
        private const float NonDemonTaxCollectorMultiplier = 3f;

        private static float _cachedMaxValue;
        private static bool _isInitialized = false;

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (++_tickCounter < (MaxCharismaMilestoneIndex - _cachedMilestoneMultiplier)) return;
            _tickCounter = 0;

            if (CharismaWorldSystem.WorldCharismaCount < 3) return;

            if (!_isInitialized) InitializeMilestones();

            if (CharismaWorldSystem.WorldCharismaCount != _lastCharismaCount)
            {
                _lastCharismaCount = CharismaWorldSystem.WorldCharismaCount;
                _cachedMilestoneMultiplier = CalculateMilestoneMultiplier(_lastCharismaCount);
            }

            if (Main.hardMode && spawnInfo.Player.ZoneUnderworldHeight)
            {
                TryAddSpawn(pool, NPCID.DemonTaxCollector, _cachedMaxValue);
            }

            if (NPC.downedGoblins && spawnInfo.Player.ZoneRockLayerHeight)
            {
                TryAddSpawn(pool, NPCID.BoundGoblin, _cachedMaxValue * NonDemonTaxCollectorMultiplier);
            }

            if (NPC.downedBoss3 && spawnInfo.Player.ZoneDungeon)
            {
                TryAddSpawn(pool, NPCID.BoundMechanic, _cachedMaxValue * NonDemonTaxCollectorMultiplier);
            }

            if (Main.hardMode && spawnInfo.Player.ZoneRockLayerHeight)
            {
                TryAddSpawn(pool, NPCID.BoundWizard, _cachedMaxValue * NonDemonTaxCollectorMultiplier);
            }

            if (spawnInfo.Player.ZoneRockLayerHeight)
            {
                TryAddSpawn(pool, NPCID.SkeletonMerchant, _cachedMaxValue * NonDemonTaxCollectorMultiplier * SkeletonMerchantRarityMultiplier);
            }

            if (NPC.downedBoss2)
            {
                TryAddSpawn(pool, NPCID.BartenderUnconscious, _cachedMaxValue * NonDemonTaxCollectorMultiplier);
            }

            if (spawnInfo.Player.ZoneUndergroundDesert)
            {
                TryAddSpawn(pool, NPCID.GolferRescue, _cachedMaxValue * NonDemonTaxCollectorMultiplier);
            }

            if (spawnInfo.SpiderCave)
            {
                TryAddSpawn(pool, NPCID.WebbedStylist, _cachedMaxValue * NonDemonTaxCollectorMultiplier);
            }

            if (spawnInfo.Player.ZoneBeach)
            {
                TryAddSpawn(pool, NPCID.SleepingAngler, _cachedMaxValue * NonDemonTaxCollectorMultiplier);
            }
        }
        private void TryAddSpawn(IDictionary<int, float> pool, int npcId, float maxChanceValue)
        {
            if (pool.ContainsKey(npcId)) return;

            float rand = Main.rand.NextFloat(maxChanceValue);

            if (rand <= _cachedMilestoneMultiplier)
            {
                pool.Add(npcId, 1f);
            }
        }

        private void InitializeMilestones()
        {
            _cachedMaxValue = GetMilestoneValue(MaxCharismaMilestoneIndex);
            _isInitialized = true;
        }

        private int GetMilestoneValue(int targetIndex)
        {
            if (targetIndex <= 0) return 0;

            List<int> cache = new List<int>();
            SortedSet<long> nextValues = new SortedSet<long> { 3, 7 };

            while (cache.Count < targetIndex)
            {
                long current = nextValues.Min;
                nextValues.Remove(current);

                if (current > int.MaxValue) break;

                cache.Add((int)current);
                nextValues.Add(current * 3);
                if (current % 7 == 0) nextValues.Add(current * 7);
            }

            return cache.Count >= targetIndex ? cache[targetIndex - 1] : int.MaxValue;
        }

        private float CalculateMilestoneMultiplier(long charisma)
        {
            int milestoneCount = 0;
            if (charisma >= 3) milestoneCount++;

            long p7 = 7;
            while (p7 <= charisma)
            {
                long p3 = 1;
                while (p7 * p3 <= charisma)
                {
                    milestoneCount++;
                    if (p3 > long.MaxValue / 3) break;
                    p3 *= 3;
                }
                if (p7 > long.MaxValue / 7) break;
                p7 *= 7;
            }
            return 1f + milestoneCount;
        }
    }
}