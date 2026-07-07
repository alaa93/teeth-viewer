Shader "TeethViewer/Tooth"
{
    Properties
    {
        _BaseColor ("Enamel Color", Color) = (0.93, 0.91, 0.85, 1)
        _SelectionColor ("Selection Rim Color", Color) = (0.25, 0.75, 1.0, 1)
        _RimPower ("Rim Power", Range(0.5, 8)) = 2.5
        _Smoothness ("Smoothness", Range(0, 1)) = 0.55
        [HideInInspector] _Selected ("Selected", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS   : TEXCOORD0;
                float3 viewDirWS  : TEXCOORD1;
                float3 positionWS : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _SelectionColor;
                float _RimPower;
                float _Smoothness;
            CBUFFER_END

            float _Selected;

            Varyings Vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs positions = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionCS = positions.positionCS;
                OUT.positionWS = positions.positionWS;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.viewDirWS = GetWorldSpaceViewDir(positions.positionWS);
                return OUT;
            }

            half4 Frag(Varyings IN) : SV_Target
            {
                float3 normalWS = normalize(IN.normalWS);
                float3 viewDirWS = normalize(IN.viewDirWS);

                Light mainLight = GetMainLight();

                float ndotl = dot(normalWS, mainLight.direction);
                float halfLambert = ndotl * 0.5 + 0.5;
                halfLambert *= halfLambert;

                float3 halfVec = normalize(mainLight.direction + viewDirWS);
                float ndoth = saturate(dot(normalWS, halfVec));
                float specular = pow(ndoth, lerp(4, 64, _Smoothness)) * _Smoothness;

                float3 lit = _BaseColor.rgb * (halfLambert * mainLight.color * mainLight.shadowAttenuation + 0.12)
                             + specular * mainLight.color;

                float fresnel = pow(1.0 - saturate(dot(normalWS, viewDirWS)), _RimPower);

                float selectMask = saturate(_Selected) * saturate(0.4 + 0.6 * fresnel);
                float3 finalColor = lerp(lit, _SelectionColor.rgb, selectMask);

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}
