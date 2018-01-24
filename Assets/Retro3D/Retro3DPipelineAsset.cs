// Retro3DPipeline
// A minimal example of a custom render pipeline with the Retro3D shader.
// https://github.com/keijiro/Retro3DPipeline

using UnityEngine.Experimental.Rendering;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
#endif

namespace Retro3D
{
    // Render pipeline asset for Retro3D
    // Nothing special here. Just a boilerplate thing.
    class Retro3DPipelineAsset : RenderPipelineAsset
    {
#if UNITY_EDITOR
        [MenuItem("Assets/Create/Render Pipeline/Retro3D/Pipeline Asset")]
        static void CreateRetro3DPipeline()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0, CreateInstance<CreateRetro3DPipelineAsset>(),
                "Retro3D Pipeline.asset", null, null
            );
        }

        class CreateRetro3DPipelineAsset : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                var instance = CreateInstance<Retro3DPipelineAsset>();
                AssetDatabase.CreateAsset(instance, pathName);
            }
        }
#endif

        protected override IRenderPipeline InternalCreatePipeline()
        {
            return new Retro3DPipeline();
        }
    }
}
