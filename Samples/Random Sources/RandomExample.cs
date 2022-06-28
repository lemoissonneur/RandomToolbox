using System.Collections.Generic;
using UnityEngine;


namespace RandomToolbox.Example
{
    public class RandomExample : MonoBehaviour
    {
        [Header("Random Sources Example : ")]
        // If you only need your randomsource to be used in a Monobehaviour
        // or in a single scene you can have it as a field
        public SystemRandomSource systemRandom = new SystemRandomSource();
        public UnityRandomSource unityRandom;// = new UnityRandomSource(10);

        public void LocalInstance()
        {
            // initialize your random source instance
            unityRandom.Start(true);

            // get your values
            Quaternion q = unityRandom.rotation;
        }

        // If you need your random source to be shared between multiples
        // monobehaviour in different scenes, you should propably use a scriptableObject
        // to keep your instance in between scene
        public SystemRandomSourceSO systemSO;
        public UnityRandomSourceSO unitySO;
        public RandomSourceBaseSO basicSO;

        public void ReferencedInstance()
        {
            // initialize your random source instance
            unitySO.Instance.Start(true);

            // get your values
            Quaternion q = unitySO.Instance.rotation;
        }

        // If you need to access the random source instance the same way you used to access
        // the unityEngine.random, you can create a static instance in a monobehaviour
        // but you wont be able to edit the seed value in the inspector
        public static readonly UnityRandomSource StaticRandomSource = new UnityRandomSource();

        public void SomeMethod()
        {
            // you can then acces this instance anywhere using :
            Quaternion q = RandomExample.StaticRandomSource.rotation;
        }
    }
}
