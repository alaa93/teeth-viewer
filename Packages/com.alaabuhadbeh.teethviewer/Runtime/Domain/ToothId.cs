using System;

namespace AlaAbuhadbeh.TeethViewer
{
    public readonly struct ToothId : IEquatable<ToothId>
    {
        public int Quadrant { get; }
        public int Position { get; }

        public ToothId(int quadrant, int position)
        {
            if (quadrant < 1 || quadrant > 4)
                throw new ArgumentOutOfRangeException(nameof(quadrant), quadrant, "FDI quadrant must be 1–4.");
            if (position < 1 || position > 8)
                throw new ArgumentOutOfRangeException(nameof(position), position, "FDI position must be 1–8.");
            Quadrant = quadrant;
            Position = position;
        }

        public int Fdi => Quadrant * 10 + Position;

        public bool IsUpper => Quadrant <= 2;
        public bool IsLower => Quadrant >= 3;

        public static ToothId FromFdi(int fdi) => new ToothId(fdi / 10, fdi % 10);

        public bool Equals(ToothId other) => Quadrant == other.Quadrant && Position == other.Position;
        public override bool Equals(object obj) => obj is ToothId other && Equals(other);
        public override int GetHashCode() => Fdi;
        public override string ToString() => Fdi.ToString();

        public static bool operator ==(ToothId a, ToothId b) => a.Equals(b);
        public static bool operator !=(ToothId a, ToothId b) => !a.Equals(b);
    }
}
