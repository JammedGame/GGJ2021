Shader "demo/water_shader"
{
	Properties
	{
		_Normal("Normal", 2D) = "white" {}
		_NormalIntensity1("Normal Intensity 1", Range( 0 , 1)) = 1
		_NormalIntensity2("Normal Intensity 2", Range( 0 , 1)) = 1
		[NoScaleOffset]_Cubemap("Cubemap", CUBE) = "black" {}

		_Tile1("Tile 1", float) = 1
		_Tile2("Tile 2", float) = 1
		_WavesSpeed1("WaveSpeed 1", float) = 1
		_WavesSpeed2("WaveSpeed 2", float) = 1

		_DepthGradientShallow("Depth Gradient Shallow", Color) = (1,1,1,1)
		_DepthGradientDeep("Depth Gradient Deep", Color) = (1,1,1,1)
		_ReflectionStrength("Reflection Strentgh", float) = 1

		_AlphaStart("Alpha Start", float) = 1
		_AlphaMaxDistance("Alpha Max Distance", float) = 1

		_FoamStrength("Foam Strength", float) = 1
		_FoamMaxDepth("Foam Max Depth", float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Overlay" "Queue"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZTest LEqual
		ZWrite Off

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			/* Declare uniforms*/
			uniform float4 _CausticsScale1;
			uniform float4 _CausticsScale2;

			uniform float4 _DepthGradientShallow;
			uniform float4 _DepthGradientDeep;
			uniform float _ReflectionStrength;

			uniform float _AlphaMaxDistance;
			uniform float _AlphaStart;

			uniform float _FoamStrength;
			uniform float _FoamMaxDepth;

			uniform float _Tile1;
			uniform float _Tile2;

			uniform float _WavesSpeed1;
			uniform float _WavesSpeed2;

			uniform float _NormalIntensity1;
			uniform float _NormalIntensity2;

			sampler2D_float _Normal;
			sampler2D_float _CameraDepthTexture;

			uniform samplerCUBE _Cubemap;

			uniform float _WaterCausticsScale;
			uniform float _WaterCausticsBigScale;

			/* Structures */
			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD0;
				float4 screenPos : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
			};

			/* Vertex shader */
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.viewDir = _WorldSpaceCameraPos - o.worldPos;
				return o;
			}

			/* Fragment shader */
			half4 frag (v2f IN) : SV_Target
			{
				// screen coordinates.
			    float2 screenPos = IN.screenPos.xy / IN.screenPos.w;
				screenPos.y = screenPos.y;

				/* get real depth */
				float eyeDepth = LinearEyeDepth(tex2D(_CameraDepthTexture, screenPos));
				float depth = eyeDepth - IN.screenPos.w;

				/* alpha per depth */
				half alpha_depth = saturate(depth / _AlphaMaxDistance);
				alpha_depth = smoothstep(0, 1, alpha_depth);
				half alpha = lerp(_AlphaStart, 1, alpha_depth);

				/* color per depth */
				half foam = smoothstep(1, 0, saturate(depth / _FoamMaxDepth)) * _FoamStrength;
				foam = smoothstep(0, 1, foam);
				foam = saturate(foam);
				//return float4(depth, depth, depth, 1);

				/* reflection */
				float2 uv2_TexCoord10 = IN.worldPos.xz / _Tile1;
				float2 panner7 = ( 1.0 * _Time.y * _WavesSpeed1 + uv2_TexCoord10);
				float2 uv_TexCoord15 = IN.worldPos.xz / _Tile2;
				float2 panner16 = ( 1.0 * _Time.y * _WavesSpeed2 + uv_TexCoord15);
				float3 waterNormal = float4(BlendNormals(UnpackScaleNormal( tex2D( _Normal, panner7 ), _NormalIntensity1), UnpackScaleNormal( tex2D( _Normal, panner16 ), _NormalIntensity2)), 0);

				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( IN.worldPos ) );
				half3 inputRay = -worldViewDir;

				float3 reflectRay = reflect(waterNormal, inputRay);
				float3 reflection = texCUBE( _Cubemap, reflectRay).rgb * _ReflectionStrength;
				reflection = smoothstep(0, 1, reflection);
				reflection = reflection < 0.5 ? 0 : 0.05;

				half3 deepColor = saturate(_DepthGradientDeep.rgb);
				half3 shallowColor = lerp(_DepthGradientDeep.rgb, _DepthGradientShallow.rgb, _DepthGradientShallow.a);
				half3 color = lerp(deepColor, shallowColor, foam) + reflection * (1 - foam);

				return half4(color, alpha);
			}

			ENDCG
		}
	}
}