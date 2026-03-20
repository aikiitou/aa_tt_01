using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EmissionFeature : ScriptableRendererFeature
{
    class EmissionPass : ScriptableRenderPass
    {
        private FilteringSettings filtering;
        private ShaderTagId shaderTag;
        private RTHandle emissionHandle;

        public EmissionPass()
        {
            // Opaqueのみ対象（必要に応じて変更）
            filtering = new FilteringSettings(RenderQueueRange.opaque);
            shaderTag = new ShaderTagId("UniversalForward");
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor desc)
        {
            desc.depthBufferBits = 0;

            RenderingUtils.ReAllocateIfNeeded(
                ref emissionHandle,
                desc,
                name: "_EmissionTex"
            );

            // ★ URP推奨：Configureでターゲット指定
            ConfigureTarget(emissionHandle);
            ConfigureClear(ClearFlag.All, Color.clear);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get("Emission Pass");

            // コマンドを一度反映
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            var sortFlags = SortingCriteria.CommonOpaque;

            var drawingSettings = CreateDrawingSettings(
                shaderTag,
                ref renderingData,
                sortFlags
            );

            var rendererListParams = new RendererListParams(
                renderingData.cullResults,
                drawingSettings,
                filtering
            );

            var rendererList = context.CreateRendererList(ref rendererListParams);

            cmd.DrawRendererList(rendererList);
            cmd.SetGlobalTexture("_EmissionTex", emissionHandle.nameID);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public void Dispose()
        {
            emissionHandle?.Release();
        }
    }

    EmissionPass pass;

    public override void Create()
    {
        pass = new EmissionPass();
        pass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }

    protected override void Dispose(bool disposing)
    {
        pass?.Dispose();
    }
}