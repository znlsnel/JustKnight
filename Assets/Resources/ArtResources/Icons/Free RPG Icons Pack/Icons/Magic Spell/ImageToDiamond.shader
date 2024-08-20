Shader "Unlit/ImageToDiamond"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
       _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
float4 _Color; 

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            { 
    
                float4 col = tex2D(_MainTex, i.uv); 
    UNITY_APPLY_FOG(i.fogCoord, col);
    float2 uv = i.uv - 0.5; // Center UV coordinates
                uv.x *= 2.0; // Adjust aspect ratio if necessary
                uv.y *= 2.0;
                float dist = abs(uv.x) + abs(uv.y); // Diamond shape equation
                if (dist > 1.0)
                    clip(-1.0); // Discard pixels outside the diamond
    
    col *= _Color;
                return col;

}
            ENDCG
        }
    }
}
