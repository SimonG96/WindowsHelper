using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Lib.Tools
{
    public static class ClipboardHelper
    {
        #region Set

        public static bool SetDataObject(object data, int tries)
        {
            for (int i = 0; i < tries; i++)
            {
                try
                {
                    Clipboard.SetDataObject(data);
                    return true;
                }
                catch (Exception e) //TODO: Once exception handler is ready throw this again (maybe only after max tries)
                {
                    Console.WriteLine($"Try {i}:");
                    Console.WriteLine(e);
                }
            }

            return false;
        }

        #endregion Set


        #region Contains

        public static bool ContainsText(int tries)
        {
            for (int i = 0; i < tries; i++)
            {
                try
                {
                    bool containsText = Clipboard.ContainsText();
                    return containsText;
                }
                catch (Exception e) //TODO: Once exception handler is ready throw this again (maybe only after max tries)
                {
                    Console.WriteLine($"Try {i}:");
                    Console.WriteLine(e);
                }
            }

            return false;
        }

        public static bool ContainsImage(int tries)
        {
            for (int i = 0; i < tries; i++)
            {
                try
                {
                    bool containsImage = Clipboard.ContainsImage();
                    return containsImage;
                }
                catch (Exception e) //TODO: Once exception handler is ready throw this again (maybe only after max tries)
                {
                    Console.WriteLine($"Try {i}:");
                    Console.WriteLine(e);
                }
            }

            return false;
        }

        #endregion Contains


        #region Get

        public static string GetText(int tries)
        {
            for (int i = 0; i < tries; i++)
            {
                try
                {
                    string text = Clipboard.GetText();
                    return text;
                }
                catch (Exception e) //TODO: Once exception handler is ready throw this again (maybe only after max tries)
                {
                    Console.WriteLine($"Try {i}:");
                    Console.WriteLine(e);
                }
            }

            return null;
        }

        public static BitmapSource GetImage(int tries)
        {
            for (int i = 0; i < tries; i++)
            {
                try
                {
                    BitmapSource bitmapSource = Clipboard.GetImage();
                    return bitmapSource;
                }
                catch (Exception e) //TODO: Once exception handler is ready throw this again (maybe only after max tries)
                {
                    Console.WriteLine($"Try {i}:");
                    Console.WriteLine(e);
                }
            }

            return null;
        }

        #endregion Get
    }
}