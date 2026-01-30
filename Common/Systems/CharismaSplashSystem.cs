using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Charisma.Common.Configs;

namespace Charisma.Common.Systems
{
    public class CharismaSplashSystem : ModSystem
    {
        private const int TotalDuration = 260;
        private const int AnimationStart = 210;
        private const int ImpactTime = 160;
        private const int FadeOutStart = 60;
        private const int FadeOutDuration = 50;

        private const float TextScale = 1f;
        private const string SplashText = "Charisma: Enabled";

        private int _timer = 0;
        private float _shakeIntensity = 0f;
        private bool _isActive = false;

        private struct VisualParticle
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public float Scale;
            public float Alpha;
            public float Rotation;
            public float RotationSpeed;
            public Color Color;
        }
        private List<VisualParticle> _particles = new();

        public override void OnWorldLoad()
        {
            if (!ModContent.GetInstance<CharismaConfig>().EnableIntroSequence)
            {
                _isActive = false;
                return;
            }

            _timer = TotalDuration;
            _shakeIntensity = 0f;
            _isActive = true;
            _particles.Clear();
        }

        public override void PostUpdateEverything()
        {
            if (!_isActive) return;

            if (_timer > 0)
            {
                _timer--;
                UpdateSequenceLogic();
                UpdateParticles();
            }
            else
            {
                _isActive = false;
            }

            if (_shakeIntensity > 0.1f)
                _shakeIntensity *= 0.90f;
            else
                _shakeIntensity = 0f;
        }
        private void UpdateSequenceLogic()
        {
            if (_timer > ImpactTime && _timer < AnimationStart)
            {
                if (_timer % 5 == 0)
                {
                    float progress = 1f - ((float)(_timer - ImpactTime) / (AnimationStart - ImpactTime));
                    float pitch = -0.6f + (progress * 1.2f);
                    SoundEngine.PlaySound(SoundID.MenuTick with { Volume = 0.4f, Pitch = pitch, PitchVariance = 0.1f });
                }
            }

            if (_timer == ImpactTime)
            {
                _shakeIntensity = 30f;
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode with { Volume = 1.0f, Pitch = -0.4f });
                SoundEngine.PlaySound(SoundID.Item29 with { Volume = 0.7f, Pitch = 0.2f });

                SpawnImpactParticles();
            }
        }

        private void SpawnImpactParticles()
        {
            Viewport viewport = Main.graphics.GraphicsDevice.Viewport;
            Vector2 screenCenter = new Vector2(viewport.Width / 2f, viewport.Height / 2f);

            for (int i = 0; i < 80; i++)
            {
                Vector2 dir = Main.rand.NextVector2Circular(1f, 1f);
                if (dir == Vector2.Zero) dir = Vector2.UnitY;
                dir.Normalize();

                float speed = Main.rand.NextFloat(8f, 25f);
                Color sparkColor = Color.Lerp(new Color(255, 255, 200), new Color(255, 215, 60), Main.rand.NextFloat());

                _particles.Add(new VisualParticle
                {
                    Position = screenCenter,
                    Velocity = dir * speed,
                    Scale = Main.rand.NextFloat(0.6f, 1.8f),
                    Alpha = 1.0f,
                    Rotation = Main.rand.NextFloat(6.28f),
                    RotationSpeed = Main.rand.NextFloat(-0.3f, 0.3f),
                    Color = sparkColor
                });
            }

            for (int i = 0; i < 40; i++)
            {
                Vector2 dir = Main.rand.NextVector2Circular(1f, 1f);
                if (dir == Vector2.Zero) dir = Vector2.UnitY;
                dir.Normalize();

                float speed = Main.rand.NextFloat(3f, 12f);
                Color glowColor = Color.Lerp(new Color(255, 215, 60), new Color(255, 160, 40), Main.rand.NextFloat());

                _particles.Add(new VisualParticle
                {
                    Position = screenCenter,
                    Velocity = dir * speed,
                    Scale = Main.rand.NextFloat(2.0f, 4.0f),
                    Alpha = 0.9f,
                    Rotation = Main.rand.NextFloat(6.28f),
                    RotationSpeed = Main.rand.NextFloat(-0.15f, 0.15f),
                    Color = glowColor
                });
            }
        }

        private void UpdateParticles()
        {
            for (int i = _particles.Count - 1; i >= 0; i--)
            {
                var p = _particles[i];
                p.Velocity.Y += 0.09f;
                p.Position += p.Velocity;
                p.Velocity *= 0.92f;
                p.Alpha *= 0.94f;
                p.Rotation += p.RotationSpeed;
                p.Scale *= 0.97f;
                _particles[i] = p;
                if (p.Alpha <= 0.01f) _particles.RemoveAt(i);
            }
        }

        public override void ModifyScreenPosition()
        {
            if (_isActive && _shakeIntensity > 0f)
            {
                Main.screenPosition += Main.rand.NextVector2Circular(_shakeIntensity, _shakeIntensity);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (!_isActive) return;

            int layerIndex = layers.Count;
            layers.Insert(layerIndex, new LegacyGameInterfaceLayer(
                "Charisma: Cinematic Intro",
                delegate
                {
                    DrawCinematicSequence(Main.spriteBatch);
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }

        private void DrawCinematicSequence(SpriteBatch sb)
        {
            Viewport viewport = Main.graphics.GraphicsDevice.Viewport;
            Vector2 center = new Vector2(viewport.Width / 2f, viewport.Height / 2f);

            float masterFade = Utils.GetLerpValue(FadeOutStart - FadeOutDuration, FadeOutStart, _timer, true);
            float buildupAlpha = Utils.GetLerpValue(AnimationStart, ImpactTime, _timer, true);
            float backgroundAlpha = buildupAlpha * masterFade;

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

            if (backgroundAlpha > 0f)
            {
                Texture2D vignetteTex = TextureAssets.Extra[ExtrasID.SharpTears].Value;
                Vector2 origin = vignetteTex.Size() / 2f;
                Vector2 scale = new Vector2(viewport.Width, viewport.Height) / vignetteTex.Size() * 2.5f;

                Color vColor = Color.Black * backgroundAlpha * 0.85f;
                sb.Draw(vignetteTex, center, null, vColor, 0f, origin, scale, SpriteEffects.None, 0f);
            }

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

            DrawPolishedSunburst(sb, viewport, backgroundAlpha);
            DrawPolishedParticles(sb);

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);

            if (_timer > 0)
            {
                DrawAnimatedText(sb, masterFade);
            }

            float flashStrength = Utils.GetLerpValue(ImpactTime - 15, ImpactTime, _timer, true);
            if (_timer <= ImpactTime && flashStrength > 0)
            {
                sb.End();
                sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

                Texture2D flashTex = TextureAssets.Extra[ExtrasID.SharpTears].Value;
                Vector2 origin = flashTex.Size() / 2f;
                Vector2 scale = new Vector2(viewport.Width, viewport.Height) / flashTex.Size() * 3f;

                sb.Draw(flashTex, center, null, Color.White * flashStrength, 0f, origin, scale, SpriteEffects.None, 0f);
            }
        }

        private void DrawPolishedSunburst(SpriteBatch sb, Viewport viewport, float opacity)
        {
            if (opacity <= 0f) return;

            Texture2D texture = TextureAssets.Extra[ExtrasID.VortexBlack].Value;
            Vector2 center = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            Vector2 origin = texture.Size() / 2f;

            float time = (float)Main.timeForVisualEffects;

            int rayCount1 = 16;
            for (int i = 0; i < rayCount1; i++)
            {
                float rot = (time * 0.01f) + (MathHelper.TwoPi / rayCount1) * i;
                float scaleMult = 1f + (float)Math.Sin(time * 0.05f + i) * 0.2f;
                Vector2 scale = new Vector2(1.0f, 6f) * scaleMult;
                Color color = new Color(255, 215, 60, 0) * 0.3f * opacity;
                sb.Draw(texture, center, null, color, rot, origin, scale, SpriteEffects.None, 0f);
            }

            int rayCount2 = 10;
            for (int i = 0; i < rayCount2; i++)
            {
                float rot = -(time * 0.015f) + (MathHelper.TwoPi / rayCount2) * i + MathHelper.Pi / rayCount2;
                float scaleMult = 1f + (float)Math.Cos(time * 0.04f + i) * 0.2f;
                Vector2 scale = new Vector2(1.8f, 4.5f) * scaleMult;
                Color color = new Color(255, 160, 50, 0) * 0.2f * opacity;
                sb.Draw(texture, center, null, color, rot, origin, scale, SpriteEffects.None, 0f);
            }
        }

        private void DrawPolishedParticles(SpriteBatch sb)
        {
            Texture2D tex = TextureAssets.Extra[ExtrasID.SharpTears].Value;
            Vector2 origin = tex.Size() / 2f;

            foreach (var p in _particles)
            {
                Vector2 scale = new Vector2(p.Scale * 0.12f);
                sb.Draw(tex, p.Position, null, p.Color * p.Alpha, p.Rotation, origin, scale, SpriteEffects.None, 0f);
            }
        }

        private void DrawAnimatedText(SpriteBatch sb, float masterFade)
        {
            if (masterFade <= 0f) return;

            DynamicSpriteFont font = FontAssets.DeathText.Value;
            string text = SplashText;

            float formationProgress = Utils.GetLerpValue(AnimationStart, ImpactTime, _timer, true);
            float easedFormation = (float)Math.Pow(formationProgress, 3);

            float currentSpacing = MathHelper.Lerp(25f, 0f, easedFormation);

            float totalWidth = 0f;
            for (int i = 0; i < text.Length; i++)
            {
                totalWidth += font.MeasureString(text[i].ToString()).X * TextScale;
                if (i < text.Length - 1) totalWidth += currentSpacing;
            }

            Vector2 centerScreen = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            Vector2 startPos = centerScreen - new Vector2(totalWidth / 2, (font.MeasureString("A").Y * TextScale) / 2);

            float currentX = startPos.X;

            for (int i = 0; i < text.Length; i++)
            {
                string charStr = text[i].ToString();
                Vector2 charSize = font.MeasureString(charStr) * TextScale;
                Vector2 basePos = new Vector2(currentX, startPos.Y);

                float charAlpha = Math.Min(1f, easedFormation * 5f) * masterFade;

                Vector2 shakeOffset = Vector2.Zero;
                if (_shakeIntensity > 0) shakeOffset = Main.rand.NextVector2Circular(_shakeIntensity, _shakeIntensity);

                if (_shakeIntensity > 1f && formationProgress >= 0.9f && masterFade > 0.9f)
                {
                    float offset = _shakeIntensity * 0.5f;
                    sb.DrawString(font, charStr, basePos + shakeOffset + new Vector2(-offset, 0), new Color(255, 0, 0, 0) * 0.5f * charAlpha, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);
                    sb.DrawString(font, charStr, basePos + shakeOffset + new Vector2(offset, 0), new Color(0, 0, 255, 0) * 0.5f * charAlpha, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);
                }

                Color charColor = new Color(255, 215, 60);
                if (formationProgress >= 0.95f && masterFade == 1f) charColor = Color.Lerp(charColor, Color.White, 0.8f);

                if (charAlpha > 0.05f)
                {
                    float spread = 2f;
                    Color outlineColor = Color.Black * charAlpha;
                    sb.DrawString(font, charStr, basePos + shakeOffset + new Vector2(-spread, 0), outlineColor, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);
                    sb.DrawString(font, charStr, basePos + shakeOffset + new Vector2(spread, 0), outlineColor, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);
                    sb.DrawString(font, charStr, basePos + shakeOffset + new Vector2(0, -spread), outlineColor, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);
                    sb.DrawString(font, charStr, basePos + shakeOffset + new Vector2(0, spread), outlineColor, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);
                }

                sb.DrawString(font, charStr, basePos + shakeOffset, charColor * charAlpha, 0f, Vector2.Zero, TextScale, SpriteEffects.None, 0f);
                currentX += charSize.X + currentSpacing;
            }
        }
    }
}