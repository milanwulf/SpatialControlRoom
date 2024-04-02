using UnityEngine;
using UnityEngine.Rendering;

namespace Flexalon.Templates
{
    // Automatically adjusts the light depending on the render pipeline.
    [ExecuteAlways, AddComponentMenu("Flexalon Templates/Template Light Config")]
    public class TemplateLightConfig : MonoBehaviour
    {
        public float StandardIntensity = 3.14f;
        public float URPIntensity = 3.14f;
        public float HDRPIntensity = 20000f;

        void Update()
        {
            var light = GetComponent<Light>();
            if (light)
            {
                if (GraphicsSettings.renderPipelineAsset?.GetType().Name.Contains("HDRenderPipelineAsset") ?? false)
                {
                    light.intensity =  HDRPIntensity;
                }
                else if (GraphicsSettings.renderPipelineAsset?.GetType().Name.Contains("UniversalRenderPipelineAsset") ?? false)
                {
                    light.intensity = URPIntensity;
                }
                else
                {
                    light.intensity = StandardIntensity;
                }
            }
        }
    }
}