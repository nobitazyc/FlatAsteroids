using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FlatAsteroids
{
    class Mainmenu
    {
        public cButton play;
        cButton option;
        cButton exit;
        cButton help;
        ContentManager content;
        GraphicsDevice graphics;
        int currentFrame = 0;

        public Mainmenu(ContentManager content, GraphicsDevice graphics)
        {
            this.content = content;
            this.graphics = graphics;
            play = new cButton(content.Load<Texture2D>("play"), "optionIcon/optionicon", new Vector2(25, graphics.Viewport.Height - 250), new Vector2(300, 0), graphics, content);
            option = new cButton(content.Load<Texture2D>("option"), "playIcon/playicon", new Vector2(39, graphics.Viewport.Height - 200), new Vector2(300, 0), graphics, content);
            exit = new cButton(content.Load<Texture2D>("exit"), "exitIcon/exit", new Vector2(20, graphics.Viewport.Height - 150), new Vector2(300, 30), graphics, content);
            help = new cButton(content.Load<Texture2D>("help"), "helpIcon/helpicon", new Vector2(24, graphics.Viewport.Height - 100), new Vector2(300, 0), graphics, content);
            
        }

        public Game1.GameState Update(MouseState mouse)
        {
            Game1.GameState currentGamestate = Game1.GameState.MainMenu;

            if (play.isClicked == true)
                currentGamestate = Game1.GameState.Playing;
            play.Update(mouse);
            if (option.isClicked == true)
                currentGamestate = Game1.GameState.Options;
            option.Update(mouse);
            if (exit.isClicked == true)
                currentGamestate = Game1.GameState.Exit;
            exit.Update(mouse);
            if (help.isClicked == true)
                currentGamestate = Game1.GameState.Help;
            help.Update(mouse);
            currentFrame++;
            if (currentFrame >= 89)
            {
                currentFrame = 0;
            }
            return currentGamestate;

        }

        public void Draw(SpriteBatch spriteBatch)
        {     
            spriteBatch.Draw(content.Load<Texture2D>("bg/bg"+currentFrame), new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), Color.White);
            spriteBatch.Draw(content.Load<Texture2D>("rocket"),new Rectangle(20,30,graphics.Viewport.Width/4,graphics.Viewport.Height/3), Color.White);
            spriteBatch.Draw(content.Load<Texture2D>("title"), new Rectangle(120, 60,(int)( graphics.Viewport.Width / 2), graphics.Viewport.Height / 7), Color.White);
            play.Draw(spriteBatch);
            option.Draw(spriteBatch);
            exit.Draw(spriteBatch);
            help.Draw(spriteBatch);

        }
    }
}
