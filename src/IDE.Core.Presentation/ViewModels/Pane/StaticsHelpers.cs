﻿namespace IDE.Core.ViewModels
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Class maintains and helps access to core facts of this application.
    /// Core facts are installation directory, name of application etc.
    /// 
    /// This class should not be used directly unless it is realy necessary.
    /// Use the <seealso cref="AppCoreModel"/> through its interface and
    /// constructor dependency injection to avoid unnecessary dependencies
    /// and problems when refactoring later on.
    /// </summary>
    public class AppHelpers
    {

        #region properties
        /// <summary>
        /// Get a path to the directory where the application
        /// can persist/load user data on session exit and re-start.
        /// </summary>
        public static string DirAppData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CadIde");

        /// <summary>
        /// Get a path to the directory where the user store his documents
        /// </summary>
        public static string MyDocumentsUserDir => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        /// <summary>
        /// Get the name of the executing assembly (usually name of *.exe file)
        /// </summary>
        public static string AssemblyTitle => Assembly.GetEntryAssembly().GetName().Name;

        public static string ApplicationTitle => "CadIde";

        /// <summary>
        /// contains edition name
        /// </summary>
        public static string ApplicationFullTitle => $"{ApplicationTitle}";

        public static string ApplicationUrl => "https://github.com/mihai-ene-public/cadide";

        public static string ApplicationVersion => Assembly.GetEntryAssembly().GetName().Version.ToString();
        public static string ApplicationRuntimeVersion => Assembly.GetEntryAssembly().ImageRuntimeVersion;

        public static int ApplicationVersionMajor => Assembly.GetEntryAssembly().GetName().Version.Major;

        //
        // Summary:
        //     Gets the path or UNC location of the loaded file that contains the manifest.
        //
        // Returns:
        //     The location of the loaded file that contains the manifest. If the loaded
        //     file was shadow-copied, the location is that of the file after being shadow-copied.
        //     If the assembly is loaded from a byte array, such as when using the System.Reflection.Assembly.Load(System.Byte[])
        //     method overload, the value returned is an empty string ("").
        public static string AssemblyEntryLocation => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);



        public static string LayoutFileName => "Layout.config";

        /// <summary>
        /// .ide folder
        /// </summary>
        public static string SolutionConfigFolderName => ".ide";

        /// <summary>
        /// Get path and file name to application specific session file
        /// </summary>
        public static string DirFileAppSessionData => Path.Combine(DirAppData, string.Format(CultureInfo.InvariantCulture, "{0}.App.session", AssemblyTitle));

        /// <summary>
        /// Get path and file name to application specific settings file
        /// </summary>
        public static string DirFileAppSettingsData
        {
            get
            {
                return System.IO.Path.Combine(DirAppData,
                                              string.Format(CultureInfo.InvariantCulture, "{0}.App.settings",
                                                             AssemblyTitle));
            }
        }
        #endregion properties

    }
}
