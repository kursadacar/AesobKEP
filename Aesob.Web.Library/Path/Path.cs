using System.Text;

namespace Aesob.Web.Library.Path
{
    public class Path
    {
        private List<string> _segments;

        public int SegmentCount => _segments?.Count ?? 0;
        public string FirstSegment => _segments.Count > 0 ? _segments[0] : null;
        public string LastSegment => _segments.Count > 0 ? _segments[_segments.Count - 1] : null;

        private Path(List<string> segments)
        {
            _segments = segments.ToList();
        }

        public Path(string path)
        {
            _segments = new List<string>();

            var pathSegments = path.Replace('/', '\\').Split('\\');

            for(int i = 0; i < pathSegments.Length; i++)
            {
                if (!string.IsNullOrEmpty(pathSegments[i]))
                {
                    _segments.Add(pathSegments[i]);
                }
            }
        }

        public Path Copy()
        {
            var newPath = new Path(_segments);
            return newPath;
        }

        public Path Prepend(string segment)
        {
            _segments.Insert(0, segment);

            return this;
        }

        public Path Append(string segment)
        {
            _segments.Add(segment);

            return this;
        }

        public bool RemoveSegment(int index)
        {
            if(index >= 0 && index < _segments.Count)
            {
                _segments.RemoveAt(index);
                return true;
            }

            return false;
        }

        public bool RemoveLastSegment()
        {
            return RemoveSegment(_segments.Count - 1);
        }

        public bool RemoveFirstSegment()
        {
            return RemoveSegment(0);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (_segments != null)
            {
                for (int i = 0; i < _segments.Count; i++)
                {
                    sb.Append(_segments[i]);
                    sb.Append('\\');
                }

                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
    }
}
