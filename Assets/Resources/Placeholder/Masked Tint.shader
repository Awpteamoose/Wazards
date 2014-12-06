Shader "Custom/Masked Tint" {

	Properties
	{
		_Color ("Tint Color", Color) = (1,1,1)
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_MaskTex ("Texture  (A = Tint Mask)", 2D) = ""
	}

	SubShader
	{

		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};

			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _MaskTex;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 tint = tex2D(_MaskTex, IN.texcoord).a * _Color;
				tint.rgb *= tint.a;

				fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
				c.rgb *= c.a;
				c += tint;
				return c;
			}
		ENDCG
		}
	}
}
