using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Wobble.Graphics;
using Wobble.Graphics.BitmapFonts;
using Wobble.Graphics.Sprites;
using Wobble.Graphics.Transformations;
using Wobble.Window;

namespace Wobble.Logging
{
    /// <summary>
    ///     A log that is drawable to the screen.
    /// </summary>
    public class DrawableLog : Sprite
    {
        /// <summary>
        ///     The text for the log.
        /// </summary>
        public SpriteTextBitmap Text { get; }

        /// <summary>
        ///     The amount of time the log has been active.
        /// </summary>
        private double TimeActive { get; set; }

        /// <summary>
        ///     If the fade out was initialized.
        /// </summary>
        private bool FadedOut { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="t"></param>
        /// <param name="level"></param>
        public DrawableLog(string t, LogLevel level)
        {
            UsePreviousSpriteBatchOptions = true;

            Text = new SpriteTextBitmap("Arial", t, 8, Color.White, Alignment.MidCenter, (int) (WindowManager.Width / 2))
            {
                Parent = this,
                UsePreviousSpriteBatchOptions = true,
                Alignment = Alignment.MidLeft,
            };

            Tint = Color.Black;
            Alpha = 0.75f;

            Size = new ScalableVector2(Text.Width + 3, Text.Height + 3);
            X = -Width;

            switch (level)
            {
                case LogLevel.Debug:
                    Tint = Color.Black;
                    break;
                case LogLevel.Warning:
                    Tint = Color.Gold;
                    break;
                case LogLevel.Important:
                    Tint = Color.DarkBlue;
                    break;
                case LogLevel.Error:
                    Tint = Color.DarkRed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            Transformations.Add(new Transformation(TransformationProperty.X, Easing.EaseOutQuint, X, 0, 300));
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (Transformations.Count == 0)
            {
                if (FadedOut)
                    Destroy();

                TimeActive += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (TimeActive > 3000 && !FadedOut)
                {
                    Transformations.Add(new Transformation(TransformationProperty.X, Easing.EaseOutQuint, X, -Width, 300));
                    FadedOut = true;
                }
            }

            base.Update(gameTime);
        }
    }
}