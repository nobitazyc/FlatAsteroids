using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FlatAsteroids
{
    class MyGame
    {
        ContentManager content;
        GraphicsDevice graphics;
        Rocket rocket;
        public Camera camera;
        List<Texture2D> background;
        List<Texture2D> background2;
        Texture2D currentBg;
        Vector2 backgroundPosition;
        List<Asteroid> asteroids;
        List<Bonus> bonus;
        Random r;
        Asteroid lastIntersectAsteroid = null;
        int currentFrame = 0;
        bool pause = false;
        bool gameover = false;
        Texture2D pauseBg;
        Vector2 pauseBgPosition;
        Texture2D pauseTexture;
        Rectangle pauseRec;
        Texture2D gameoverTexture;
        Rectangle gameoverRec;
        cButton resume;
        cButton quit;
        KeyboardState lastKey;
        int level = 1;
        int score = 0;
        Vector2 scorePosition;
        Vector2 levelPosition;
        Vector2 lifePosition;
        Texture2D healthTexture;
        Rectangle healthRec;
        SpriteFont font1;
        Song bgsound1;
        float bg1volume = 1f;
        SoundEffect crashsound;
        SoundEffect hitsound;
        bool changesong = false;

        public MyGame(ContentManager content, GraphicsDevice graphics)
        {
            this.content = content;
            this.graphics = graphics;
            rocket = new Rocket(graphics,content);
            camera = new Camera(graphics.Viewport);
            backgroundPosition = new Vector2(-graphics.Viewport.Width/2, -graphics.Viewport.Height/2);
            pauseBgPosition = new Vector2(-graphics.Viewport.Width / 2, -graphics.Viewport.Height / 2);
            healthTexture = content.Load<Texture2D>("health/health0");
            asteroids = new List<Asteroid>();
            background = new List<Texture2D>();
            background2 = new List<Texture2D>();
            bonus = new List<Bonus>();
            r = new Random();
            for (int i = 0; i < 5; i++)
            {
                
                Asteroid newasteroid = new Asteroid(content, graphics.Viewport,r.Next(0,3),r.Next(3,4),r.Next(-700,-300),r.Next(-700,-300),r.Next(-5,5),r.Next(-5,5),(float)(r.Next(1,2)/10));
                asteroids.Add(newasteroid);
            }
            for (int i = 0; i <= 150; i++)
            {
                Texture2D backgroundTexture = content.Load<Texture2D>("gamebg/gamebg" + i);
                background.Add(backgroundTexture);
            }
           
            currentBg = background[0];
            pauseBg = content.Load<Texture2D>("pausebg");
            pauseTexture = content.Load<Texture2D>("pause");
            gameoverTexture = content.Load<Texture2D>("gameover");
            font1 = content.Load<SpriteFont>("SpriteFont1");
            bgsound1 = content.Load<Song>("audio/bgsound1");
            crashsound = content.Load<SoundEffect>("audio/crash");
            hitsound = content.Load<SoundEffect>("audio/hit");

            MediaPlayer.Play(bgsound1);
            MediaPlayer.IsRepeating = true;
        }

        public Game1.GameState Update(MouseState mouse, GameTime gametime)
        {
           
            int removeIndex = 0;
            int removeBullet = 0;
            int removeBonus = 0;
            bool intersect = false;
            bool intersectBonus = false;
            Asteroid distroidAsteroid = null;

          
            if (pause == false && gameover == false)
            {
                rocket.Update(mouse, background[0], backgroundPosition);
                camera.Update(gametime, rocket,backgroundPosition,background[0]);

                if (Keyboard.GetState().IsKeyDown(Keys.Escape) && lastKey.IsKeyUp(Keys.Escape))
                {
                    pause = true;
                }
                //remove all the asteroids that need to be disappear
                for (int i = 0; i < asteroids.Count; i++)
                {
                    if (asteroids[i].remove)
                    {
                        score += (asteroids[i].randomshape - 2); 
                        asteroids.RemoveAt(i); 
                        i--;
                    }
                }

                //check the intersect of the asteroid and the bullets
                foreach (Asteroid asteroid in asteroids)
                {
                    asteroid.Update(background[0], backgroundPosition);
                    for (int i = 0; i < rocket.bullets.Count; i++)
                    {
                        if (IntersectPixel.intersectPixel(asteroid.asteroidRec, asteroid.textureData, rocket.bullets[i].bulletRec, rocket.bullets[i].textureData))
                        {
                            distroidAsteroid = asteroid;
                            asteroid.disapper = true;
                            removeBullet = i;
                            intersect = true;
                            crashsound.Play();
                            break;
                        }
                    }
                    if (intersect == true)
                    {
                        break;
                    }
                    removeIndex++;
                }

                // if any asteroid intersect with bullet, then remove the asteroid
                if (intersect == true)
                {
                    if (distroidAsteroid != null)
                    {
                        if (r.Next(0, 10) == 2)
                        {
                            Bonus newBonus = new Bonus(content, graphics.Viewport, r.Next(0, 4), (int)distroidAsteroid.position.X, (int)distroidAsteroid.position.Y, r.Next(-5, 5), r.Next(-5, 5), (float)(r.Next(1, 2) / 10));
                            bonus.Add(newBonus);
                        }
                        if (distroidAsteroid.randomshape > 3)
                        {
                            if (distroidAsteroid.distroid.A == 255)
                            {
                                Asteroid newasteroid = new Asteroid(content, graphics.Viewport, r.Next(0, 4), distroidAsteroid.randomshape - 1, (int)distroidAsteroid.position.X, (int)distroidAsteroid.position.Y, r.Next(-5, 5), r.Next(-5, 5), (float)(r.Next(1, 2) / 10));
                                asteroids.Add(newasteroid);
                                newasteroid = new Asteroid(content, graphics.Viewport, r.Next(0, 4), distroidAsteroid.randomshape - 1, (int)distroidAsteroid.position.X, (int)distroidAsteroid.position.Y, r.Next(-5, 5), r.Next(-5, 5), (float)(r.Next(1, 2) / 10));
                                asteroids.Add(newasteroid);
                               
                            }
 
                        }
                       
                        rocket.bullets.RemoveAt(removeBullet);
                    }
                }


                //check if the rocket intersect with asteroid
                foreach (Asteroid asteroid in asteroids)
                {
                    if (IntersectPixel.intersectPixel(asteroid.asteroidRec, asteroid.textureData, rocket.rocketRectangle, rocket.textureData) && asteroid != lastIntersectAsteroid)
                    {
                        rocket.health -= 10;
                        lastIntersectAsteroid = asteroid;
                        hitsound.Play();
                        break;
                    }
                }

                //check if the rocket intersect with asteroid
                foreach (Bonus bounsitem in bonus)
                {
                    bounsitem.Update(background[0], backgroundPosition);
                    if (IntersectPixel.intersectPixel(bounsitem.bonusRec, bounsitem.textureData, rocket.rocketRectangle, rocket.textureData))
                    {
                        switch (bounsitem.type)
                        {
                            case 0:
                                rocket.damageup++;
                                break;
                            case 1:
                                level++;
                                asteroids.Clear();
                                for (int i = 0; i < level + 4; i++)
                                {
                                    
                                    Asteroid newasteroid;
                                    if (level <= 4)
                                    {
                                        newasteroid = new Asteroid(content, graphics.Viewport, r.Next(0, 3), r.Next(3, level + 3), r.Next(-700, -300), r.Next(-700, -300), r.Next(-5, 5), r.Next(-5, 5), (float)(r.Next(1, 2) / 10));
                                    }
                                    else
                                    {
                                        newasteroid = new Asteroid(content, graphics.Viewport, r.Next(0, 3), r.Next(3, 7), r.Next(-700, -300), r.Next(-700, -300), r.Next(-5, 5), r.Next(-5, 5), (float)(r.Next(1, 2) / 10));
                                    }
                                    asteroids.Add(newasteroid);
                                }
                                break;
                            case 2:
                                rocket.life++;
                                break;
                            case 3:
                                rocket.tangentialVelocity += 1;
                                break;

                        }
                        intersectBonus = true;
                        break;
                    }
                    removeBonus++;
                }

                if (intersectBonus)
                {
                    bonus.RemoveAt(removeBonus);
                    intersectBonus = false;
                }

                if (asteroids.Count == 0)
                {
                    level++;
                    for (int i = 0; i < level+4; i++)
                    {
                        Asteroid newasteroid;
                        if (level <= 4)
                        {
                          newasteroid  = new Asteroid(content, graphics.Viewport, r.Next(0, 3), r.Next(3, level + 3), r.Next(-700, -300), r.Next(-700, -300), r.Next(-5, 5), r.Next(-5, 5), (float)(r.Next(1, 2) / 10));
                        }
                        else
                        {
                          newasteroid = new Asteroid(content, graphics.Viewport, r.Next(0, 3), r.Next(3, 7), r.Next(-700, -300), r.Next(-700, -300), r.Next(-5, 5), r.Next(-5, 5), (float)(r.Next(1, 2) / 10));
                        }
                        asteroids.Add(newasteroid);
                    }
                }

                currentFrame++;
                if (currentFrame >= 150)
                {
                    currentFrame = 0;
                }
               
                currentBg = background[currentFrame];
               

                scorePosition = new Vector2((int)camera.center.X+20, (int)camera.center.Y +40);
                levelPosition = new Vector2((int)camera.center.X + 20, (int)camera.center.Y + 80);
                lifePosition = new Vector2((int)camera.center.X + 20, (int)camera.center.Y + 120);
                healthRec = new Rectangle((int)camera.center.X + 20, (int)camera.center.Y + 400, rocket.health * 8, healthTexture.Height);
                if (rocket.health <= 100 && rocket.health > 80)
                {
                    healthTexture = content.Load<Texture2D>("health/health0");
                }
                else if (rocket.health < 80 && rocket.health > 60)
                {
                    healthTexture = content.Load<Texture2D>("health/health1");
                }
                else if (rocket.health < 60 && rocket.health > 20)
                {
                    healthTexture = content.Load<Texture2D>("health/health3");
                }
                else if (rocket.health < 20)
                {
                    healthTexture = content.Load<Texture2D>("health/health2");
                }

                if (rocket.life == 1)
                {
                    bg1volume = 0;
                    if (changesong == false)
                    {
                        bgsound1 = content.Load<Song>("audio/bgsound2");
                        MediaPlayer.Play(bgsound1);
                        MediaPlayer.IsRepeating = true;
                        changesong = true;
                    }
                }
                else if (rocket.life == 0)
                {
                    gameover = true;
                }
            }

            else if( pause )
            {
                pauseRec =new Rectangle((int)rocket.rocketPosition.X , (int)rocket.rocketPosition.Y -150,pauseTexture.Width,pauseTexture.Height);
                if( Keyboard.GetState().IsKeyDown(Keys.Escape) && lastKey.IsKeyUp(Keys.Escape))
                {
                    pause = false;
                }
            }

            else if (gameover)
            {
                gameoverRec = new Rectangle((int)camera.center.X+20, (int)camera.center.Y+200, pauseTexture.Width, pauseTexture.Height);
            }

            lastKey = Keyboard.GetState();
            return Game1.GameState.Playing;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
           
            spriteBatch.Draw(currentBg, backgroundPosition, Color.White);
            spriteBatch.DrawString(font1, "Score: " + score, scorePosition, Color.White);
            spriteBatch.DrawString(font1, "level: " + level, levelPosition, Color.White);
            spriteBatch.DrawString(font1, "life: " + rocket.life, lifePosition, Color.White);
            spriteBatch.Draw(healthTexture, healthRec, Color.White);
            
            rocket.Draw(spriteBatch);
            foreach (Asteroid asteroid in asteroids)
            {
                asteroid.Draw(spriteBatch);
            }

            foreach (Bonus bounsitem in bonus)
            {
                bounsitem.Draw(spriteBatch);
            }
            if (pause)
            {
                spriteBatch.Draw(pauseBg, pauseBgPosition, Color.White);
                spriteBatch.Draw(pauseTexture, pauseRec, Color.White);
            }
            if (gameover)
            {
                spriteBatch.Draw(pauseBg, pauseBgPosition, Color.White);
                spriteBatch.Draw(gameoverTexture,gameoverRec,Color.White);

            }
        }
    }
}
