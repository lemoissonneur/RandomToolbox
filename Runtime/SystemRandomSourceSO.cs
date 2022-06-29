using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

using Random = System.Random;


namespace RandomToolbox
{
    /// <summary>
    /// A SystemRandomSource instance contained in a ScriptableObject to be used
    /// in different scenes
    /// </summary>
    [CreateAssetMenu(menuName = "lemoissonneur/Random Toolbox/System Source SO")]
    public class SystemRandomSourceSO : RandomSourceBaseSO
    {
        /// <summary>
        /// SystemRandomSource instance
        /// </summary>
        public new SystemRandomSource Instance = new SystemRandomSource();

        /// <summary>
        /// Override to give the base class acces to the derived RandomSourceBase instance
        /// </summary>
        protected override RandomSourceBase m_baseInstance => Instance;
    }

    /// <summary>
    /// Safe Random source based on System.Random
    /// </summary>
    [Serializable]
    public class SystemRandomSource : RandomSourceBase
    {
        /// <summary>
        /// our System.Random instance
        /// </summary>
        private Random m_Random = null;

        /// <summary>
        /// Instantiate a new SystemRandomSource but does not initialize it
        /// </summary>
        public SystemRandomSource() { }

        /// <summary>
        /// Instantiate a new UnityRandomSource and Initialize with given seed.
        /// </summary>
        /// <param name="seed">Seed to initialize the random generator</param>
        public SystemRandomSource(int seed)
        {
            m_seed = seed;
            Start();
        }

        /// <summary>
        /// Initialize the Seed system with the current seed
        /// </summary>
        public override void Start()
        {
            m_Random = new Random(m_seed);
        }

        /// <summary>
        /// same as System.Random.Next()
        /// </summary>
        /// <returns>int</returns>
        public int Next() => m_Random.Next();
        
        /// <summary>
        /// same as System.Random.Next(int maxValue)
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public int Next(int maxValue) => m_Random.Next(maxValue);

        /// <summary>
        /// same as System.Random.Next(int minValue, int maxValue)
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public int Next(int minValue, int maxValue) => m_Random.Next(minValue, maxValue);

        /// <summary>
        /// same as System.Random.NextBytes(byte[] buffer)
        /// </summary>
        /// <param name="buffer"></param>
        public void NextBytes(byte[] buffer) => m_Random.NextBytes(buffer);

        /// <summary>
        /// same as System.Random.NextDouble()
        /// </summary>
        /// <returns></returns>
        public double NextDouble() => m_Random.NextDouble();

        /// <summary>
        /// Get the current state to save and restore it later
        /// </summary>
        /// <returns></returns>
        public State Save()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using MemoryStream temp = new MemoryStream();
            binaryFormatter.Serialize(temp, m_Random);
            return new State() { bytes = temp.ToArray() };
        }

        /// <summary>
        /// Restore the random source with the given state
        /// </summary>
        /// <param name="state"></param>
        public void Restore(State state)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using MemoryStream temp = new MemoryStream(state.bytes);
            m_Random = (Random)binaryFormatter.Deserialize(temp);
        }

        /// <summary>
        /// Used to save System.Random state
        /// </summary>
        [Serializable]
        public struct State
        {
            public byte[] bytes; 
        }
    }
}

