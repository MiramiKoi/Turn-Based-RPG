using Runtime.ModelCollections;

namespace Runtime.AsyncLoad
{
    public class AddressableModel
    {
        private readonly LoadModelCollection _loadModels = new();

        public IModelCollection<ILoadModel> LoadModels => _loadModels;

        public LoadModel<T> Load<T>(string key)
        {
            LoadModel<T> model;
            if (_loadModels.TryGet(key, out var loadModel))
            {
                model = (LoadModel<T>)loadModel;
                model.RefCount++;
                return model;
            }

            model = new LoadModel<T>(key);
            _loadModels.Add(key, model);
            return model;
        }

        public void Unload<T>(LoadModel<T> loadModel)
        {
            loadModel.RefCount--;
            if (loadModel.RefCount == 0)
                _loadModels.Remove(loadModel.Key);
        }

        public void UnloadAll()
        {
            _loadModels.Clear();
        }
    }
}