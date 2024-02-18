namespace BlackberryMead.Utility
{
    /// <summary>
    /// Helper class for file management.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Gets the name of a file with the given path.
        /// </summary>
        /// <param name="path">Path of the file.</param>
        /// <returns>The name of the file at the given path</returns>
        public static string GetFileName(string path, bool includeExtension = false)
        {
            string[] _ = path.Split('\\');
            if (includeExtension)
                return _[_.Length - 1];
            return _[_.Length - 1].Split('.')[0];
        }
    }
}
