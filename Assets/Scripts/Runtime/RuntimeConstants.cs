using UnityEngine.SceneManagement;

namespace TandC.GeometryAstro
{
    public static class RuntimeConstants
    {
        public static class Scenes
        {
            public static readonly int Bootstrap = SceneUtility.GetBuildIndexByScenePath("0.Bootstrap");
            public static readonly int Loading = SceneUtility.GetBuildIndexByScenePath("1.Loading");
            public static readonly int Meta = SceneUtility.GetBuildIndexByScenePath("2.Meta");
            public static readonly int Menu = SceneUtility.GetBuildIndexByScenePath("3.Menu");
            public static readonly int Core = SceneUtility.GetBuildIndexByScenePath("4.Core");
            public static readonly int Empty = SceneUtility.GetBuildIndexByScenePath("5.Empty");
        }
    }
}