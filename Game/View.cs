using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Game
{
    public enum TweenType
    {
        Instant,
        Linear,
        QuadraticInOut,
        CubicInOut,
        QuarticOut
    }
    class View
    {
        private Vector2 position;

        public double rotation;

        public double zoom;

        private Vector2 positionGoTo, posistionFrom;
        private TweenType tweenType;
        private int currentStep, tweenSteps;

        public Vector2 Position
        {
            get
            {
                return this.position;
            }
        }

        public Vector2 PositionGoTo
        {
            get
            {
                return this.positionGoTo;
            }
        }

        public Vector2 ToWorld(Vector2 input)
        {
            input /= (float)zoom;
            Vector2 dX = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            Vector2 dY = new Vector2((float)Math.Cos(rotation) + MathHelper.PiOver2, (float)Math.Sin(rotation) + MathHelper.PiOver2);

            return (this.position + dX * input.X + dY * input.Y);
        }

        public View(Vector2 startPosition, double startRotation = 0.0, double startZoom = 1.0)
        {
            this.position = startPosition;
            this.rotation = startRotation;
            this.zoom = startZoom;
        }

        public void Update()
        {
            if (currentStep < tweenSteps)
            {
                switch (tweenType)
                {
                    case TweenType.Linear:
                        position = posistionFrom + (positionGoTo - posistionFrom) * GetLinear((float)currentStep / tweenSteps);
                        break;
                    case TweenType.QuadraticInOut:
                        position = posistionFrom + (positionGoTo - posistionFrom) * GetQuadraticInOut((float)currentStep / tweenSteps);
                        break;
                    case TweenType.CubicInOut:
                        position = posistionFrom + (positionGoTo - posistionFrom) * GetCubicInOut((float)currentStep / tweenSteps);
                        break;
                    case TweenType.QuarticOut:
                        position = posistionFrom + (positionGoTo - posistionFrom) * GetQuarticOut((float)currentStep / tweenSteps);
                        break;
                }

                currentStep++;
            }
            else
            {
                position = positionGoTo;
            }
        }

        public void SetPosition(Vector2 newPosition)
        {
            this.position = newPosition;
            this.positionGoTo = newPosition;
            this.posistionFrom = newPosition;
            tweenSteps = 0;
            currentStep = 0;
            tweenType = TweenType.Instant;
        }

        public void SetPosition(Vector2 newPosition, TweenType type, int numSteps)
        {
            this.posistionFrom = position;
            this.position = newPosition;
            this.positionGoTo = newPosition;
            tweenSteps = numSteps;
            currentStep = 0;
            tweenType = type;
        }

        public float GetLinear(float t)
        {
            return t;
        }
        public float GetQuadraticInOut(float t)
        {
            return (t * t) / ((2 * t * t) - (2 * t) + 1);
        }
        public float GetCubicInOut(float t)
        {
            return (t * t * t) / ((3 * t * t) - (3 * t) + 1);
        }
        public float GetQuarticOut(float t)
        {
            return -((t - 1) * (t - 1) * (t - 1) * (t - 1)) + 1;
        }


        public void ApplyTranform()
        {
            Matrix4 transform = Matrix4.Identity;

            transform = Matrix4.Mult(transform,Matrix4.CreateTranslation(-position.X, -position.Y, 0));
            transform = Matrix4.Mult(transform,Matrix4.CreateRotationZ(-(float)rotation));
            transform = Matrix4.Mult(transform,Matrix4.CreateScale((float)zoom, (float)zoom, 1.0f));

            GL.MultMatrix(ref transform);
        }
    }
}
