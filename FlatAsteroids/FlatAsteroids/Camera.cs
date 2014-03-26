using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlatAsteroids
{
    class Camera
    {
        public Matrix transform;
        Viewport view;
        public Vector2 center;
     
        public Camera(Viewport view)
        {
            this.view = view;
            center = new Vector2();
        }

        public void Update(GameTime gameTime, Rocket rocket,Vector2 backgroundPosition,Texture2D background)
        {
          

            
            if ((rocket.rocketPosition.X - view.Width / 2) <= backgroundPosition.X)
            {
                if( rocket.rocketPosition.X > backgroundPosition.X)
                {
                    center.X = backgroundPosition.X;
                }
                else
                {
                    center.X = backgroundPosition.X + background.Width -view.Width;
                }
            }
            else if ((rocket.rocketPosition.X - view.Width / 2) >= backgroundPosition.X + background.Width - view.Width)
            {
                if (rocket.rocketPosition.X < backgroundPosition.X + background.Width)
                {
                    center.X = backgroundPosition.X + background.Width - view.Width;
                }
                else
                {
                    center.X = backgroundPosition.X;
                }
            }
            else
            {
                center.X = rocket.rocketPosition.X - view.Width / 2;
            }

            if ((rocket.rocketPosition.Y - view.Height / 2) <= backgroundPosition.Y)
            {
                if (rocket.rocketPosition.Y > backgroundPosition.Y)
                {
                    center.Y = backgroundPosition.Y;
                }
                else
                {
                    center.Y = backgroundPosition.Y + background.Height - view.Height;
                }
            }
            else if ((rocket.rocketPosition.Y - view.Height / 2) >= backgroundPosition.Y + background.Height - view.Height)
            {
                if (rocket.rocketPosition.Y < backgroundPosition.Y + background.Height)
                {
                    center.Y = backgroundPosition.Y + background.Height - view.Height;
                }
                else
                {
                    center.Y = backgroundPosition.Y;
                }
            }
            else
            {
                center.Y = rocket.rocketPosition.Y - view.Height / 2;
            }
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        }
    }
}
