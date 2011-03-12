using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace GraysInvaders
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Shoot : Core.Sprite
    {
        #region STATES
        /// <summary>
        /// direccion del disparo
        /// </summary>
        protected int direction;
        #endregion

        public Shoot(Game game, ref Texture2D texture, Vector2 position, int direction)
            : base(game)
        {
            #region CONSTRUCTOR

            this.texture = texture;
            this.direction = direction;
            this.speed = new Vector2(0,8);
            this.frameSize = new Point(4, 8);
            this.currentFrame = new Point(0, 0);
            this.sheetSize = new Point(4, 0);
            this.position = position;

            this.millisecondsPerFrame = 60;

            visible = true;
            enable = true;
            #endregion
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (enable)
            {
                if (speed.Y < 0)
                {
                    position.Y += speed.Y * direction;
                }
                if (speed.Y > 0)
                {
                    position.Y -= speed.Y * direction;
                }

                #region SCREENBOUNDSCONTROL
                if (position.Y < 0)
                {
                    die = true;
                }
                if (position.Y > 1280 - frameSize.Y)
                {
                    die = true;
                }
                #endregion
            }
            
            base.Update(gameTime);
        }

        public void Intesects()
        {
            enable = false;
            Game.Components.Remove(this);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sBatch =
                (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            if (visible)
            {
                sBatch.Draw(this.texture, this.position, new Rectangle(
                    currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y), Color.White);
            }

            base.Draw(gameTime);
        }
    }
}
