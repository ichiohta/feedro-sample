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

        public static  async Task<T> Deserialize<T>(string label, Func<T> defaultValueGenerator, int numberOfRetry = 3)
        {
            const int retryInterval  = 100;
            Exception innerException = null;

            do
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
                catch (UnauthorizedAccessException e)
                {
                    innerException = e;
                }

                await Task.Delay(retryInterval);
            }
            while (numberOfRetry-- > 0);

            throw new IOException(
                string.Format("Unable to open file '{0}'", label),
                innerException);
        }
    }
}
