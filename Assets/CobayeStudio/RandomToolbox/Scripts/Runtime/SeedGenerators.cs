using System;
using UnityEngine;


namespace CobayeStudio.RandomToolbox
{
    /// <summary>
    /// Class for seed generation
    /// </summary>
    public static class SeedGenerators
    {
        /// <summary>
        /// Allow to easily select a static seed generator in editor
        /// </summary>
        public enum Generator
        {
            [InspectorName("Current Date Time")] CurrentDateTimeBasedSeed = 0,
            [InspectorName("System Start Time")] SystemStartTimeSeed = 1,
            //[InspectorName("MyCustomSeedGenerationMethod")] MyCustomSeedGenerationMethod = 2,
        }

        /// <summary>
        /// Generate seed from Generator enum value
        /// </summary>
        /// <param name="value">SeedGenerators.Generator enum value</param>
        /// <returns>new generated seed</returns>
        public static int GetSeed(this Generator value)
        {
            switch (value)
            {
                default: return default;
                case Generator.CurrentDateTimeBasedSeed: return CurrentDateTimeBasedSeed();
                case Generator.SystemStartTimeSeed: return SystemStartTimeSeed();
                // case Generator.MyCustomSeedGenerationMethod: return MyCustomSeedGenerationMethod();
            }
        }

        /// <summary>
        /// Get a new seed based on current date and time
        /// </summary>
        /// <returns>Return the result of System.DateTime.Now.Ticks</returns>
        public static int CurrentDateTimeBasedSeed() => (int)DateTime.Now.Ticks;

        /// <summary>
        /// Get a new seed based on time elapsed since the machine we are running on started
        /// </summary>
        /// <returns>Return the result of System.Environment.TickCount</returns>
        public static int SystemStartTimeSeed() => Environment.TickCount;

        /// <summary>
        /// Add your own seed generation method here
        /// </summary>
        /// <returns>returned seed</returns>
        //public static int MyCustomSeedGenerationMethod() => ???;
    }
}
