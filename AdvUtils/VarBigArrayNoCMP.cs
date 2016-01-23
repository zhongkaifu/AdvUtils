using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdvUtils
{
    public class VarBigArrayNoCMP<T>
    {
        public const long sizePerBlock = 1024 * 1024 * 64; //(<<26bits)
        public const int moveBit = 26;
        public long size_;
        public List<T[]> arrList;

        long blockSizeInTotal_;
        private object ll = new object();

        public T this[long offset]
        {
            get
            {
                if (offset >= size_)
                {
                    //resize array size, it need to be synced,
                    //for high performance, we use double check to avoid useless resize call and save memory
                    lock (ll)
                    {
                        if (offset >= size_)
                        {
                            Resize(offset + 1);
                        }
                    }
                }

                long nBlock = offset >> moveBit;
                return arrList[(int)nBlock][offset & (sizePerBlock-1)];
            }
            set
            {
                if (offset >= size_)
                {
                    //resize array size, it need to be synced,
                    //for high performance, we use double check to avoid useless resize call and save memory
                    lock (ll)
                    {
                        if (offset >= size_)
                        {
                            Resize(offset + 1);
                        }
                    }
                }

                long nBlock = offset >> moveBit;
                arrList[(int)nBlock][offset & (sizePerBlock-1)] = value;
            }
        }



        private void Resize(long new_size)
        {
            while (blockSizeInTotal_ <= new_size)
            {
                arrList.Add(new T[sizePerBlock]);
                blockSizeInTotal_ += sizePerBlock;
            }

            size_ = new_size;
        }

        //construct variable size big array
        //size is array's default length
        //lowBounding is the lowest bounding of the array
        //when accessing the position which is outer bounding, the big array will be extend automatically.
        public VarBigArrayNoCMP(long size)
        {
            size_ = size;
            arrList = new List<T[]>();

            for (blockSizeInTotal_ = 0; blockSizeInTotal_ < size_; 
                blockSizeInTotal_ += sizePerBlock)
            {
                arrList.Add(new T[sizePerBlock]);
            }
        }
    }
}
