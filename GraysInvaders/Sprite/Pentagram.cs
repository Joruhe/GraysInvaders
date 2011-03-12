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
    public class Pentagram : Core.Sprite
    {
        public Pentagram(Game game, ref Texture2D texture, Vector2 position)
            : base(game)
        {
            // TODO: Construct any child components here

            this.texture = texture;
            this.frameSize = new Point(120, 720);
            this.currentFrame = new Point(0, 1);
            this.position = position;
            this.sheetSize = new Point(0,1);
            this.visible = false;
            this.enable = true;
            this.die = false;
            this.collisionOffset = new Point(0, 0);

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

            base.Update(gameTime);
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
