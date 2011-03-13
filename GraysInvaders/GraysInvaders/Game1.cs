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
using GraysInvaders.Core;
using GraysInvaders.Sprite;

namespace GraysInvaders
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region VARIABLES
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch = null;


        public enum difficult
        {
            easy = 70,
            normal = 50,
            hard = 30,
            insane = 10
        }

        // Audio stuff
        private SoundEffect explosion;
        private Song backMusic;
        private Song backMusic2;

        Color colorin;

        #region GAMECOMPONENTS
        private BackgroundPixelManager backgoundManager;
        private Tank player;
        private InvasorsManager invasorsManager;
        private Shoot shooter;
        private Ufo ufo;
        private Pentagram pentagramP1;
        private Pentagram pentagramP2;
        #endregion

        #region BACKGROUND
        private Texture2D tileMap;
        #endregion

        #region TANK
        private Texture2D tank;
        #endregion

        #region INVASOR
        private Texture2D invasor;
        private String nameInvasor = "invasor_sheet";
        #endregion

        #region UFO
        private Texture2D ufo_t;
        #endregion

        #region Pentagram
        private Texture2D pentagam_sheet;
        #endregion

        #region Tempo

        #endregion

        /*
        #region SHOOT
        private Texture2D shoot;
        #endregion
        */

        #endregion

        #region STATES
        /// <summary>
        /// Scala del sprite
        /// </summary>
        protected Vector2 scale = new Vector2(1f, 1f);
        /// <summary>
        /// para seleccionar el tipo de TileMap
        /// </summary>
        protected String nameTileMap = "BarrierMap";
        /// <summary>
        /// para seleccionar el tamaño del tank
        /// </summary>
        protected String nameTank = "tank_sheet";
        /// <summary>
        /// ancho de la pantalla necesario para BackgroundPixelManager
        /// </summary>
        private bool screen = false;

        private bool ufoBonus = false;

        float ASPECTHD;
        float ASPECTSD;
        float ASPECTWIDTHRATIO;
        float ASPECTHEIGHTRATIO;
        float VIEWPORTWIDTH;
        float VIEWPORTHEIGHT;
        float ASPECTRATIO;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            
          //  graphics.ToggleFullScreen();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            #region ISWIDESCREEN
            if (!GraphicsAdapter.DefaultAdapter.IsWideScreen)
            {
                if (graphics.GraphicsDevice.DisplayMode.AspectRatio == 1.25f)
                {
                    ASPECTHD = 16.0f / 9.0f;
                    ASPECTSD = 5f / 4f;
                    ASPECTWIDTHRATIO = 1280f / 1280f;
                    ASPECTHEIGHTRATIO = 1024f / 720f;
                    ASPECTRATIO = ASPECTSD / ASPECTHD;
                    VIEWPORTHEIGHT = 720f;
                    VIEWPORTWIDTH = 1280f;
                }
                else
                {
                    ASPECTHD = 16.0f / 9.0f;
                    ASPECTSD = 4f / 3f;
                    ASPECTWIDTHRATIO = 1024f / 1280f;
                    ASPECTHEIGHTRATIO = 1024f / 720f;
                    ASPECTRATIO = ASPECTSD / ASPECTHD;
                    VIEWPORTHEIGHT = 720f;
                    VIEWPORTWIDTH = 1280;
                }

            }
            #endregion 

            #region BACKGROUND
            tileMap = Content.Load<Texture2D>(@"Barrier\" + nameTileMap);
            backgoundManager = new BackgroundPixelManager(this, ref tileMap, screen);
            Components.Add(backgoundManager);
            #endregion

            #region TANK
            tank = Content.Load<Texture2D>(@"Tank\" + nameTank);
            player = new Tank(this, ref tank, screen);
            Components.Add(player);
            player.PutinStartPosition();
            #endregion

            #region INVASOR
            invasor = Content.Load<Texture2D>(@"Invasors\" + nameInvasor);
            invasorsManager = new InvasorsManager(this, ref invasor, difficult.normal.ToString());
            Components.Add(invasorsManager);
            #endregion

            #region UFO
            ufo_t = Content.Load<Texture2D>(@"Ufo\" + "ufo_sheet");
            ufo = new Ufo(this, ref ufo_t);
            Components.Add(ufo);
            ufo.PutinStartPosition();
            #endregion

            #region PENTAGRAM
            pentagam_sheet = Content.Load<Texture2D>(@"Pentagram\pentagram_sheet");
            pentagramP1 = new Pentagram(this, ref pentagam_sheet, Vector2.Zero);
            pentagramP2 = new Pentagram(this, ref pentagam_sheet, new Vector2(1280 - 120,0));
            Components.Add(pentagramP1);
            Components.Add(pentagramP2);
            #endregion

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// 
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(this.GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);

            // Load audio elements
            explosion = Content.Load<SoundEffect>(@"music\robotz");
            backMusic = Content.Load<Song>(@"music\rush");
            //backMusic2 = Content.Load<Song>(@"music\w");

            // Play the background music
            //MediaPlayer.Play(backMusic);
            //MediaPlayer.Play(backMusic2);

            //Services.AddService(typeof(Vector2), scale);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            #region PADCONTROL
            // Allows the game to exit
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            /*if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                colorin = Color.Green;
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
                colorin = Color.Red;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed)
                colorin = Color.Yellow;
            if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed)
                colorin = Color.Blue;
            if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed)
                colorin = Color.Violet;
            if (GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed)
                colorin = Color.Purple;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                colorin = Color.White;
            if (GamePad.GetState(PlayerIndex.One).Triggers.Right == 1f)
                colorin = Color.Pink;
            if (GamePad.GetState(PlayerIndex.One).Triggers.Left == 1f)
                colorin = Color.Orange;
       /*     if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
                colorin = Color.Orchid;
            if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
                colorin = Color.Maroon;
            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
                colorin = Color.Gold;
            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
                colorin = Color.Cyan;*/
            #endregion
            // TODO: Add your update logic here

            DoLogic();

            int count = 0;
            int countInvasor = 0;
            int countComplete = 0;
            int countShoot = 0;
            int countTank = 0;
            List<Invasor> list = new List<Invasor>();

            #region CONTADOR COMPONENTES
            foreach (GameComponent gc in Components)
            {
                if (gc is Background)
                    count++;
                if (gc is Shoot)
                    countShoot++;
                if (gc is Invasor)
                {
                    countInvasor++;
                    if (countInvasor == 1 
                        && ((Invasor)gc).die)
                    {
                       // countInvasor = 0;
                    //    list.Add((Invasor)gc);
                    }    
                }
                if (gc is Tank)
                {
                    countTank++;
                }
            }
            if (countTank == 0)
            {
                player = new Tank(this, ref tank, screen);
                Components.Add(player);
                player.PutinStartPosition();
            }

            if (count == 0)
                Exit();
            if (countInvasor == 0)
            {
                //countComplete++;
                countComplete = 1;
                invasorsManager.SheetInvaders(this, ref invasor);
                #region SELECT DIFFICULTY
                switch (countComplete)
                {
                    case 1:
                        {
                            invasorsManager.SelectDifficult(difficult.normal.ToString());
                            break;
                        }
                    case 2:
                        {
                            invasorsManager.SelectDifficult(difficult.hard.ToString());
                            break;
                        }
                    default:
                        {
                            invasorsManager.SelectDifficult(difficult.insane.ToString());
                            break;
                        }
                }
                #endregion
            }

            #endregion

            if (ufoBonus)
                UfoBonusYes();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            #region MATRIX TRANSFORM
            /*        // TODO: Add your drawing code here
             float aspect = 16.0f / 9.0f;

GraphicsDevice.Clear(Color.Black);
Matrix transform = Matrix.CreateScale(1, GraphicsDevice.Viewport.AspectRatio / aspect, 1) * Matrix.CreateTranslation(0, (GraphicsDevice.Viewport.Height - GraphicsDevice.Viewport.Height / aspect) / 4.0f, 0);
spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, transform);

*/
            if (!GraphicsAdapter.DefaultAdapter.IsWideScreen)
            {
                GraphicsDevice.Clear(Color.Black);
                Matrix transform = Matrix.CreateScale(ASPECTWIDTHRATIO, ASPECTWIDTHRATIO, 1) *
                    Matrix.CreateTranslation(0, (VIEWPORTHEIGHT / ASPECTHD) / 4f, 0);

                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, transform);
            }
            else
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            }

            Texture2D planet = Content.Load<Texture2D>(@"backdrop");
            spriteBatch.Draw(planet, Vector2.Zero, new Rectangle(0, 0, 1280, 720), Color.White, 0f, Vector2.Zero, 1f,
                SpriteEffects.None, 1f);
            base.Draw(gameTime);

            spriteBatch.End();
            #endregion
        }

        private void DoLogic()
        {
            #region VARIABLES
            List<Shoot> listShoot = new List<Shoot>();
            List<Shoot> listCollShoot = new List<Shoot>();
            List<Invasor> listCollInvasor = new List<Invasor>();
            List<Background> listCollBackground = new List<Background>();
            Tank collTank = null;
            Ufo collUfo = null;
            int countInvasor = 0;
            Invasor invasorDied = null;
            #endregion

            foreach (GameComponent gc in Components)
            {
                if(gc is Shoot)
                    if (((Shoot)gc).die)
                    {
                        listCollShoot.Add((Shoot)gc);
                    }
                    else
                        listShoot.Add((Shoot)gc);
                if (gc is Invasor)
                {
                    countInvasor++;
                    if (((Invasor)gc).die)
                    {
                        invasorDied = ((Invasor)gc);
                    }
                }
                
            }

            if (countInvasor == 1 
                && invasorDied != null)
                listCollInvasor.Add(invasorDied);

            foreach (GameComponent gc in Components)
                foreach (Shoot sh in listShoot)
                {
                    #region TANK
                    if (gc is Tank)
                    {
                        if (sh.collisionRect.Intersects(((Tank)gc).collisionRectOffset) && !((Tank)gc).die)
                        {
                            collTank = (Tank)gc;
                            listCollShoot.Add(sh);
                        }
                        if (!((Tank)gc).enable && ((Tank)gc).die)
                        {
                            collTank = (Tank)gc;

                        }
                    } 

                    #endregion

                    #region INVASOR
                    if (gc is Invasor)
                    {
                        if (sh.collisionRect.Intersects(((Invasor)gc).collisionRectOffset) 
                            && !((Invasor)gc).die
                            && !((Invasor)gc).shooted)
                        {
                            ((Invasor)gc).shooted = true;
                            listCollInvasor.Add((Invasor)gc);
                            listCollShoot.Add(sh);
                            explosion.Play(.5f,0f,0f);
                        }
                        if (!((Invasor)gc).enable && ((Invasor)gc).die)
                        {
                            listCollInvasor.Add((Invasor)gc);
                        }
                    }
                    #endregion

                    #region BACKGROUND 
                    if (gc is Background)
                    {
                        if (sh.collisionRect.Intersects(((Background)gc).collisionRect))
                        {
                            listCollBackground.Add((Background)gc);
                            listCollShoot.Add(sh);
                        }
                    }
                    #endregion

                    #region UFO
                    if (gc is Ufo)
                    {
                        if (sh.collisionRect.Intersects(((Ufo)gc).collisionRectOffset) && !((Ufo)gc).die)
                        {
                            collUfo = (Ufo)gc;
                            listCollShoot.Add(sh);
                        }
                        if (!((Ufo)gc).enable && ((Ufo)gc).die)
                        {
                            collUfo = (Ufo)gc;
                            ufoBonus = true;
                        }
                    }
                    #endregion

                    #region SHOOT
                    if (gc is Shoot)
                    {
                        if (sh.collisionRect.Intersects(((Shoot)gc).collisionRect)
                            && sh.position != ((Shoot)gc).position)
                        {
                            listCollShoot.Add(sh);
                            listCollShoot.Add((Shoot)gc);
                        }
                    }
                    #endregion
                }

            #region INTERSECTIONS
            if (collTank != null)
                player.Intesects(collTank);
            if (listCollInvasor.Count > 0)
                foreach (Invasor ivr in listCollInvasor)
                    invasorsManager.Intesects(ivr, listCollInvasor);
            if (listCollBackground.Count > 0)
                foreach (Background bg in listCollBackground)
                    backgoundManager.Intesects(bg);
            if (listCollShoot.Count > 0)
                foreach (Shoot sh in listCollShoot)
                    sh.Intesects();
            if (collUfo != null)
                ufo.Intesects(collUfo);
            #endregion
        }

        private void UfoBonusYes()
        {
            List<Background> listBack = new List<Background>();
            foreach (GameComponent gc in Components)
                if (gc is Background)
                {
                    listBack.Add((Background)gc);
                }

            foreach (Background bg in listBack)
                Components.Remove(bg);
            backgoundManager.LoadPixelBackgroud();
            ufoBonus = false;
            Components.Add(ufo = new Ufo(this, ref ufo_t));
            ufo.PutinStartPosition();
        }
    }
}
