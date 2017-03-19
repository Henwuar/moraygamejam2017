Shader "Unlit/Water"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MainColor("Colour", Color) = (0.0, 0.0, 1.0, 0.5)
		_Timer("Timer", Float) = 0
		_RippleHeight("Ripple Height", Float) = 1
		_RippleFrequency("Ripple Frequency", Float) = 10
	}
	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

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

			sampler2D _MainTex;
			float4 _MainColor;
			float _Timer;
			float _RippleFrequency;
			float _RippleHeight;
			
			v2f vert (appdata v)
			{
				v2f o;
				//apply an offset to make ripples
				float waveOffset = sin(_RippleFrequency*(v.vertex.x + _Timer*3.1415f)) + cos(_RippleFrequency*(v.vertex.z + _Timer*6.2830f));
				v.vertex.y += _RippleHeight*(waveOffset);

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				col *= _MainColor;
				return col;
			}
			ENDCG
		}
	}
}
