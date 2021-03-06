﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GravityShift
{
    /// <summary>
    /// 
    /// </summary>
    class MainMenu
    {
        public enum MenuChoices { StartGame, Options, Exit, Credits }

        Dictionary<MenuChoices, Texture2D> mUnselected;
        Dictionary<MenuChoices, Texture2D> mSelected;

        Texture2D mTitle;
        Texture2D mBackground;

        IControlScheme mControls;
        GraphicsDeviceManager mGraphics;

        MenuChoices mCurrentChoice = MenuChoices.StartGame;

        public MainMenu(IControlScheme controlScheme, GraphicsDeviceManager graphics)
        {
            mControls = controlScheme;
            mGraphics = graphics;

            mUnselected = new Dictionary<MenuChoices, Texture2D>();
            mSelected = new Dictionary<MenuChoices, Texture2D>();
        }

        public void Load(ContentManager content)
        {
            mUnselected.Add(MenuChoices.StartGame, content.Load<Texture2D>("Images\\Menu\\Main\\PlayUnselected"));
            mUnselected.Add(MenuChoices.Exit, content.Load<Texture2D>("Images\\Menu\\Main\\ExitUnselected"));
            mUnselected.Add(MenuChoices.Options, content.Load<Texture2D>("Images\\Menu\\Main\\OptionsUnselected"));
            mUnselected.Add(MenuChoices.Credits, content.Load<Texture2D>("Images\\Menu\\Main\\CreditsUnselected"));

            mSelected.Add(MenuChoices.StartGame, content.Load<Texture2D>("Images\\Menu\\Main\\PlaySelected"));
            mSelected.Add(MenuChoices.Exit, content.Load<Texture2D>("Images\\Menu\\Main\\ExitSelected"));
            mSelected.Add(MenuChoices.Options, content.Load<Texture2D>("Images\\Menu\\Main\\OptionsSelected"));
            mSelected.Add(MenuChoices.Credits, content.Load<Texture2D>("Images\\Menu\\Main\\CreditsSelected"));

            mTitle = content.Load<Texture2D>("Images\\Menu\\Mr_Gravity");
            mBackground = content.Load<Texture2D>("Images\\Menu\\backgroundSquares1");

        }


        public void Update(GameTime gametime, ref GameStates states, Level mainMenuLevel)
        {
            PhysicsEnvironment env = mainMenuLevel.Environment;
            if (mControls.isBackPressed(false))
                states = GameStates.Exit;
            if (mControls.isAPressed(false) || mControls.isStartPressed(false))
            {
                if (mCurrentChoice == MenuChoices.StartGame)
                {
                    states = GameStates.Level_Selection;
                    mainMenuLevel.Reload();
                }
                if (mCurrentChoice == MenuChoices.Exit)
                    states = GameStates.Exit;
                if (mCurrentChoice == MenuChoices.Options)
                    states = GameStates.Options;
                if (mCurrentChoice == MenuChoices.Credits)
                {
                    states = GameStates.Credits;
                    mainMenuLevel.Reload();
                }
                env.GravityDirection = GravityDirections.Down;
            }

            if (env.GravityDirection == GravityDirections.Down)
                mCurrentChoice = MenuChoices.StartGame;
            if (env.GravityDirection == GravityDirections.Left)
                mCurrentChoice = MenuChoices.Credits;
            if (env.GravityDirection == GravityDirections.Right)
                mCurrentChoice = MenuChoices.Options;
            if (env.GravityDirection == GravityDirections.Up)
                mCurrentChoice = MenuChoices.Exit;
        }

        public void Draw(GameTime gametime, SpriteBatch spriteBatch, Matrix scale)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                scale);

            float[] mSize = new float[2] { (float)mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Width / (float)mGraphics.GraphicsDevice.Viewport.Width, (float)mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Height / (float)mGraphics.GraphicsDevice.Viewport.Height };
#if XBOX360
            Point center = mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center;
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, mGraphics.GraphicsDevice.Viewport.Width, mGraphics.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(mTitle, new Rectangle(center.X - (int)(mTitle.Width * mSize[0]) / 2, mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Top, (int)(mTitle.Width * mSize[0]), (int)(mTitle.Height * mSize[1])), Color.White);

            for (int i = 0; i < 4; i++)
            {
                MenuChoices choice = (MenuChoices)i;
                if (choice == mCurrentChoice)
                    spriteBatch.Draw(mSelected[choice], GetRegion(choice, mSelected[choice]), Color.White);
                else
                    spriteBatch.Draw(mUnselected[choice], GetRegion(choice, mUnselected[choice]), Color.White);
            }
#else
            Point center = mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Center;
            spriteBatch.Draw(mBackground, new Rectangle(0, 0, mGraphics.GraphicsDevice.Viewport.Width, mGraphics.GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.Draw(mTitle, new Rectangle(center.X - (int)(mTitle.Width * mSize[0]) / 2, mGraphics.GraphicsDevice.Viewport.TitleSafeArea.Top, (int)(mTitle.Width * mSize[0]), (int)(mTitle.Height * mSize[1])), Color.White);

            foreach (MenuChoices choice in Enum.GetValues(typeof(MenuChoices)))
                if (choice == mCurrentChoice)
                    spriteBatch.Draw(mSelected[choice], GetRegion(choice, mSelected[choice]), Color.White);
                else
                    spriteBatch.Draw(mUnselected[choice], GetRegion(choice, mUnselected[choice]), Color.White);
#endif

            spriteBatch.End();
        }

        public Rectangle GetRegion(MenuChoices choice, Texture2D texture)
        {
            Viewport viewport = mGraphics.GraphicsDevice.Viewport;

            float[] mSize = new float[2] { (float)viewport.TitleSafeArea.Width / (float)mGraphics.GraphicsDevice.Viewport.Width, (float)viewport.TitleSafeArea.Height / (float)mGraphics.GraphicsDevice.Viewport.Height };

            if (choice == MenuChoices.StartGame)
                return new Rectangle(viewport.TitleSafeArea.Center.X - ((int)(texture.Width * mSize[0]) / 2),
                    viewport.TitleSafeArea.Bottom - (int)(texture.Height * mSize[1]), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            if (choice == MenuChoices.Exit)
                return new Rectangle(viewport.TitleSafeArea.Center.X - ((int)(texture.Width * mSize[0]) / 2),
                    viewport.TitleSafeArea.Top + (int)(mTitle.Height * mSize[1]), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            if (choice == MenuChoices.Options)
                return new Rectangle(viewport.TitleSafeArea.Right - ((int)(texture.Width * mSize[0])) - (int)(mTitle.Height * mSize[1]),
                    viewport.TitleSafeArea.Center.Y + (int)(mTitle.Height * mSize[1]) / 2 - ((int)(texture.Height * mSize[1]) / 2), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            if (choice == MenuChoices.Credits)
                return new Rectangle(viewport.TitleSafeArea.Left + (int)(mTitle.Height * mSize[1]),
                    viewport.TitleSafeArea.Center.Y + (int)(mTitle.Height * mSize[1]) / 2 - ((int)(texture.Height * mSize[1]) / 2), (int)(texture.Width * mSize[0]), (int)(texture.Height * mSize[1]));
            return new Rectangle();
        }
    }
}
