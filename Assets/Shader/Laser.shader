Shader "Unlit/Laser"
{
    Properties
    {
        _LaserWidth("Laser Width",Float) = 1 
        _Color("Color",Color) = (1,1,1,1)
        _NoiseScale("Noise Scale",Float) = 1
       
        _ClmapMin("Clmap Min",Range(0,1)) = 0
        _SmoothStepMax("SmoothStep Max",Range(0,1)) = 1
        _LaserSpeed("Laser Speed",Float) = 1
    }
    SubShader
    {
        Tags 
        {
            "Queue" = "Transparent" 
            "IgnoreProjector" = "True"
            "RenderPipeline" = "UniversalPipeline"
            "RenderType"="Transparent" 
        }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                
                float4 vertex : SV_POSITION;
            };
            half _LaserWidth;
            half4 _Color;
            half _NoiseScale;
            half _ClmapMin;
            half _SmoothStepMax;
            half _LaserSpeed;
            float2 noisePos(float2 p) {
                p *= _NoiseScale;
                return p;
            }
            
            float2 random(float2 p){
                return  -1.0 + 2.0 * frac(
                    sin(
                        float2(
                            dot(p, float2(127.1,311.7)),
                            dot(p, float2(269.5,183.3))
                        )
                    ) * 43758.5453
                );
            }
            float noise_perlin (float2 p) {
                float2 i = floor(p); // 获取当前网格索引i
                float2 f = frac(p); // 获取当前片元在网格内的相对位置
                // 计算梯度贡献值
                float a = dot(random(i),f); // 梯度向量与距离向量点积运算
                float b = dot(random(i + float2(1., 0.)),f - float2(1., 0.));
                float c = dot(random(i + float2(0., 1.)),f - float2(0., 1.));
                float d = dot(random(i + float2(1., 1.)),f - float2(1., 1.));
                // 平滑插值
                float2 u = smoothstep(0.,1.,f);
                // 叠加四个梯度贡献值
                return smoothstep(0,_SmoothStepMax,clamp(lerp(lerp(a,b,u.x),lerp(c,d,u.x),u.y),_ClmapMin,2));
                
            }
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = v.uv;
               
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half laserWidth = -100/_LaserWidth;
                half noise = noise_perlin(noisePos(half2(i.uv.y,i.uv.y)+ _Time.y*_LaserSpeed)) ;
                half4 col;
                col.a = exp(abs(i.uv.y-0.5f)*laserWidth) * noise;
                //col.a = 1;
                col.rgb = _Color.rgb * 1.25;
               // col.rgb = noise;
                return col;
            }
            ENDHLSL
        }
    }
}
