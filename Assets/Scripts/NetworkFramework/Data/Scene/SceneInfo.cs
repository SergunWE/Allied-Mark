using UnityEngine.SceneManagement;

namespace NetworkFramework.Data.Scene
{
    public struct SceneInfo
    {
        public readonly int Index;
        public readonly string Name;
        public readonly LoadSceneMode LoadMode;

        public SceneInfo(int index, LoadSceneMode loadMode = LoadSceneMode.Single, string name = null)
        {
            Index = index;
            LoadMode = loadMode;
            Name = name ?? NameFromIndex(index);
        }
        
        private static string NameFromIndex(int buildIndex)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            int slash = path.LastIndexOf('/');
            string name = path[(slash + 1)..];
            int dot = name.LastIndexOf('.');
            return name[..dot];
        }
    }
}