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
    class Tank : Core.Sprite
    {
        #region VARIABLES
        /// <summary>
        /// Screen Area
        /// </summary>
        protected Rectangle screenBounds;
        /// <summary>
        /// Last key pressed
        /// </summary>
        protected KeyboardState oldKeyBoardState;
        /// <summary>
        /// Last button pressed
        /// </summary>
        protected GamePadState oldGamePadState;

        // Audio stuff
        private SoundEffect pum;

        #region SHOOT
        private Texture2D shoot;
        Shoot shooter;
        #endregion

        #endregion

        #region STATES
        /// <summary>
        /// width of screen
        /// </summary>
        private bool screen = false;
        /// <summary>
        /// Banda superior e inferior para mantener la proporción.
        /// </summary>
        private const int BANDA = 96;
        /// <summary>
        /// Tamaño del spirte Shoot para que al disparar con colisiones con el tanke.
        /// </summary>
        private const UInt16 SHOOTSIZE = 8;
        /// <summary>
        /// tamaño màximo del sheet para que cuando se dibuje la destrucción
        /// </summary>
        private const UInt16 MAXSIZESHEET = 9;
        /// <summary>
        /// margen lateral de separación en la colocación
        /// </summary>
        private const int SCREENWIDTH = 120;
        private const int SCREENTHEND = 1160; 
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game"></param>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        public Tank(Game game, ref Texture2D texture, bool screen)
            : base(game)
        {
            #region CONSTRUCTOR
            base.texture = texture;
            this.frameSize = new Point(64, 64);
            this.currentFrame = new Point(0, 0);
            this.position = Vector2.Zero;
            this.sheetSize = new Point(0, 0);
            this.visible = true;
            this.enable = true;
            this.screen = screen;
            this.die = false;
            this.collisionOffset = new Point(0,32);

            this.millisecondsPerFrame = 60;

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

        public Tank(Game game, Tank tank) 
            : base(game)
        {
            #region CONSTRUCTRO
            this.texture = tank.texture;
            this.frameSize = new Point(64, 64);
            this.currentFrame = tank.currentFrame;
            this.position = tank.position;
            this.sheetSize = new Point(MAXSIZESHEET, 0);
            this.visible = true;
            this.enable = true;
            this.die = true;
            this.collisionOffset = new Point(0, 32);
            this.screenBounds = tank.screenBounds;

            this.millisecondsPerFrame = 60;
            #endregion
        }

        /// <summary>
        /// Put the ship in your start position in the screen
        /// </summary>
        public void PutinStartPosition()
        {
            position.X = (SCREENTHEND + this.frameSize.X) / 2;//screenBounds.Width / 2;
            position.Y = 720 - 40 - (frameSize.Y);
            /*for (int i = 0; i < 8; i++)
            {
                Background bg = new Background(base.Game,
                Game.Content.Load<Texture2D>(@"Barrier\4278190080"), 
                new Vector2(position.X + i*8,position.Y));
                Game.Components.Add(bg);
            }*/
        }

        protected override void LoadContent()
        {

            // Load audio elements
            pum = Game.Content.Load<SoundEffect>(@"music\LASER");
            base.LoadContent();
        }

        /// <summary>
        /// Update the Tank position
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (enable)
            {
                #region UPDATEPOSITION
                if (!die)
                {
                    GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                    KeyboardState keyboardState = Keyboard.GetState();

                    bool left, right, shootGreen;

                    shootGreen = (oldKeyBoardState.IsKeyDown(Keys.S) &&
                        (keyboardState.IsKeyUp(Keys.S)));

                    //                left = (oldKeyBoardState.IsKeyDown(Keys.Left) &&
                    //                    (keyboardState.IsKeyUp(Keys.Left)));

                    shootGreen |= (oldGamePadState.Buttons.A == ButtonState.Pressed) &&
                        (gamePadState.Buttons.A == ButtonState.Released);

                    //              left |= (oldGamePadState.DPad.Left == ButtonState.Pressed) &&
                    //                (gamePadState.DPad.Left == ButtonState.Released);

                    left = (gamePadState.DPad.Left == ButtonState.Pressed ||
                        keyboardState.IsKeyDown(Keys.Left));
                    right = (gamePadState.DPad.Right == ButtonState.Pressed ||
                        keyboardState.IsKeyDown(Keys.Right));

                    if (shootGreen)
                    {
                        #region SHOOT
                        //if (!Game.Components.Contains((IGameComponent)shooter))
                        //{
                        shoot = Game.Content.Load<Texture2D>(@"Shoot\" + "shoot_sheet");
                        shooter = new Shoot(Game, ref shoot,
                            new Vector2(position.X + frameSize.X / 2, position.Y - SHOOTSIZE), 1);
                        Game.Components.Add(shooter);
                        pum.Play(.5f,0f,0f);
                        //}
                        #endregion

                    }

                    if (left)
                    {
                        this.position.X -= 3;//(float)Math.PI;
                    }

                    if (right)
                    {
                        this.position.X += 3;//(float)Math.PI;
                    }

                    oldKeyBoardState = keyboardState;
                    oldGamePadState = gamePadState;
                }
                #endregion

                #region SCREENBOUNDSCONTROL
                if (position.X < SCREENWIDTH)//screenBounds.Left)
                {
                    position.X = SCREENWIDTH;//screenBounds.Left;
                }
                if (position.X > SCREENTHEND - frameSize.X)//screenBounds.Width - frameSize.X)
                {
                    position.X = SCREENTHEND - frameSize.X; //- frameSize.X;//screenBounds.Width - frameSize.X;
                }
                if (position.Y < screenBounds.Top)
                {
                    position.Y = screenBounds.Top;
                }
                if (position.Y > screenBounds.Height - frameSize.Y)
                {
                    position.Y = screenBounds.Height - frameSize.Y;
                }
                #endregion
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw the Tank
        /// </summary>
        /// <param name="gameTime"></param>
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

            if (currentFrame.X == 8)
            {
                visible = false;
                enable = false;
            }

            base.Draw(gameTime);
        }

        public void Intesects(Tank tank)
        {
            if (!tank.die)
            {
                Tank tankDie = new Tank(Game, tank);
                Game.Components.Add(tankDie);
            }

            Game.Components.Remove(tank);
        }
    }
}
