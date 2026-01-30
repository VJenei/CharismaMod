using Microsoft.Xna.Framework;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Charisma.Common.Configs
{
    public class CharismaConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("UserInterface")]

        [DefaultValue(true)]
        public bool EnableIntroSequence;

        [DefaultValue(true)]
        public bool ShowCharismaOverlay;

        [Range(0f, 4000f)]
        [Increment(1f)]
        [DefaultValue(typeof(Vector2), "322, 252")]
        public Vector2 OverlayPosition;
    }
}