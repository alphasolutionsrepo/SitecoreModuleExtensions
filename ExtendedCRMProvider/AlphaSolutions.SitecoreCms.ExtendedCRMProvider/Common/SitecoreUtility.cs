using System;

namespace AlphaSolutions.SitecoreCms.ExtendedCRMProvider.Common
{
    public class SitecoreUtility
    {
        /// <summary>
        /// Method to get a sitecore setting, in a specified type.
        /// </summary>
        /// <typeparam name="T">The type the setting value should be returned in.</typeparam>
        /// <param name="settingName">The name of the sitecore setting.</param>
        /// <returns></returns>
        public static T GetSitecoreSetting<T>(string settingName)
        {
            return GetSitecoreSetting(settingName, default(T));
        }

        /// <summary>
        /// Method to get a sitecore setting, in a specified type.
        /// </summary>
        /// <typeparam name="T">The type the setting value should be returned in.</typeparam>
        /// <param name="settingName">The name of the sitecore setting.</param>
        /// <param name="defaultValue">The default value to return if no setting is found.</param>
        /// <returns></returns>
        public static T GetSitecoreSetting<T>(string settingName, T defaultValue)
        {
            var value = global::Sitecore.Configuration.Settings.GetSetting(settingName);

            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
