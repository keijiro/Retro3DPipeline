using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

namespace Retro3D
{
    public class Retro3DPipeline : RenderPipeline
    {
        CommandBuffer _clearCommand;

        public override void Dispose()
        {
            base.Dispose();

            if (_clearCommand != null) _clearCommand.Dispose();
        }

        public override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            base.Render(context, cameras);

            if (_clearCommand == null)
            {
                _clearCommand = new CommandBuffer();
                _clearCommand.name = "Clear";
                _clearCommand.ClearRenderTarget(true, true, Color.black);
            }

            foreach (var camera in cameras)
            {
                context.ExecuteCommandBuffer(_clearCommand);

                var culled = new CullResults();
                CullResults.Cull(camera, context, out culled);
                context.SetupCameraProperties(camera);

                var settings = new DrawRendererSettings(camera, new ShaderPassName("Base"));
                var filter = new FilterRenderersSettings(true);
                filter.renderQueueRange = RenderQueueRange.opaque;
                context.DrawRenderers(culled.visibleRenderers, ref settings, filter);

                context.Submit();
            }
        }
    }
}
