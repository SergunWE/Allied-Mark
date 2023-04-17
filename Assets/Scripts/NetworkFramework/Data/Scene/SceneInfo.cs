using UnityEngine.SceneManagement;

namespace NetworkFramework.Data.Scene
{
    /// <summary>
    /// A structure for storing scene information. Designed to refer to scenes by index and name
    /// </summary>
    public struct SceneInfo
    {
        /// <summary>
        /// Scene index in the project build
        /// </summary>
        public readonly int Index;
        /// <summary>
        /// Scene name in the project build
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// The default scene load type
        /// <value>Single - closes all current loaded Scenes and loads a Scene.
        /// Additive - Adds the Scene to the current loaded Scenes
        /// </value>
        /// </summary>
        public readonly LoadSceneMode LoadMode;

        /// <summary>
        /// Structure Builder
        /// </summary>
        /// <param name="index">Scene index in the build</param>
        /// <param name="loadMode">Type of scene loading</param>
        /// <param name="name">Optional parameter, scene name in the build</param>
        public SceneInfo(int index, LoadSceneMode loadMode = LoadSceneMode.Single, string name = null)
        {
            Index = index;
            LoadMode = loadMode;
            Name = name ?? NameFromIndex(index);
        }
        
        /// <summary>
        /// Getting the scene name from the scene index
        /// </summary>
        /// <param name="buildIndex">Scene Index</param>
        /// <returns>string name</returns>
        /// <exception cref="System.Exception">
        /// Error when entering the index incorrectly
        /// </exception>
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