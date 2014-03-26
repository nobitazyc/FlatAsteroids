using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace FlatAsteroids
{
    class Asteroid
    {
        public Texture2D asteroidTexture;
        public Rectangle asteroidRec;
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 origin;
        public float rotation=0;
        public float rotationSpeed;
        public Color[] textureData;
        public int randomshape = 3;
        public bool disapper = false;
        public Color distroid = new Color(255, 255, 255, 255);
        public bool remove = false;

        public Asteroid(ContentManager content, Viewport view,int color, int shape, int positionX, int positionY, int velocityX, int velocityY,float rotation)
        {
            int randomcolor = color;
            randomshape = shape;
            asteroidTexture = content.Load<Texture2D>("asteroid/asteroid"+randomshape+"_"+randomcolor);
            position = new Vector2(positionX,positionY);
            velocity = new Vector2(velocityX, velocityY);
            rotationSpeed = rotation;
            origin = new Vector2(asteroidTexture.Width / 2, asteroidTexture.Height / 2);
            textureData = new Color[asteroidTexture.Width*asteroidTexture.Height];
            asteroidTexture.GetData(textureData);
            
        }

        public void Update(Texture2D background,Vector2 backgroundPosition)
        {
           
            if (position.X < backgroundPosition.X+20)
            {
                position.X = backgroundPosition.X + background.Width-20;
            }
            else if (position.X > background.Width + backgroundPosition.X-20)
            {
                position.X = backgroundPosition.X+20;
            }

            if (position.Y < backgroundPosition.Y+20)
            {
                position.Y = backgroundPosition.Y + background.Height-20;
            }
            else if (position.Y > background.Height + backgroundPosition.Y-20)
            {
                position.Y = backgroundPosition.Y+20;
            }
            position += velocity;
            rotation += rotationSpeed;
            textureData = new Color[asteroidTexture.Width * asteroidTexture.Height];
            asteroidTexture.GetData(textureData);
            asteroidRec = new Rectangle((int)position.X, (int)position.Y, asteroidTexture.Width, asteroidTexture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (disapper == false)
                spriteBatch.Draw(asteroidTexture, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0);
            else
            {
                if (distroid.A > 0)
                {
                    spriteBatch.Draw(asteroidTexture, position, null, distroid, rotation, origin, 1f, SpriteEffects.None, 0);
                    distroid.A -= 5;
                    distroid.B -= 5;
                    distroid.G -= 5;
                    distroid.R -= 5;
                }
                else
                {
                    remove = true;
                }
            }

        }
    }
}
