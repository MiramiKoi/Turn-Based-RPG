using Runtime.Common;
using Runtime.CustomAsync;

namespace Runtime.AsyncLoad
{
    public class LoadModel<T> : ILoadModel
    {
        public CustomAwaiter LoadAwaiter { get; } = new();

        public T Result { get; set; }

        public string Key { get; }
        public int RefCount { get; set; }

        public LoadModel(string key)
        {
            Key = key;
            RefCount = 1;
        }

        public void CompleteLoad()
        {
            LoadAwaiter.Complete();
        }

        public void DisposeLoad()
        {
            LoadAwaiter.Dispose();
        }

        public IPresenter CreatePresenter()
        {
            return new LoadPresenter<T>(this);
        }
    }
}
