using System;

namespace RiseExtensions {

    public static class MathUtilities {

        /// <summary>
        /// Clamp the specified value to be between the minimum and maximum values.
        /// </summary>
        /// <returns>The clamped value.</returns>
        /// <param name="val">Value.</param>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T> {
            if (val.CompareTo(min) < 0) return min;
            if (val.CompareTo(max) > 0) return max;
            return val;
        }

        /// <summary>
        /// Checks whether the magnitude of the float value is greater than the passed epsilon value.
        /// </summary>
        /// <returns><c>true</c>, if epsilon check passes, <c>false</c> otherwise.</returns>
        /// <param name="val">Value.</param>
        /// <param name="epsilon">Epsilon.</param>
        public static bool CheckEpsilon(this float val, float epsilon) {
            return Math.Abs(val) > 0;
        }

        /// <summary>
        /// Checks whether the magnitude of the float value is greater than the passed epsilon value.
        /// </summary>
        /// <returns><c>true</c>, if epsilon check passes, <c>false</c> otherwise.</returns>
        /// <param name="val">Value.</param>
        /// <param name="epsilon">Epsilon.</param>
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

    public enum SapType {
        SPRING,
        SUMMER,
        AUTUMN,
        WINTER
    }

    public enum GameMode {
        TIMED,
        ZEN,
    }
}
