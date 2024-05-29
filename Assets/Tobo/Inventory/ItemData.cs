using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using Tobo.Net;
using UnityEngine;

namespace Tobo.Inventory
{
    public class ItemData : IBufferStruct
    {
        private ItemStack stack;
        private Dictionary<string, NumericData> numericData;
        private Dictionary<string, string> stringData;

        public ItemData(ItemStack stack)
        {
            numericData = new Dictionary<string, NumericData>();
            stringData = new Dictionary<string, string>();
            this.stack = stack;
        }

        public bool TryGetString(string key, out string value) => stringData.TryGetValue(key, out value);
        public string GetString(string key) => stringData[key];
        public void SetString(string key, string value) => stringData[key] = value;


        public bool TryGet(string key, out NumericData value) => numericData.TryGetValue(key, out value);

        public NumericData Get(string key)
        {
            if (!numericData.ContainsKey(key))
            {
                Debug.LogWarning($"ItemStack ({stack.Item.name}) does not have ItemData for key {key}");
                return 0;
            }
            return numericData[key];
        }

        public void Set(string key, float value)
        {
            numericData[key] = new NumericData(value);
        }
        public void Set(string key, int value)
        {
            numericData[key] = new NumericData(value);
        }
        public void Set(string key, bool value)
        {
            numericData[key] = new NumericData(value);
        }

        /// <summary>
        /// Flips the boolean value of <paramref name="key"/>
        /// </summary>
        /// <param name="key"></param>
        public void Flip(string key)
        {
            numericData[key] = new NumericData(!numericData[key].Bool);
        }

        public void Add(string key, int amount)
        {
            numericData[key] = new NumericData(numericData[key].Int + amount);
        }
        public void Add(string key, float amount)
        {
            numericData[key] = new NumericData(numericData[key].Float + amount);
        }

        public void Subtract(string key, int amount)
        {
            Add(key, -amount);
        }
        public void Subtract(string key, float amount)
        {
            Add(key, -amount);
        }

        public void Clear()
        {
            numericData.Clear();
            stringData.Clear();
        }


        public bool EqualTo(ItemData other)
        {
            if (other == null) return false;

            if (other == this) return true;

            bool sameKeys = numericData.Keys.Count == other.numericData.Keys.Count &&
                stringData.Keys.Count == other.stringData.Keys.Count;

            if (!sameKeys) return false;

            bool numericValuesEqual = numericData.Keys.All(k =>
                other.numericData.ContainsKey(k) && numericData[k].Int == other.numericData[k].Int);

            if (!numericValuesEqual) return false;

            bool stringValuesEqual = stringData.Keys.All(k =>
                other.stringData.ContainsKey(k) && object.Equals(stringData[k], other.stringData[k]));

            return stringValuesEqual;
        }


        // Kind of like a ByteBuffer but different
        // Almost done, it was so complicated tho
        /*
        private readonly byte[] bytes;

        // Assuming there are no gaps

        // mmmm how do I handle changing strings?
        public List<Data> CurrentData = new List<Data>();

        public ItemData(int byteCapacity)
        {
            bytes = new byte[byteCapacity];
        }

        #region Get & Set
        public unsafe ItemData Set<T>(int start, T value) where T : unmanaged
        {
            int size = sizeof(T);

            Span<byte> span = new Span<byte>(bytes, start, size);
            MemoryMarshal.Write(span, ref value);

            return this;
        }
        public unsafe T Get<T>(int start) where T : unmanaged
        {
            ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(this.bytes, start, sizeof(T));
            T value = MemoryMarshal.Read<T>(bytes);

            return value;
        }

        public unsafe ItemData SetString(int start, string value)
        {
            Span<byte> sizeDestinationSpan = new Span<byte>(bytes, start, sizeof(ushort));
            ushort strSize = (ushort)value.Length;
            MemoryMarshal.Write(sizeDestinationSpan, ref strSize);

            Span<byte> stringDestinationSpan = new Span<byte>(bytes, start + sizeof(ushort), value.Length * sizeof(char));

            ReadOnlySpan<byte> arrayBytes = MemoryMarshal.AsBytes(value.AsSpan());

            for (int i = 0; i < stringDestinationSpan.Length; i++)
            {
                stringDestinationSpan[i] = arrayBytes[i];
            }

            return this;
        }
        public unsafe string GetString(int start)
        {
            ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(this.bytes, start, sizeof(ushort));
            ushort length = MemoryMarshal.Read<ushort>(bytes);

            return string.Create(length, 0, (Span<char> chars, int _) =>
            {
                ReadOnlySpan<byte> stringChars = new ReadOnlySpan<byte>(this.bytes, start + sizeof(ushort), length * sizeof(char));

                for (int i = 0; i < chars.Length; i++)
                {
                    chars[i] = MemoryMarshal.Read<char>(stringChars.Slice(i * sizeof(char), sizeof(char)));
                }
            });
        }
        #endregion

        public class Data
        {
            private readonly ItemData data;

            public readonly Type Type;
            public readonly int Pointer;
            public readonly int Size;

            // I don't like all the yucky code repetition but for readonly stuff
            //  I can't move it to a method soooo yeah
            public Data(ItemData data, int value)
            {
                this.data = data;
                Type = Type.Int;
                Size = sizeof(int);
            }

            public Data(ItemData data, float value)
            {
                this.data = data;
                Type = Type.Float;
                Size = sizeof(float);
            }

            public Data(ItemData data, string value, int characterLimit = 31)
            {
                if (value == null)
                    value = string.Empty;

                if (value.Length > characterLimit)
                    value = value.Substring(0, characterLimit);

                this.data = data;
                Type = Type.String;
                Size = sizeof(char) * characterLimit + sizeof(ushort);
            }

            public Data(ItemData data, bool value)
            {
                this.data = data;
                Type = Type.Bool;
                Size = sizeof(bool);
            }



            //public int GetInt(ItemData from)
            //{
            //
            //}
        }

        public enum Type
        {
            Int,
            Float,
            String,
            Bool
        }
        */

        [StructLayout(LayoutKind.Explicit)]
        public struct NumericData
        {
            [FieldOffset(0)]
            public float Float;
            [FieldOffset(0)]
            public int Int;
            [FieldOffset(0)]
            public bool Bool;

            public NumericData(float value)
            {
                Int = 0;
                Bool = false;
                Float = value; // As long as this is set last
            }

            public NumericData(int value)
            {
                Float = 0;
                Bool = false;
                Int = value;
            }

            public NumericData(bool value)
            {
                Int = 0;
                Float = 0;
                Bool = value;
            }

            public static implicit operator NumericData(float value) => new NumericData(value);
            public static implicit operator NumericData(int value) => new NumericData(value);
            public static implicit operator NumericData(bool value) => new NumericData(value);

            public static implicit operator float(NumericData data) => data.Float;
            public static implicit operator int(NumericData data) => data.Int;
            public static implicit operator bool(NumericData data) => data.Bool;

            /*
            public override bool Equals(object obj)
            {
                if (obj is NumericData data)
                    return Int == data.Int; // Perfect
                return false;
            }

            public override int GetHashCode()
            {
                return Int; // Also perfect
            }
            */
        }

        public void Serialize(ByteBuffer buf)
        {
            buf.Add(numericData.Count);
            buf.Add(stringData.Count);

            foreach (KeyValuePair<string, NumericData> pair in numericData)
            {
                buf.AddString(pair.Key);
                buf.Add(pair.Value.Int);
            }

            foreach (KeyValuePair<string, string> pair in stringData)
            {
                buf.AddString(pair.Key);
                buf.AddString(pair.Value);
            }
        }

        public void Deserialize(ByteBuffer buf)
        {
            int numNumbers = buf.Read<int>();
            int numStrings = buf.Read<int>();

            numericData = new Dictionary<string, NumericData>(numNumbers);
            stringData = new Dictionary<string, string>(numStrings);

            for (int i = 0; i < numNumbers; i++)
            {
                numericData.Add(buf.Read(), new NumericData(buf.Read<int>()));
            }

            for (int i = 0; i < numStrings; i++)
            {
                stringData.Add(buf.Read(), buf.Read());
            }
        }
    }
}
