Shader "Unlit/NewUnlitShader"
{
   Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0, 1, 1, 1)
        _OutlineWidth ("Outline Width", Range(0.001, 0.03)) = 0.005
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        // Outline Pass
        Pass
        {
            Name "OUTLINE"
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            v2f vert(appdata v)
            {
                v2f o;
                float3 norm = normalize(v.normal) * _OutlineWidth;
                v.vertex.xyz += norm;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }

        // Base Object Pass
        Pass
        {
            Name "BASE"
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _Color;  // ✅ Fix: Added return value for BASE pass
            }
            ENDCG
        }
    }
}

