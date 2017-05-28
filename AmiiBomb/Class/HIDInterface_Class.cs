using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using System.IO;

namespace AmiiBomb
{
    internal class HIDDevice
    {
        #region constants
        private const int DIGCF_DEFAULT = 0x1;
        private const int DIGCF_PRESENT = 0x2;
        private const int DIGCF_ALLCLASSES = 0x4;
        private const int DIGCF_PROFILE = 0x8;
        private const int DIGCF_DEVICEINTERFACE = 0x10;

        private const short FILE_ATTRIBUTE_NORMAL = 0x80;
        private const short INVALID_HANDLE_VALUE = -1;
        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint FILE_SHARE_READ = 0x00000001;
        private const uint FILE_SHARE_WRITE = 0x00000002;
        private const uint CREATE_NEW = 1;
        private const uint CREATE_ALWAYS = 2;
        private const uint OPEN_EXISTING = 3;

        #endregion

        #region win32_API_declarations
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid,
                                                      IntPtr Enumerator,
                                                      IntPtr hwndParent,
                                                      uint Flags);

        [DllImport("hid.dll", SetLastError = true)]
        private static extern void HidD_GetHidGuid(ref Guid hidGuid);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiEnumDeviceInterfaces(
           IntPtr hDevInfo,
           //ref SP_DEVINFO_DATA devInfo,
           IntPtr devInfo,
           ref Guid interfaceClassGuid,
           UInt32 memberIndex,
           ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData
        );

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean SetupDiGetDeviceInterfaceDetail(
           IntPtr hDevInfo,
           ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
           ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
           UInt32 deviceInterfaceDetailDataSize,
           out UInt32 requiredSize,
           ref SP_DEVINFO_DATA deviceInfoData
        );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess,
            uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition,
            uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadFile(SafeFileHandle hFile, byte[] lpBuffer,
           uint nNumberOfBytesToRead, ref uint lpNumberOfBytesRead, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteFile(SafeFileHandle hFile, byte[] lpBuffer,
           uint nNumberOfBytesToWrite, ref uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

        [DllImport("hid.dll", SetLastError = true)]
        private static extern bool HidD_GetPreparsedData(
            SafeFileHandle hObject,
            ref IntPtr PreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        private static extern Boolean HidD_FreePreparsedData(ref IntPtr PreparsedData);

        [DllImport("hid.dll", SetLastError = true)]
        private static extern int HidP_GetCaps(
            IntPtr pPHIDP_PREPARSED_DATA,					// IN PHIDP_PREPARSED_DATA  PreparsedData,
            ref HIDP_CAPS myPHIDP_CAPS);				// OUT PHIDP_CAPS  Capabilities

        [DllImport("hid.dll", SetLastError = true)]
        private static extern Boolean HidD_GetAttributes(SafeFileHandle hObject, ref HIDD_ATTRIBUTES Attributes);

        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool HidD_GetFeature(
           IntPtr hDevice,
           IntPtr hReportBuffer,
           uint ReportBufferLength);

        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool HidD_SetFeature(
           IntPtr hDevice,
           IntPtr ReportBuffer,
           uint ReportBufferLength);

        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool HidD_GetProductString(
           SafeFileHandle hDevice,
           IntPtr Buffer,
           uint BufferLength);

        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool HidD_GetSerialNumberString(
           SafeFileHandle hDevice,
           IntPtr Buffer,
           uint BufferLength);

        [DllImport("hid.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        private static extern Boolean HidD_GetManufacturerString(
            SafeFileHandle hDevice,
            IntPtr Buffer,
            uint BufferLength);

        #endregion

        #region structs

        public struct interfaceDetails
        {
            public string manufacturer;
            public string product;
            public string serialNumber;
            public ushort VID;
            public ushort PID;
            public string devicePath;
            public int IN_reportByteLength;
            public int OUT_reportByteLength;
            public ushort versionNumber;
        }

        // HIDP_CAPS
        [StructLayout(LayoutKind.Sequential)]
        private struct HIDP_CAPS
        {
            public System.UInt16 Usage;					// USHORT
            public System.UInt16 UsagePage;				// USHORT
            public System.UInt16 InputReportByteLength;
            public System.UInt16 OutputReportByteLength;
            public System.UInt16 FeatureReportByteLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            public System.UInt16[] Reserved;				// USHORT  Reserved[17];			
            public System.UInt16 NumberLinkCollectionNodes;
            public System.UInt16 NumberInputButtonCaps;
            public System.UInt16 NumberInputValueCaps;
            public System.UInt16 NumberInputDataIndices;
            public System.UInt16 NumberOutputButtonCaps;
            public System.UInt16 NumberOutputValueCaps;
            public System.UInt16 NumberOutputDataIndices;
            public System.UInt16 NumberFeatureButtonCaps;
            public System.UInt16 NumberFeatureValueCaps;
            public System.UInt16 NumberFeatureDataIndices;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct SP_DEVICE_INTERFACE_DATA
        {
            public uint cbSize;
            public Guid InterfaceClassGuid;
            public uint Flags;
            public IntPtr Reserved;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HIDD_ATTRIBUTES
        {
            public Int32 Size;
            public Int16 VendorID;
            public Int16 ProductID;
            public Int16 VersionNumber;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct COMMTIMEOUTS
        {
            public UInt32 ReadIntervalTimeout;
            public UInt32 ReadTotalTimeoutMultiplier;
            public UInt32 ReadTotalTimeoutConstant;
            public UInt32 WriteTotalTimeoutMultiplier;
            public UInt32 WriteTotalTimeoutConstant;
        }

        #endregion

        #region globals
        public bool deviceConnected { get; set; }
        private SafeFileHandle handle_read;
        private SafeFileHandle handle_write;
        private FileStream FS_read;
        private FileStream FS_write;
        private HIDP_CAPS capabilities;
        public interfaceDetails productInfo;
        public event dataReceivedEvent dataReceived;    //The calling class can subscribe to this event
        public delegate void dataReceivedEvent(byte[] message);
        public byte[] readData;
        private bool useAsyncReads;

        #endregion

        #region static_methods

        public static interfaceDetails[] getConnectedDevices()
        {
            interfaceDetails[] devices = new interfaceDetails[0];

            //Create structs to hold interface information
            SP_DEVINFO_DATA devInfo = new SP_DEVINFO_DATA();
            SP_DEVICE_INTERFACE_DATA devIface = new SP_DEVICE_INTERFACE_DATA();
            devInfo.cbSize = (uint)Marshal.SizeOf(devInfo);
            devIface.cbSize = (uint)(Marshal.SizeOf(devIface));

            Guid G = new Guid();
            HidD_GetHidGuid(ref G); //Get the guid of the HID device class

            IntPtr i = SetupDiGetClassDevs(ref G, IntPtr.Zero, IntPtr.Zero, DIGCF_DEVICEINTERFACE | DIGCF_PRESENT);

            //Loop through all available entries in the device list, until false
            SP_DEVICE_INTERFACE_DETAIL_DATA didd = new SP_DEVICE_INTERFACE_DETAIL_DATA();
            if (IntPtr.Size == 8) // for 64 bit operating systems
                didd.cbSize = 8;
            else
                didd.cbSize = 4 + Marshal.SystemDefaultCharSize; // for 32 bit systems

            int j = -1;
            bool b = true;
            int error;
            SafeFileHandle tempHandle;

            while (b)
            {
                j++;

                b = SetupDiEnumDeviceInterfaces(i, IntPtr.Zero, ref G, (uint)j, ref devIface);
                error = Marshal.GetLastWin32Error();
                if (b == false)
                    break;

                uint requiredSize = 0;
                bool b1 = SetupDiGetDeviceInterfaceDetail(i, ref devIface, ref didd, 256, out requiredSize, ref devInfo);
                string devicePath = didd.DevicePath;

                //create file handles using CT_CreateFile
                tempHandle = CreateFile(devicePath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE,
                    IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

                //get capabilites - use getPreParsedData, and getCaps
                //store the reportlengths
                IntPtr ptrToPreParsedData = new IntPtr();
                bool ppdSucsess = HidD_GetPreparsedData(tempHandle, ref ptrToPreParsedData);
                if (ppdSucsess == false)
                    continue;

                HIDP_CAPS capabilities = new HIDP_CAPS();
                int hidCapsSucsess = HidP_GetCaps(ptrToPreParsedData, ref capabilities);

                HIDD_ATTRIBUTES attributes = new HIDD_ATTRIBUTES();
                bool hidAttribSucsess = HidD_GetAttributes(tempHandle, ref attributes);

                string productName = "";
                string SN = "";
                string manfString = "";
                IntPtr buffer = Marshal.AllocHGlobal(126);//max alloc for string; 
                if (HidD_GetProductString(tempHandle, buffer, 126)) productName = Marshal.PtrToStringAuto(buffer);
                if (HidD_GetSerialNumberString(tempHandle, buffer, 126)) SN = Marshal.PtrToStringAuto(buffer);
                if (HidD_GetManufacturerString(tempHandle, buffer, 126)) manfString = Marshal.PtrToStringAuto(buffer);
                Marshal.FreeHGlobal(buffer);

                //Call freePreParsedData to release some stuff
                HidD_FreePreparsedData(ref ptrToPreParsedData);

                //If connection was sucsessful, record the values in a global struct
                interfaceDetails productInfo = new interfaceDetails();
                productInfo.devicePath = devicePath;
                productInfo.manufacturer = manfString;
                productInfo.product = productName;
                productInfo.PID = (ushort)attributes.ProductID;
                productInfo.VID = (ushort)attributes.VendorID;
                productInfo.versionNumber = (ushort)attributes.VersionNumber;
                productInfo.IN_reportByteLength = (int)capabilities.InputReportByteLength;
                productInfo.OUT_reportByteLength = (int)capabilities.OutputReportByteLength;

                if (!stringIsInteger(SN))
                    productInfo.serialNumber = SN;     //Check that serial number is actually a number

                int newSize = devices.Length + 1;
                Array.Resize(ref devices, newSize);
                devices[newSize - 1] = productInfo;
            }
            SetupDiDestroyDeviceInfoList(i);

            return devices;
        }

        #endregion

        #region constructors
        /// <summary>
        /// Creates an object to handle read/write functionality for a USB HID device
        /// Uses one filestream for each of read/write to allow for a write to occur during a blocking
        /// asnychronous read
        /// </summary>
        /// <param name="VID">The vendor ID of the USB device to connect to</param>
        /// <param name="PID">The product ID of the USB device to connect to</param>
        /// <param name="serialNumber">The serial number of the USB device to connect to</param>
        /// <param name="useAsyncReads">True - Read the device and generate events on data being available</param>
        public HIDDevice(ushort VID, ushort PID, string serialNumber, bool useAsyncReads)
        {
            interfaceDetails[] devices = getConnectedDevices();

            //loop through all connected devices to find one with the correct details
            for (int i = 0; i < devices.Length; i++)
            {
                if ((devices[i].VID == VID) && (devices[i].PID == PID) && (devices[i].serialNumber == serialNumber))
                    initDevice(devices[i].devicePath, useAsyncReads);
            }

            if (!deviceConnected)
            {
                string hexVID = numToHexString(VID);
                string hexPID = numToHexString(PID);
                throw new Exception("Device with VID: 0x" + hexVID + " PID: 0x" + hexPID + " SerialNumber: " + serialNumber.ToString() + " could not be found");
            }
        }

        /// <summary>
        /// Creates an object to handle read/write functionality for a USB HID device
        /// Uses one filestream for each of read/write to allow for a write to occur during a blocking
        /// asnychronous read
        /// </summary>
        /// <param name="devicePath">The USB device path - from getConnectedDevices</param>
        /// <param name="useAsyncReads">True - Read the device and generate events on data being available</param>
        public HIDDevice(string devicePath, bool useAsyncReads)
        {
            initDevice(devicePath, useAsyncReads);

            if (!deviceConnected)
            {
                throw new Exception("Device could not be found");
            }
        }
        #endregion

        #region functions
        private void initDevice(string devicePath, bool useAsyncReads)
        {
            deviceConnected = false;

            //create file handles using CT_CreateFile
            handle_read = CreateFile(devicePath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE,
                IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

            handle_write = CreateFile(devicePath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE,
                IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

            //get capabilites - use getPreParsedData, and getCaps
            //store the reportlengths
            IntPtr ptrToPreParsedData = new IntPtr();
            bool ppdSucsess = HidD_GetPreparsedData(handle_read, ref ptrToPreParsedData);

            capabilities = new HIDP_CAPS();
            int hidCapsSucsess = HidP_GetCaps(ptrToPreParsedData, ref capabilities);

            HIDD_ATTRIBUTES attributes = new HIDD_ATTRIBUTES();
            bool hidAttribSucsess = HidD_GetAttributes(handle_read, ref attributes);

            string productName = "";
            string SN = "";
            string manfString = "";
            IntPtr buffer = Marshal.AllocHGlobal(126);//max alloc for string; 
            if (HidD_GetProductString(handle_read, buffer, 126)) productName = Marshal.PtrToStringAuto(buffer);
            if (HidD_GetSerialNumberString(handle_read, buffer, 126)) SN = Marshal.PtrToStringAuto(buffer);
            if (HidD_GetManufacturerString(handle_read, buffer, 126)) manfString = Marshal.PtrToStringAuto(buffer);
            Marshal.FreeHGlobal(buffer);

            //Call freePreParsedData to release some stuff
            HidD_FreePreparsedData(ref ptrToPreParsedData);
            //SetupDiDestroyDeviceInfoList(i);

            if (handle_read.IsInvalid)
                return;

            deviceConnected = true;

            //If connection was sucsessful, record the values in a global struct
            productInfo = new interfaceDetails();
            productInfo.devicePath = devicePath;
            productInfo.manufacturer = manfString;
            productInfo.product = productName;
            productInfo.serialNumber = SN;
            productInfo.PID = (ushort)attributes.ProductID;
            productInfo.VID = (ushort)attributes.VendorID;
            productInfo.versionNumber = (ushort)attributes.VersionNumber;
            productInfo.IN_reportByteLength = (int)capabilities.InputReportByteLength;
            productInfo.OUT_reportByteLength = (int)capabilities.OutputReportByteLength;

            //use a filestream object to bring this stuff into .NET
            FS_read = new FileStream(handle_read, FileAccess.ReadWrite, capabilities.OutputReportByteLength, false);
            FS_write = new FileStream(handle_write, FileAccess.ReadWrite, capabilities.InputReportByteLength, false);

            this.useAsyncReads = useAsyncReads;
            if (useAsyncReads)
                readAsync();
        }

        public void Close()
        {
            if (FS_read != null)
                FS_read.Close();
            if (FS_write != null)
                FS_write.Close();

            if ((handle_read != null) && (!(handle_read.IsInvalid)))
                handle_read.Close();
            if ((handle_write != null) && (!(handle_write.IsInvalid)))
                handle_write.Close();

            this.deviceConnected = false;
        }

        public void Write(byte[] data)
        {
            if (data.Length > capabilities.OutputReportByteLength)
                throw new Exception("Output report must not exceed " + (capabilities.OutputReportByteLength - 1).ToString() + " bytes");

            //uint numBytesWritten = 0;
            byte[] packet = new byte[capabilities.OutputReportByteLength];
            Array.Copy(data, 0, packet, 1, data.Length);            //start at 1, as the first byte must be zero for HID report
            packet[0] = 0;

            if (FS_write.CanWrite)
                FS_write.Write(packet, 0, packet.Length);
            //else
                //throw new Exception("Filestream unable to write");
        }

        //This read function will be used with asychronous operation, called by the constructor if async reads are used
        private void readAsync()
        {
            readData = new byte[capabilities.InputReportByteLength];
            if (FS_read.CanRead)
                FS_read.BeginRead(readData, 0, readData.Length, new AsyncCallback(GetInputReportData), readData);
            else
                throw new Exception("Device is unable to read");
        }

        private void GetInputReportData(IAsyncResult ar)
        {
            try {
                FS_read.EndRead(ar); //must call an endread before starting another one
                //TODO handle exception with PCB is reaet
            }
            catch (Exception) {
                Close();
            }

            //Reset the read thread to read the next report
            if (FS_read.CanRead)
                FS_read.BeginRead(readData, 0, readData.Length, new AsyncCallback(GetInputReportData), readData);
            else
                throw new Exception("Device is unable to read");

            dataReceived(readData);                                     //triggers the event to be heard by the calling class
        }
        
        /// <summary>
        /// This read function is for normal synchronous reads
        /// </summary>
        /// <returns></returns>
        public byte[] Read()
        {
            if (useAsyncReads == true)
                throw new Exception("A synchonous read cannot be executed when operating in async mode");

            //Call readFile
            byte[] readBuf = new byte[capabilities.InputReportByteLength];
            FS_read.Read(readBuf, 0, readBuf.Length);
            return readBuf;
        }
        #endregion

        #region utilities

        public static bool stringIsInteger(string val)
        {
            Double result;
            return Double.TryParse(val, System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.CurrentCulture, out result);
        }

        public static string numToHexString(ushort num)
        {
            return String.Format("{0:X}", num);
        }

        #endregion
    }
}