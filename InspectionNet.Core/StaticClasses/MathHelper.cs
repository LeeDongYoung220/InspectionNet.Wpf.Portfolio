using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

using InspectionNet.Core.Implementations;

namespace InspectionNet.Core.StaticClasses
{
    public static class MathHelper
    {
        // 소인수 분해 함수
        public static List<int> PrimeFactorization(int n)
        {
            List<int> factors = new List<int>();

            // 2로 나누어 떨어질 때까지 나누기
            while (n % 2 == 0)
            {
                factors.Add(2);
                n /= 2;
            }

            // 3부터 홀수만 검사 (2는 이미 처리했으므로)
            for (int i = 3; i * i <= n; i += 2)
            {
                while (n % i == 0)
                {
                    factors.Add(i);
                    n /= i;
                }
            }

            // 마지막 남은 소수가 1보다 크다면 추가
            if (n > 1)
            {
                factors.Add(n);
            }

            return factors;
        }

        // 소인수 들을 이용해서 약수 찾는 함수
        public static List<int> GetDivisorsFromPrimeFactors(int n)
        {
            List<int> primeFactors = PrimeFactorization(n);
            HashSet<int> divisors = new HashSet<int>();

            int factorCount = primeFactors.Count;
            int subsetCount = 1 << factorCount; // 부분 집합 개수 = 2^개수

            // 모든 소인수 조합을 이용해 약수 생성
            for (int i = 0; i < subsetCount; i++)
            {
                int divisor = 1;
                for (int j = 0; j < factorCount; j++)
                {
                    if ((i & (1 << j)) != 0)
                    {
                        divisor *= primeFactors[j];
                    }
                }
                divisors.Add(divisor);
            }

            return divisors.OrderBy(x => x).ToList();
        }

        public static List<int> GetDivisors(int num)
        {
            List<int> divisors = new List<int>();
            for (int i = 1; i * i <= num; i++)
            {
                if (num % i == 0)
                {
                    divisors.Add(i);
                    if (i != num / i) // i와 num / i가 다를 때만 추가
                    {
                        divisors.Add(num / i);
                    }
                }
            }
            divisors.Sort(); // 정렬 (필요하면)
            return divisors;
        }

        public static float ConvertAngleToSigned(float unsignedAngle)
        {
            float signedAngle = unsignedAngle % 360; // 0 ~ 360 사이로 변환
            if (signedAngle > 180) signedAngle -= 360;
            return signedAngle;
        }

        public static float ConvertAngleToUnsigned(float signedAngle)
        {
            float unsignedAngle = signedAngle < 0 ? signedAngle + 360 : signedAngle;
            return unsignedAngle;
        }

        public static void ConvertRotation(float deg, float originX, float originY, ref float pointX, ref float pointY)
        {
            double rad = ConvertDeg2Rad(deg);
            float resX = (pointX - originX) * (float)Math.Cos(rad) - (pointY - originY) * (float)Math.Sin(rad) + originX;
            float resY = (pointX - originX) * (float)Math.Sin(rad) + (pointY - originY) * (float)Math.Cos(rad) + originY;
            pointX = resX;
            pointY = resY;
        }
        public static void ConvertRotation(double deg, double originX, double originY, ref double pointX, ref double pointY)
        {
            double rad = ConvertDeg2Rad(deg);
            double resX = (pointX - originX) * Math.Cos(rad) - (pointY - originY) * Math.Sin(rad) + originX;
            double resY = (pointX - originX) * Math.Sin(rad) + (pointY - originY) * Math.Cos(rad) + originY;
            pointX = resX;
            pointY = resY;
        }
        public static PointF ConvertRotation(float deg, PointF origin, PointF point)
        {
            double rad = ConvertDeg2Rad(deg);
            float resX = (point.X - origin.X) * (float)Math.Cos(rad) - (point.Y - origin.Y) * (float)Math.Sin(rad) + origin.X;
            float resY = (point.X - origin.X) * (float)Math.Sin(rad) + (point.Y - origin.Y) * (float)Math.Cos(rad) + origin.Y;
            return new PointF(resX, resY);
        }
        //public static PointF ConvertRotation(float deg, PointF origin, PointF point)
        //{
        //    double rad = ConvertDeg2Rad(deg);
        //    double cos = Math.Cos(rad);
        //    double sin = Math.Sin(rad);

        //    double tx = origin.X * (1 - cos) + origin.Y * sin;
        //    double ty = origin.Y * (1 - cos) - origin.X * sin;

        //    double[,] matrix = new double[3, 3]
        //    {
        //        { cos, -sin, tx },
        //        { sin, cos, ty },
        //        { 0, 0, 1 }
        //    };

        //    double resX = matrix[0, 0] * point.X + matrix[0, 1] * point.Y + matrix[0, 2];
        //    double resY = matrix[1, 0] * point.X + matrix[1, 1] * point.Y + matrix[1, 2];

        //    return new PointF((float)resX, (float)resY);
        //}
        public static PointD ConvertRotation(double deg, PointD origin, PointD point)
        {
            double rad = ConvertDeg2Rad(deg);
            double resX = (point.X - origin.X) * Math.Cos(rad) - (point.Y - origin.Y) * Math.Sin(rad) + origin.X;
            double resY = (point.X - origin.X) * Math.Sin(rad) + (point.Y - origin.Y) * Math.Cos(rad) + origin.Y;
            return new PointD(resX, resY);
        }

        public static double ConvertDeg2Rad(double deg) => (Math.PI / 180.0) * deg;
        public static double ConvertRad2Deg(double rad) => (180.0 / Math.PI) * rad;

        public static double GetAngle(float originX, float originY, float pointX, float pointY) => ConvertRad2Deg(Math.Atan2(pointY - originY, pointX - originX));
        public static double GetAngle(double originX, double originY, double pointX, double pointY) => ConvertRad2Deg(Math.Atan2(pointY - originY, pointX - originX));
        public static double GetAngle(PointF p1, PointF p2) => ConvertRad2Deg(Math.Atan2(p2.Y - p1.Y, p2.X - p1.X));
        public static double GetAngle(PointD p1, PointD p2) => ConvertRad2Deg(Math.Atan2(p2.Y - p1.Y, p2.X - p1.X));

        public static double GetDistance(float p1X, float p1Y, float p2X, float p2Y) => Math.Sqrt(Math.Pow(p1X - p2X, 2) + Math.Pow(p1Y - p2Y, 2));
        public static double GetDistance(double p1X, double p1Y, double p2X, double p2Y) => Math.Sqrt(Math.Pow(p1X - p2X, 2) + Math.Pow(p1Y - p2Y, 2));
        public static double GetDistance(PointF p1, PointF p2) => Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        public static double GetDistance(PointD p1, PointD p2) => Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));

        public static PointF CalcIntersectPoint(PointF A, PointF B, PointF C, PointF D)
        {
            // Line AB represented as a1x + b1y = c1
            double a1 = B.Y - A.Y;
            double b1 = A.X - B.X;
            double c1 = a1 * (A.X) + b1 * (A.Y);

            // Line CD represented as a2x + b2y = c2
            double a2 = D.Y - C.Y;
            double b2 = C.X - D.X;
            double c2 = a2 * (C.X) + b2 * (C.Y);

            double determinant = a1 * b2 - a2 * b1;

            if (determinant.Equals(0.0))
            {
                // The lines are parallel. This is simplified
                // by returning a pair of FLT_MAX
                return default;
            }
            else
            {
                double x = (b2 * c1 - b1 * c2) / determinant;
                double y = (a1 * c2 - a2 * c1) / determinant;
                return new PointF(Convert.ToSingle(x), Convert.ToSingle(y));
            }
        }
        public static PointD CalcIntersectPoint(PointD A, PointD B, PointD C, PointD D)
        {
            // Line AB represented as a1x + b1y = c1
            double a1 = B.Y - A.Y;
            double b1 = A.X - B.X;
            double c1 = a1 * (A.X) + b1 * (A.Y);

            // Line CD represented as a2x + b2y = c2
            double a2 = D.Y - C.Y;
            double b2 = C.X - D.X;
            double c2 = a2 * (C.X) + b2 * (C.Y);

            double determinant = a1 * b2 - a2 * b1;

            if (determinant.Equals(0.0))
            {
                // The lines are parallel. This is simplified
                // by returning a pair of FLT_MAX
                return default;
            }
            else
            {
                double x = (b2 * c1 - b1 * c2) / determinant;
                double y = (a1 * c2 - a2 * c1) / determinant;
                return new PointD(x, y);
            }
        }
        public static PointF CalcIntersectPoint(PointF point1, double angle1, PointF point2, double angle2)
        {
            // 각도를 라디안으로 변환
            double rad1 = ConvertDeg2Rad(angle1);
            double rad2 = ConvertDeg2Rad(angle2);

            // 직선의 방정식: y = mx + b 형태로 변환
            double m1 = Math.Tan(rad1);
            double m2 = Math.Tan(rad2);

            // 직선의 y절편 계산
            double b1 = point1.Y - m1 * point1.X;
            double b2 = point2.Y - m2 * point2.X;

            // 두 직선이 평행한 경우 교점이 없음
            if (m1.Equals(m2))
            {
                return default;
            }

            // 교점 계산
            double x = (b2 - b1) / (m1 - m2);
            double y = m1 * x + b1;

            return new PointF((float)x, (float)y);
        }
        public static PointD CalcIntersectPoint(PointD point1, double angle1, PointD point2, double angle2)
        {
            // 각도를 라디안으로 변환
            double rad1 = ConvertDeg2Rad(angle1);
            double rad2 = ConvertDeg2Rad(angle2);

            // 직선의 방정식: y = mx + b 형태로 변환
            double m1 = Math.Tan(rad1);
            double m2 = Math.Tan(rad2);

            // 직선의 y절편 계산
            double b1 = point1.Y - m1 * point1.X;
            double b2 = point2.Y - m2 * point2.X;

            // 두 직선이 평행한 경우 교점이 없음
            if (m1.Equals(m2))
            {
                return default;
            }

            // 교점 계산
            double x = (b2 - b1) / (m1 - m2);
            double y = m1 * x + b1;

            return new PointD(x, y);
        }

        //한점과 기울기를 알 때, 해당 기울기를 가지는 직선과 다른 한점 사이의 거리를 구하는 함수
        public static double GetDistanceFromPointToLine(PointF point, double deg, PointF linePoint)
        {
            double slope = Math.Tan(ConvertDeg2Rad(deg));
            double a = -slope;
            double b = 1;
            double c = slope * linePoint.X - linePoint.Y;

            return Math.Abs(a * point.X + b * point.Y + c) / Math.Sqrt(a * a + b * b);
        }
        public static double GetDistanceFromPointToLine(PointD point, double deg, PointD linePoint)
        {
            double slope = Math.Tan(ConvertDeg2Rad(deg));
            double a = -slope;
            double b = 1;
            double c = slope * linePoint.X - linePoint.Y;

            return Math.Abs(a * point.X + b * point.Y + c) / Math.Sqrt(a * a + b * b);
        }

        public static float GetAngleBetweenTwoPointsWithFixedPoint(PointF tPt1, PointF tPt2, PointF tPtFixed)
        {
            float snAngle1 = (float)Math.Atan2(tPt1.Y - tPtFixed.Y, tPt1.X - tPtFixed.X);
            float snAngle2 = (float)Math.Atan2(tPt2.Y - tPtFixed.Y, tPt2.X - tPtFixed.X);
            return snAngle1 - snAngle2;
        }

        public static PointF GetPerpendicularFoot(PointF point, double lineAngle, PointF linePoint)
        {
            // 수직선인 경우 (90도 또는 270도)
            if (Math.Abs(lineAngle % 180).Equals(90))
            {
                return new PointF(linePoint.X, point.Y);
            }

            double rad = ConvertDeg2Rad(lineAngle);
            double m1 = Math.Tan(rad); // 직선의 기울기
            double m2 = -1 / m1;        // 수선의 기울기

            // 원래 직선: y = m1 * (x - x0) + y0
            // 수선:      y = m2 * (x - x1) + y1
            double x0 = linePoint.X;
            double y0 = linePoint.Y;
            double x1 = point.X;
            double y1 = point.Y;

            // 두 직선의 교점 (수선의 발)
            double x = (m1 * x0 - m2 * x1 + y1 - y0) / (m1 - m2);
            double y = m1 * (x - x0) + y0;

            return new PointF((float)x, (float)y);
        }

        public static List<PointF> ExpandPolygon(List<PointF> points, double offset)
        {
            List<PointF> newPoints = new List<PointF>();
            if (points.Count == 0) return newPoints;
            for (int i = 0; i < points.Count; i++)
            {
                int curr = i;
                int prev = (i + points.Count - 1) % points.Count;
                int next = (i + 1) % points.Count;

                //PointF prevVector = new PointF(points[i].X - points[prev].X, points[i].Y - points[prev].Y);
                //prevVector = VectorNormalize(prevVector);
                //PointF _prevVector = new PointF(prevVector.Y, -prevVector.X);   //Pixel 좌표계 기준 -90deg 회전

                //PointF nextVector = new PointF(points[next].X - points[i].X, points[next].Y - points[i].Y);
                //nextVector = VectorNormalize(nextVector);
                //PointF _nextVector = new PointF(nextVector.Y, -nextVector.X);   //Pixel 좌표계 기준 -90deg 회전

                if (points[curr] != default && points[prev] != default && points[next] != default)
                {
                    Vector2 vPrev = new Vector2(points[curr].X - points[prev].X, points[curr].Y - points[prev].Y);
                    vPrev = Vector2.Normalize(vPrev);
                    vPrev *= (float)offset;
                    Vector2 _vPrev = new Vector2(vPrev.Y, -vPrev.X);

                    PointF pPrev1 = new PointF(points[prev].X + _vPrev.X, points[prev].Y + _vPrev.Y);
                    PointF pPrev2 = new PointF(points[curr].X + _vPrev.X, points[curr].Y + _vPrev.Y);

                    Vector2 vNext = new Vector2(points[next].X - points[curr].X, points[next].Y - points[curr].Y);
                    vNext = Vector2.Normalize(vNext);
                    vNext *= (float)offset;
                    Vector2 _vNext = new Vector2(vNext.Y, -vNext.X);

                    PointF pNext1 = new PointF(points[curr].X + _vNext.X, points[curr].Y + _vNext.Y);
                    PointF pNext2 = new PointF(points[next].X + _vNext.X, points[next].Y + _vNext.Y);

                    newPoints.Add(CalcIntersectPoint(pPrev1, pPrev2, pNext1, pNext2));
                }
            }

            return newPoints;
        }
    }
}
