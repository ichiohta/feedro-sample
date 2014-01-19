using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;

namespace Feedro.Model.Helper
{
    public static class StorageHelper
    {
        public static async Task Serialize<T>(string label, T value)
        {
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(label, CreationCollisionOption.ReplaceExisting))
            {
                new DataContractSerializer(typeof(T)).WriteObject(stream, value);
            }
        }

        public static  async Task<T> Deserialize<T>(string label, Func<T> defaultValueGenerator)
        {
            try
            {
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(label))
                {
                    return (T)(new DataContractSerializer(typeof(T)).ReadObject(stream));
                }
            }
            catch (FileNotFoundException)
            {
                return defaultValueGenerator();
            }
        }
    }
}
