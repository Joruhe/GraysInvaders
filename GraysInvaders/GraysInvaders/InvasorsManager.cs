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
    public class InvasorsManager : Microsoft.Xna.Framework.GameComponent
    {
        #region STATES
        /// <summary>
        /// Numero de invasores
        /// </summary>
        private const int INVASORS = 60;
        /// <summary>
        /// incremento de la velocidad de los frames
        /// </summary>
        private const int SPEEDINCREMENT = 10;
        private const int INVASORSHOOTERS = 12;
        private float INCREMENTSHOOTS = 0.70f;
        /// <summary>
        /// margen lateral de separación en la colocación
        /// </summary>
        private const int SCREENWIDTH = 160;
        private const int SCREENTHEND = 1120; 
        /// <summary>
        /// niveles de dificultad
        /// </summary>
        private int difficult;
        #endregion


        // Audio stuff
        private SoundEffectInstance moveEffect;
        private SoundEffect SoundEffectMove;

        public InvasorsManager(Game game, ref Texture2D texture, string difficult)
            : base(game)
        {
            // TODO: Construct any child components here

            SelectDifficult(difficult);

            SheetInvaders(game, ref texture);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            // Load audio elements
            SoundEffectMove = Game.Content.Load<SoundEffect>(@"music\duck");
            moveEffect = SoundEffectMove.CreateInstance();
            base.Initialize();
        }

        public void SelectDifficult(string difficult)
        {
            if (difficult == "easy")
            {
                this.difficult = 90;
                INCREMENTSHOOTS = .1f;
            }
            if (difficult == "normal")
            {
                this.difficult = 70;
                INCREMENTSHOOTS = .3f;
            }
            if (difficult == "hard")
            {
                this.difficult = 50;
                INCREMENTSHOOTS = .5f;
            }
            if (difficult == "insane")
            {
                this.difficult = 30;
                INCREMENTSHOOTS = .7f;
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            float last, first;
            last = SCREENWIDTH;
            first = SCREENTHEND;
            float down = 0;
            float timeForSoundMove = 1;

            #region LAST POSITION
            foreach (GameComponent gc in Game.Components)
                if (gc is Invasor)
                {
                    if (!((Invasor)gc).die)
                    {
                        if (((Invasor)gc).position.X > last)
                            last = ((Invasor)gc).position.X;
                        if (((Invasor)gc).position.X < first)
                            first = ((Invasor)gc).position.X;
                        if (((Invasor)gc).position.Y > down)
                        {
                            down = (((Invasor)gc).position.Y);
                        }

                        timeForSoundMove = ((Invasor)gc).timeSinceLastFrame;
                    }
                }
            #endregion

            if (timeForSoundMove == 0)
            {
                moveEffect.Play();
            }

            #region ASIGNATION LAST POSITION
            foreach (GameComponent gc in Game.Components)
                if (gc is Invasor)
                {
                    ((Invasor)gc).spaceRight = last;
                    ((Invasor)gc).spaceLeft = first;
                    ((Invasor)gc).downInvasor = down;
                }
            #endregion


            base.Update(gameTime);
        }

        public void SheetInvaders(Game game, ref Texture2D texture)
        {
            for (int i = 0; i < INVASORS; i++)
            {
                Invasor invasor = new Invasor(game, ref texture, i, INVASORSHOOTERS, this.difficult);
                if (i > INVASORS - INVASORSHOOTERS)
                    invasor.shootin = true;
                Game.Components.Add(invasor);
            }
        }

        public void Intesects(Invasor invasor, List<Invasor> list)
        {
            #region VARIABLES
            invasor.enable = false;
            int shootinInvasorNum = -1;
            int invasorShooter = -1;
            bool shootNoAssigned = true;
            #endregion

            #region REASIGNACION SHOOT
            for (int i = Game.Components.Count - 1; i >= 0; i--)
                if (Game.Components[i] is Invasor)
                {
                    #region ASSIGN SHOOT X1 + NOT SHOOT FRIEND
                    if (((Invasor)Game.Components[i]).numInvader ==
                        invasor.numInvader - INVASORSHOOTERS
                        && shootNoAssigned)
                    {
                        ((Invasor)Game.Components[i]).shootin = true;
                        shootinInvasorNum =
                            ((Invasor)Game.Components[i]).numInvader;
                        invasorShooter = i;

                        #region NOT SHOOT FRIEND
                        foreach (GameComponent gc in Game.Components)
                        {
                            if (gc is Invasor)
                            {
                                if (((Invasor)gc).numInvader - shootinInvasorNum > 0
                                    && (((Invasor)gc).numInvader - shootinInvasorNum)
                                    % INVASORSHOOTERS == 0
                                    && ((Invasor)gc).numInvader != invasor.numInvader)
                                {
                                    ((Invasor)Game.Components[i]).shootin = false;
                                }
                            }
                        }
                        #endregion

                        i = 0;
                    }
                    #endregion

                    #region ASSIGN SHOOT X2
                    else if (((Invasor)Game.Components[i]).numInvader ==
                        invasor.numInvader - INVASORSHOOTERS * 2
                        && shootNoAssigned)
                    {
                        ((Invasor)Game.Components[i]).shootin = true;
                        shootinInvasorNum =
                            ((Invasor)Game.Components[i]).numInvader;
                        shootNoAssigned = false;
                        i = 0;
                    }
                    #endregion

                    #region ASSIGN SHOOT X3
                    else if (((Invasor)Game.Components[i]).numInvader ==
                            invasor.numInvader - INVASORSHOOTERS * 3
                        && shootNoAssigned)
                    {
                        ((Invasor)Game.Components[i]).shootin = true;
                        shootinInvasorNum =
                            ((Invasor)Game.Components[i]).numInvader;
                        shootNoAssigned = false;
                        i = 0;
                    }
                    #endregion

                    #region ASSIGN SHOOT X4
                    else if (((Invasor)Game.Components[i]).numInvader ==
                            invasor.numInvader - INVASORSHOOTERS * 4
                        && shootNoAssigned)
                    {
                        ((Invasor)Game.Components[i]).shootin = true;
                        shootinInvasorNum =
                            ((Invasor)Game.Components[i]).numInvader;
                        shootNoAssigned = false;
                        i = 0;
                    }
                    #endregion
                }
             #endregion

            if (!invasor.die)
            {
                Invasor invasorDie = new Invasor(Game, invasor);
                Game.Components.Add(invasorDie);
            }

            Game.Components.Remove(invasor);

            #region SPEED INCREMENTS
            if (!invasor.die)
            {
                foreach (GameComponent gc in Game.Components)
                    if (gc is Invasor)
                    {
                        if (((Invasor)gc).speed.X * ((Invasor)gc).speed.X 
                            > (INCREMENTSHOOTS * (INVASORS - 1)) *(INCREMENTSHOOTS * (INVASORS - 1)))
                        {
                            if (((Invasor)gc).speed.X > 0)
                            {
                                ((Invasor)gc).speed =
                                         new Vector2(((Invasor)gc).speed.X * (0.4f+INCREMENTSHOOTS),
                                             ((Invasor)gc).speed.Y);
                                ((Invasor)gc).timeToMove = 0;
                            }
                            else
                            {
                                ((Invasor)gc).speed =
                                         new Vector2(((Invasor)gc).speed.X * (0.4f+INCREMENTSHOOTS),
                                             ((Invasor)gc).speed.Y);
                                ((Invasor)gc).timeToMove = 0;
                            }
                        }
                        else
                        {
                            if (((Invasor)gc).speed.X > 0)
                            {
                                ((Invasor)gc).speed =
                                         new Vector2(((Invasor)gc).speed.X + INCREMENTSHOOTS,
                                             ((Invasor)gc).speed.Y);
                            }
                            else
                            {
                                ((Invasor)gc).speed =
                                         new Vector2(((Invasor)gc).speed.X - INCREMENTSHOOTS,
                                             ((Invasor)gc).speed.Y);
                            }
                        }

                        ((Invasor)gc).millisecondsPerFrame -= SPEEDINCREMENT;
                        //((Invasor)gc).timeToMove = ((Invasor)gc).timeToMove - 25;

                        if (((Invasor)gc).shootquick > difficult)
                        {
                            if (difficult == 90)
                                ((Invasor)gc).shootquick -= 1;
                            if (difficult == 70)
                                ((Invasor)gc).shootquick += 1;
                            if (difficult == 50)
                                ((Invasor)gc).shootquick -= 1;
                            if (difficult == 30)
                                ((Invasor)gc).shootquick -= 1;
                        }
                    }
            }
            #endregion
        }

    }
}
