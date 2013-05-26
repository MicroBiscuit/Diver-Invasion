using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Diver_Invasion.Src
{
    public class Character
    {
        public Vector2 position;
        public Vector2 codeposition;
        public float speed;
        public bool player;
        public bool active;
        public int shooting;
        public Rectangle Rectangle;
        public Texture2D image;
        public int enemy_type;
        public Bullet[] B_Bullet = new Bullet[5];
        public Bullet[] P_Bullet = new Bullet[20];
        public int die_time;
        Random random = new Random();

        public Character(ContentManager Content, Vector2 clientBounds)
        {
            for (int i = 0; i < B_Bullet.Length; ++i)
                B_Bullet[i] = new Bullet(Content);
            for (int i = 0; i < P_Bullet.Length; ++i)
                P_Bullet[i] = new Bullet(Content);
            die_time = 0;
            shooting = -1;
            position = new Vector2(0, clientBounds.Y / 2 - Rectangle.Height / 2);
            Rectangle = new Rectangle((int)position.X, (int)position.Y, 64, 48);
        }

        public void Dostuff()
        {
            int temp;
            if (enemy_type == 0) { temp = random.Next(0, 25); }
            else if (enemy_type == 1) { temp = random.Next(0, 20); }
            else if (enemy_type == 2) { temp = random.Next(0, 15); }
            if (temp == 1)
            {
                for (int i = 0; i < Game1.max_enemey_bullets; i++)
                {
                    if (B_Bullet[i].active == false)
                    {
                        B_Bullet[i].active = true;
                        B_Bullet[i].position.X = position.X - Rectangle.Width;
                        B_Bullet[i].position.Y = position.Y + Rectangle.Width / 2;
                        break;
                    }
                }
            }
        }

        public void Draw(float counter)
        {
            if (player)
            {
                if (shooting == -1)
                    Game1.spriteBatch.Draw(image, position, Color.White);
                else
                {
                    Game1.spriteBatch.Draw(image, position, Color.White);
                }
            }
            else
            {
                Game1.spriteBatch.Draw(image, position, new Rectangle((int)codeposition.X, 0, Rectangle.Width, Rectangle.Height), Color.White); 
            }
        }

        public void Randomize(ContentManager Content)
        {
            image = Content.Load<Texture2D>("Sprites/Enemies");
            player = false;
            active = false;
            position.X = 640;
            position.Y = (random.Next(0, 11 + 1)) * 48;
            int temp;
            temp = random.Next(0, 3);
            if (temp == 0)
            {
                enemy_type = 0;
                codeposition.X = 0;
                speed = 0.5;
                for (int i = 0; i < Game1.max_enemey_bullets; i++)
                {
                    B_Bullet[i].codeposition.X = 17;
                    B_Bullet[i].codeposition.Y = 0;
                    B_Bullet[i].speed = 0.7;
                }
            }
            else if (temp == 1)
            {
                enemy_type = 1;
                codeposition.X = 64;
                speed = 2.0;
                for (int i = 0; i < Game1.max_enemey_bullets; i++)
                {
                    B_Bullet[i].codeposition.X = 17;
                    B_Bullet[i].codeposition.Y = 0;
                    B_Bullet[i].speed = 2.5;
                }
            }
            else if (temp == 2)
            {
                enemy_type = 2;
                codeposition.X = 128;
                speed = 3.0;
                for (int i = 0; i < Game1.max_enemey_bullets; i++)
                {
                    B_Bullet[i].codeposition.X = 17;
                    B_Bullet[i].codeposition.Y = 0;
                    B_Bullet[i].speed = 3.5;
                }
            }
        }
    }
}
