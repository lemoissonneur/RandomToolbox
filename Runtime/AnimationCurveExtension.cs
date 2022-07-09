using UnityEngine;


namespace RandomToolbox
{
    /// <summary>
    /// Allow evaluation of an area under a unity AnimationCurve using linear approximation.
    /// </summary>
    public static class AnimationCurveExtension
    {
        public static uint stepsCount = defaultStepsCount;
        public static float stepSize = defaultStepSize;

        public const uint defaultStepsCount = 100;
        public const float defaultStepSize = 0.01f;

        /// <summary>
        /// Get area under the curve from first key to last key with linear approximation.
        /// Use stepsCount for reliable performances, stepSize for reliable results.
        /// </summary>
        /// <param name="function">AnimationCurve to evaluate</param>
        /// <param name="stepsCount">Number of steps in the linear approximation (1 minimum)</param>
        /// <returns>The area under the curve (result may be negative if the curve Y values are < 0)</returns>
        public static float GetArea(this AnimationCurve function, uint stepsCount = defaultStepsCount)
        {
            float x0 = function.keys[0].time;
            float x1 = function.keys[function.length - 1].time;

            return function.GetArea(x0, x1, stepsCount);
        }

        /// <summary>
        /// Get area under the curve from first key to last key with linear approximation.
        /// </summary>
        /// <param name="function">AnimationCurve to evaluate</param>
        /// <param name="stepSize">Size of each steps in the linear approximation (must be > 0)</param>
        /// <returns>The area under the curve (result may be negative if the curve Y values are < 0)</returns>
        public static float GetArea(this AnimationCurve function, float stepSize = defaultStepSize)
        {
            float x0 = function.keys[0].time;
            float x1 = function.keys[function.length - 1].time;

            return function.GetArea(x0, x1, stepSize);
        }

        /// <summary>
        /// Get area under the curve between two given keys index.
        /// </summary>
        /// <param name="function">AnimationCurve to evaluate</param>
        /// <param name="firstKeyIndex">index of the first key</param>
        /// <param name="lastKeyIndex">index of the second key</param>
        /// <param name="stepsCount">Number of steps in the linear approximation (1 minimum)</param>
        /// <returns>The area under the curve (result may be negative if the curve Y values are < 0)</returns>
        public static float GetArea(this AnimationCurve function, uint firstKeyIndex, uint lastKeyIndex, uint stepsCount = defaultStepsCount)
        {
            float x0 = function.keys[firstKeyIndex].time;
            float x1 = function.keys[lastKeyIndex].time;

            return function.GetArea(x0, x1, stepsCount);
        }

        /// <summary>
        /// Get area under the curve between two given keys index.
        /// </summary>
        /// <param name="function">AnimationCurve to evaluate</param>
        /// <param name="firstKeyIndex">index of the first key</param>
        /// <param name="lastKeyIndex">index of the second key</param>
        /// <param name="stepSize">Size of each steps in the linear approximation (must be > 0)</param>
        /// <returns>The area under the curve (result may be negative if the curve Y values are < 0)</returns>
        public static float GetArea(this AnimationCurve function, uint firstKeyIndex, uint lastKeyIndex, float stepSize = defaultStepSize)
        {
            float x0 = function.keys[firstKeyIndex].time;
            float x1 = function.keys[lastKeyIndex].time;

            return function.GetArea(x0, x1, stepSize);
        }

        /// <summary>
        /// Get area under the curve from x0 to x1
        /// </summary>
        /// <param name="function">AnimationCurve to evaluate</param>
        /// <param name="x0">first X value</param>
        /// <param name="x1">second X value</param>
        /// <param name="stepsCount">Number of steps in the linear approximation (1 minimum)</param>
        /// <returns>The area under the curve (result may be negative if the curve Y values are < 0)</returns>
        public static float GetArea(this AnimationCurve function, float x0, float x1, uint stepsCount = defaultStepsCount)
        {
            float stepSize = (x1 - x0) / stepsCount;

            return function.GetArea(x0, x1, stepSize);
        }

        /// <summary>
        /// Get area under the curve from x0 to x1
        /// </summary>
        /// <param name="function">AnimationCurve to evaluate</param>
        /// <param name="x0">first X value</param>
        /// <param name="x1">second X value</param>
        /// <param name="stepSize">Size of each steps in the linear approximation (must be > 0)</param>
        /// <returns>The area under the curve (result may be negative if the curve Y values are < 0)</returns>
        public static float GetArea(this AnimationCurve function, float x0, float x1, float stepSize = defaultStepSize)
        {
            float area = 0;
            float yA = function.Evaluate(x0);
            float yB;

            float x;
            for (x = x0 + stepSize; x < x1; x += stepSize)
            {
                yB = function.Evaluate(x);
                area += stepSize * (yA + yB) / 2;
                yA = yB;
            }

            // correction for over/under shoot
            area += (x1 - (x - stepSize)) * (yA + function.Evaluate(x1)) / 2;

            return area;
        }
















    }
}
