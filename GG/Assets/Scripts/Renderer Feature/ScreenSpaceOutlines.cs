using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static ScreenSpaceOutlines;

public class ScreenSpaceOutlines : ScriptableRendererFeature
{
    [System.Serializable]
    public class ViewSpaceNormalsTextureSettings
    {
        public RenderTextureFormat colorFormat;
        public int depthBufferBits;
        public FilterMode filterMode;
        public Color backgroundColor;
    }



    private class ViewSpaceNormalsTexturePass : ScriptableRenderPass
    {
        private ViewSpaceNormalsTextureSettings normalsTextureSettings;
        private readonly List<ShaderTagId> shaderTagIdList;
        private readonly RenderTargetHandle normals;
        private readonly Material normalsMaterial;
        private FilteringSettings filteringSettings;




        public ViewSpaceNormalsTexturePass(RenderPassEvent renderPassEvent, LayerMask outlinesLayerMask, ViewSpaceNormalsTextureSettings settings)
        {
            normalsMaterial = new Material(Shader.Find("Shader Graphs/ViewSpaceNormalsShader"));

            shaderTagIdList = new List<ShaderTagId>
            {
                new ShaderTagId("UniversalForward"),
                new ShaderTagId("UniversalForwardOnly"),
                new ShaderTagId("LightweightForward"),
                new ShaderTagId("SRPDefaultUnlit"),
            };

            this.renderPassEvent = renderPassEvent;
            normals.Init("_SceneViewSpaceNormals");
            filteringSettings = new FilteringSettings(RenderQueueRange.opaque, outlinesLayerMask);

            normalsTextureSettings = settings;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            base.Configure(cmd, cameraTextureDescriptor);

            //normals Texture Descriptor setup
            RenderTextureDescriptor normalsTextureDescriptor = cameraTextureDescriptor;
            normalsTextureDescriptor.colorFormat = normalsTextureSettings.colorFormat;
            normalsTextureDescriptor.depthBufferBits = normalsTextureSettings.depthBufferBits;
            cmd.GetTemporaryRT(normals.id, normalsTextureDescriptor, normalsTextureSettings.filterMode);

            ConfigureTarget(normals.Identifier());
            ConfigureClear(ClearFlag.All, normalsTextureSettings.backgroundColor);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!normalsMaterial)
                return;


            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler("SceneViewSpaceNormalsTextureCreation")))
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                DrawingSettings drawSettings = CreateDrawingSettings(shaderTagIdList, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
                drawSettings.overrideMaterial = normalsMaterial;
                context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filteringSettings);
            }
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);

        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            base.OnCameraCleanup(cmd);
            cmd.ReleaseTemporaryRT(normals.id);
        }

    }

 

    private class ScreenSpaceOutlinesPass : ScriptableRenderPass
    {
        //SSOP fields
        [SerializeField]
        private ViewSpaceNormalsTextureSettings viewSpaceNormalsTextureSettings;

        private readonly Material screenSpaceOutlineMaterial;
        private RenderTargetIdentifier cameraColorTarget;
        private RenderTargetIdentifier temporaryBuffer;
        private int temporaryBufferID = Shader.PropertyToID("_TemporaryBuffer");

        public ScreenSpaceOutlinesPass(RenderPassEvent renderPassEvent)
        {
            this.renderPassEvent = renderPassEvent;
            screenSpaceOutlineMaterial = new Material(Shader.Find("Shader Graphs/OutlineShader"));
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            cameraColorTarget = renderingData.cameraData.renderer.cameraColorTarget;
            temporaryBuffer = renderingData.cameraData.renderer.cameraColorTarget;

        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!screenSpaceOutlineMaterial)
                return;

            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler("ScreenSpaceOutlines")))
            {
                Blit(cmd, cameraColorTarget, temporaryBuffer, screenSpaceOutlineMaterial);
                Blit(cmd, temporaryBuffer, cameraColorTarget);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            base.OnCameraCleanup(cmd);
            cmd.ReleaseTemporaryRT(temporaryBufferID);
        }

    }



    #region ScreenSpaceOutlines variables and methods
    //variables
    [SerializeField]
    private RenderPassEvent renderPassEvent;
    [SerializeField]
    private LayerMask outlinesLayerMask;
    public ViewSpaceNormalsTextureSettings viewSpaceNormalsTextureSettings = new ViewSpaceNormalsTextureSettings();
    private ViewSpaceNormalsTexturePass viewSpaceNormalsTexturePass;
    private ScreenSpaceOutlinesPass screenSpaceOutlinesPass;

    public override void Create()
    {
        viewSpaceNormalsTexturePass = new ViewSpaceNormalsTexturePass(renderPassEvent, outlinesLayerMask, viewSpaceNormalsTextureSettings);
        screenSpaceOutlinesPass = new ScreenSpaceOutlinesPass(renderPassEvent);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(viewSpaceNormalsTexturePass);
        renderer.EnqueuePass(screenSpaceOutlinesPass);
    }

    #endregion

}
