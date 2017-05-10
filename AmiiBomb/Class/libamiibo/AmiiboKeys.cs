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
using System.IO;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace AmiiBomb
{
    public class AmiiboKeys
    {
        public const int HMAC_POS_DATA = 0x008;
        public const int HMAC_POS_TAG = 0x1B4;

        private AmiiboKeys() { }

        private KeygenMasterkeys data;
        private KeygenMasterkeys tag;

        internal static AmiiboKeys Unserialize(BinaryReader reader)
        {
            return new AmiiboKeys
            {
                data = KeygenMasterkeys.Unserialize(reader),
                tag = KeygenMasterkeys.Unserialize(reader)
            };
        }

        internal void Serialize(BinaryWriter writer)
        {
            this.data.Serialize(writer);
            this.tag.Serialize(writer);
        }

        public bool Unpack(byte[] tag, byte[] plain)
        {
            byte[] internalBytes = NtagHelpers.GetInternalTag(tag);

            // Generate keys
            KeygenDerivedkeys dataKeys = GenerateKey(this.data, internalBytes);
            KeygenDerivedkeys tagKeys = GenerateKey(this.tag, internalBytes);

            // Decrypt
            dataKeys.Cipher(internalBytes, plain, false);

            // Init OpenSSL HMAC context
            HMac hmacCtx = new HMac(new Sha256Digest());

            // Regenerate tag HMAC. Note: order matters, data HMAC depends on tag HMAC!
            hmacCtx.Init(new KeyParameter(tagKeys.hmacKey));
            hmacCtx.BlockUpdate(plain, 0x1D4, 0x34);
            hmacCtx.DoFinal(plain, HMAC_POS_TAG);

            // Regenerate data HMAC
            hmacCtx.Init(new KeyParameter(dataKeys.hmacKey));
            hmacCtx.BlockUpdate(plain, 0x029, 0x1DF);
            hmacCtx.DoFinal(plain, HMAC_POS_DATA);

            Array.Copy(tag, 0x208, plain, 0x208, 0x014);

            return
                NativeHelpers.MemCmp(plain, internalBytes, HMAC_POS_DATA, 32) &&
                NativeHelpers.MemCmp(plain, internalBytes, HMAC_POS_TAG, 32);
        }

        public void Pack(byte[] plain, byte[] tag)
        {
            byte[] cipher = new byte[NtagHelpers.NFC3D_AMIIBO_SIZE];

            // Generate keys
            var tagKeys = GenerateKey(this.tag, plain);
            var dataKeys = GenerateKey(this.data, plain);

            // Init OpenSSL HMAC context
            HMac hmacCtx = new HMac(new Sha256Digest());

            // Generate tag HMAC
            hmacCtx.Init(new KeyParameter(tagKeys.hmacKey));
            hmacCtx.BlockUpdate(plain, 0x1D4, 0x34);
            hmacCtx.DoFinal(cipher, HMAC_POS_TAG);

            // Generate data HMAC
            hmacCtx.Init(new KeyParameter(dataKeys.hmacKey));
            hmacCtx.BlockUpdate(plain, 0x029, 0x18B);           // Data
            hmacCtx.BlockUpdate(cipher, HMAC_POS_TAG, 0x20);    // Tag HMAC
            hmacCtx.BlockUpdate(plain, 0x1D4, 0x34);            // Tag
            hmacCtx.DoFinal(cipher, HMAC_POS_DATA);

            // Encrypt
            dataKeys.Cipher(plain, cipher, true);

            // Convert back to hardware
            NtagHelpers.InternalToTag(cipher, tag);

            Array.Copy(plain, 0x208, tag, 0x208, 0x014);
        }

        public static AmiiboKeys LoadKeys(string path)
        {
            if (!File.Exists(path))
                return null;

            try
            {
                using (var reader = new BinaryReader(File.OpenRead(path)))
                {
                    var result = AmiiboKeys.Unserialize(reader);

                    if ((result.data.magicBytesSize > 16) || (result.tag.magicBytesSize > 16))
                        return null;

                    return result;
                }
            }
            catch
            {
                return null;
            }
        }

        private static byte[] CalcSeed(byte[] dump)
        {
            byte[] key = new byte[KeygenMasterkeys.NFC3D_KEYGEN_SEED_SIZE];
            Array.Copy(dump, 0x029, key, 0x00, 0x02);
            Array.Copy(dump, 0x1D4, key, 0x10, 0x08);
            Array.Copy(dump, 0x1D4, key, 0x18, 0x08);
            Array.Copy(dump, 0x1E8, key, 0x20, 0x20);
            return key;
        }

        private KeygenDerivedkeys GenerateKey(KeygenMasterkeys masterKeys, byte[] dump)
        {
            byte[] seed = CalcSeed(dump);
            return masterKeys.GenerateKey(seed);
        }
    }
}