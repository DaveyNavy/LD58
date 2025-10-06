// Unity Shader for a Blurry "Censor" Pixelation Effect
Shader "Custom/CensorBlurShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        // A larger number means bigger, blurrier blocks
        _BlockSize ("Block Size", Range(1, 128)) = 16
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "IgnoreProjector"="True"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                float4 color        : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float4 color        : COLOR;
            };
            
            // --- We now use TEXTURE2D and SAMPLER macros for tex2Dlod ---
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _Color;
            float _BlockSize;


            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            // --- Fragment (Pixel) Shader ---
            half4 frag(Varyings IN) : SV_Target
            {
                // 1. Calculate the UV coordinates for the center of the block
                float2 blockCenterUV = (floor(IN.uv * _BlockSize) + 0.5) / _BlockSize;
    
                // 2. Determine which Mipmap level to sample from
                float mipLevel = log2(_BlockSize);

                // 3. Sample the texture using the specified mip level
                half4 blurredColor = SAMPLE_TEXTURE2D_LOD(_MainTex, sampler_MainTex, blockCenterUV, mipLevel);

                // --- LOGIC CHANGE IS HERE ---
                // Instead of tinting the original color, we tint the new blurred color
                blurredColor *= IN.color * _Color; 
    
                // Return the blurred color directly. Its alpha will define the shape.
                return blurredColor; 
            }
            ENDHLSL
        }
    }
}