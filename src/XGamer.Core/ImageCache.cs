namespace XGamer.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Media.Imaging;

    public class ImageCache
    {
        private static readonly object lockObject = new object();

        private Dictionary<string, BitmapImage> cacheDictionary;
        private string[] imageFilePaths;
        private int imageIndex;          

        public ImageCache(string imageFolderPath)
        {
            cacheDictionary = new Dictionary<string, BitmapImage>();
            imageFilePaths = Directory.GetFiles(imageFolderPath, "*.jpg", SearchOption.AllDirectories);
            imageIndex = 0;
        }

        public void ProcessImageFolder()
        {
            while (imageIndex < imageFilePaths.Length)
            {
                BitmapImage bi = new BitmapImage();
                string imagePath = imageFilePaths[imageIndex];
                try
                {
                    bi.BeginInit();
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.UriSource = new Uri(imagePath);
                    bi.EndInit();
                }
                catch
                {
                    bi = null;
                }

                string fileName = Path.GetFileName(imagePath);
                SetImage(bi, fileName);
                imageIndex += 1;
            }
        }

        public BitmapImage GetImage(string key)
        {
            if (cacheDictionary.ContainsKey(key))
            {
                return cacheDictionary[key];
            }

            return null;
        }

        private void SetImage(BitmapImage bi, string key)
        {
            lock (lockObject)
            {
                cacheDictionary[key] = bi;
            }
        }
    }
}
