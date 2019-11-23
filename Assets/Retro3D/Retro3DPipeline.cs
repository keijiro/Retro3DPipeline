// Retro3DPipeline
// A minimal example of a custom render pipeline with the Retro3D shader.
// https://github.com/keijiro/Retro3DPipeline

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Retro3D
{
    // Render pipeline runtime class
    public class Retro3DPipeline : RenderPipeline
    {
        // Temporary command buffer
        // Reused between frames to avoid GC allocation.
        // Rule: Clear commands right after calling ExecuteCommandBuffer.
        CommandBuffer _cb;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_cb != null)
            {
                _cb.Dispose();
                _cb = null;
            }
        }

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            //base.Render(context, cameras);

            // Lazy initialization of the temporary command buffer.
            if (_cb == null) _cb = new CommandBuffer();

            // Constants used in the camera render loop.
            var rtDesc = new RenderTextureDescriptor(256, 224, RenderTextureFormat.Default, 24);
            var rtID = Shader.PropertyToID("_LowResScreen");

            foreach (var camera in cameras)
            {
                // Set the camera up.
                context.SetupCameraProperties(camera);

                // Setup commands: Initialize the temporary render texture.
                _cb.name = "Setup";
                _cb.GetTemporaryRT(rtID, rtDesc);
                _cb.SetRenderTarget(rtID);
                _cb.ClearRenderTarget(true, true, camera.backgroundColor);
                context.ExecuteCommandBuffer(_cb);
                _cb.Clear();

                // Do basic culling.
                ScriptableCullingParameters scp;
                camera.TryGetCullingParameters(out scp);
                var culled = context.Cull(ref scp);
                
                //// Render visible objects that has "Base" light mode tag.
                var sorting = new SortingSettings(camera);
                var settings = new DrawingSettings(new ShaderTagId("Base"), sorting);
                var filter = FilteringSettings.defaultValue;
                context.DrawRenderers(culled, ref settings, ref filter);

                // Blit the render result to the camera destination.
                _cb.name = "Blit";
                _cb.Blit(rtID, BuiltinRenderTextureType.CameraTarget);
                context.ExecuteCommandBuffer(_cb);
                _cb.Clear();
                
                context.Submit();
            }

        }
    }
}
