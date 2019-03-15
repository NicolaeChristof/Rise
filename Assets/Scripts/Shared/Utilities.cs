using System;

namespace RiseExtensions {

    public static class MathUtilities {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T> {
            if (val.CompareTo(min) < 0) return min;
            if (val.CompareTo(max) > 0) return max;
            return val;
        }

        public static bool CheckEpsilon(this float val, float epsilon) {
            return Math.Abs(val) > 0;
        }

        public static bool CheckEpsilon(this double val, double epsilon) {
            return Math.Abs(val) > 0;
        }
    }

    public static class Tags {
        public static readonly string BRANCH = "Branch";
        public static readonly string DEADZONE = "Dead Zone";
        public static readonly string SAP = "Sap";
        public static readonly string PLAYER = "Player";
    }

}
