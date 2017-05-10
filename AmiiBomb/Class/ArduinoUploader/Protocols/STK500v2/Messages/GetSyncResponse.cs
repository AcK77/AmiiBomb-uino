using System;
using System.Text;

namespace ArduinoUploader.Protocols.STK500v2.Messages
{
    internal class GetSyncResponse : Response
    {
        internal bool IsInSync
        {
            get
            {
                return Bytes.Length > 1 
                    && Bytes[0] == Constants.CMD_SIGN_ON 
                    && Bytes[1] == Constants.STATUS_CMD_OK;
            }
        }

        internal string Signature
        {
            get
            {
                var signatureLength = Bytes[2];
                var signature = new byte[signatureLength];
                Buffer.BlockCopy(Bytes, 3, signature, 0, signatureLength);
                return Encoding.ASCII.GetString(signature);
            }
        }
    }
}
