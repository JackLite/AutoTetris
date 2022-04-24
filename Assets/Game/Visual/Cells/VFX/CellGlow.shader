Shader "Goldstein/CellGlow"
{
    Properties
    {
        [PerRenderedData][HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        _Glow ("Intensity", Range(1, 10)) = 1
    }
    SubShader
    {
        // No culling or depth
        Tags
        {
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Blend One SrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            float     _Glow;
            sampler2D _MainTex;


            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = i.color * tex2D(_MainTex, i.uv);
                col.rgb *= _Glow;
                col.rgb *= col.a;
                return col;
            }
            ENDHLSL
        }
    }
}