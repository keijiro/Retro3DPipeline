using UnityEditor;
using UnityEngine.Experimental.Rendering;
using UnityEditor.ProjectWindowCallback;

namespace Retro3D
{
    class Retro3DPipelineAsset : RenderPipelineAsset
    {
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

        protected override IRenderPipeline InternalCreatePipeline()
        {
            return new Retro3DPipeline();
        }
    }
}
