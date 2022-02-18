using NUnit.Framework;


namespace CobayeStudio.RandomToolbox.EditModeTests
{
    public class RandomSourcesTests
    {
        /// <summary>
        /// Check that when having multiples UnityRandomSources instances
        /// the result of each instance is always the same
        /// </summary>
        [Test]
        public void MultipleSeedSystemInstanceAsNoImpact()
        {
            // setup
            int instanceCount = 10;
            int valueCount = 100;
            UnityRandomSource[] instances = new UnityRandomSource[instanceCount];
            float[,] resultsA = new float[instanceCount, valueCount];
            float[,] resultsB = new float[instanceCount, valueCount];

            // generate a different seed for each instance
            for (int i = 0; i < instanceCount; i++)
            {
                UnityRandomSource newInstance = new UnityRandomSource();
                newInstance.Start(true);
                instances[i] = newInstance;
            }

            // run

            // call a value on each instance
            for (int i = 0; i < instanceCount; i++)
                for(int v = 0; v < valueCount; v++)
                    resultsA[i,v] = instances[i].value;

            // re initialize each instance
            foreach (UnityRandomSource instance in instances)
                instance.Start();

            // call a value on each instance again, but browse
            // instance array backward to change call order
            for (int i=instanceCount-1; i>=0; i--)
                for (int v = 0; v < valueCount; v++)
                    resultsB[i,v] = instances[i].value;

            // assert results are the same
            Assert.AreEqual(resultsA, resultsB);
        }

        /// <summary>
        /// Check that calling directly UnityEngine.Random doesn't impact
        /// a UnityRandomSource instance results
        /// </summary>
        [Test]
        public void ExternUnityEngineRandomCallAsNoImpact()
        {
            // setup
            int callCount = 10;
            int seed = 123456789;
            UnityRandomSource instance = new UnityRandomSource();
            float[] resultsA = new float[callCount];
            float[] resultsB = new float[callCount];
            float[] externA = new float[callCount];
            float[] externB = new float[callCount];

            // run
            instance.Start(seed);

            // get callCount values and call UnityEngine.Random in between each call
            for (int i = 0; i < callCount; i++)
            {
                resultsA[i] = instance.value;
                externA[i] = UnityEngine.Random.value;
            }

            // restart
            instance.Start();

            // get callCount values again but dont call UnityEngine.Random
            for (int i = 0; i < callCount; i++)
                resultsB[i] = instance.value;

            // get callCount values from UnityEngine.Random
            for (int i = 0; i < callCount; i++)
                externB[i] = UnityEngine.Random.value;

            // assert our instance results are the same after restart(no impact from UnityEngine.Random call)
            Assert.AreEqual(resultsA, resultsB);
        }

        /// <summary>
        /// Check that restarting a UnityRandomSource instance doesn't
        /// restart UnityEngine.Random
        /// </summary>
        [Test]
        public void UnityRandomSourceRestartDoesntImpactUnityEngineRandom()
        {
            // setup
            int callCount = 10;
            int seed = 123456789;
            UnityRandomSource instance = new UnityRandomSource();
            float[] resultsA = new float[callCount];
            float[] resultsB = new float[callCount];
            float[] externA = new float[callCount];
            float[] externB = new float[callCount];

            // run
            instance.Start(seed);

            // get callCount values and call UnityEngine.Random in between each call
            for (int i = 0; i < callCount; i++)
            {
                resultsA[i] = instance.value;
                externA[i] = UnityEngine.Random.value;
            }

            // restart
            instance.Start();

            // get callCount values again but dont call UnityEngine.Random
            for (int i = 0; i < callCount; i++)
                resultsB[i] = instance.value;

            // get callCount values from UnityEngine.Random
            for (int i = 0; i < callCount; i++)
                externB[i] = UnityEngine.Random.value;

            // assert UnityEngine.Random results are not the same (no impact from our instance restart call)
            Assert.AreNotEqual(externA, externB);
        }
    }
}
