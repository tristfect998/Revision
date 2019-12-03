Shader "Custom/NewSurfaceShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_LavaTexture ("texture de lave", 2D) = "white" {}
		_RockTexture("texture de roche", 2D) = "white" {}
		_SpeedScroll("Scroll speed", Range(1,100)) = 15
		_DisplacementFactor("Facteur de déplacement", Range(0,1))= 0.5
		_DispTexture("Texture de déplacement", 2D) = "black" {}
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows vertex:disp

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _LavaTexture;
			sampler2D _RockTexture;

		struct Input {
			float2 uv_LavaTexture;
			float2 uv_RockTexture;
			float2 uv_DispTexture;
		};

		struct appdata {
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
		};

		sampler2D _DispTexture;
		float _DisplacementFactor;

		void disp(inout appdata v) {
			float splat = tex2Dlod(_DispTexture, float4(v.texcoord.xy, 0, 0)).r * _DisplacementFactor;
			v.vertex.xyz -= v.normal * splat;
			v.vertex.xyz += v.normal * _DisplacementFactor;
		}

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		half _SpeedScroll;


		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed2 scrolledUV = IN.uv_LavaTexture;
			fixed2 scrolledUVDisp = IN.uv_DispTexture;
			fixed scrollValue = _Time * _SpeedScroll;
			scrolledUV += fixed2(scrollValue, scrollValue);
			scrollValue = _Time * _SpeedScroll / 3;
			fixed2 scrolledUV2 = IN.uv_LavaTexture + fixed2(scrollValue, scrollValue);
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_LavaTexture, scrolledUV);
			fixed4 d = tex2D(_RockTexture, scrolledUV);
			o.Albedo = c.rgb - d.rgb * 2.5;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
