Shader "Custom/OutlineFillMap" {
    Properties {
        _OutlineColor ("Outline Color", Color) = (0,0,1,1) // 파란색으로 설정
        _EmissionColor ("Emission Color", Color) = (0,0,1,1) // 발광 색상
        _Opacity ("Opacity", Range(0, 1)) = 0 // 불투명하게 설정
    }
    SubShader {
        Tags { "RenderType"="Transparent" }
        LOD 200

        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
            };

            fixed4 _OutlineColor;
            fixed4 _EmissionColor;
            float _Opacity;

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // 윗면 감지: 법선 벡터가 Y축 위쪽(0.7 이상의 값)을 향하는 경우에만 적용
                float isTopFace = step(0.7, i.worldNormal.y); // 감지 범위를 완화하여 윗면 인식 범위 확장
                fixed4 col = fixed4(0, 0, 0, 0); // 기본 색상은 투명

                // 윗면일 경우 색상 적용
                if (isTopFace > 0.0) {
                    col.rgb = _OutlineColor.rgb; // 윗면에 컬러 적용
                    col.rgb += _EmissionColor.rgb * _Opacity; // 윗면에 발광 효과 추가
                    col.a = _Opacity; // 윗면의 불투명도 설정
                }

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
