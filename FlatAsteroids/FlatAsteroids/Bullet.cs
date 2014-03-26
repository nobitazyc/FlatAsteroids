using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace FlatAsteroids
{
    class Bullet
    {
        public Texture2D bulletTexture;
        public Rectangle bulletRec;
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 origin;
        public Color[] textureData;

        public bool isVisible;
        

        public Bullet(ContentManager content)
        {
            Random r = new Random();
            int random =(int)Math.Floor(r.NextDouble() * 4);
            bulletTexture = content.Load<Texture2D>("bullet"+random);
            textureData = new Color[bulletTexture.Width * bulletTexture.Height];
            bulletTexture.GetData(textureData);
            isVisible = false;
            bulletRec = new Rectangle((int)position.X, (int)position.Y, bulletTexture.Width, bulletTexture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture,position,null,Color.White,0f,origin,0.7f,SpriteEffects.None,0);
        }
        
    }
}
