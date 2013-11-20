using Windows.Storage;

namespace Feedro.Model.Helper
{
    public static class SettingHelper
    {
        public static T GetLocalSetting<T>(string name, T defaultValue)
        {
            object value = ApplicationData.Current.LocalSettings.Values[name];
            return value != null ? (T)value : defaultValue;
        }

        public static void SetLocalSetting<T>(string name, T value)
        {
            ApplicationData.Current.LocalSettings.Values[name] = value;
        }
    }
}
