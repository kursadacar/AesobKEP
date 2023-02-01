namespace Aesob.Web.Library.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Sort<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, IComparer<TKey> keyComparer)
        {
            var kvps = dictionary.ToList();
            var comparisonWrapper = new DictionaryComparisonWrapper<TKey, TValue>(keyComparer);

            kvps.Sort(comparisonWrapper);

            dictionary.Clear();

            for(int i = 0; i < kvps.Count; i++)
            {
                dictionary.Add(kvps[i].Key, kvps[i].Value);
            }
        }

        public static void Sort<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, IComparer<TValue> valueComparer)
        {
            var kvps = dictionary.ToList();
            var comparisonWrapper = new DictionaryComparisonWrapper<TKey, TValue>(valueComparer);

            kvps.Sort(comparisonWrapper);

            dictionary.Clear();

            for(int i = 0; i < kvps.Count; i++)
            {
                dictionary.Add(kvps[i].Key, kvps[i].Value);
            }
        }

        private class DictionaryComparisonWrapper<TKey, TValue> : IComparer<KeyValuePair<TKey, TValue>>
        {
            private readonly IComparer<TValue> _valueComparer;
            private readonly IComparer<TKey> _keyComparer;

            public DictionaryComparisonWrapper(IComparer<TKey> keyComparer)
            {
                _keyComparer = keyComparer;
            }

            public DictionaryComparisonWrapper(IComparer<TValue> valueComparer)
            {
                _valueComparer = valueComparer;
            }

            public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
            {
                if(_keyComparer != null)
                {
                    return _keyComparer.Compare(x.Key, y.Key);
                }

                if(_valueComparer != null)
                {
                    return _valueComparer.Compare(x.Value, y.Value);
                }

                Debug.FailedAssert("Dictionary Comparison Wrapper needs at least one non-null comparer");
                return 0;
            }
        }
    }
}
