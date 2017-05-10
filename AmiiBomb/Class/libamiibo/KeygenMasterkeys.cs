/*
 * Copyright (C) 2015 Marcos Vives Del Sol
 * Copyright (C) 2016 Benjamin Krämer
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AmiiBomb
{
    public class KeygenMasterkeys
    {
        public const int NFC3D_KEYGEN_SEED_SIZE = 64;

        public byte[] hmacKey;      // 16 bytes
        public string typeString;   // 14 bytes
        public byte rfu;
        public byte magicBytesSize;
        public byte[] magicBytes;   // 16 bytes
        public byte[] xorPad;       // 32 bytes

        internal static KeygenMasterkeys Unserialize(BinaryReader reader)
        {
            return new KeygenMasterkeys
            {
                hmacKey = reader.ReadBytes(16),
                typeString = new string(reader.ReadChars(14)),
                rfu = reader.ReadByte(),
                magicBytesSize = reader.ReadByte(),
                magicBytes = reader.ReadBytes(16),
                xorPad = reader.ReadBytes(32)
            };
        }

        internal void Serialize(BinaryWriter writer)
        {
            char[] typeStringChars = new char[14];
            typeString.CopyTo(0, typeStringChars, 0, typeString.Length);

            writer.Write(hmacKey);
            writer.Write(typeStringChars);
            writer.Write(rfu);
            writer.Write(magicBytesSize);
            writer.Write(magicBytes);
            writer.Write(xorPad);
        }

        public KeygenDerivedkeys GenerateKey(byte[] baseSeed)
        {
            byte[] preparedSeed = new byte[DrbgCtx.NFC3D_DRBG_MAX_SEED_SIZE];
            var preparedSeedSize = PrepareSeed(baseSeed, preparedSeed);
            return DrbgCtx.GenerateBytes(this.hmacKey, preparedSeed, preparedSeedSize);
        }

        private int PrepareSeed(byte[] baseSeed, byte[] output)
        {
            Debug.Assert(baseSeed != null);
            Debug.Assert(output != null);

            int start = 0;

            // 1: Copy whole type string
            Encoding.ASCII.GetBytes(this.typeString, 0, this.typeString.Length, output, 0);
            start += this.typeString.Length;

            // 2: Append (16 - magicBytesSize) from the input seed
            int leadingSeedBytes = 16 - this.magicBytesSize;
            Array.Copy(baseSeed, 0, output, start, leadingSeedBytes);
            start += leadingSeedBytes;

            // 3: Append all bytes from magicBytes
            Array.Copy(this.magicBytes, 0, output, start, this.magicBytesSize);
            start += this.magicBytesSize;

            // 4: Append bytes 0x10-0x1F from input seed
            Array.Copy(baseSeed, 0x10, output, start, 16);
            start += 16;

            // 5: Xor last bytes 0x20-0x3F of input seed with AES XOR pad and append them
            for (int i = 0; i < 32; i++)
            {
                output[start + i] = (byte) (baseSeed[i + 32] ^ this.xorPad[i]);
            }
            start += 32;

            return start;
        }
    }
}