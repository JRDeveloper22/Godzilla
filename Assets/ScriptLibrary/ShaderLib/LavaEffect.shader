Shader "WeiShaderLib/LavaEffect" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
		_FlowMap("Flow Map", 2D) = "grey" {}
		_Extrude("Extrude", Range(-1, 1)) = 0.2
		_Speed("Speed", Range(0.001, 1.0)) = 0.25
	}
	SubShader{
		Pass{
			Tags{ "RenderType" = "Opaque" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				fixed2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			sampler2D _FlowMap;
			fixed _Extrude;
			fixed _Speed;

			fixed4 _MainTex_ST;

			v2f vert(appdata_base IN) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, IN.vertex);
				o.uv = TRANSFORM_TEX(IN.texcoord, _MainTex); //detail see Pixelation shader
				return o;
			}

			fixed4 frag(v2f v) : COLOR{
				fixed4 c;
				//get and uncompress the flow vector for this pixel
				
				half3 flowVal = (tex2D(_FlowMap, v.uv) * 2 - 1) * _Extrude; //see Note 1.1

				float timeOffset1 = frac(_Time.y * _Speed + 0.5);
				float timeOffset2 = frac(_Time.y * _Speed);

				half4 col1 = tex2D(_MainTex, v.uv - flowVal.xy * timeOffset1); 
				half4 col2 = tex2D(_MainTex, v.uv - flowVal.xy * timeOffset2);

				half lerpVal = abs((timeOffset1 - 0.5) * 2);
				c = lerp(col1, col2, lerpVal);

				return c;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
