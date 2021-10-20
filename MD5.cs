using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Linq;
namespace RSA
{
    class MD5
    {
        const uint dwA = 0x67452301;
        const uint dwB = 0xefcdab89;
        const uint dwC = 0x98badcfe;
        const uint dwD = 0x10325476;
        public static uint F(uint X, uint Y, uint Z)
        {
            return (((X) & (Y)) | ((~X) & (Z)));
        }

        public static uint ROTATE(uint x, int n)
        {
            return (((x) << (n)) | ((x) >> (32 - (n))));
        }

        public static void fun1(ref uint a, uint b, uint c, uint d, uint k, int s, uint t)
        {
            a = ROTATE(a + F(b, c, d) + k + t, s) + b;
        }

        public static uint G(uint X, uint Y, uint Z)
        {
            return ((X) & (Z) | (Y) & (~Z));
        }

        public static void fun2(ref uint a, uint b, uint c, uint d, uint k, int s, uint t)
        {
            a = b + ROTATE((a + G(b, c, d) + k + t), s);
        }

        public static uint H(uint X, uint Y, uint Z)
        {
            return ((X) ^ (Y) ^ (Z));
        }

        public static void fun3(ref uint a, uint b, uint c, uint d, uint k, int s, uint t)
        {
            a = b + ROTATE((a + H(b, c, d) + k + t), s);
        }

        public static uint I(uint X, uint Y, uint Z)
        {
            return ((Y) ^ ((X) | (~Z)));
        }

        public static void fun4(ref uint a, uint b, uint c, uint d, uint k, int s, uint t)
        {
            a = b + ROTATE((a + I(b, c, d) + k + t), s);
        }

        public static void SetBytes<T>(ref T[] src, int srcOffset, int count, T value)
        {
            var tmp = Enumerable.Repeat(value, count);
            Array.Copy(src, srcOffset, tmp.ToArray(), 0, count);
        }
        public static uint[] CodeMD5(string buffer)
        {
            uint[] digest = new uint[4];
            int size = buffer.Length;
            if (buffer.Length == 0) return digest;
            int exp = ((size + 8) / 64) * 64 + 56;
            byte[] data = new byte[exp + 8];
            System.Buffer.BlockCopy(buffer.ToCharArray(), 0, data, 0, size);
            data[size] = 0x80;
            SetBytes<byte>(ref data, size + 1, exp - size - 1, 0);
            byte[] mass = BitConverter.GetBytes(size * sizeof(char) * 8);
            System.Buffer.BlockCopy(mass, 0, data, exp, 4);
            data[exp + 7] = 0;

            uint[] M = Array.ConvertAll(data, Convert.ToUInt32);
            uint[] X = new uint[16];
            int blockM = (exp + 8) / 64;

            digest[0] = dwA;
            digest[1] = dwB;
            digest[2] = dwC;
            digest[3] = dwD;

            byte[] tmp = data;
            int tempRef = 0;
            for (int n = 0; n < blockM; n++)
            {
                System.Buffer.BlockCopy(tmp, tempRef, X, 0, 64);
                tempRef = +64;

                uint A = digest[0];
                uint B = digest[1];
                uint C = digest[2];
                uint D = digest[3];

                /* Round 1. */

                fun1(ref A, B, C, D, X[0], 7, 0xd76aa478);
                fun1(ref D, A, B, C, X[1], 12, 0xe8c7b756);
                fun1(ref C, D, A, B, X[2], 17, 0x242070db);
                fun1(ref B, C, D, A, X[3], 22, 0xc1bdceee);
                fun1(ref A, B, C, D, X[4], 7, 0xf57c0faf);
                fun1(ref D, A, B, C, X[5], 12, 0x4787c62a);
                fun1(ref C, D, A, B, X[6], 17, 0xa8304613);
                fun1(ref B, C, D, A, X[7], 22, 0xfd469501);
                fun1(ref A, B, C, D, X[8], 7, 0x698098d8);
                fun1(ref D, A, B, C, X[9], 12, 0x8b44f7af);
                fun1(ref C, D, A, B, X[10], 17, 0xffff5bb1);
                fun1(ref B, C, D, A, X[11], 22, 0x895cd7be);
                fun1(ref A, B, C, D, X[12], 7, 0x6b901122);
                fun1(ref D, A, B, C, X[13], 12, 0xfd987193);
                fun1(ref C, D, A, B, X[14], 17, 0xa679438e);
                fun1(ref B, C, D, A, X[15], 22, 0x49b40821);

                /* Round 2. */

                fun2(ref A, B, C, D, X[1], 5, 0xf61e2562);
                fun2(ref D, A, B, C, X[6], 9, 0xc040b340);
                fun2(ref C, D, A, B, X[11], 14, 0x265e5a51);
                fun2(ref B, C, D, A, X[0], 20, 0xe9b6c7aa);
                fun2(ref A, B, C, D, X[5], 5, 0xd62f105d);
                fun2(ref D, A, B, C, X[10], 9, 0x2441453);
                fun2(ref C, D, A, B, X[15], 14, 0xd8a1e681);
                fun2(ref B, C, D, A, X[4], 20, 0xe7d3fbc8);
                fun2(ref A, B, C, D, X[9], 5, 0x21e1cde6);
                fun2(ref D, A, B, C, X[14], 9, 0xc33707d6);
                fun2(ref C, D, A, B, X[3], 14, 0xf4d50d87);
                fun2(ref B, C, D, A, X[8], 20, 0x455a14ed);
                fun2(ref A, B, C, D, X[13], 5, 0xa9e3e905);
                fun2(ref D, A, B, C, X[2], 9, 0xfcefa3f8);
                fun2(ref C, D, A, B, X[7], 14, 0x676f02d9);
                fun2(ref B, C, D, A, X[12], 20, 0x8d2a4c8a);

                /* Round 3. */

                fun3(ref A, B, C, D, X[5], 4, 0xfffa3942);
                fun3(ref D, A, B, C, X[8], 11, 0x8771f681);
                fun3(ref C, D, A, B, X[11], 16, 0x6d9d6122);
                fun3(ref B, C, D, A, X[14], 23, 0xfde5380c);
                fun3(ref A, B, C, D, X[1], 4, 0xa4beea44);
                fun3(ref D, A, B, C, X[4], 11, 0x4bdecfa9);
                fun3(ref C, D, A, B, X[7], 16, 0xf6bb4b60);
                fun3(ref B, C, D, A, X[10], 23, 0xbebfbc70);
                fun3(ref A, B, C, D, X[13], 4, 0x289b7ec6);
                fun3(ref D, A, B, C, X[0], 11, 0xeaa127fa);
                fun3(ref C, D, A, B, X[3], 16, 0xd4ef3085);
                fun3(ref B, C, D, A, X[6], 23, 0x4881d05);
                fun3(ref A, B, C, D, X[9], 4, 0xd9d4d039);
                fun3(ref D, A, B, C, X[12], 11, 0xe6db99e5);
                fun3(ref C, D, A, B, X[15], 16, 0x1fa27cf8);
                fun3(ref B, C, D, A, X[2], 23, 0xc4ac5665);

                /* Round 4. */

                fun4(ref A, B, C, D, X[0], 6, 0xf4292244);
                fun4(ref D, A, B, C, X[7], 10, 0x432aff97);
                fun4(ref C, D, A, B, X[14], 15, 0xab9423a7);
                fun4(ref B, C, D, A, X[5], 21, 0xfc93a039);
                fun4(ref A, B, C, D, X[12], 6, 0x655b59c3);
                fun4(ref D, A, B, C, X[3], 10, 0x8f0ccc92);
                fun4(ref C, D, A, B, X[10], 15, 0xffeff47d);
                fun4(ref B, C, D, A, X[1], 21, 0x85845dd1);
                fun4(ref A, B, C, D, X[8], 6, 0x6fa87e4f);
                fun4(ref D, A, B, C, X[15], 10, 0xfe2ce6e0);
                fun4(ref C, D, A, B, X[6], 15, 0xa3014314);
                fun4(ref B, C, D, A, X[13], 21, 0x4e0811a1);
                fun4(ref A, B, C, D, X[4], 6, 0xf7537e82);
                fun4(ref D, A, B, C, X[11], 10, 0xbd3af235);
                fun4(ref C, D, A, B, X[2], 15, 0x2ad7d2bb);
                fun4(ref B, C, D, A, X[9], 21, 0xeb86d391);

                digest[0] += A;
                digest[1] += B;
                digest[2] += C;
                digest[3] += D;
            }
            return digest;
        }
        static void Main()
        {
            Console.WriteLine("Введите текст, который нужно зашифровать: ");
            string text = Console.ReadLine();
            uint[] result = CodeMD5(text);
            Console.WriteLine("Зашифрованное сообщение: {0}{1}{2}{3}", result[0], result[1],result[2],result[3]);
            Console.ReadLine();
        }
    }
}