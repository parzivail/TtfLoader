using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TtfLoader
{
    internal static class BinaryReaderExt
    {
        public static T Read<T>(this BinaryReader r) where T : IBinarySerializable
        {
            var t = Activator.CreateInstance<T>();
            t.Read(r);
            return t;
        }

        public static T[] ReadMany<T>(this BinaryReader r, int count) where T : IBinarySerializable
        {
            var t = new T[count];

            for (var i = 0; i < count; i++) t[i] = r.Read<T>();

            return t;
        }
    }
}
