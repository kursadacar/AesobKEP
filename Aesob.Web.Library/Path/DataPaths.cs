using System.IO;
using System.Runtime.CompilerServices;

namespace Aesob.Web.Library.Path
{
    public static class DataPaths
    {
        private static Dictionary<string, Path> _cachedPaths;

        public static Path Root
        {
            get
            {
                var cached = GetCachedPath();
                if (cached != null)
                {
                    return cached;
                }

                var path = new Path(AppDomain.CurrentDomain.BaseDirectory);
#if DEBUG
                while (path.SegmentCount > 0)
                {
                    var pathString = path.ToString();

                    if(!Directory.Exists(pathString))
                    {
                        break;
                    }

                    if(Directory.GetDirectories(pathString).Contains(pathString + "\\Data"))
                    {
                        break;
                    }
                    else
                    {
                        path.RemoveLastSegment();
                    }
                }
#endif

                CachePath(path);

                return path;

            }
        }

        public static Path DataFolder
        {
            get
            {
                var cached = GetCachedPath();
                if (cached != null)
                {
                    return cached;
                }

                var path = new Path(Root.ToString());
                path.Append("Data");

                CachePath(path);

                return path;
            }
        }

        public static Path ConfigFolder
        {
            get
            {
                var cached = GetCachedPath();
                if (cached != null)
                {
                    return cached;
                }

                var path = DataFolder.Copy();
                path.Append("Config");

                CachePath(path);

                return path;
            }
        }

        private static Path GetCachedPath([CallerMemberName] string memberName = "")
        {
            if(_cachedPaths != null && _cachedPaths.TryGetValue(memberName, out var cachedPath))
            {
                return cachedPath;
            }

            return null;
        }

        private static void CachePath(Path path, [CallerMemberName] string memberName = "")
        {
            if(_cachedPaths == null)
            {
                _cachedPaths = new Dictionary<string, Path>();
            }

            if(!string.IsNullOrEmpty(memberName))
            {
                _cachedPaths[memberName] = path;
            }
        }
    }
}
