using System;

namespace Redox.API.Player
{
    /// <summary>
    /// Represents a generic vector point.
    /// </summary>
    public class Position
    {
        public float X;
        public float Y;
        public float Z;

        public Position(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Position))
                return false;

            Position pos = (Position)obj;
            return (this.X == pos.X) && (this.Y == pos.Y) && (this.Z == pos.Z);
        }
    }
}
