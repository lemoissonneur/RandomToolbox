using UnityEngine;
using System;


namespace RandomToolbox.Samples
{
    public class ProbabilitySample : MonoBehaviour
    {
        [TextArea]
        public string partitions =
            "A partition is definition of how to slice something.\n" +
            "You can drag the cuts with your mouse to adjust each slice and change their color.";
        public Partition myPartition;

        [Space]
        [Space]
        [Space]
        [Space]
        [Space]

        [TextArea]
        public string valuesPartitions =
            "This is useful if you want to manage the probability of occurence of some values" +
            "when you use random number generators. This allow for more control in your game design.";
        public UnityRandomSourceSO myRandom;
        public Partition<int> myFakeDice;

        public int RollTheDice()
        {
            int value = myFakeDice.GetObject(myRandom.Instance.value);
            return value;
        }

        [Space]
        [Space]
        [Space]
        [Space]
        [Space]

        [TextArea]
        public string genericPartitions =
            "You can use partitions with any types. As long as it have a Propertydrawer," +
            "it will be displayed in the inspector.";
        public Partition<Vector3> myVectors;
        public Partition<UnityEngine.Object> myObjects;
        public Partition<MyCustomStruct> myCustoms;

        [Serializable]
        public struct MyCustomStruct
        {
            public int someInt;
            public string someString;
        }
    }
}
