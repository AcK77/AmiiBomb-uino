using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AmiiBomb
{
    class Helper_Class
    {
        [DllImport("user32.dll")] [return: MarshalAs(UnmanagedType.Bool)] static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public UInt32 dwTimeout;
        }

        public static bool FlashWindowEx(Form form)
        {
            IntPtr hWnd = form.Handle;
            FLASHWINFO fInfo = new FLASHWINFO();

            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = hWnd;
            fInfo.dwFlags = 3 | 12;
            fInfo.uCount = UInt32.MaxValue;
            fInfo.dwTimeout = 0;

            return FlashWindowEx(ref fInfo);
        }
        public static byte[] String_To_Byte_Array(string Hex)
        {
            return Enumerable.Range(0, Hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(Hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string Clean_NewLine_Spaces(string String)
        {
            return new Regex("[ ]{2,}").Replace(String.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", ""), " ");
        }

        public static string Get_Source_From_Url(string Url)
        {
            try
            {
                WebClient Client = new WebClient();
                StreamReader PageReader = new StreamReader(Client.OpenRead(Url), Encoding.UTF8, true);

                return PageReader.ReadToEnd();
            }
            catch (WebException)
            {
                return "";
            }
        }

        public static UInt16 swapOctetsUInt16(UInt16 toSwap)
        {
            Int32 tmp = 0;
            tmp = toSwap >> 8;
            tmp = tmp | ((toSwap & 0xff) << 8);
            return (UInt16)tmp;
        }

        public static string SHA1_File(byte[] Data)
        {
            return BitConverter.ToString(new SHA1CryptoServiceProvider().ComputeHash(Data)).Replace("-", "");
        }

        public static bool ValidSHA1(byte[] Data, string Hash)
        {
            if (SHA1_File(Data) == Hash)
                return true;
            else
                return false;
        }

        public static Image DropShadow(Image Picture)
        {
            Bitmap shadow = (Bitmap)Picture.Clone();

            for (int y = 0; y < shadow.Height; y++)
            {

                for (int x = 0; x < shadow.Width; x++)
                {
                    var color = shadow.GetPixel(x, y);

                    color = Color.FromArgb((int)((double)color.A * 0.1), 0, 0, 0);

                    shadow.SetPixel(x, y, color);

                }
            }

            var finalComposite = new Bitmap(Picture.Width + 50, Picture.Height, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(finalComposite))
            {
                g.Transform = new Matrix(new Rectangle(0, 0, shadow.Width, shadow.Height), new Point[]{
                    new Point(50,20),
                    new Point(shadow.Width+50, 20),
                    new Point(0, shadow.Height)
                });

                g.DrawImageUnscaled(shadow, new Point(0, 0));

                g.ResetTransform();
                g.DrawImageUnscaled(Picture, new Point(0, 0));

                return finalComposite;
            }
        }

        public static void DoubleBuffered(Control control, bool enable)
        {
            var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            doubleBufferPropertyInfo.SetValue(control, enable, null);
        }

        public static string FirstLetterToUpperCase(string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException("There is no first letter");

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception caught in process: " + ex);
                return false;
            }
        }
    }

    public static class TreeViewExtensions
    {
        public static List<string> GetExpansionState(this TreeNodeCollection nodes)
        {
            return nodes.Descendants()
                        .Where(n => n.IsExpanded)
                        .Select(n => n.FullPath)
                        .ToList();
        }

        public static void SetExpansionState(this TreeNodeCollection nodes, List<string> savedExpansionState)
        {
            foreach (var node in nodes.Descendants()
                                      .Where(n => savedExpansionState.Contains(n.FullPath)))
            {
                node.Expand();
            }
        }

        public static IEnumerable<TreeNode> Descendants(this TreeNodeCollection c)
        {
            foreach (var node in c.OfType<TreeNode>())
            {
                yield return node;

                foreach (var child in node.Nodes.Descendants())
                {
                    yield return child;
                }
            }
        }
    }
}
