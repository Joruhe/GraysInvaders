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
    public class BackgroundPixelManager : Microsoft.Xna.Framework.GameComponent
    {
        #region VARIABLES
        /// <summary>
        /// textura del TileMap
        /// </summary>
        private Texture2D texture;
        #endregion

        #region STATES
        /// <summary>
        /// array del color del TileMap
        /// </summary>
        uint[] bgColorData { get; set; }
        /// <summary>
        /// width of screen
        /// </summary>
        UInt16 screen { get; set; }
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="game"></param>
        /// <param name="texture"></param>
        /// <param name="screen"></param>
        public BackgroundPixelManager(Game game, ref Texture2D texture, bool screen)
            : base(game)
        {
            // TODO: Construct any child components here
            this.texture = texture;
            this.screen = 1280;
            LoadPixelBackgroud();
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
            base.Update(gameTime);
        }

        public void Intesects(Background background)
        {
            Game.Components.Remove(background);
        }

        /// <summary>
        /// Carga del tiles basado en el color del pixel de una imagen.
        /// </summary>
        public void LoadPixelBackgroud()
        {
            bgColorData = new uint[texture.Width * texture.Height];
            texture.GetData<uint>(bgColorData);
            int x = 0, y = 0;

            for (int i = 0; i < bgColorData.Length; i++)
            {
                if (bgColorData[i] != 4294967295)
                {
                    BackgroudPixelComponentConstructor(bgColorData[i], new Vector2(x, y));
                }
                // calcula la posición en pantalla
                x += 8;
                if (x >= screen)
                {
                    x = 0;
                    y += 8;
                }
            }
        }

        /// <summary>
        /// Método de creación de cada tile
        /// </summary>
        private void BackgroudPixelComponentConstructor(uint tile, Vector2 position)
        {
            Background bg = new Background(base.Game,
                Game.Content.Load<Texture2D>(@"Barrier\" + tile), position);
            Game.Components.Add(bg);
        }

    }
}
