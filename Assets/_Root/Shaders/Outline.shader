Shader "Custom/Outline"
{
    Properties 
    {
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 0

        _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth("Outline Width", Range(0, 0.2)) = 0.1
    }
    
    SubShader 
    {
        Pass
        {
            Tags 
            {
                "Queue" = "Transparent+100"
                "RenderType" = "Transparent"
            }
            
            Cull Off
            ZTest [_ZTest]
            ZWrite Off
            ColorMask 0

            Stencil 
            {
                Ref 1
                Pass Replace
            }
        }
        
        Pass 
        {
            Tags 
            {
                "Queue" = "Transparent+110"
                "RenderType" = "Transparent"
                "DisableBatching" = "True"
            }
            
            Name "Fill"
            Cull Off
            ZTest [_ZTest]
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB
            
            Stencil 
            {
                Ref 1
                Comp NotEqual
            }

            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            struct appdata 
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float3 smoothNormal : TEXCOORD3;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f 
            {
                float4 position : SV_POSITION;
                fixed4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            uniform fixed4 _OutlineColor;
            uniform float _OutlineWidth;

            float4 outline(float4 vertexPos, float outlineValue)
            {
                float4x4 scale = float4x4
                (
                    1 + outlineValue, 0, 0, 0,
                    0, 1 + outlineValue, 0, 0,
                    0, 0, 1 + outlineValue, 0,
                    0, 0, 0, 1 + outlineValue
                );

                return mul(scale, vertexPos);
            }

            v2f vert(appdata input) 
            {
                v2f output;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                float3 normal = any(input.smoothNormal) ? input.smoothNormal : input.normal;
                float3 viewPosition = UnityObjectToViewPos(input.vertex);
                float3 viewNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, normal));

                float4 vertexPos = outline(input.vertex, _OutlineWidth);
                output.position = UnityObjectToClipPos(vertexPos);
                output.color = _OutlineColor;

                return output;
            }

            fixed4 frag(v2f input) : SV_Target 
            {
                return input.color;
            }
            
            ENDCG
        }
    }
}
