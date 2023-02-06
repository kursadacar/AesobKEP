using System.IO;
using System.Runtime.CompilerServices;

namespace Aesob.Web.Library.Path
{
    public static class ApplicationPath
    {
        private static Path _cachedRoot;
        private static Dictionary<string, Path> _cachedPaths;

        public static Path Root
        {
            get
            {
                if(_cachedRoot == null)
                {
                    var path = new Path(AppDomain.CurrentDomain.BaseDirectory);
#if DEBUG
                    while (path.SegmentCount > 0)
                    {
                        var pathString = path.ToString();

                        if (!Directory.Exists(pathString))
                        {
                            break;
                        }

                        if (Directory.GetDirectories(pathString).Contains(pathString + "\\Data"))
                        {
                            break;
                        }
                        else
                        {
                            path.RemoveLastSegment();
                        }
                    }
#endif

                    _cachedRoot = path;
                }

                return _cachedRoot.Copy();
            }
        }

        public static Path DataFolder => GetCachedPath("Data");

        public static Path ConfigFolder => GetCachedPath("Config");

        public static Path LogFolder => GetCachedPath("Log");

        private static Path GetCachedPath(string subFolderName, [CallerMemberName] string memberName = "")
        {
            Path cachedPath = null;

            if(_cachedPaths == null || !_cachedPaths.TryGetValue(memberName, out cachedPath))
            {
                if (_cachedPaths == null)
                {
                    _cachedPaths = new Dictionary<string, Path>();
                }

                if (!string.IsNullOrEmpty(memberName) && !string.IsNullOrEmpty(subFolderName))
                {
                    cachedPath = Root.Append(subFolderName);
                    _cachedPaths[memberName] = cachedPath;
                }
            }

            return cachedPath.Copy();
        }
    }
}
