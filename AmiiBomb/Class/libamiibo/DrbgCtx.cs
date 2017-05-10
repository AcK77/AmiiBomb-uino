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
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace AmiiBomb
{
    public class DrbgCtx
    {
        public const int NFC3D_DRBG_MAX_SEED_SIZE = 480; /* Hardcoded max size in 3DS NFC module */
        public const int NFC3D_DRBG_OUTPUT_SIZE = 32; /* Every iteration generates 32 bytes */

        private readonly HMac hmacCtx;
        private bool used;
        private ushort iteration;
        private readonly byte[] buffer;

        private DrbgCtx(byte[] hmacKey, byte[] seed, int seedSize)
        {
            Debug.Assert(hmacKey != null);
            Debug.Assert(seed != null);
            Debug.Assert(seedSize <= NFC3D_DRBG_MAX_SEED_SIZE);

            // Initialize primitives
            this.used = false;
            this.iteration = 0;
            this.buffer = new byte[sizeof (ushort) + seedSize];

            // The 16-bit counter is prepended to the seed when hashing, so we'll leave 2 bytes at the start
            Array.Copy(seed, 0, this.buffer, sizeof(ushort), seedSize);

            // Initialize underlying HMAC context
            this.hmacCtx = new HMac(new Sha256Digest());
            this.hmacCtx.Init(new KeyParameter(hmacKey));
        }

        private void Step(byte[] output, int offset)
        {
            Debug.Assert(output != null);

            if (this.used)
            {
                // If used at least once, reinitialize the HMAC
                this.hmacCtx.Reset();
            }
            else {
                this.used = true;
            }

            // Store counter in big endian, and increment it
            this.buffer[0] = (byte) (this.iteration >> 8);
            this.buffer[1] = (byte) (this.iteration >> 0);
            this.iteration++;

            // Do HMAC magic
            this.hmacCtx.BlockUpdate(buffer, 0, buffer.Length);
            this.hmacCtx.DoFinal(output, offset);
        }

        public static KeygenDerivedkeys GenerateBytes(byte[] hmacKey, byte[] seed, int seedSize)
        {
            int offset = 0;
            int outputSize = 16 * 3;
            byte[] temp = new byte[NFC3D_DRBG_OUTPUT_SIZE];
            byte[] output = new byte[outputSize];

            DrbgCtx rngCtx = new DrbgCtx(hmacKey, seed, seedSize);
            while (outputSize > 0)
            {
                if (outputSize < NFC3D_DRBG_OUTPUT_SIZE)
                {
                    rngCtx.Step(temp, 0);
                    Array.Copy(temp, 0, output, offset, outputSize);
                    break;
                }

                rngCtx.Step(output, offset);
                offset += NFC3D_DRBG_OUTPUT_SIZE;
                outputSize -= NFC3D_DRBG_OUTPUT_SIZE;
            }

            var outkeys = new KeygenDerivedkeys
            {
                aesKey = new byte[16],
                aesIV = new byte[16],
                hmacKey = new byte[16]
            };
            Array.Copy(output, 0, outkeys.aesKey, 0, outkeys.aesKey.Length);
            Array.Copy(output, 16, outkeys.aesIV, 0, outkeys.aesIV.Length);
            Array.Copy(output, 32, outkeys.hmacKey, 0, outkeys.hmacKey.Length);

            return outkeys;
        }
    }
}