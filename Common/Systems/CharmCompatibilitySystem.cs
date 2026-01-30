using Charisma.Content.Items.Charms.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Charisma.Common.Systems
{
    public class CharmCompatibilitySystem : ModSystem
    {
        public static Dictionary<int, HashSet<int>> IncompatibleItems = new Dictionary<int, HashSet<int>>();

        public override void PostAddRecipes()
        {
            IncompatibleItems.Clear();

            Dictionary<int, HashSet<int>> parentToChildren = new Dictionary<int, HashSet<int>>();

            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.createItem.ModItem is BaseCharm parentCharm)
                {
                    int parentType = recipe.createItem.type;

                    foreach (Item ingredient in recipe.requiredItem)
                    {
                        if (ingredient.ModItem is BaseCharm)
                        {
                            if (!parentToChildren.ContainsKey(parentType))
                                parentToChildren[parentType] = new HashSet<int>();

                            parentToChildren[parentType].Add(ingredient.type);
                        }
                    }
                }
            }

            foreach (var parent in parentToChildren.Keys)
            {
                HashSet<int> allDescendants = GetAllDescendants(parent, parentToChildren);

                foreach (int child in allDescendants)
                {
                    AddConflict(parent, child);
                    AddConflict(child, parent);
                }
            }
        }

        private HashSet<int> GetAllDescendants(int parent, Dictionary<int, HashSet<int>> mapping)
        {
            HashSet<int> descendants = new HashSet<int>();

            if (mapping.ContainsKey(parent))
            {
                foreach (int child in mapping[parent])
                {
                    descendants.Add(child);

                    descendants.UnionWith(GetAllDescendants(child, mapping));
                }
            }

            return descendants;
        }

        private void AddConflict(int itemA, int itemB)
        {
            if (!IncompatibleItems.ContainsKey(itemA))
                IncompatibleItems[itemA] = new HashSet<int>();

            IncompatibleItems[itemA].Add(itemB);
        }
    }
}