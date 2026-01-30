using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Charisma.Common.Systems;
using Charisma.Common.Configs;
using System.Collections.Generic;
using System;
using System.Reflection;
using Terraria.ModLoader.Config;

namespace Charisma.Common.UI
{
    public class CharismaOverlay : UIState
    {
        private UIText charismaText;
        private UIPanel area;
        private bool dragging = false;
        private Vector2 offset;
        private int _lastCharisma = -1;

        public override void OnInitialize()
        {
            area = new UIPanel();

            var config = ModContent.GetInstance<CharismaConfig>();
            area.Left.Set(config.OverlayPosition.X, 0f);
            area.Top.Set(config.OverlayPosition.Y, 0f);

            area.Width.Set(140, 0);
            area.Height.Set(40, 0);
            area.BackgroundColor = new Color(0, 0, 0, 0);
            area.BorderColor = new Color(0, 0, 0, 0);

            area.OnLeftMouseDown += DragStart;
            area.OnLeftMouseUp += DragEnd;

            charismaText = new UIText("Charisma: 0", 0.8f);
            charismaText.HAlign = 0.5f;
            charismaText.VAlign = 0.5f;

            area.Append(charismaText);
            Append(area);
        }

        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            offset = new Vector2(evt.MousePosition.X - area.Left.Pixels, evt.MousePosition.Y - area.Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            Vector2 end = evt.MousePosition;
            dragging = false;

            float newX = (float)Math.Round(end.X - offset.X);
            float newY = (float)Math.Round(end.Y - offset.Y);

            area.Left.Set(newX, 0f);
            area.Top.Set(newY, 0f);

            Recalculate();

            var config = ModContent.GetInstance<CharismaConfig>();
            config.OverlayPosition = new Vector2(newX, newY);

            typeof(ConfigManager)
                .GetMethod("Save", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                ?.Invoke(null, new object[] { config });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (dragging)
            {
                float newX = (float)Math.Round(Main.mouseX - offset.X);
                float newY = (float)Math.Round(Main.mouseY - offset.Y);

                area.Left.Set(newX, 0f);
                area.Top.Set(newY, 0f);
                Recalculate();
            }
            else
            {
                var config = ModContent.GetInstance<CharismaConfig>();
                if (area.Left.Pixels != config.OverlayPosition.X || area.Top.Pixels != config.OverlayPosition.Y)
                {
                    area.Left.Set(config.OverlayPosition.X, 0f);
                    area.Top.Set(config.OverlayPosition.Y, 0f);
                    Recalculate();
                }
            }

            int current = CharismaWorldSystem.WorldCharismaCount;
            if (charismaText != null && _lastCharisma != current)
            {
                charismaText.SetText($"Charisma: {current}");
                _lastCharisma = current;
            }
        }
    }

    public class CharismaUISystem : ModSystem
    {
        internal CharismaOverlay CharismaOverlay;
        internal UserInterface CharismaInterface;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                CharismaOverlay = new CharismaOverlay();
                CharismaInterface = new UserInterface();
                CharismaInterface.SetState(CharismaOverlay);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            CharismaInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (!ModContent.GetInstance<CharismaConfig>().ShowCharismaOverlay)
                return;

            if (!Main.playerInventory)
                return;

            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "Charisma: Overlay",
                    delegate
                    {
                        CharismaInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}