using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace RandomToolbox
{
    /// <summary>
    /// The base class is used for PropertyDrawer declaration and shared methods between the 2 derived class
    /// </summary>
    [Serializable]
    public abstract class PartitionBase
    {
        /// <summary>
        /// Base class for PropertyDrawer
        /// </summary>
        [Serializable]
        public abstract class ElementBase
        {
            /// <summary>
            /// Color of the part in inspector
            /// </summary>
            public Color Color = Color.white;

            /// <summary>
            /// Allocated amount
            /// </summary>
            public float Value = 0.0f;
        }

        /// <summary>
        /// overriden in derived class to give this class access to partition elements
        /// </summary>
        protected abstract IReadOnlyList<ElementBase> _elements { get; }

        /// <summary>
        /// True if the sum of all elements value is equal to 1.0
        /// </summary>
        public bool IsCorrect => _elements.Sum(e => e.Value) == 1.0f;

        /// <summary>
        /// Return the index of elements at the given value in the partition range
        /// </summary>
        /// <param name="value">value in the 0-1 range</param>
        /// <returns>Return 0 if given value is lower than 0, last index of elements list if value is more than 1, or -1 if list is empty</returns>
        public int GetIndex(float value)
        {
            if (_elements.Count == 0)   return -1;
            if (value <= 0.0f)          return 0;
            if (value >= 1.0f)          return _elements.Count - 1;

            int index = 0;
            float sum = _elements[index].Value;

            while (value > sum && index < _elements.Count)
            {
                sum += _elements[++index].Value;
            }

            return index;
        }

        /// <summary>
        /// Set the values for all elements in the partitions.
        /// If the number of given value is lower than the current number of elements, the remaining elements value will be set to zero.
        /// If the number of given value is higher than the current number of elements, the remaining given value will be ignored.
        /// If the sum of values is not equal to 1.0, the partition will be corrected using the given correction rule (or the default one)
        /// </summary>
        /// <param name="values">An array of float value that sum up to 1.0</param>
        /// <param name="rule">The correction rule to use if the sum is not 1.0</param>
        public virtual void SetValues(float[] values, CorrectionRule rule = CorrectionRule.Default)
        {
            for (int index = 0; index < _elements.Count; index++)
            {
                if (index < values.Length)
                    _elements[index].Value = values[index];
                else
                    _elements[index].Value = 0;
            }

            if (!IsCorrect) CorrectPartition(rule);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="rule"></param>
        public void SetValue(int index, float value, CorrectionRule rule = CorrectionRule.Default)
        {
            _elements[index].Value = value;

            CorrectPartition(rule, index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="lastEditedValueIndex"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public void CorrectPartition(CorrectionRule rule = CorrectionRule.Default,
            int lastEditedValueIndex = 0,
            float minValue = 0.0f,
            float maxValue = 1.0f)
        {
            // copy values
            float[] values = new float[_elements.Count];
            for (int i = 0; i < _elements.Count; i++) values[i] = _elements[i].Value;

            // correct values with the rule
            CorrectPartition(values, rule, lastEditedValueIndex, minValue, maxValue);

            // re applly corrected values
            for (int i = 0; i < _elements.Count; i++) _elements[i].Value = values[i];
        }



        /// <summary>
        /// 
        /// </summary>
        public enum CorrectionRule
        {
            AdjustAll = 0,          // edit all proportionally (if over, reduce all others) (if under raise all others)
            AdjustLeftAndRight = 1, // edit (increase or decrease) at both left and right of last edited value
            AdjustLeftOnly = 2,     // edit (increase or decrease) at the left of last edited value
            AdjustRightOnly = 3,    // edit (increase or decrease) at the right of last edited value
            None = 5,               // no correction applied

            Default = 0
        }

        /// <summary>
        /// Check a partition, and apply Default correction
        /// </summary>
        /// <param name="values">an array of values</param>
        public static void DefaultPartitionCorrection(float[] values)
        {
            float sum = values.Sum();
            float delta = 1.0f - sum;

            if(sum == 0.0f)
            {
                float v = 1 / values.Length;
                for (int i = 0; i < values.Length; i++)
                    values[i] = v;
            }
            else if (delta != 0.0f)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] += delta * values[i] / sum;
            }
        }

        /// <summary>
        /// Correct a partition if the sum of value is not equal to 1.0
        /// </summary>
        /// <param name="values">values of the partition</param>
        /// <param name="rule">the rule to use when editing values <see cref="CorrectionRule"/></param>
        /// <param name="lastEditedValueIndex">specify the last edited value if you don't want it to be changed</param>
        /// <param name="minValue">specify a mininum to use when editing partition values (can't be more than 1/n)</param>
        /// <param name="maxValue">specify a maximum to use when editing partition values (can't be less than 1/n)</param>
        public static void CorrectPartition(
            float[] values,
            CorrectionRule rule = CorrectionRule.Default,
            int lastEditedValueIndex = 0,
            float minValue = 0.0f,
            float maxValue = 1.0f)
        {
            if (values.Length == 0 || rule == CorrectionRule.None)
                return;

            // minimum value can't be more than 1/n
            minValue = Mathf.Clamp(minValue, 0.0f, 1.0f / values.Length);
            // maximum value can't be less than 1/n
            maxValue = Mathf.Clamp(maxValue, 1.0f / values.Length, 1.0f);

            float sum = values.Sum();
            float delta = 1.0f - sum;
            int left = 0, right = 0, range = 0;

            while (delta != 0.0f)
            {
                range++;
                left = lastEditedValueIndex - range;
                right = lastEditedValueIndex + range;

                switch (rule)
                {
                    case CorrectionRule.AdjustLeftAndRight:
                        if (left < 0) rule = CorrectionRule.AdjustRightOnly;
                        else if (right >= values.Length) rule = CorrectionRule.AdjustLeftOnly;
                        else
                        {
                            float halfDelta = delta / 2;
                            values[left] = Mathf.Clamp(values[left] + halfDelta, minValue, maxValue);
                            values[right] = Mathf.Clamp(values[right] + halfDelta, minValue, maxValue);
                        }
                        break;

                    case CorrectionRule.AdjustLeftOnly:
                        if (left < 0) rule = CorrectionRule.Default;
                        else
                            values[left] = Mathf.Clamp(values[left] + delta, minValue, maxValue);
                        break;

                    case CorrectionRule.AdjustRightOnly:
                        if (right >= values.Length) rule = CorrectionRule.Default;
                        else
                            values[right] = Mathf.Clamp(values[right] + delta, minValue, maxValue);
                        break;

                    case CorrectionRule.Default:
                        DefaultPartitionCorrection(values);
                        break;
                }

                delta = 1.0f - values.Sum();
            }
        }

    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Partition : PartitionBase
    {
        [Serializable] public class Element : ElementBase { }

        protected override IReadOnlyList<ElementBase> _elements => Elements as IReadOnlyList<ElementBase>;

        /// <summary>
        /// List of Elements in the partition
        /// </summary>
        public List<Element> Elements = new List<Element>();

        /// <summary>
        /// Shorthand for Elements[GetIndex(value)]
        /// </summary>
        /// <param name="value">value in the 0-1 range</param>
        /// <returns>null if no element found</returns>
        public Element GetElement(float value)
        {
            int index = GetIndex(value);
            if (index != -1) return Elements[index];
            else return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="rule"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public void AddElement(Element element, CorrectionRule rule = CorrectionRule.Default, float minValue = 0.0f, float maxValue = 1.0f)
        {
            Elements.Add(element);

            CorrectPartition(rule, Elements.Count-1, minValue, maxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rule"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public void AddElement(float value, CorrectionRule rule = CorrectionRule.Default, float minValue = 0.0f, float maxValue = 1.0f)
        {
            Elements.Add(new Element() { Value = value });

            CorrectPartition(rule, Elements.Count - 1, minValue, maxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rule"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public void RemoveElementAt(int index, CorrectionRule rule = CorrectionRule.Default, float minValue = 0.0f, float maxValue = 1.0f)
        {
            if (index >= 0 && index < Elements.Count)
            {
                Elements.RemoveAt(index);

                CorrectPartition(rule, 0, minValue, maxValue);
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Partition<T> : PartitionBase
    {
        [Serializable]
        public class Element : ElementBase
        {
            /// <summary>
            /// Corresponding data for the elements
            /// </summary>
            public T Object;
        }

        protected override IReadOnlyList<ElementBase> _elements => Elements as IReadOnlyList<ElementBase>;

        /// <summary>
        /// List of Elements in the partition
        /// </summary>
        public List<Element> Elements = new List<Element>();

        /// <summary>
        /// Shorthand for Elements[GetIndex(value)]
        /// </summary>
        /// <param name="value">value in the 0-1 range</param>
        /// <returns>null if no element found</returns>
        public Element GetElement(float value)
        {
            int index = GetIndex(value);
            if (index != -1) return Elements[index];
            else return null;
        }

        /// <summary>
        /// Shorthand for Elements[GetIndex(value)].Object
        /// </summary>
        /// <param name="value">value in the 0-1 range</param>
        /// <returns>default if no element found</returns>
        public T GetObject(float value)
        {
            int index = GetIndex(value);
            if (index != -1) return Elements[index].Object;
            else return default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="Object"></param>
        /// <returns></returns>
        public bool GetObject(float value, out T Object)
        {
            int index = GetIndex(value);
            if (index != -1)
            {
                Object = Elements[index].Object;
                return true;
            }
            else
            {
                Object = default;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Contains(T obj)
        {
            foreach (Element e in Elements)
                if (obj.Equals(e.Object))
                    return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="rule"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public void AddElement(Element element, CorrectionRule rule = CorrectionRule.Default, float minValue = 0.0f, float maxValue = 1.0f)
        {
            Elements.Add(element);

            CorrectPartition(rule, Elements.Count - 1, minValue, maxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rule"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public void AddElement(float value, CorrectionRule rule = CorrectionRule.Default, float minValue = 0.0f, float maxValue = 1.0f)
        {
            Elements.Add(new Element() { Value = value });

            CorrectPartition(rule, Elements.Count - 1, minValue, maxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rule"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public void RemoveElementAt(int index, CorrectionRule rule = CorrectionRule.Default, float minValue = 0.0f, float maxValue = 1.0f)
        {
            if (index >= 0 && index < Elements.Count)
            {
                Elements.RemoveAt(index);

                CorrectPartition(rule, 0, minValue, maxValue);
            }
        }

    }
}
