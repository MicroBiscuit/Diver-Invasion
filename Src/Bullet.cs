using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Diver_Invasion.Src
{
    public class Bullet
    {
        public Vector2 position;
        public float speed;
        public Vector2 codeposition;
        public Texture2D image;
        public Rectangle Rectangle;
        public bool active;

        public Bullet(ContentManager Content)
        {
            position = new Vector2(0, 0);
            Rectangle = new Rectangle((int)position.X, (int)position.Y, 16, 16);
            image = Content.Load<Texture2D>("Sprites/Bullets");
            speed = 5.0f;
            codeposition = new Vector2(0, 0);
            active = false;
        }

        public void Draw(float counter)
        {
            Game1.spriteBatch.Draw(image, position, new Rectangle((int)counter * Rectangle.Width + (int)codeposition.X, (int)codeposition.Y, Rectangle.Width, Rectangle.Height), Color.White);
        }
    }
}
