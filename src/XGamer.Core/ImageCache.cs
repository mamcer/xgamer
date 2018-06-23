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
            this.cacheDictionary = new Dictionary<string, BitmapImage>();
            this.imageFilePaths = Directory.GetFiles(imageFolderPath, "*.jpg", SearchOption.AllDirectories);
            this.imageIndex = 0;
        }

        public void ProcessImageFolder()
        {
            while (this.imageIndex < this.imageFilePaths.Length)
            {
                BitmapImage bi = new BitmapImage();
                string imagePath = this.imageFilePaths[this.imageIndex];
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
                this.SetImage(bi, fileName);
                this.imageIndex += 1;
            }
        }

        public BitmapImage GetImage(string key)
        {
            if (this.cacheDictionary.ContainsKey(key))
            {
                return this.cacheDictionary[key];
            }

            return null;
        }

        private void SetImage(BitmapImage bi, string key)
        {
            lock (lockObject)
            {
                this.cacheDictionary[key] = bi;
            }
        }
    }
}
