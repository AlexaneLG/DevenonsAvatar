// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/AVProWindowsMedia/CompositeYVYU_2_RGBA" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_TextureWidth ("Texure Width", Float) = 256.0
	}
	SubShader 
	{
		Pass
		{ 
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
		
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma exclude_renderers flash xbox360 ps3 gles
//#pragma fragmentoption ARB_precision_hint_fastest
#pragma fragmentoption ARB_precision_hint_nicest
#pragma multi_compile SWAP_RED_BLUE_ON SWAP_RED_BLUE_OFF
#include "AVProWindowsMedia_Shared.cginc"
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
float _TextureWidth;
float4 _MainTex_ST;
float4 _MainTex_TexelSize;

struct v2f {
	float4 pos : POSITION;
	float3 uv : TEXCOORD0;
};

v2f vert( appdata_img v )
{
	v2f o;
	o.pos = UnityObjectToClipPos (v.vertex);
	o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
	
	// On D3D when AA is used, the main texture & scene depth texture
	// will come out in different vertical orientations.
	// So flip sampling of the texture when that is the case (main texture
	// texel size will have negative Y).
	#if SHADER_API_D3D9
	if (_MainTex_TexelSize.y < 0)
	{
		o.uv.y = 1-o.uv.y;
	}
	#endif
	
	o.uv.z = v.vertex.x * _TextureWidth * 0.5;

	return o;
}

float4 frag (v2f i) : COLOR
{	
	float4 col = tex2D(_MainTex, i.uv.xy);
#if defined(SWAP_RED_BLUE_ON)
	col = col.bgra;
#endif

	//yvyu
	float y = col.x;
	float u = col.w;
	float v = col.y;
	
	if (frac(i.uv.z) > 0.5 )
	{
		// ODD PIXELS
		y = col.z;
		
		/*
		float4 col2 = tex2D(_MainTex, uv.xy + float2(_MainTex_TexelSize.x, 0.0));
#if defined(SWAP_RED_BLUE_ON)
		col2 = col2.bgra;
#endif
		u = (col.w + col2.w) * 0.5;
		v = (col.y + col2.y) * 0.5;*/
	}

	float4 oCol = convertYUV(y, u, v);
	
	return oCol;
} 
ENDCG
		}
	}
	
	FallBack Off
}