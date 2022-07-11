using UnityEngine;
using NUnit.Framework;
using System;


namespace RandomToolbox
{
    public class CumulativeDistributionTests
    {
        /// <summary>
        /// number of sample taken when evaluating a curve
        /// </summary>
        public const int CurveSampleCount = 1000;

        /// <summary>
        /// number of random calls for the test
        /// </summary>
        public const int ResultsSampleCount = 100000;

        /// <summary>
        /// width used to compare area under the normal curve and the results.
        /// Lowering the value increase precision, but will need higher "ResultsSampleCount" to pass the test
        /// </summary>
        public const float ResultsSampleWidth = 0.1f;

        /// <summary>
        /// Max delta when comparing expected and result floats
        /// Lowering the value reduce tolerance, but will need higher "ResultsSampleCount" to pass the test
        /// </summary>
        public const float ResultsTolerance = 0.001f;

        /// <summary>
        /// Check CumulativeDistribution results for a normal distribution
        /// </summary>
        [Test]
        public void CumulativeDistributionNormalResults()
        {
            // values for the normal distribution function
            float mean = 10, std_dev = 2;

            // random source
            UnityRandomSource random = new UnityRandomSource();
            random.Start();

            // Whe sample the normal distribution from [mean-samplingRange] to [mean+samplingRange]
            float samplingRange = std_dev * 10;

            // number of sampling points
            int steps = (int)(2 * samplingRange / ResultsSampleWidth);
            float sampleWeight = (float)1 / (float)ResultsSampleCount;

            // data arrays
            float[] expected = new float[steps];
            float[] results = new float[steps];

            // instantiate our Cumulative distribution function from a normal distribution equation
            CumulativeDistribution TestDistribution = new CumulativeDistribution(
                (x) => EvaluateNormalY(x, mean, std_dev),
                mean - samplingRange,
                mean + samplingRange,
                CurveSampleCount);

            // build expected results table, this is the area under the normal curve
            // between each sample points
            for(int i=0; i< steps; i++)
            {
                float x = mean - samplingRange + i * ResultsSampleWidth;
                float ya = EvaluateNormalY(x, mean, std_dev);
                float yb = EvaluateNormalY(x + ResultsSampleWidth, mean, std_dev);
                expected[i] = ResultsSampleWidth * ( ya + yb ) / 2;
            }

            // function to translate a result to it's index in the result table
            Func<float, int> insertInTable = x =>
            {
                float index_t = Mathf.InverseLerp(mean - samplingRange, mean + samplingRange, x);
                float index_f = Mathf.Lerp(0, steps - 1, index_t);
                int index = Mathf.RoundToInt(index_f);
                return index;
            };

            // build the result table with our test distribution class
            for(int i=0; i<ResultsSampleCount; i++)
            {
                float v = TestDistribution.GetValue(random.value);
                int index = insertInTable(v);
                results[index] += sampleWeight;
            }

            // assert
            Assert.That(expected, Is.EqualTo(results).Within(ResultsTolerance));
        }

        public static float EvaluateNormalY(float x, float mean, float std_dev)
        {
            return 1 / (std_dev * Mathf.Sqrt(2 * Mathf.PI)) * Mathf.Exp(-Mathf.Pow((x - mean) / std_dev, 2) / 2);
        }
    }
}
