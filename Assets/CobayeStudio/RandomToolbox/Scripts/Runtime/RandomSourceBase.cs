using System;
using UnityEngine;


namespace CobayeStudio.RandomToolbox
{
    /// <summary>
    /// Base class for RandomSource contained in ScriptableObjects
    /// </summary>
    public abstract class RandomSourceBaseSO : ScriptableObject
    {
        /// <summary>
        /// RandomSource instance contained in this ScriptableObject
        /// </summary>
        public RandomSourceBase Instance => m_baseInstance;

        /// <summary>
        /// Override in derived class to allow the base class acces to the RandomSource instance
        /// </summary>
        protected abstract RandomSourceBase m_baseInstance { get; }

        /// <summary>
        /// If True, Start() will be called when ScriptableObnject is Awake
        /// </summary>
        public bool InitializeOnAwake => m_initializeOnAwake;
        [Tooltip("If True, Start() will be called when ScriptableObnject is Awake")]
        [SerializeField] private bool m_initializeOnAwake = false;

        /// <summary>
        /// If True, Reseed() will be called when ScriptableObnject is Awake.
        /// Since a reseed force a re-init if this field is true, it force 'InitializeOnAwake' to true
        /// </summary>
        public bool ReseedOnAwake => m_reseedOnAwake;
        [Tooltip("If True, Reseed() will be called when ScriptableObnject is Awake.\nSince a reseed force a re-init if this field is true, it force 'InitializeOnAwake' to true")]
        [SerializeField] private bool m_reseedOnAwake = false;

        private void Awake()
        {
            if (ReseedOnAwake)
                m_baseInstance.Reseed();
            else if (m_initializeOnAwake)
                m_baseInstance.Start();
        }

        private void OnValidate()
        {
            // Since reseed of the random source trigger a re-initialization
            // if m_reseedOnAwake is true, then m_initializeOnAwake must be true as well
            m_initializeOnAwake |= m_reseedOnAwake;
        }
    }

    /// <summary>
    /// Base class for Seed System
    /// </summary>
    [Serializable]
    public abstract class RandomSourceBase
    {
        /// <summary>
        /// Current seed
        /// </summary>
        [SerializeField] protected int m_seed = default;

        /// <summary>
        /// Current select seed generator to use for Reseed()
        /// </summary>
        public SeedGenerators.Generator SeedGenerator = default;

        /// <summary>
        /// The current seed used
        /// </summary>
        public int Seed => m_seed;

        /// <summary>
        /// Initialize the Seed system with the current seed
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Initialize the Seed system with the given new seed
        /// </summary>
        /// <param name="newSeed">seed used to initialize the system</param>
        public void Start(int newSeed)
        {
            m_seed = newSeed;
            Start();
        }

        /// <summary>
        /// If reseed is true, generate a new seed using the current SeedGenerator value and initialize random source
        /// </summary>
        /// <param name="reseed"></param>
        public void Start(bool reseed)
        {
            if (reseed) m_seed = SeedGenerator.GetSeed();
            Start();
        }

        /// <summary>
        /// Initialize the Seed system with a new seed from the given seed generator
        /// </summary>
        /// <param name="seedGenerator">Generator to use to get a new seed</param>
        public void Start(SeedGenerators.Generator seedGenerator)
        {
            SeedGenerator = seedGenerator;
            m_seed = SeedGenerator.GetSeed();
            Start();
        }

        /// <summary>
        /// Generate a new seed using the current SeedGenerator value and initialize random source
        /// this is a shorthand for 'Start(SeedGenerator.GetSeed());'
        /// </summary>
        public void Reseed()
        {
            m_seed = SeedGenerator.GetSeed();
            Start();
        }
    }
}
