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
    public class Invasor : Core.Sprite
    {
        #region VARIABLES
        #region SHOOT
        /// <summary>
        /// textura del disparo que se pasará por referencia
        /// </summary>
        private Texture2D shoot;
        /// <summary>
        /// variable Shoot, para el disparo
        /// </summary>
        Shoot shooter;
        #endregion
        #endregion

        #region STATES
        /// <summary>
        /// Random para la frecuencia de disparos
        /// </summary>
        protected Random random;
        /// <summary>
        /// tamaño maximo de sheet para cuando se anima la destrucción
        /// </summary>
        private const int MAXSHEETSIZE = 7;
        /// <summary>
        /// tamaño del board
        /// </summary>
        private const int BOARD = 16;
        /// <summary>
        /// margen de arriba de separación en la colocacion de los invasores
        /// </summary>
        private const int MARGENHEIGHT = 128;
        /// <summary>
        /// margen lateral de separación en la colocación
        /// </summary>
        private const int MARGENWIDTH = 216;
        /// <summary>
        /// margen lateral de separación en la colocación
        /// </summary>
        private const int SCREENWIDTH = 120;
        private const int SCREENTHEND = 1160;
        private const int LINESPACE = 7;
        /// <summary>
        /// tiempo hasta el siguiente movimiento
        /// </summary>
        protected int timeSinceLastMove = 0;
        /// <summary>
        /// milisegons necsario para el siguiente movimiento
        /// </summary>
        public float timeToMove;
        protected int timeSicenLastShoot = 0;
        public int timeToShoot;
        /// <summary>
        /// espacio hacia la izquierda necesario para encontrar el limite
        /// </summary>
        public float spaceLeft;
        /// <summary>
        /// espacio hacia la derecha necesario para encontrar el limite
        /// </summary>
        public float spaceRight = 32;
        /// <summary>
        /// direccion (positiva derecha, negativa izquierda) del invasor
        /// </summary>
        protected int direction = 1;
        /// <summary>
        /// dispara si verdadero
        /// </summary>
        public bool shootin = false;
        /// <summary>
        /// tamaño maximo de sheet para la animacion de la destruccion
        /// </summary>
        private const UInt16 SHOOTSIZE = 8;
        /// <summary>
        /// pobabilidad de disparo
        /// </summary>
        public int shootquick = 95;
        public float downInvasor;
        /// <summary>
        /// numero de invasor
        /// </summary>
        public int numInvader;
        public bool shooted = false;
        #endregion


        public Invasor(Game game, ref Texture2D texture, 
            int position, int INVASORSHOOTERS, int difficult)
            : base(game)
        {

            #region CONSTRUCTOR
            this.texture = texture;
            this.speed = new Vector2(1f, 0f);
            this.frameSize = new Point(64, 64);
            this.currentFrame = new Point(0, 0);
            this.sheetSize = new Point(2, 0);
            this.collisionOffset =  new Point(10,14);

            numInvader = position;
            PutinStartPosition(position, INVASORSHOOTERS);

            this.millisecondsPerFrame = 660;
            this.timeToMove = 125;
            this.timeToShoot = difficult * 25;

            visible = true;
            enable = true;

            random = new Random(this.GetHashCode());
            #endregion
        }

        public Invasor(Game game, Invasor invasor)
            :base(game)
        {
            #region CONSTRUCTOR
            this.texture = invasor.texture;
            this.speed = invasor.speed;
            this.frameSize = invasor.frameSize;
            this.currentFrame = invasor.currentFrame;
            this.sheetSize = new Point(9, 0);
            this.collisionOffset = new Point(10, 14);
            this.position = invasor.position;

            this.millisecondsPerFrame = 60;
            this.timeToMove = invasor.timeToMove;

            visible = true;
            enable = true;
            die = true;

            random = new Random((int)(DateTime.Now.Ticks));//new Random(this.GetHashCode());
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

                #region BARRIER INTERSECTION
                if (position.Y > 720 - 168 - frameSize.Y)
                {
                    enable = false;
                    Game.Exit();
                }
                #endregion

                #region RIGHT MOVE
                if (speed.X > 0)
                {
                    if (SCREENTHEND < spaceRight + frameSize.X)
                    {
                        speed = new Vector2(speed.X * -1, speed.Y);
                        position.Y += (frameSize.Y / LINESPACE);
                    }
                }
                #endregion

                #region LEFT MOVE
                if (speed.X < 0)
                {
                    if (spaceLeft < SCREENWIDTH)
                    {
                        speed = new Vector2(speed.X * -1, speed.Y);
                        position.Y += (frameSize.Y / LINESPACE);
                    }
                }
                #endregion

               timeSinceLastMove += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastMove > timeToMove)
                {
                    timeSinceLastMove = 0;

                    position.X += speed.X;
                }

                #region RANDOM SHOOT
               // timeSicenLastShoot += gameTime.ElapsedGameTime.Milliseconds;
                //if (timeSicenLastShoot > timeToShoot)
                {
                    //timeSicenLastShoot += gameTime.ElapsedGameTime.Milliseconds;
                    //if (timeSicenLastShoot > timeToShoot)
                    {
                        timeSicenLastShoot = 0;
                        //random = new Random(this.GetHashCode());
                        int ran = random.Next(0, 15000);
                        ///ANTES DE DISPARAR COMPROBAR SI COLISIONA
                        if (ran <= (shootquick) && shootin && !die) //&& WeAreYoureFriend())
                        {
                            if (!Game.Components.Contains((IGameComponent)shooter))
                            {
                                shoot = Game.Content.Load<Texture2D>(@"Shoot\" + "shoot_sheet");
                                shooter = new Shoot(Game, ref shoot,
                                    new Vector2(position.X + frameSize.X / 2,
                                        position.Y + SHOOTSIZE + frameSize.Y), -1);
                                Game.Components.Add(shooter);
                            }
                        }
                    }
                }
                #endregion


            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Permite dibujarse a s
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

                if (currentFrame.X == 8)
                {
                    visible = false;
                    enable = false;
                }
            }
                        
            base.Draw(gameTime);
        }

        /// <summary>
        /// Posiciona los invasores en su posición inicial
        /// </summary>
        /// <param name="i"></param>
        private void PutinStartPosition(int i, int INVASORSHOOTERS)
        {
            position = new Vector2((MARGENWIDTH + (frameSize.X * i) 
                % ( (frameSize.X * INVASORSHOOTERS))), 
                BOARD + MARGENHEIGHT + (frameSize.Y * (i / INVASORSHOOTERS) ));
        }

        private bool WeAreYoureFriend()
        {
            bool friend = true;

            if (speed.X*speed.X/timeToMove > 19/timeToMove && timeToMove != 0)
                foreach (GameComponent gc in Game.Components)
                {
                    if (gc is Invasor)
                    {
                        if (((Invasor)gc).position.Y >= this.position.Y + 128
                            && ((Invasor)gc).position != this.position
                            &&((((Invasor)gc).position.X + 64 >= this.position.X)
                            || ((Invasor)gc).position.X - 64 <= this.position.X))
                            return false;
                    }
                }

            return friend;
        }
    }
}
