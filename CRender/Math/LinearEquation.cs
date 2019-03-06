using System;

namespace CRender.Math
{
    [Obsolete]
    public struct LinearEquation_XMajor
    {
        public float this[float x] => _slope * x + _interceptY;

        private readonly float _slope;

        private readonly float _interceptY;

        public LinearEquation_XMajor(Vector2 a, Vector2 b)
        {
            _slope = (a.Y - b.Y) / (a.X - b.X);
            _interceptY = a.Y - _slope * a.X;
        }
    }

    [Obsolete]
    public struct LinearEquation_YMajor
    {
        public float this[float y] => _slopeInv * y + _interceptX;

        private readonly float _slopeInv;

        private readonly float _interceptX;

        public LinearEquation_YMajor(Vector2 a, Vector2 b)
        {
            _slopeInv = (a.X - b.X) / (a.Y - b.Y);
            _interceptX = a.X - _slopeInv * a.Y;
        }
    }
}
