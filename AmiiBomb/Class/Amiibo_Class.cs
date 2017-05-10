using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace AmiiBomb
{
    class Amiibo_Class
    {
        public static byte[] Generate_Password(string Long_UID)
        {
            int Padding = 0;

            if (Long_UID.Length == 9*2) { Padding = 2; }

            int pw1 = 0xAA ^ Convert.ToInt32(Long_UID.Substring(2, 2), 16) ^ Convert.ToInt32(Long_UID.Substring(6 + Padding, 2), 16);
            int pw2 = 0x55 ^ Convert.ToInt32(Long_UID.Substring(4, 2), 16) ^ Convert.ToInt32(Long_UID.Substring(8 + Padding, 2), 16);
            int pw3 = 0xAA ^ Convert.ToInt32(Long_UID.Substring(6 + Padding, 2), 16) ^ Convert.ToInt32(Long_UID.Substring(10 + Padding, 2), 16);
            int pw4 = 0x55 ^ Convert.ToInt32(Long_UID.Substring(8 + Padding, 2), 16) ^ Convert.ToInt32(Long_UID.Substring(12 + Padding, 2), 16);

            return Helper_Class.String_To_Byte_Array(pw1.ToString("X2") + pw2.ToString("X2") + pw3.ToString("X2") + pw4.ToString("X2"));
        }

        public static byte[] Calculate_Long_UID(string Short_UID)
        {
            /*
                0x00 - UID0 - Manufacturer Code (0x04 for NXP)
                0x01 - UID1
                0x02 - UID2
                0x03 - BCC0 - 0x88 ^ UID0 ^ UID1 ^ UID2
                0x04 - UID3 (Never 0x88)
                0x05 - UID4
                0x06 - UID5
                0x07 - UID6
                0x08 - BCC1 - UID3 ^ UID4 ^ UID5 ^ UID6
            */

            int BCC0 = 0x88 ^ Convert.ToInt32(Short_UID.Substring(0, 2), 16) ^ Convert.ToInt32(Short_UID.Substring(2, 2), 16) ^ Convert.ToInt32(Short_UID.Substring(4, 2), 16);
            int BCC1 = Convert.ToInt32(Short_UID.Substring(6, 2), 16) ^ Convert.ToInt32(Short_UID.Substring(8, 2), 16) ^ Convert.ToInt32(Short_UID.Substring(10, 2), 16) ^ Convert.ToInt32(Short_UID.Substring(12, 2), 16);

            return Helper_Class.String_To_Byte_Array(Short_UID.Substring(0, 6) + BCC0.ToString("X2") + Short_UID.Substring(6, 8) + BCC1.ToString("X2"));
        }

        public static bool IsEncrypted(byte[] Data)
        {
            /*
                0x04 - Manufacturer Code (0x04 for NXP)
                0x48 - Internal Byte, Always 0x48
                Todo - Add a LockBytes Check ?
            */
            if (Data[9] == 0x48) return true;
            else return false;
        }

        public static byte[] Decrypt(byte[] Data, AmiiboKeys Keyfile)
        {
            byte[] Decrypted = new byte[NtagHelpers.NFC3D_AMIIBO_SIZE];
            Keyfile.Unpack(Data, Decrypted);
            return Decrypted;
        }

        public static byte[] Encrypt(byte[] Data, AmiiboKeys Keyfile)
        {
            byte[] Encrypted = new byte[NtagHelpers.NFC3D_NTAG_SIZE];
            Keyfile.Pack(Data, Encrypted);
            return Encrypted;
        }

        public static byte[] Patch(byte[] Data, string UID)
        {
            // ToDo: Fix UID Cast

            byte[] Decrypted_Amiibo;
            if (IsEncrypted(Data)) Decrypted_Amiibo = Amiibo_Class.Decrypt(Data, Main_Form.AmiiKeys);
            else Decrypted_Amiibo = Data;

            byte[] Password_Amiibo = Amiibo_Class.Generate_Password(UID);
            byte[] UID_Long = NtagHelpers.StringToByteArrayFastest(UID);

            Array.Copy(UID_Long, 0x008, Decrypted_Amiibo, 0x000, 0x001); //Put LastChar of Long UID
            Array.Copy(new byte[] { 0x00, 0x00 }, 0x000, Decrypted_Amiibo, 0x002, 0x002); //Reset Static Lock Bytes
            Array.Copy(UID_Long, 0x000, Decrypted_Amiibo, 0x1D4, 0x008); //Modify UID
            Array.Copy(new byte[] { 0x00, 0x00, 0x00 }, 0x000, Decrypted_Amiibo, 0x208, 0x003); //Reset Dynamic Lock Bytes
            Array.Copy(Password_Amiibo, 0x000, Decrypted_Amiibo, 0x214, 0x004); //Modify Password
            Array.Copy(new byte[] { 0x80, 0x80 }, 0x000, Decrypted_Amiibo, 0x218, 0x002); //Reset PACK0 & PACK1

            return Amiibo_Class.Encrypt(Decrypted_Amiibo, Main_Form.AmiiKeys);
        }

        public static string Get_NFC_ID(byte[] internalTag)
        {
            return String.Format("{0:X2}{1:X2}{2:X2}{3:X2}-{4:X2}{5:X2}{6:X2}{7:X2}",
                    internalTag[0x1DC], internalTag[0x1DD], internalTag[0x1DE], internalTag[0x1DF], internalTag[0x1E0], internalTag[0x1E1], internalTag[0x1E2], internalTag[0x1E3]);
        }

        public static string Get_Character_ID(byte[] internalTag)
        {
            return String.Format("{0:X2}{1:X2}", internalTag[0x1E0], internalTag[0x1E1]);
        }

        public static string Get_GameSeries_ID(byte[] internalTag)
        {
            return String.Format("{0:X2}{1:X2}{2:X2}", internalTag[0x1DC], internalTag[0x1DD], internalTag[0x1DE]).Substring(0, 3);
        }

        //Need to fix strange size of Nickname and owner of 0xF instead of 0x14!!

        public static string Get_Amiibo_Nickname(byte[] internalTag)
        {
            byte[] Amiibo_Nickname_Buffer = new byte[0x14];
            Array.Copy(internalTag, 0x02C + 0x0C, Amiibo_Nickname_Buffer, 0x000, Amiibo_Nickname_Buffer.Length);

            return Encoding.BigEndianUnicode.GetString(Amiibo_Nickname_Buffer);
        }

        public static string Get_Amiibo_Mii_Nickname(byte[] internalTag)
        {
            byte[] Amiibo_Mii_Nickname_Buffer = new byte[0x14];
            Array.Copy(internalTag, 0x02C + 0x020 + 0x01A, Amiibo_Mii_Nickname_Buffer, 0x000, Amiibo_Mii_Nickname_Buffer.Length);

            return Encoding.Unicode.GetString(Amiibo_Mii_Nickname_Buffer);
        }

        public static string Get_Amiibo_Write_Counter(byte[] internalTag)
        {
            byte[] Amiibo_Write_Counter = new byte[0x02];
            Array.Copy(internalTag, 0x02C + 0x088, Amiibo_Write_Counter, 0x000, Amiibo_Write_Counter.Length);
            Array.Reverse(Amiibo_Write_Counter);

            return BitConverter.ToInt16(Amiibo_Write_Counter, 0).ToString();
        }

        public static string Get_Amiibo_AppID(byte[] internalTag)
        {
            byte[] Amiibo_AppID = new byte[0x04];
            Array.Copy(internalTag, 0x02C + 0x08A, Amiibo_AppID, 0x000, Amiibo_AppID.Length);

            return BitConverter.ToString(Amiibo_AppID).Replace("-", "");
        }

        public static string Get_Amiibo_Initialized_AppID(byte[] internalTag)
        {
            byte[] Amiibo_Initialized_AppID = new byte[0x08];
            Array.Copy(internalTag, 0x02C + 0x080, Amiibo_Initialized_AppID, 0x000, Amiibo_Initialized_AppID.Length);
            string AppID = BitConverter.ToString(Amiibo_Initialized_AppID).Replace("-", "");
            return AppID.Substring(0, 8) + "-" + AppID.Substring(8, 8);
        }

        public static string Get_Amiibo_Country(byte[] internalTag)
        {
            byte[] Amiibo_CountryCode = new byte[0x01];
            Array.Copy(internalTag, 0x02C + 0x001, Amiibo_CountryCode, 0x000, Amiibo_CountryCode.Length);

            return CountryCode_Class.Get_Country_Name(Amiibo_CountryCode[0]);
        }

        public static int Get_Amiibo_Initialize_UserData(byte[] internalTag)
        {
            byte[] Amiibo_Initialize = new byte[0x01];
            Array.Copy(internalTag, 0x02C, Amiibo_Initialize, 0x000, Amiibo_Initialize.Length);

            return Amiibo_Initialize[0] & 0x30;
        }

        public static string Get_Amiibo_LastModifiedDate(byte[] internalTag)
        {
            var Amiibo_Date_Buffer = new byte[0x02];
            Array.Copy(internalTag, 0x02C + 0x006, Amiibo_Date_Buffer, 0x000, Amiibo_Date_Buffer.Length);
            Array.Reverse(Amiibo_Date_Buffer);

            return NtagHelpers.DateTimeFromTag((BitConverter.ToUInt16(Amiibo_Date_Buffer, 0))).ToShortDateString();
        }

        public static byte[] Dump_AppData(byte[] Data)
        {
            byte[] Decrypted_Amiibo;
            if (IsEncrypted(Data)) Decrypted_Amiibo = Amiibo_Class.Decrypt(Data, Main_Form.AmiiKeys);
            else Decrypted_Amiibo = Data;

            byte[] Amiibo_AppData = new byte[0x0D8];
            Array.Copy(Decrypted_Amiibo, 0x0DC, Amiibo_AppData, 0x000, 0x0D8);

            return Amiibo_AppData;
        }

        public static byte[] WriteAppData(byte[] Data, byte[] AppData)
        {
            byte[] Decrypted_Amiibo;
            if (IsEncrypted(Data)) Decrypted_Amiibo = Amiibo_Class.Decrypt(Data, Main_Form.AmiiKeys);
            else Decrypted_Amiibo = Data;

            Array.Copy(AppData, 0x000, Decrypted_Amiibo, 0x0DC, 0x0D8);

            if (IsEncrypted(Data)) return Amiibo_Class.Encrypt(Decrypted_Amiibo, Main_Form.AmiiKeys);
            else return Decrypted_Amiibo;
            
        }
    }
}
