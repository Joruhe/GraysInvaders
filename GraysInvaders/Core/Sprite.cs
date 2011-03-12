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


namespace GraysInvaders.Core
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Sprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region VARIABLES
        ///<summary>
        ///Miembro donde se carga la textura
        ///</summary>
        public Texture2D texture;
        ///<summary>
        ///Miembro de la posición del sprite respecto el (0,0)
        ///</summary>
        public Vector2 position;
        ///<summary>
        ///Miembro que contiene la posición del sprite
        ///</summary>
        ///<remarks>
        ///Se trata del tamaño real del sprite, no de la imagen que lo contiene
        ///</remarks>
        public Point frameSize;
        ///<summary>
        ///Miembro que contiene la posición actual dentrol de la imagen del sheetSize
        ///</summary>
        ///<remarks>
        ///empieza en 0 y acaba en N-1,como los vectores
        ///</remarks>
        public Point currentFrame;
        /// <summary>
        /// Posición del frame del sprite actual
        /// </summary>
        public Point sheetSize;
        /// <summary>
        /// Devulve el Rectangle del tamaño del frame del sprite
        /// </summary>
        /// <remarks>
        /// En el tetris esto es el Rectangle de uno de los 4 cuadrados que forman la figura
        /// </remarks>
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)frameSize.X,
                (int)frameSize.Y);
            }
        }

        public Rectangle collisionRectOffset
        {
            get
            {
                return new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    frameSize.X - collisionOffset.X,
                    frameSize.Y - collisionOffset.Y);
            }

            set
            {
                collisionRectOffset = value;
            }
        }

        #endregion

        #region STATES
        /// <summary>
        /// Sección en pixels que se utiliza para modificar el framSize (recortar tamaño)
        /// para la detección de colisiones contra sprites.
        /// </summary>
        /// <remarks>
        ///  Su valor siempre es 0 en el Tetris.
        /// </remarks>
        public Point collisionOffset { get; set; }
        /// <summary>
        /// Puntuación de cada sprite.
        /// </summary>
        public int scoreValue { get; protected set; }
        /// <summary>
        /// Miembro para determinar la velocidad en X e Y
        /// </summary>
        public Vector2 speed { get; set; }
        /// <summary>
        /// Milisegundos que hay que esperar para pasar al siguiente frame
        /// </summary>
        public float millisecondsPerFrame { get; set; }
        /// <summary>
        /// Tiempo en milisegundos transcurridos desde el último frame.
        /// </summary>
        public int timeSinceLastFrame = 0;
        /// <summary>
        /// Milisegundos de juego transcurridos
        /// </summary>
        public int elapsedGameTimeMilliseconds { get; protected set; }
        /// <summary>
        /// activa o desactiva la visibilidad del sprite
        /// </summary>
        protected bool visible;
        /// <summary>
        /// activva o descativa el sprite
        /// </summary>
        public bool enable;
        protected bool VerticalAnimation = false;


        public bool die = false;
        #endregion

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="game"></param>
        public Sprite(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="game"></param>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="frameSize"></param>
        /// <param name="currentFrame"></param>
        /// <param name="sheetSize"></param>
        public Sprite(Game game, ref Texture2D texture, Vector2 position, Point frameSize, Point currentFrame,
            Point sheetSize)
            : base(game)
        {
            // TODO: Construct any child components here
            this.texture = texture;
            this.position = position;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
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

            elapsedGameTimeMilliseconds = gameTime.ElapsedGameTime.Milliseconds;
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;

                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    if (VerticalAnimation)
                    {
                        ++currentFrame.Y;
                        if (currentFrame.Y >= sheetSize.Y)
                        {
                            currentFrame.Y = 0;
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
