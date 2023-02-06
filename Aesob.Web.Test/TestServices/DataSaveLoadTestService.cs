using Aesob.Web.Core.Public;
using Aesob.Web.Library;
using Aesob.Web.Library.Service;

namespace Aesob.Web.Test.TestServices
{
    internal class DataSaveLoadTestService : IAesobService
    {
        private IAesobService _thisAsInterface;
        private Random _random;

        public DataSaveLoadTestService()
        {
            _thisAsInterface = this;
            _random = new Random();
        }

        public void Start()
        {
            for(int i = 0; i < 10; i++)
            {
                RunTest();
            }

        }

        private void RunTest()
        {
            _thisAsInterface.ClearData();

            var savedDatas = SaveTestData();
            _thisAsInterface.LoadDataFromFile();

            foreach(var data in savedDatas)
            {
                var loadedData = _thisAsInterface.GetData(data.Name);
                var match = DoesDataMatch(data, loadedData);
                Debug.Assert(match, "Saved data does not match loaded data");
            }
        }

        private List<IServiceData> SaveTestData()
        {
            List<IServiceData> dataList = new List<IServiceData>();
            var nodeCount = _random.Next(6, 12);

            for (int i = 0; i < nodeCount; i++)
            {
                var testData = CreateTestData(null, i);
                _thisAsInterface.SetData(testData.Name, testData);
                dataList.Add(testData);
            }

            _thisAsInterface.SaveData();

            return dataList;
        }

        private IServiceData CreateTestData(IServiceData parentData, int index)
        {
            var nameToUse = (parentData?.Name ?? "Name") + "_" + index;
            var valueToUse = (parentData?.Value ?? "Value") + "_" + index;

            var testData = _thisAsInterface.CreateServiceData(nameToUse, valueToUse);

            if (_random.Next(0, 100) > 70)
            {
                var childCount = _random.Next(1, 5);
                for (int i = 0; i < childCount; i++)
                {
                    var subData = CreateTestData(testData, i);
                    testData.AddSubData(subData);
                }
            }

            return testData;
        }

        private bool DoesDataMatch(IServiceData first, IServiceData second)
        {
            if (first.Name != second.Name || first.Value != second.Value || first.SubData.Count != second.SubData.Count)
            {
                return false;
            }

            int subDataCount = first.SubData.Count;

            for (int i = 0; i < subDataCount; i++)
            {
                var match = DoesDataMatch(first.SubData.ElementAt(i), second.SubData.ElementAt(i));
                if (!match)
                {
                    return false;
                }
            }

            return true;
        }

        public void Stop()
        {
        }

        public void Update(float dt)
        {
        }
    }
}
