namespace XGamer.Core
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media.Imaging;
    using System.IO;

    public class ImageCache
    {
        public static readonly object LockObject = new object();

        private readonly Dictionary<string, BitmapImage> _cacheDictionary;
        private readonly string[] _imageFilePaths;
        private int _imageIndex;          

        public ImageCache(string imageFolderPath)
        {
            _cacheDictionary = new Dictionary<string, BitmapImage>();
            _imageFilePaths = Directory.GetFiles(imageFolderPath, "*.jpg", SearchOption.AllDirectories);
            _imageIndex = 0;
        }

        public void ProcessImageFolder()
        {
            while (_imageIndex < _imageFilePaths.Length)
            {
                BitmapImage bi = new BitmapImage();
                string imagePath = _imageFilePaths[_imageIndex];
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
                _imageIndex += 1;
            }
        }

        private void SetImage(BitmapImage bi, string key)
        {
            lock (LockObject)
            {
                _cacheDictionary[key] = bi;
            }
        }

        public BitmapImage GetImage(string key)
        {
            lock (LockObject)
            {
                if (_cacheDictionary.ContainsKey(key))
                {
                    return _cacheDictionary[key];
                }
            }

            return null;
        }
    }
}