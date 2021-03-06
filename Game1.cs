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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Diver_Invasion
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        public static int max_enemey_bullets = 1;

        SpriteFont font;

        Src.Character dude;
        Src.Character[] enemy = new Src.Character[10];

        float offset;
        int barrier_count;
        int lives;

        float counter;
        float counterB;
        int dude_counter;
        bool game_over;
        int esc_counter;

        int score = 0;

        bool pause;

        Texture2D Background;
        SoundEffect sound_shoot;
        SoundEffect sound_die;
        Texture2D title;
        Song song;

        Random random;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();
            this.IsMouseVisible = true;

            for (int i = 0; i < enemy.Length; ++i)
                enemy[i] = new Diver_Invasion.Src.Character(Content, new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height));

            dude = new Diver_Invasion.Src.Character(Content, new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height));

            offset = 0;
            barrier_count = 2;
            lives = 10;

            for (int i = 0; i < 10; i++)
            {
                enemy[i].Randomize(Content);
                enemy[i].active = false;
            }

            counterB = 0;
            dude_counter = 0;
            game_over = false;
            esc_counter = 0;

            score = 0;

            Console.WriteLine(dude.active);

            pause = true;

            dude.active = true;

            random = new Random();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Fonts/Font");

            Background = Content.Load<Texture2D>("Sprites/Background");
            sound_shoot = Content.Load<SoundEffect>("Sounds/Shoot");
            sound_die = Content.Load<SoundEffect>("Sounds/Explosion");
            title = Content.Load<Texture2D>("Sprites/Title");
            song = Content.Load<Song>("Sounds/Bg_song");
            MediaPlayer.Play(song);
            dude.image = Content.Load<Texture2D>("Sprites/Dude");
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
            KeyboardState state = Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (state.IsKeyDown(Keys.Escape))
                this.Exit();

            if (pause && esc_counter <= 0)
            {
                pause = false;
                esc_counter = 50;
            }
            else if (pause == false && esc_counter <= 0)
            {
                pause = true;
                esc_counter = 50;
            }

            if (state.IsKeyDown(Keys.F4)) { this.Exit(); }
            if (!pause)
            {
                if (state.IsKeyDown(Keys.W) && dude.active == true)
                {
                    if (dude.position.Y - dude.speed >= 0)
                    {
                        dude.position.Y -= dude.speed;
                        offset += 0.9;
                    }
                }
                else if (state.IsKeyDown(Keys.S) && dude.active == true)
                {
                    if (dude.position.Y + dude.speed <= 640 + 48)
                    {
                        dude.position.Y += dude.speed;
                        offset -= 0.9;
                    }
                }
                if (state.IsKeyDown(Keys.A) && dude.active == true)
                {
                    if (dude.position.X - dude.speed >= 0)
                        dude.position.X -= dude.speed;
                }
                else if (state.IsKeyDown(Keys.D) && dude.active == true)
                {
                    if (dude.position.X + dude.speed <= 480 + 48)
                        dude.position.X += dude.speed;
                }
                if (state.IsKeyDown(Keys.F) && dude.active == true && barrier_count > 0 && dude_counter <= 0)
                {
                    dude_counter = 50;
                    barrier_count--;
                }
                if (counter < 0.1)
                {
                    if (state.IsKeyDown(Keys.Space) && dude.active == false)
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            if (dude.P_Bullet[i].active == false)
                            {
                                dude.P_Bullet[i].position.X = dude.position.X + dude.Rectangle.Width;
                                dude.P_Bullet[i].position.Y = dude.position.Y + dude.Rectangle.Width / 2;
                                dude.P_Bullet[i].active = true;
                                dude.shooting = (int)counter;
                                sound_shoot.Play();
                                break;
                            }
                        }
                    }
                }
                for (int i = 0; i < 20; i++)
                {
                    if (dude.P_Bullet[i].active == true)
                    {
                        dude.P_Bullet[i].position.X += dude.P_Bullet[i].speed;
                        if (dude.P_Bullet[i].position.X >= 640) { dude.P_Bullet[i].active = false; }
                    }
                }
                int temp_spawn;
                if ((int)counterB % 10 == 1)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (enemy[i].active == false)
                        {
                            temp_spawn = (int)random.Next(0, 5);
                            if (temp_spawn >= 3)
                            {
                                enemy[i].Randomize();
                                enemy[i].active = true;
                                break;
                            }
                        }
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    if (enemy[i].active == true)
                    {
                        enemy[i].Dostuff();
                        enemy[i].position.X -= enemy[i].speed;
                        if (enemy[i].position.X < -48) { enemy[i].active = false; enemy[i].position.X = 640; }
                    }
                    for (int j = 0; j < max_enemey_bullets; j++)
                    {
                        if (enemy[i].B_Bullet[j].active == true)
                        {
                            enemy[i].B_Bullet.position.X -= enemy[i].B_Bullet[j].speed;
                            if (enemy[i].B_Bullet[j].position.X < -16) { enemy[i].B_Bullet[j].active = false; }
                        }
                    }
                }

                if (dude.active == true)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (dude.Rectangle.Intersects(enemy[i].Rectangle) && enemy[i].active == true && dude_counter <= 0)
                        {
                            dude.active = false;
                            dude.die_time = counter;
                            enemy[i].active = false;
                            lives--;
                            sound_die.Play();
                            dude_counter = 100;
                        }
                        else
                        {
                            for (int j = 0; j < max_enemey_bullets; j++)
                            {
                                if (dude.Rectangle.Intersects(enemy[i].B_Bullet[j].Rectangle) && enemy[i].B_Bullet[j].active == true && dude_counter <= 0)
                                {
                                    dude.active = false;
                                    dude.die_time = counter;
                                    enemy[i].B_Bullet[j].active = false;
                                    lives--;
                                    sound_die.Play();
                                    dude_counter = 100;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if (dude.P_Bullet[i].Rectangle(enemy[j].Rectangle) && enemy[j].active == true && dude.P_Bullet[i].active == true)
                            {
                                score++;
                                enemy[j].active = false;
                                sound_die.Play();
                            }
                        }
                    }
                    counterB += 9;
                }
                if (dude_counter > 0) { dude_counter--; }
                counter += 0.1;
                if (esc_counter > 0) { esc_counter--; }
                gameTime.ElapsedGameTime--;
                if(counter >= 2.0){counter = 0.0;}
                if(dude.shooting != -1)
                {
                    if(dude.shooting + 1 == (int)counter)
                    {
                        dude.shooting = -1;
                    }
                }
                if(dude.active == false && dude.die_time + 3 >= counter)
                {
                    dude.position.X = 0;
                    dude.position.Y = Window.ClientBounds.Height/2 - dude.Rectangle.Height/2;
                    dude.active = true;
                }
                if(lives <= 0)
                {
                    pause = true;
                    game_over = true;
                }
                if(score%10 == 0 && score > 0)
                {
                    if(dude.speed < 4)
                    {
                        dude.speed += 1;
                        for(int i = 0; i < 20; i++){dude.P_Bullet[i].speed += 0.2;}
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if (!game_over)
            {
                spriteBatch.Draw(Background, new Vector2(0, -300+offset);
                if(dude.active == true)
                {
                    dude.Draw(counter);
                }
                for(int i = 0; i < 20; i++)
                {
                    if(dude.P_Bullet[i].active == true)
                    {
                        dude.P_Bullet[i].Draw(counter);
                    }
                    if(i < 10)
                    {
                        if(enemy[i].active == true)
                        {
                            enemy[i].Draw(counter);
                        }
                        for(int j = 0; j < max_enemey_bullets; j++)
                        {
                            if(enemy[i].B_Bullet[j].active == true)
                            {
                                enemy[i].B_Bullet[j].Draw(counter);
                            }
                        }
                    }
                }
                if(dude_counter > 0)
                {
                }
            }

            base.Draw(gameTime);

            spriteBatch.End();
        }

        private void print_annoying_text(float counter)
        {
            Vector2 tposition = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
            int cntr = ((int)counter * 10);
            spriteBatch.DrawString(font, "Pause", new Vector2(tposition.X, tposition.Y - 10 + 1), Color.Black);
            spriteBatch.DrawString(font, "Controls:", new Vector2(tposition.X, tposition.Y + 12 + 1), Color.Black);
            spriteBatch.DrawString(font, "WASD keys to move", new Vector2(tposition.X, tposition.Y + 24 + 1), Color.Black);
            spriteBatch.DrawString(font, "Spacebar to shoot", new Vector2(tposition.X, tposition.Y + 36 + 1), Color.Black);
            spriteBatch.DrawString(font, "F to use bullet-shield", new Vector2(tposition.X, tposition.Y + 48 + 1), Color.Black);
            spriteBatch.DrawString(font, "ESC to unpause", new Vector2(tposition.X, tposition.Y + 60 + 1), Color.Black);

            spriteBatch.DrawString(font, "Pause", new Vector2(tposition.X, tposition.Y - 10 - 1), Color.Black);
            spriteBatch.DrawString(font, "Controls:", new Vector2(tposition.X, tposition.Y + 12 - 1), Color.Black);
            spriteBatch.DrawString(font, "WASD keys to move", new Vector2(tposition.X, tposition.Y + 24 - 1), Color.Black);
            spriteBatch.DrawString(font, "Spacebar to shoot", new Vector2(tposition.X, tposition.Y + 36 - 1), Color.Black);
            spriteBatch.DrawString(font, "F to use bullet-shield", new Vector2(tposition.X, tposition.Y + 48 - 1), Color.Black);
            spriteBatch.DrawString(font, "ESC to unpause", new Vector2(tposition.X, tposition.Y + 60 - 1), Color.Black);

            spriteBatch.DrawString(font, "Pause", new Vector2(tposition.X - 1, tposition.Y - 10), Color.Black);
            spriteBatch.DrawString(font, "Controls:", new Vector2(tposition.X - 1, tposition.Y + 12), Color.Black);
            spriteBatch.DrawString(font, "WASD keys to move", new Vector2(tposition.X - 1, tposition.Y + 24), Color.Black);
            spriteBatch.DrawString(font, "Spacebar to shoot", new Vector2(tposition.X - 1, tposition.Y + 36), Color.Black);
            spriteBatch.DrawString(font, "F to use bullet-shield", new Vector2(tposition.X - 1, tposition.Y + 48), Color.Black);
            spriteBatch.DrawString(font, "ESC to unpause", new Vector2(tposition.X - 1, tposition.Y + 60), Color.Black);

            spriteBatch.DrawString(font, "Pause", new Vector2(tposition.X + 1, tposition.Y - 10), Color.Black);
            spriteBatch.DrawString(font, "Controls:", new Vector2(tposition.X + 1, tposition.Y + 12 + 1), Color.Black);
            spriteBatch.DrawString(font, "WASD keys to move", new Vector2(tposition.X + 1, tposition.Y + 24 + 1), Color.Black);
            spriteBatch.DrawString(font, "Spacebar to shoot", new Vector2(tposition.X + 1, tposition.Y + 36 + 1), Color.Black);
            spriteBatch.DrawString(font, "F to use bullet-shield", new Vector2(tposition.X + 1, tposition.Y + 48 + 1), Color.Black);
            spriteBatch.DrawString(font, "ESC to unpause", new Vector2(tposition.X + 1, tposition.Y + 60 + 1), Color.Black);

            spriteBatch.DrawString(font, "Pause", new Vector2(tposition.X, tposition.Y - 10), Color.Black);
            spriteBatch.DrawString(font, "Controls:", new Vector2(tposition.X, tposition.Y + 12), Color.Black);
            spriteBatch.DrawString(font, "WASD keys to move", new Vector2(tposition.X, tposition.Y + 24), Color.Black);
            spriteBatch.DrawString(font, "Spacebar to shoot", new Vector2(tposition.X, tposition.Y + 36), Color.Black);
            spriteBatch.DrawString(font, "F to use bullet-shield", new Vector2(tposition.X, tposition.Y + 48), Color.Black);
            spriteBatch.DrawString(font, "ESC to unpause", new Vector2(tposition.X, tposition.Y + 60), Color.Black);
        }
    }
}
