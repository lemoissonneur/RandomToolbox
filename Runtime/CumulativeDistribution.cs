using UnityEngine;
using System;


namespace RandomToolbox
{
    public class CumulativeDistribution
    {
        // Cumulative Distribution function is the integral
        // of the Probability Density function
        public Curve InverseCumulativeDistribution;

        public CumulativeDistribution(AnimationCurve probabilityDensity, int sampleSteps)
        {
            if (probabilityDensity != null && probabilityDensity.keys.Length > 1)
            {
                Curve ProbabilityDensityIntegral = Curve.Integrate(
                    probabilityDensity.Evaluate,
                    probabilityDensity.keys[0].time,
                    probabilityDensity.keys[probabilityDensity.keys.Length - 1].time,
                    sampleSteps
                );

                InverseCumulativeDistribution = Curve.Invert(ProbabilityDensityIntegral);
            }
            else Debug.Log("probabilityDensity param must have at least two keys");
        }

        public float GetValue(float value)
        {
            return InverseCumulativeDistribution.Evaluate(
                Mathf.Lerp(
                    InverseCumulativeDistribution.Min.x,
                    InverseCumulativeDistribution.Max.x,
                    value
                )
            );
        }
    }

    [Serializable]
    public struct Curve
    {
        [SerializeField] internal Vector2[] _points;
        [SerializeField] internal Vector2 _min, _max;

        public Vector2 Min => _min;
        public Vector2 Max => _max;

        public int Length => _points.Length;

        public float Evaluate(float x)
        {
            float clampedX = Mathf.Clamp(x, _points[0].x, _points[_points.Length - 1].x);

            for (int i=0; i < _points.Length-1; i++)
            {
                if (_points[i].x >= clampedX)
                {
                    float xInterpolate = Mathf.InverseLerp(_points[i].x, _points[i + 1].x, x);
                    return Mathf.Lerp(_points[i].y, _points[i+1].y, xInterpolate);
                }
            }

            return _points[_points.Length - 1].y;
        }

        public static Curve Integrate(Curve f, float from, float to, int steps)
        {
            return Integrate(f.Evaluate, from, to, steps);
        }

        public static Curve Integrate(AnimationCurve f, float from, float to, int steps)
        {
            return Integrate(f.Evaluate, from, to, steps);
        }

        public static Curve Integrate(Func<float, float> f, float from, float to, int steps)
        {
            Curve integralCurve = new Curve
            {
                _points = new Vector2[steps + 1]
            };

            float segment = (to - from) / steps;
            float lastY = f(from);
            float sum = 0;
            integralCurve._points[0] = new Vector2(from, 0);
            for (int i = 1; i <= steps; i++)
            {
                float x = from + i * segment;
                float nextY = f(x);
                sum += segment * (nextY + lastY) / 2;
                lastY = nextY;
                integralCurve._points[i] = new Vector2(x, sum);
            }

            integralCurve._min = new Vector2(from, 0);
            integralCurve._max = new Vector2(to, sum);

            return integralCurve;
        }

        public static Curve Invert(Curve f)
        {
            Curve invertedCurve = new Curve
            {
                _points = new Vector2[f.Length]
            };

            for (int i=0; i<f.Length; i++)
            {
                invertedCurve._points[i] = new Vector2(f._points[i].y, f._points[i].x);
            }

            invertedCurve._min = new Vector2(f.Min.y, f.Min.x);
            invertedCurve._max = new Vector2(f.Max.y, f.Max.x);

            return invertedCurve;
        }
    }
}

