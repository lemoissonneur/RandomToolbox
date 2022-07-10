using System;
using UnityEngine;

using Random = UnityEngine.Random;


namespace RandomToolbox
{
    /// <summary>
    /// A unityRandomSource instance contained in a ScriptableObject to be used
    /// in different scenes
    /// </summary>
    [CreateAssetMenu(menuName = "lemoissonneur/Random Toolbox/Unity Source SO")]
    public class UnityRandomSourceSO : RandomSourceBaseSO
    {
        /// <summary>
        /// UnityRandomSource instance
        /// </summary>
        public new UnityRandomSource Instance;

        /// <summary>
        /// Override to give the base class acces to the derived RandomSourceBase instance
        /// </summary>
        protected override RandomSourceBase m_baseInstance => Instance;
    }

    /// <summary>
    /// Safe Random source based on UnityEngine.Random
    /// </summary>
    [Serializable]
    public class UnityRandomSource : RandomSourceBase
    {
        /// <summary>
        /// current save state of the Random System
        /// </summary>
        private Random.State State = default;

        /// <summary>
        /// temp struct to save and restore the state of UnityEngine.Random between calls
        /// </summary>
        private Random.State m_originState;

        /// <summary>
        /// Instantiate a new UnityRandomSource but does not initialize it
        /// </summary>
        public UnityRandomSource() { }

        /// <summary>
        /// Instantiate a new UnityRandomSource and Initialize with given seed.
        /// </summary>
        /// <param name="seed">Seed to initialize the random generator</param>
        public UnityRandomSource(int seed)
        {
            m_seed = seed;
            Start();
        }

        /// <summary>
        /// Instantiate a new UnityRandomSource with a new generated seed
        /// based on given seed generator
        /// </summary>
        /// <param name="generateSeed">Generate a</param>
        public UnityRandomSource(SeedGenerators.Generator seedGenerator)
        {
            SeedGenerator = seedGenerator;
            m_seed = SeedGenerator.GetSeed();
            Start();
        }

        /// <summary>
        /// Instantiate a new UnityRandomSource with a given state
        /// </summary>
        /// <param name="state">the state to use for initialize</param>
        public UnityRandomSource(Random.State state)
        {
            State = state;
        }

        /// <summary>
        /// Initialize the Seed system with the current seed
        /// </summary>
        public override void Start()
        {
            m_originState = Random.state;
            Random.InitState(m_seed);
            SaveState();
        }

        /// <summary>
        /// Save the current Random.state and restore the previous state, this function is called after each Random call
        /// </summary>
        private void SaveState()
        {
            State = Random.state;
            Random.state = m_originState;
        }

        /// <summary>
        /// Save the current Random.state in m_originState and load our saved random.state before each Random call
        /// </summary>
        private void LoadState()
        {
            m_originState = Random.state;
            Random.state = State;
        }

        /// <summary>
        /// Returns a random number between 0.0 [inclusive] and 1.0 [inclusive] (Read Only).
        /// </summary>
        public float value { get { LoadState(); float result = Random.value; SaveState(); return result; } }

        /// <summary>
        /// Returns a random rotation (Read Only).
        /// </summary>
        public Quaternion rotation { get { LoadState(); Quaternion result = Random.rotation; SaveState(); return result; } }

        /// <summary>
        /// Returns a random rotation with uniform distribution (Read Only).
        /// </summary>
        public Quaternion rotationUniform { get { LoadState(); Quaternion result = Random.rotationUniform; SaveState(); return result; } }

        /// <summary>
        /// Returns a random point inside a circle with radius 1 (Read Only).
        /// </summary>
        public Vector2 insideUnitCircle { get { LoadState(); Vector2 result = Random.insideUnitCircle; SaveState(); return result; } }

        /// <summary>
        /// Returns a random point inside a sphere with radius 1 (Read Only).
        /// </summary>
        public Vector3 insideUnitSphere { get { LoadState(); Vector3 result = Random.insideUnitSphere; SaveState(); return result; } }

        /// <summary>
        /// Returns a random point on the surface of a sphere with radius 1 (Read Only).
        /// </summary>
        public Vector3 onUnitSphere { get { LoadState(); Vector3 result = Random.onUnitSphere; SaveState(); return result; } }

        /// <summary>
        /// Generates a random color from HSV and alpha ranges.
        /// </summary>
        /// <returns>A random color with HSV and alpha values in the input ranges.</returns>
        public Color ColorHSV()
        {
            LoadState();
            Color result = Random.ColorHSV();
            SaveState();
            return result;
        }

        /// <summary>
        /// Generates a random color from HSV and alpha ranges.
        /// </summary>
        /// <param name="hueMin">Minimum hue [0..1].</param>
        /// <param name="hueMax">Maximum hue [0..1].</param>
        /// <returns>A random color with HSV and alpha values in the input ranges.</returns>
        public Color ColorHSV(float hueMin, float hueMax)
        {
            LoadState();
            Color result = Random.ColorHSV(hueMin, hueMax);
            SaveState();
            return result;
        }

        /// <summary>
        /// Generates a random color from HSV and alpha ranges.
        /// </summary>
        /// <param name="hueMin">Minimum hue [0..1].</param>
        /// <param name="hueMax">Maximum hue [0..1].</param>
        /// <param name="saturationMin">Minimum saturation [0..1].</param>
        /// <param name="saturationMax">Maximum saturation[0..1].</param>
        /// <returns>A random color with HSV and alpha values in the input ranges.</returns>
        public Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax)
        {
            LoadState();
            Color result = Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax);
            SaveState();
            return result;
        }

        /// <summary>
        /// Generates a random color from HSV and alpha ranges.
        /// </summary>
        /// <param name="hueMin">Minimum hue [0..1].</param>
        /// <param name="hueMax">Maximum hue [0..1].</param>
        /// <param name="saturationMin">Minimum saturation [0..1].</param>
        /// <param name="saturationMax">Maximum saturation[0..1].</param>
        /// <param name="valueMin">Minimum value [0..1].</param>
        /// <param name="valueMax">Maximum value [0..1].</param>
        /// <returns>A random color with HSV and alpha values in the input ranges.</returns>
        public Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax)
        {
            LoadState();
            Color result = Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax);
            SaveState();
            return result;
        }

        /// <summary>
        /// Generates a random color from HSV and alpha ranges.
        /// </summary>
        /// <param name="hueMin">Minimum hue [0..1].</param>
        /// <param name="hueMax">Maximum hue [0..1].</param>
        /// <param name="saturationMin">Minimum saturation [0..1].</param>
        /// <param name="saturationMax">Maximum saturation[0..1].</param>
        /// <param name="valueMin">Minimum value [0..1].</param>
        /// <param name="valueMax">Maximum value [0..1].</param>
        /// <param name="alphaMin">Minimum alpha [0..1].</param>
        /// <param name="alphaMax">Maximum alpha [0..1].</param>
        /// <returns>A random color with HSV and alpha values in the input ranges.</returns>
        public Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, float alphaMin, float alphaMax)
        {
            LoadState();
            Color result = Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, alphaMin, alphaMax);
            SaveState();
            return result;
        }

        /// <summary>
        /// Return a random integer number between min [inclusive] and max [exclusive] (ReadOnly).
        /// </summary>
        /// <param name="min">min int value [inclusive]</param>
        /// <param name="max">max int value [exclusive]</param>
        /// <returns>random integer number</returns>
        public int Range(int min, int max)
        {
            LoadState();
            int result = Random.Range(min, max);
            SaveState();
            return result;
        }

        /// <summary>
        /// Return a random float number between min [inclusive] and max [inclusive] (ReadOnly).
        /// </summary>
        /// <param name="min">min float value [inclusive]</param>
        /// <param name="max">max float value [inclusive]</param>
        /// <returns>random float number</returns>
        public float Range(float min, float max)
        {
            LoadState();
            float result = Random.Range(min, max);
            SaveState();
            return result;
        }

        /// <summary>
        /// Create a new instance with the same random state to be used to preview values
        /// </summary>
        /// <returns></returns>
        public UnityRandomSource Sample => new UnityRandomSource(State);

        /// <summary>
        /// Get the current state to save and restore it later
        /// </summary>
        /// <returns></returns>
        public Random.State Save()
        {
            return State;
        }

        /// <summary>
        /// Restore the random source with the given state
        /// </summary>
        /// <param name="state"></param>
        public void Restore(Random.State state)
        {
            State = state;
        }
    }
}
