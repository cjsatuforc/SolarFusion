﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace SolarFusion.Core.Screen
{
    class ScreenMenuRoot : BaseGUIScreen
    {
        List<AnimatedBGEntity> mAnimatedBGObjects = null;
        ContentManager _content = null;
        Random _obj_random = null;

        public ScreenMenuRoot()
            : base("Root_Menu", true, "System/UI/Logos/static_jumpista")
        {
            _obj_random = new Random();

            MenuItemBasic mi_play = new MenuItemBasic("Play");
            MenuItemBasic mi_options = new MenuItemBasic("Options");
            MenuItemBasic mi_credits = new MenuItemBasic("Credits");
            MenuItemBasic mi_exit = new MenuItemBasic("Exit");

            mi_play.OnSelected += EventTriggerGoToCharSelect;
            mi_options.OnSelected += EventTriggerGoToOptions;
            mi_credits.OnSelected += EventTriggerGoToCredits;
            mi_exit.OnSelected += DefaultTriggerMenuBack;

            this._list_menuitems.Add(mi_play);
            this._list_menuitems.Add(mi_options);
            this._list_menuitems.Add(mi_credits);
            this._list_menuitems.Add(mi_exit);
        }

        public override void loadContent()
        {
            if (this._content == null)
                this._content = new ContentManager(ScreenManager.Game.Services, SysConfig.CONFIG_CONTENT_ROOT);

            this.mAnimatedBGObjects = new List<AnimatedBGEntity>();

            bool IsSelectedUnique = false;  

            for (int i = 0; i < 20; i++)
            {
                int randItem = this._obj_random.Next(0, 2);

                int randDirX = this._obj_random.Next(0, 2); // 0 = Left to Right, 1 = Right to Left
                float randPosX = 0f;
                float randPosY = 0f;
                int randRotDir = this._obj_random.Next(0, 2); // 0 = Left to Right, 1 = Right to Left
                float randRotSpeed = (float)((this._obj_random.NextDouble() * Math.Abs(0.04 - 0.01)) + 0.01); // Generate random rotation speed between 0.01 and 0.06 every frame.

                if(randDirX == 0)
                    randPosX = (float)(this._obj_random.Next(-300, ScreenManager.GraphicsDevice.Viewport.Width) - ScreenManager.GraphicsDevice.Viewport.Width);
                else
                    randPosX = (float)(this._obj_random.Next(0, ScreenManager.GraphicsDevice.Viewport.Width) + ScreenManager.GraphicsDevice.Viewport.Width);

                randPosY = (float)this._obj_random.Next(0, ScreenManager.GraphicsDevice.Viewport.Height);

                switch (randItem)
                {
                    case 0: //Grandfather clock
                        this.mAnimatedBGObjects.Add(new AnimatedBGEntity(this._content.Load<Texture2D>("Sprites/Misc/Animated/anim_grandfather_clock"), 4, 1, (float)((this._obj_random.NextDouble() * 10) - 5), new Vector2(randPosX, randPosY), this._obj_random.Next(0, 4), 4, 1f, 1f, randDirX, 0, randRotDir, randRotSpeed));
                        break;
                    case 1: //Other items
                        this.mAnimatedBGObjects.Add(new AnimatedBGEntity(this._content.Load<Texture2D>("Sprites/Misc/Animated/anim_coin"), 9, 1, (float)((this._obj_random.NextDouble() * 10) - 5), new Vector2(randPosX, randPosY), this._obj_random.Next(0, 10), 20, 1f, 1f, randDirX, 0, randRotDir, randRotSpeed));
                        break;
                }

                if (IsSelectedUnique == false)
                {
                    IsSelectedUnique = true;

                    int randItemUnique = this._obj_random.Next(0, 1);
                    int randDirXUnique = this._obj_random.Next(0, 2); // 0 = Left to Right, 1 = Right to Left
                    float randPosXUnique = 0f;
                    float randPosYUnique = 0f;
                    int randRotDirUnique = this._obj_random.Next(0, 2); // 0 = Left to Right, 1 = Right to Left
                    float randRotSpeedUnique = (float)((this._obj_random.NextDouble() * Math.Abs(0.04 - 0.01)) + 0.01); // Generate random rotation speed between 0.01 and 0.06 every frame.

                    if (randPosXUnique == 0)
                        randPosXUnique = (float)(this._obj_random.Next(-300, ScreenManager.GraphicsDevice.Viewport.Width) - ScreenManager.GraphicsDevice.Viewport.Width);
                    else
                        randPosXUnique = (float)(this._obj_random.Next(0, ScreenManager.GraphicsDevice.Viewport.Width) + ScreenManager.GraphicsDevice.Viewport.Width);

                    randPosYUnique = (float)this._obj_random.Next(0, ScreenManager.GraphicsDevice.Viewport.Height);

                    switch (randItemUnique)
                    {
                        case 0: //Megaman
                            this.mAnimatedBGObjects.Add(new AnimatedBGEntity(this._content.Load<Texture2D>("Sprites/Misc/Unique/anim_megaman"), 8, 1, (float)((_obj_random.NextDouble() * 10) - 5), new Vector2(randPosXUnique, randPosYUnique), _obj_random.Next(0, 9), 24, 1f, 1f, randDirXUnique, 0, randRotDirUnique, randRotSpeedUnique));
                            break;
                    }
                }
            }

            base.loadContent();
        }

        public override void update()
        {
            if (this.mAnimatedBGObjects != null)
            {
                foreach (AnimatedBGEntity entity in this.mAnimatedBGObjects)
                {
                    entity.Update(GlobalGameTimer);

                    if (entity.RotationDirection == 0)
                        entity.Animation.Rotation += entity.RotationSpeed; //Rotate Left to Right
                    else
                        entity.Animation.Rotation -= entity.RotationSpeed; //Rotate Right to Left

                    if (entity.DirectionX == 0) //Left to Right
                    {
                        if (entity.Animation.Position.X > ScreenManager.GraphicsDevice.Viewport.Width + (entity.Animation.AnimationWidth + entity.Animation.AnimationHeight))
                        {
                            entity.Animation.Position = new Vector2(entity.Animation.Position.X - (ScreenManager.GraphicsDevice.Viewport.Width + entity.Animation.AnimationWidth + entity.Animation.AnimationHeight + 100), entity.Animation.Position.Y);
                        }
                        else
                        {
                            entity.Animation.Position = new Vector2(entity.Animation.Position.X + entity.GetSpeedX, entity.Animation.Position.Y);
                        }
                    }
                    else //Right to Left
                    {
                        if (entity.Animation.Position.X < 0 - (entity.Animation.AnimationWidth + entity.Animation.AnimationHeight))
                        {
                            entity.Animation.Position = new Vector2(entity.Animation.Position.X + (ScreenManager.GraphicsDevice.Viewport.Width + entity.Animation.AnimationWidth + entity.Animation.AnimationHeight + 100), entity.Animation.Position.Y);
                        }
                        else
                        {
                            entity.Animation.Position = new Vector2(entity.Animation.Position.X - entity.GetSpeedX, entity.Animation.Position.Y);
                        }
                    }
                }
            }

            base.update();
        }

        public override void render()
        {
            GraphicsDevice tgd = ScreenManager.GraphicsDevice;
            SpriteBatch tsb = ScreenManager.SpriteBatch;
            tsb.Begin();

            if (mAnimatedBGObjects != null)
            {
                foreach (AnimatedBGEntity entity in mAnimatedBGObjects)
                {
                    entity.Draw(tsb);
                }
            }

            tsb.End();
            
            base.render();
        }

        //---------------EVENT HANDLERS-------------------------------------------------------------

        /// <summary>
        /// Event Handler to Go to character select screen.
        /// </summary>
        void EventTriggerGoToCharSelect(object sender, EventPlayer e)
        {
            ScreenManager.addScreen(new ScreenCharSelect(), e.PlayerIndex);
        }

        /// <summary>
        /// Event Handler to Go to the Options Screen.
        /// </summary>
        void EventTriggerGoToOptions(object sender, EventPlayer e)
        {
            ScreenManager.addScreen(new ScreenOptions(), e.PlayerIndex);
        }

        /// <summary>
        /// Event Handler to Go to the Credits Screen.
        /// </summary>
        void EventTriggerGoToCredits(object sender, EventPlayer e)
        {
            ScreenManager.addScreen(new ScreenCredits(), e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex? pplyrindex)
        {
            const string tmessage = "Are you sure you want to exit?";
            ScreenMsgBox tmsgbox = new ScreenMsgBox(SysConfig.ASSET_CONFIG_MSGBOX_BG, tmessage);

            tmsgbox.onAccepted += EventTriggerMsgBoxConfirm;
            ScreenManager.addScreen(tmsgbox, pplyrindex);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void EventTriggerMsgBoxConfirm(object sender, EventPlayer e)
        {
            ScreenManager.Game.Exit();
        }
    }
}