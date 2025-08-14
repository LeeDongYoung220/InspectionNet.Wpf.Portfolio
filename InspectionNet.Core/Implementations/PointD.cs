using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionNet.Core.Implementations
{
    public struct PointD
    {
        #region Properties
        public double X { get; set; }
        public double Y { get; set; }

        public static readonly PointD Empty = new();

        public readonly bool IsEmpty => Equals(X, 0.0) && Equals(Y, 0.0);
        #endregion

        #region Constructors
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public PointD(PointF pointF)
        {
            X = pointF.X;
            Y = pointF.Y;
        }
        #endregion

        #region Methods
        public override readonly bool Equals(object? obj)
        {
            if (obj is not PointD)
                return false;

            PointD comp = (PointD)obj;
            return Equals(X, comp.X) && Equals(Y, comp.Y);
        }

        public override readonly int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();

        public static bool operator ==(PointD left, PointD right)
        {
            return Equals(left.X, right.X) && Equals(left.Y, right.Y);
        }

        public static bool operator !=(PointD left, PointD right)
        {
            return !(left == right);
        }

        public override readonly string ToString() => $"{{X={X}, Y={Y}}}";
        #endregion
    }

}
