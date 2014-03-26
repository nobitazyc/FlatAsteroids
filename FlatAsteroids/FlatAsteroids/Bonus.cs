using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace FlatAsteroids
{
    class Bonus
    {
        public Texture2D bonusTexture;
        public Rectangle bonusRec;
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 origin;
        public float rotation = 0;
        public float rotationSpeed;
        public Color[] textureData;
        public bool disappear;
        public int type;

        public Bonus(ContentManager content, Viewport view, int type, int positionX, int positionY, int velocityX, int velocityY, float rotation)
        {
            this.type = type;
            switch (type)
            {
                case 0:
                    bonusTexture = content.Load<Texture2D>("bonus/damageup");
                    break;
                case 1:
                     bonusTexture = content.Load<Texture2D>("bonus/levelup");
                    break;
                case 2:
                     bonusTexture = content.Load<Texture2D>("bonus/lifeup");
                    break;
                case 3:
                     bonusTexture = content.Load<Texture2D>("bonus/speedup");
                    break;

            }

            position = new Vector2(positionX, positionY);
            velocity = new Vector2(velocityX, velocityY);
            rotationSpeed = rotation;
            origin = new Vector2(bonusTexture.Width / 2, bonusTexture.Height / 2);
            textureData = new Color[bonusTexture.Width * bonusTexture.Height];
            bonusTexture.GetData(textureData);
        }


        public void Update(Texture2D background, Vector2 backgroundPosition)
        {

            if (position.X < backgroundPosition.X + 20)
            {
                disappear = true;
            }
            else if (position.X > background.Width + backgroundPosition.X - 20)
            {
                disappear = true;
            }

            if (position.Y < backgroundPosition.Y + 20)
            {
                disappear = true;
            }
            else if (position.Y > background.Height + backgroundPosition.Y - 20)
            {
                disappear = true;
            }
            position += velocity;
            rotation += rotationSpeed;
            textureData = new Color[bonusTexture.Width * bonusTexture.Height];
            bonusTexture.GetData(textureData);
            bonusRec = new Rectangle((int)position.X, (int)position.Y, bonusTexture.Width, bonusTexture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (disappear == false)
                spriteBatch.Draw(bonusTexture, position, null, Color.White, rotation, origin, 1f, SpriteEffects.None, 0);

        }
    }
}
