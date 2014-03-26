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
    class cButton
    {
        Texture2D textureText;
        String iconPath;
        Texture2D currentIcon;
        Vector2 textPosition;
        Vector2 iconPosition;
        Rectangle textRectangel;
        Rectangle iconRectangel;
        Vector2 sizeText;
        Vector2 sizeIcon;
        ContentManager content;
        const int flashSpeed = 5;
        Color color = new Color(255, 255, 255, 255);
        int currentFrame = 0;
        public bool down;
        public bool isClicked;
        public bool hover;


        public cButton(Texture2D newTextureText, String newTextureIconPath, Vector2 newTextPosition, Vector2 newIconPosition, GraphicsDevice graphics, ContentManager newContent)
        {
            textureText = newTextureText;
            textPosition = newTextPosition;
            iconPath = newTextureIconPath;
            iconPosition = newIconPosition;
            content = newContent;
            sizeText = new Vector2((float)(graphics.Viewport.Width / 4.5), (float)(graphics.Viewport.Height / 10));
            sizeIcon = new Vector2((float)(graphics.Viewport.Width / 25), (float)(graphics.Viewport.Width / 25));
        }


        public void Update(MouseState mouse)
        {
            textRectangel = new Rectangle((int)textPosition.X, (int)textPosition.Y, (int)sizeText.X, (int)sizeText.Y);
            iconRectangel = new Rectangle((int)iconPosition.X, (int)iconPosition.Y, (int)sizeIcon.X, (int)sizeIcon.Y);
            Rectangle mouseRectangel = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRectangel.Intersects(textRectangel))
            {
                hover = true;
                if (color.B == 255)
                    down = false;
                if (color.B == 0)
                    down = true;
                if (down)
                    color.B += flashSpeed;
                else
                    color.B -= flashSpeed;
                if (mouse.LeftButton == ButtonState.Pressed)
                    isClicked = true;

                currentIcon = content.Load<Texture2D>(iconPath+currentFrame);
                currentFrame++;
                if (currentFrame >=30)
                {
                    currentFrame = 0;
                }
            }

            else if (color.B < 255)
            {
                hover = false;
                color.B += flashSpeed;
                isClicked = false;
            }
        }




        public void Draw(SpriteBatch spriteBatch)
        {
            if (hover == false)
                spriteBatch.Draw(textureText, textRectangel, color);
            else
            {
                spriteBatch.Draw(currentIcon, iconPosition, new Color(0,0,0,100));
                spriteBatch.Draw(textureText, textRectangel, color);
            }
        }
    }
}
