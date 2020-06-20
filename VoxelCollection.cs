using System;
using System.Collections.Generic;

namespace VoxelTest
{
    public class VoxelCollection<T>
    {
        private class VoxelStore<U>
        {
            public bool stored;
            public U value;
        }

        private Dictionary<long, VoxelStore<T>> dataStore = new Dictionary<long, VoxelStore<T>>();
        private Action<Vector3, T> destroyCallback = null;

        public void Set(Vector3 position, T value, int precision)
        {
            long storeAddr = GetAddress(position, precision);
            Destroy(storeAddr);
            Create(storeAddr);
            dataStore[storeAddr].stored = true;
            dataStore[storeAddr].value = value;
        }

        public T Get(Vector3 position, int precision)
        {
            return Get(GetAddress(position, precision), precision);
        }

        public T Get(long address, int precision)
        {
            if (precision == 0)
            {
                if (dataStore.ContainsKey(1) && dataStore[1].stored)
                {
                    return dataStore[1].value;
                }
                else
                {
                    return default(T);
                }
            }
            if (dataStore.ContainsKey(address) && dataStore[address].stored)
            {
                return dataStore[address].value;
            }
            return Get(GetParent(address), precision - 1);
        }

        public static long GetAddress(Vector3 position, int precision)
        {
            long retVal = 1;
            for (int i = 0; i < precision; i++)
            {
                retVal = retVal * 10;
                if (position.x > 0.5d)
                {
                    retVal = retVal + 1;
                    position.x = position.x - 0.5d;
                }
                if (position.y > 0.5d)
                {
                    retVal = retVal + 2;
                    position.y = position.y - 0.5d;
                }
                if (position.z > 0.5d)
                {
                    retVal = retVal + 4;
                    position.z = position.z - 0.5d;
                }
                position.x = position.x * 2d;
                position.y = position.y * 2d;
                position.z = position.z * 2d;
            }
            return retVal;
        }

        public static Vector3 GetVector(long position)
        {
            Vector3 retVal = new Vector3(0d, 0d, 0d);
            while (position > 1)
            {
                long lastDigit = position - ((position / 10) * 10);
                retVal.x = retVal.x / 2d;
                retVal.y = retVal.y / 2d;
                retVal.z = retVal.z / 2d;
                //X offset
                if ((lastDigit & 1) == 1)
                {
                    retVal.x = retVal.x + 0.25d;
                }
                else
                {
                    retVal.x = retVal.x - 0.25d;
                }
                //Y offset
                if ((lastDigit & 2) == 2)
                {
                    retVal.y = retVal.y + 0.25d;
                }
                else
                {
                    retVal.y = retVal.y - 0.25d;
                }
                //Z offset
                if ((lastDigit & 4) == 4)
                {
                    retVal.z = retVal.z + 0.25d;
                }
                else
                {
                    retVal.z = retVal.z - 0.25d;
                }
                position = position / 10;
            }
            retVal.x = retVal.x + 0.5d;
            retVal.y = retVal.y + 0.5d;
            retVal.z = retVal.z + 0.5d;
            return retVal;
        }

        public static long GetParent(long address)
        {
            if (address <= 1)
            {
                throw new Exception("Can't get the parent of the root node");
            }
            return address / 10;
        }

        public static IEnumerable<long> GetChildren(long address)
        {
            long childbase = address * 10;
            yield return childbase;
            yield return childbase + 1;
            yield return childbase + 2;
            yield return childbase + 3;
            yield return childbase + 4;
            yield return childbase + 5;
            yield return childbase + 6;
            yield return childbase + 7;
        }

        public void Create(long address)
        {
            if (address != 1)
            {
                long parent = GetParent(address);
                if (!dataStore.ContainsKey(parent))
                {
                    Create(parent);
                }
            }
            dataStore[address] = new VoxelStore<T>();
        }

        public void Destroy(long address)
        {
            DestroyChildren(address);
            if (dataStore.ContainsKey(address))
            {
                if (dataStore[address].stored)
                {
                    T destroyVal = dataStore[address].value;
                    if (destroyCallback != null)
                    {
                        destroyCallback(GetVector(address), destroyVal);
                    }
                }
                dataStore.Remove(address);
            }
        }

        public void DestroyChildren(long address)
        {
            foreach (long child in GetChildren(address))
            {
                if (dataStore.ContainsKey(child))
                {
                    Destroy(child);
                }
            }
        }

        public void SetDestroyCallback(Action<Vector3, T> destroyCallback)
        {
            this.destroyCallback = destroyCallback;
        }
    }
}
