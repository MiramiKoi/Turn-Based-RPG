using Runtime.Common;
using Runtime.CustomAsync;

namespace Runtime.AsyncLoad
{
    public interface ILoadModel
    {
        public CustomAwaiter LoadAwaiter { get; }
        public string Key { get; }

        public IPresenter CreatePresenter();
    }
}
