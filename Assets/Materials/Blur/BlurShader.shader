Shader "Custom/UnlitTexture"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _GridSize("Size of Blur Grid",float) = 1
        _BlurAmount("Amount of Blur",Range(0,1)) = 0.5
    }

    // Universal Render Pipeline subshader. If URP is installed this will be used.
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalRenderPipeline"}

        Pass
        {
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                float4 positionHCS  : SV_POSITION;
            };

            //TEXTURE2D(_BaseMap);
            //SAMPLER(sampler_BaseMap);
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            CBUFFER_START(UnityPerMaterial)
            // float4 _BaseMap_ST;
            //half4 _BaseColor;

            float _GridSize;
            float _BlurAmount;

            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;//Get size of Texture;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                //return SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                float3 col = float3(0,0,0);
                
                int min =  -round(_GridSize/2);
                int max = round(_GridSize/2);

                float count = (max - min + 1) * (max - min + 1);
                for(int y = min; y<=max; y++){
                    for(int x = min; x<=max; x++){
                        float2 uv = IN.uv + float2(_MainTex_TexelSize.x * x , _MainTex_TexelSize.y * y);
                        col += SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,uv).xyz;
                        
                    }
                }
                col /= count;

                float3 originCol = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,IN.uv).xyz;
                
                float3 finalCol = lerp(originCol,col,_BlurAmount);

                return float4(finalCol,0);
            }
            ENDHLSL
        }
    }
}