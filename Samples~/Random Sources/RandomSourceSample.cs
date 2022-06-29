using UnityEngine;


namespace RandomToolbox.Samples
{
    public class RandomSourceSample : MonoBehaviour
    {
        [TextArea]
        public string RandomSources =
            "Use the 'UnityRandomSource' class to get a RandomNumberGenerator " +
            "similar to unity static 'Random' class. But this class wont be impacted by " +
            "other components calling unity's Random class.";
        public UnityRandomSource myRandom;

        public void Awake()
        {
            // Dont' forget to start your random source once. either with the serialized seed in the inspector ...
            myRandom.Start();

            // ... or by giving it a new seed ...
            //myRandom.Start(123456789);

            // ... or by generating a new seed with the selected Seed Generator in the inspector.
            //myRandom.Start(true);

            myOtherRandom.Start();


            // You don't need to start your instance in an SO if you checked the field 'Initialize On Awake'
            if (myRandomSO.InitializeOnAwake == false)
                myRandomSO.Instance.Start();
        }

        [Space]
        [Space]
        [Space]
        [Space]
        [Space]

        [TextArea]
        public string MultipleSources =
            "You can have multiples instance of this class if you need multiples " +
            "RandomNumberGenerator.";
        public UnityRandomSource myOtherRandom;

        private void Update()
        {
            Quaternion a = myRandom.rotation;
            Quaternion b = myOtherRandom.rotation;

            Debug.Log("myRandom generated : " + a + "          myOtherRandom generated : " + b);
        }

        [Space]
        [Space]
        [Space]
        [Space]
        [Space]

        [TextArea]
        public string ScriptableObjects =
            "If you need to use your RandomNumberGenerator in multiples scenes, " +
            "you should probably use the ScriptableObject based class 'UnityRandomSourceSO'.";
        public UnityRandomSourceSO myRandomSO;

        private void CallingAnSO()
        {
            Quaternion q = myRandomSO.Instance.rotation;
        }

        [Space]
        [Space]
        [Space]
        [Space]
        [Space]

        [TextArea]
        public string SystemRandom =
            "A version based on System.Random instead of UnityEngine.Random also exist. " +
            "It use the same principle, but dont have all the unity specific methods like color generation.";
        public SystemRandomSource systemSource;
        public SystemRandomSourceSO systemSourceSO;

        [Space]
        [Space]
        [Space]
        [Space]
        [Space]

        [TextArea]
        public string SaveRandom =
            "Both version provide Save() and Restore() methods to use with your game save system.";
        public Random.State myRandomSavedState;
        public SystemRandomSource.State systemSourceSavedState;

        public void OnSaveCommand()
        {
            // save those value somewhere
            myRandomSavedState = myRandom.Save();
            systemSourceSavedState = systemSource.Save();
        }

        public void OnRestoreCommand()
        {
            myRandom.Restore(myRandomSavedState);
            systemSource.Restore(systemSourceSavedState);
        }

        [Space]
        [Space]
        [Space]
        [Space]
        [Space]

        // If you need to access the random source instance the same way you used to access
        // the unityEngine.random, you can create a static instance in a monobehaviour
        // but you wont be able to edit the seed value in the inspector
        public static readonly UnityRandomSource StaticRandomSource = new UnityRandomSource();

        public void SomeMethod()
        {
            // you can then acces this instance anywhere using :
            Quaternion q = RandomSourceSample.StaticRandomSource.rotation;
        }
    }
}
