Shader "Unlit/UIWaveEffect"
{

    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BrightColorDepth  ("brightColorDepth",  Range(0.1, 1.0))  =  1.0 
        _DarkColorDepth  ("darkColorDepth",  Range(0.1, 1.0))  =  1.0 
        _DarkIntensity  ("DarkIntensity",  Range(0.1, 1.0))  =  1.0  
        _BrightIntensity  ("brightIntensity",  Range(0.1, 1.0))  =  1.0  
        _HpPercent ("hpPercent", Range(0.0, 1.0)) = 1.0 
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
            #define TAU 6.28318530718
            #define MAX_ITER 5


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
            float  _BrightColorDepth;
            float  _DarkColorDepth;
            float _DarkIntensity;
            float _BrightIntensity;
            float _HpPercent; 

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
                float time = _Time.y * .5 + 23.0;
                time  *=  0.4;  
                float2 uv = i.uv;
                float2 p = fmod(uv * TAU, TAU) - 250.0;  
                float2 ip = p;   
                float c = 1.0;  
                float inten = .005;

                for (int n = 0; n < MAX_ITER; n++)
                {
                    float t = time * (1.0 - (3.5 / float(n + 1)));
                    ip = p + float2(cos(t - ip.x) + sin(t + ip.y), sin(t - ip.y) + cos(t + ip.x));
                    c += 1.0 / length(float2(p.x / (sin(ip.x + t) / inten), p.y / (cos(ip.y + t) / inten)));
                }

                c /= float(MAX_ITER);
                c = 1.17 - pow(c, 1.4);  
                float3 colour = pow(abs(c), 8.0);
    
                if (i.uv.x > _HpPercent)
                {
                    if (_HpPercent > 0.0f && i.uv.x - _HpPercent <= 0.1) 
                    { 
                         // 0 ~ 1 - 1 = Dark 0 = Bright
                                float rate = saturate((i.uv.x - _HpPercent) / 0.1);
            
                                colour = (clamp(colour + float3(_DarkColorDepth, 0.0, 0.0), 0.0, 1.0) * rate)
                                + (clamp(colour + float3(_BrightColorDepth, 0.0, 0.0), 0.0, 1.0) * (1.0 - rate));
            
                                colour *= (_BrightIntensity * (1.0 - rate)) + (_DarkIntensity * rate);
             
                    } 
                    else
                    {
                        colour = clamp(colour + float3(_DarkColorDepth, 0.0, 0.0), 0.0, 1.0);
                        colour *= _DarkIntensity;
                    } 
                }
                else
                {
                        colour = clamp(colour + float3(_BrightColorDepth, 0.0, 0.0), 0.0, 1.0);
                        colour *= _BrightIntensity;
                 }
    

                return fixed4(colour, 1.0);
            }
            ENDCG 
        }
    }
}
