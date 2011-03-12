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


namespace GraysInvaders.Sprite
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Ufo : Core.Sprite
    {
        #region VARIABLES
        protected Rectangle screenBounds;
        Random random;
        #endregion

        #region STATES
        private const int MAXSHEETSIZE = 9;
        private int direction = 1;
        private bool go = false;
        /// <summary>
        /// tiempo hasta el siguiente movimiento
        /// </summary>
        protected int timeSinceLastMove = 0;
        /// <summary>
        /// milisegons necsario para el siguiente movimiento
        /// </summary>
        public float timeMove = 1000;
        #endregion

        public Ufo(Game game, ref Texture2D texture)
            : base(game)
        {
            #region CONSTRUCTOR
            base.texture = texture;
            this.frameSize = new Point(64, 64);
            this.currentFrame = new Point(0, 0);
            this.position = Vector2.Zero;
            this.sheetSize = new Point(0, 0);
            this.visible = false;
            this.enable = true;
            this.collisionOffset = new Point(2, 7);

            this.millisecondsPerFrame = 60;
            random = new Random((int)(DateTime.Now.Ticks));//this.GetHashCode());

#if XBOX360
            // On the 360, we need to be careful about the TV's "safe" area.
            screenBounds = new Rectangle(
                (int)(Game.Window.ClientBounds.Width * 0.03f),
                (int)(Game.Window.ClientBounds.Height * 0.03f),
                Game.Window.ClientBounds.Width -
                (int)(Game.Window.ClientBounds.Width * 0.03f),
                Game.Window.ClientBounds.Height -
                (int)(Game.Window.ClientBounds.Height * 0.03f));
#else
            screenBounds = new Rectangle(0,0,
            Game.Window.ClientBounds.Width,
            Game.Window.ClientBounds.Height);
#endif
            #endregion
        }

        public Ufo(Game game, Ufo ufo)
            : base(game)
        {
            base.texture = ufo.texture;
            this.frameSize = new Point(64, 64);
            this.currentFrame = ufo.currentFrame;
            this.position = ufo.position;
            this.sheetSize = new Point(MAXSHEETSIZE, 0);
            this.visible = true;
            this.enable = true;
            this.die = true;
            this.collisionOffset = new Point(2, 7);

            this.millisecondsPerFrame = 60;
            random = new Random((int)(DateTime.Now.Ticks));//this.GetHashCode());
        }

        public void PutinStartPosition()
        {
            if (random.Next(100) > 50)
            {
                direction = -1;
                position.X = 1280 + frameSize.X;
                position.Y = 0 + frameSize.Y;
            }
            else
            {
                direction = 1;
                position.X = 0 - frameSize.X;
                position.Y = 0 + frameSize.Y;
            }
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
                //random = new Random((int)(DateTime.Now.Ticks));//this.GetHashCode());
                int count = 0;

                foreach (GameComponent gc in Game.Components)
                    if (gc is Invasor)
                        count++;
                timeSinceLastMove += gameTime.ElapsedGameTime.Milliseconds;
                if(timeSinceLastMove > timeMove)
                {
                    timeSinceLastMove = 0;
                    if (this.random.Next(0, 100) < 1)
                    {
                        go = true;
                        visible = true;
                    }
                }

                if (go)
                {
                    if (direction > 0)
                    {
                        position.X += 5;
                        if (position.X > 1280)
                        {
                            go = false;
                            PutinStartPosition();
                        }
                    }
                    else
                    {
                        position.X -= 5;
                        if (position.X < 0 - frameSize.X)
                        {
                            go = false;
                            PutinStartPosition();
                        }
                    }
                }
            }


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sBatch =
              (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            if (visible)
            {
                sBatch.Draw(this.texture, new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    frameSize.X,
                    frameSize.Y), new Rectangle(
                    currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y), Color.White,0f,Vector2.Zero,SpriteEffects.None,.9f);

                if (currentFrame.X == 8)
                {
                    visible = false;
                    enable = false;
                }
            }
            
            base.Draw(gameTime);
        }

        public void Intesects(Ufo ufo)
        {
            if (!ufo.die)
            {
                Ufo ufoDie = new Ufo(Game, ufo);
                Game.Components.Add(ufoDie);
            }

            Game.Components.Remove(ufo);
        }
    }
}
