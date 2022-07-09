using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace RandomToolbox
{
    [Serializable]
    public class Proba
    {
        public AnimationCurve ProbabilityDensity;

        // Cumulative Distribution function is the integral
        // of the Probability Density function
        public Integral InverseCumulativeDistribution;

        public Proba()
        {
            InverseCumulativeDistribution = new Integral(
                ProbabilityDensity.Evaluate,
                ProbabilityDensity.keys[0].time,
                ProbabilityDensity.keys[ProbabilityDensity.keys.Length - 1].time,
                100);
        }

        private Func<float, float> _func;
        private float[] _values;
        private float _from, _to;

    }

    [Serializable]
    public struct Function
    {
        private Vector2[] _points;
        private Vector2 _min, _max;

        public float xMin => _min.x;
        public float xMax => _min.y;
        public float yMin => _max.x;
        public float yMax => _max.y;

        public int Length => _points.Length;
        public Vector2 first => _points[0];
        public Vector2 last => _points[_points.Length - 1];
        public float this[int i] => _points[i].y;

        public float Evaluate(float x)
        {
            float clampedX = Mathf.Clamp(x, xMin, xMax);
            float t = Mathf.InverseLerp(xMin, xMax, clampedX);
            int lastIndex = _points.Length - 1;
            int index = (int)(t * lastIndex);
            if (index == lastIndex)
                return _points[index].y;
            else
            {
                float subX = Mathf.InverseLerp(index, index + 1, t * lastIndex);
                float Y = Mathf.Lerp(_points[index].y, _points[index + 1].y, subX);
                return Y;
            }
        }


        public float Evaluate01(float x)
        {
            float clampedX = Mathf.Clamp(x, xMin, xMax);
            float t = Mathf.InverseLerp(xMin, xMax, clampedX);
            int lastIndex = _points.Length - 1;
            int index = (int)(t * lastIndex);
            if (index == lastIndex)
                return _points[index].y;
            else
            {
                float subX = Mathf.InverseLerp(index, index + 1, t * lastIndex);
                float Y = Mathf.Lerp(_points[index].y, _points[index + 1].y, subX);
                return Y;
            }
        }

        public static Function Integrate(Function f, float from, float to, int steps)
        {
            return Integrate(f.Evaluate, from, to, steps);
        }

        public static Function Integrate(AnimationCurve f, float from, float to, int steps)
        {
            return Integrate(f.Evaluate, from, to, steps);
        }

        public static Function Integrate(Func<float, float> f, float from, float to, int steps)
        {
            return default;
        }

    }

    [Serializable]
    public struct Integral
    {
        //public AnimationCurve editorPreview;
        private Func<float, float> _func;
        private float[] _values;
        private float _from, _to;

        public Integral(Func<float, float> func,
                             float from, float to, int steps)
        {
            _values = new float[steps + 1];
            _func = func;
            _from = from;
            _to = to;
            ComputeValues();
        }

        private void ComputeValues()
        {
            int n = _values.Length;
            float segment = (_to - _from) / (n - 1);
            float lastY = _func(_from);
            float sum = 0;
            _values[0] = 0;
            for (int i = 1; i < n; i++)
            {
                float x = _from + i * segment;
                float nextY = _func(x);
                sum += segment * (nextY + lastY) / 2;
                lastY = nextY;
                _values[i] = sum;
            }
        }

        /// <summary>
        /// Evaluates the integrated function at any point in the interval.
        /// </summary>
        public float Evaluate(float x)
        {
            Debug.Assert(_from <= x && x <= _to);
            float t = Mathf.InverseLerp(_from, _to, x);
            int lower = (int)(t * _values.Length);
            int upper = (int)(t * _values.Length + .5f);
            if (lower == upper || upper >= _values.Length)
                return _values[lower];
            float innerT = Mathf.InverseLerp(lower, upper, t * _values.Length);
            return (1 - innerT) * _values[lower] + innerT * _values[upper];
        }

        /// <summary>
        /// Returns the total value integrated over the whole interval.
        /// </summary>
        public float Total
        {
            get
            {
                return _values[_values.Length - 1];
            }
        }
    }

    public class DistributionDemo : MonoBehaviour
    {
        public Proba d;

        private void OnValidate()
        {
            //d.area = d.function.GetArea(100);
        }

        public static float EvaluateNormalY(float x, float mean, float std_dev)
        {
            return 1 / (std_dev * Mathf.Sqrt(2 * Mathf.PI)) * Mathf.Exp(-Mathf.Pow((x - mean) / std_dev, 2) / 2);
        }

    }
}

