using System;
using System.Collections.Generic;

namespace VoxelTest
{
    public class VoxelTestMain
    {
        public static void Main()
        {
            VoxelCollection<int> voxels = new VoxelCollection<int>();
            voxels.Set(new Vector3(0d, 0d, 0d), 0, 0);
            voxels.Set(new Vector3(0.25d, 0.25d, 0.25d), 1, 1);
            voxels.Set(new Vector3(0.25d, 0.25d, 0.25d), 2, 2);
            voxels.Set(new Vector3(0.25d, 0.25d, 0.25d), 3, 3);
            voxels.Set(new Vector3(0.25d, 0.25d, 0.25d), 4, 4);
            voxels.Set(new Vector3(0.25d, 0.25d, 0.25d), 5, 5);
            for (double d = 0d; d < 1d; d += 0.01d)
            {
                Vector3 pos = new Vector3(d, 0.25d, 0.25d);
                int data = voxels.Get(pos, 10);
                Console.WriteLine($"Pos: {pos}, value: {data}");
            }
            /*
            for (int i = 0; i < 15; i++)
            {
                Vector3 target = new Vector3(1, 1, 1);
                long addr = VoxelCollection<int>.GetAddress(target, i);
                Vector3 pos = VoxelCollection<int>.GetVector(addr);
                Vector3 delta = pos - target;
                Console.WriteLine($"Precision {i}, Addr: {addr}, Delta: {delta}");
            }
            */
        }
    }
}