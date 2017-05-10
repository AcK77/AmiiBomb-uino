//https://github.com/Falco20019/libamiibo

using System;

namespace AmiiBomb
{
    static class ArraySegment
    {
        public static void Copy<T>(byte[] source, ArraySegment<T> destination, int length)
        {
            Array.Copy(source, 0, destination.Array, destination.Offset, length);
        }
        public static void Copy<T>(byte[] source, int sourceIndex, ArraySegment<T> destination, int destinationIndex, int length)
        {
            Array.Copy(source, sourceIndex, destination.Array, destination.Offset + destinationIndex, length);
        }
        public static void Copy<T>(ArraySegment<T> source, ArraySegment<T> destination, int length)
        {
            Array.Copy(source.Array, source.Offset, destination.Array, destination.Offset, length);
        }
        public static void Copy<T>(ArraySegment<T> source, int sourceIndex, ArraySegment<T> destination, int destinationIndex, int length)
        {
            Array.Copy(source.Array, source.Offset + sourceIndex, destination.Array, destination.Offset + destinationIndex, length);
        }

        public static void CopyFrom<T>(this ArraySegment<T> destination, byte[] source)
        {
            Copy(source, destination, source.Length);
        }
        public static void CopyFrom<T>(this ArraySegment<T> destination, byte[] source, int length)
        {
            Copy(source, destination, length);
        }
        public static void CopyFrom<T>(this ArraySegment<T> destination, byte[] source, int sourceIndex, int destinationIndex, int length)
        {
            Copy(source, sourceIndex, destination, destinationIndex, length);
        }
        public static void CopyFrom<T>(this ArraySegment<T> destination, ArraySegment<T> source)
        {
            Copy(source, destination, source.Count);
        }
        public static void CopyFrom<T>(this ArraySegment<T> destination, ArraySegment<T> source, int length)
        {
            Copy(source, destination, length);
        }
        public static void CopyFrom<T>(this ArraySegment<T> destination, ArraySegment<T> source, int sourceIndex, int destinationIndex, int length)
        {
            Copy(source, sourceIndex, destination, destinationIndex, length);
        }
    }
}
