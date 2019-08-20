using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Input;
using OpenTK.Input;

namespace Game
{
    class Render : GameWindow
    {
        List<Texture2D> textures;
        View view;
        public Render(int width, int height) 
            : base(width, height)
        {
            GL.Enable(EnableCap.Texture2D);

            view = new View(Vector2.Zero, 0.0, 1.0);
            
            Input.Initialize(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            textures = new List<Texture2D>();
            base.OnLoad(e);
            textures.Add(ContentPipe.LoadTexture("T1/bricks.png"));
            textures.Add(ContentPipe.LoadTexture("T1/dark_oak_planks.png"));
            textures.Add(ContentPipe.LoadTexture("T1/dirt.png"));
            textures.Add(ContentPipe.LoadTexture("T1/grass_block_side.png"));
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (Input.MousePress(MouseButton.Left))
            {
                var x = Mouse.GetCursorState().X - X;
                var y = Mouse.GetCursorState().Y - Y;
                Vector2 pos = new Vector2(x, y) - new Vector2(Width, Height) / 2f;
                pos = view.ToWorld(pos);
                view.SetPosition(pos, TweenType.Linear, 15);
            }
            if (Input.KeyPress(Key.Up))
            {
                view.SetPosition(view.PositionGoTo + new Vector2(0, -20), TweenType.Linear, 15);
            }
            if (Input.KeyPress(Key.Down))
            {
                view.SetPosition(view.PositionGoTo + new Vector2(0, 20), TweenType.Linear, 15);
            }
            if (Input.KeyPress(Key.Left))
            {
                view.SetPosition(view.PositionGoTo + new Vector2(-20, 0), TweenType.Linear, 15);
            }
            if (Input.KeyPress(Key.Right))
            {
                view.SetPosition(view.PositionGoTo + new Vector2(20, 0), TweenType.Linear, 15);
            }

            view.Update();
            Input.Update();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.CornflowerBlue);

            Spritebatch.Begin(this.Width,this.Height);
            view.ApplyTranform();

            for (int i = -960; i < 960; i+=48) //земля
            {
                Spritebatch.Draw(textures[3], new Vector2(i, 0), new Vector2(3f, 3f), Color.Transparent, new Vector2(0, 0));
                for (int j = 48; j < 480; j += 48) //земля
                {
                    Spritebatch.Draw(textures[2], new Vector2(i, j), new Vector2(3f, 3f), Color.Transparent, new Vector2(0, 0));
                }
            }
            Spritebatch.Draw(textures[0], new Vector2(-96, 0), new Vector2(3f, 3f), Color.Transparent, new Vector2(0, 0));
            Spritebatch.Draw(textures[0], new Vector2(240, 0), new Vector2(3f, 3f), Color.Transparent, new Vector2(0, 0));

            for (int i = -48; i < 240; i += 48)
            {
                for (int j = -144; j < 0; j += 48)
                {
                    Spritebatch.Draw(textures[1], new Vector2(i, j), new Vector2(3f, 3f), Color.DarkGray, new Vector2(0, 0));
                }
            }
            Spritebatch.Draw(textures[1], new Vector2(-96, -48), new Vector2(3f, 3f), Color.DarkGray, new Vector2(0, 0));
            Spritebatch.Draw(textures[1], new Vector2(-96, -96), new Vector2(3f, 3f), Color.DarkGray, new Vector2(0, 0));
            Spritebatch.Draw(textures[1], new Vector2(240, -48), new Vector2(3f, 3f), Color.DarkGray, new Vector2(0, 0));
            Spritebatch.Draw(textures[1], new Vector2(240, -96), new Vector2(3f, 3f), Color.DarkGray, new Vector2(0, 0));

            for (int i = -48; i < 240; i += 48)
            {
                Spritebatch.Draw(textures[1], new Vector2(i, 0), new Vector2(3f, 3f), Color.Transparent, new Vector2(0, 0));
            }

            for (int i = -96; i < 288; i += 48)
            {
                Spritebatch.Draw(textures[0], new Vector2(i, -192), new Vector2(3f, 3f), Color.Transparent, new Vector2(0, 0));
            }
            Spritebatch.Draw(textures[0], new Vector2(-96, -144), new Vector2(3f, 3f), Color.Transparent, new Vector2(0, 0));
            Spritebatch.Draw(textures[0], new Vector2(240, -144), new Vector2(3f, 3f), Color.Transparent, new Vector2(0, 0));

            SwapBuffers();
        }
    }
}
