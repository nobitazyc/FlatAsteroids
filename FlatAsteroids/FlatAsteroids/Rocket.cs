using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace FlatAsteroids
{
    class Rocket
    {
        Texture2D textureRocket;
        public Vector2 rocketPosition;
        Vector2 rocketOriginPosition;
        public Rectangle rocketRectangle;
        Vector2 rocketOrigin;
        float rotation;
        Vector2 distance;
        public Vector2 rocketVelocity;
        public float tangentialVelocity = 5f;
        float friction = 0.1f;
        public List<Bullet> bullets = new List<Bullet>();
        ContentManager content;
        KeyboardState lastKey;
        Viewport view;
        public Color[] textureData;
        public int health = 100;
        Color distroid = new Color(255, 255, 255, 255);
       
        SoundEffect shootSound;
        public int life = 3;
        public int damageup = 0;


        public Rocket(GraphicsDevice graphics, ContentManager content)
        {
            this.content = content;
            view = graphics.Viewport;
            textureRocket = content.Load<Texture2D>("rocketGame");
            
            rocketOriginPosition= rocketPosition = new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2);
            textureData = new Color[textureRocket.Width * textureRocket.Height];
            textureRocket.GetData(textureData);
            shootSound = content.Load<SoundEffect>("audio/shoot");
        }

        public void Update(MouseState mouse,Texture2D background,Vector2 backgroundPosition)
        {
            //distance.X = mouse.X - rocketPosition.X;
           // distance.Y = mouse.Y - rocketPosition.Y;
            //rotation = (float)Math.Atan2(distance.Y, distance.X)-30f;
            if (rocketPosition.X < backgroundPosition.X)
            {
                rocketPosition.X = backgroundPosition.X + background.Width;
            }
            else if (rocketPosition.X > backgroundPosition.X + background.Width)
            {
                rocketPosition.X = backgroundPosition.X;
            }
            if (rocketPosition.Y < backgroundPosition.Y)
            {
               rocketPosition.Y = backgroundPosition.Y + background.Height;
            }
            else if (rocketPosition.Y > backgroundPosition.Y + background.Height)
            {
                rocketPosition.Y = backgroundPosition.Y;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                rotation += 0.1f;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                rotation -= 0.1f;


            if (Keyboard.GetState().IsKeyDown(Keys.Up)) 
            {
                
                rocketVelocity.X = ((float)Math.Cos(rotation + 29.9f)) * tangentialVelocity;
               
                
                rocketVelocity.Y = ((float)Math.Sin(rotation + 29.9f)) * tangentialVelocity;
                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
              
                rocketVelocity.X = ((float)Math.Cos(rotation + 208.9f)) * tangentialVelocity;
                rocketVelocity.Y = ((float)Math.Sin(rotation + 208.9f)) * tangentialVelocity;
               
            }
            switch(damageup)
            {
                case 0:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        Shoot(10);
                        shootSound.Play();
                    }
                    else if (rocketVelocity != Vector2.Zero)
                    {
                        Vector2 i = rocketVelocity;

                        rocketVelocity = i -= friction * i;
                    }
                    break;
                case 1:
           
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        Shoot(20);
                        shootSound.Play();
                    }
                    else if (rocketVelocity != Vector2.Zero)
                    {
                        Vector2 i = rocketVelocity;

                        rocketVelocity = i -= friction * i;
                    }
                    break;

                case 2:

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        Shoot(40);
                        shootSound.Play();
                    }
                    else if (rocketVelocity != Vector2.Zero)
                    {
                        Vector2 i = rocketVelocity;

                        rocketVelocity = i -= friction * i;
                    }
                    break;

                default:

                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        Shoot2(40);
                        shootSound.Play();
                    }
                    else if (rocketVelocity != Vector2.Zero)
                    {
                        Vector2 i = rocketVelocity;

                        rocketVelocity = i -= friction * i;
                    }
                    break;
            }
            UpdateBullets();
            rocketPosition += rocketVelocity;
            rocketRectangle = new Rectangle((int)rocketPosition.X, (int)rocketPosition.Y, textureRocket.Width, textureRocket.Height);
            rocketOrigin = new Vector2(textureRocket.Width / 2, textureRocket.Height / 2);
            
            lastKey = Keyboard.GetState();
         

            if (distroid.A <= 0)
            {
                life--;
                damageup = 0;
                tangentialVelocity = 5f;
                if (life > 0)
                {
                    health = 100;
                    rocketPosition = rocketOriginPosition;
                    distroid.A = 255;
                    distroid.B = 255;
                    distroid.G = 255;
                    distroid.R = 255;
                }


            }
        }


        public void UpdateBullets()
        {
            foreach (Bullet bullet in bullets)
            {
                bullet.position += bullet.velocity;
                bullet.bulletRec = new Rectangle((int)bullet.position.X, (int)bullet.position.Y, bullet.bulletTexture.Width, bullet.bulletTexture.Height);
                bullet.textureData = new Color[bullet.bulletTexture.Width * bullet.bulletTexture.Height];
                bullet.bulletTexture.GetData(bullet.textureData);
                if (Vector2.Distance(bullet.position, rocketPosition) > 500)
                {
                    bullet.isVisible = false;
                }
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].isVisible == false)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }


        public void Shoot(int limit)
        {
            Bullet newBullet = new Bullet(content);
            newBullet.velocity = new Vector2((float)Math.Cos(rotation+30f), (float)Math.Sin(rotation+30f))* 5f + rocketVelocity;
            newBullet.position = rocketPosition + newBullet.velocity * 5f;
            newBullet.isVisible = true;
            if (bullets.Count < limit)
            {
                bullets.Add(newBullet);
            }
         
        }

        public void Shoot2(int limit)
        {
            Bullet newBullet = new Bullet(content);
            newBullet.velocity = new Vector2((float)Math.Cos(rotation + 30f), (float)Math.Sin(rotation + 30f)) * 5f + rocketVelocity;
            newBullet.position = rocketPosition + newBullet.velocity * 4f;
            newBullet.isVisible = true;
            if (bullets.Count < limit)
            {
                bullets.Add(newBullet);
            }
            newBullet.velocity = new Vector2((float)Math.Cos(rotation + 30f), (float)Math.Sin(rotation + 30f)) * 5f + rocketVelocity;
            newBullet.position = rocketPosition + newBullet.velocity * -4f;
            newBullet.isVisible = true;
            if (bullets.Count < limit)
            {
                bullets.Add(newBullet);
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (health > 0)
            {
                foreach (Bullet bullet in bullets)
                {
                    bullet.Draw(spriteBatch);
                }
                spriteBatch.Draw(textureRocket, rocketPosition, null, Color.White, rotation, rocketOrigin, 0.7f, SpriteEffects.None, 0);
                
            }
            else if (distroid.A > 0)
            {
                spriteBatch.Draw(textureRocket, rocketPosition, null, distroid, rotation, rocketOrigin, 1f, SpriteEffects.None, 0);
                distroid.A -= 5;
                distroid.B -= 5;
                distroid.G -= 5;
                distroid.R -= 5;
            }

            
        }
    }
}
