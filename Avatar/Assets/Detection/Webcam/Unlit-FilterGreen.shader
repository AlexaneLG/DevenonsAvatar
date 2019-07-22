// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/FilterGreen" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Taper ("Taper", Range (-1, 1)) = 0
    _ClampX ("ClampX", float) = 0
	_ClampYup ("ClampYup", float) = 0
	_ClampYdown ("ClampYdown", float) = 0
	   
}

SubShader {
	Tags {"Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 300
	Cull Off
	ZWrite Off
	ZTest  Always
	Blend SrcAlpha OneMinusSrcAlpha 
	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Taper;
			float _ClampX,_ClampYup,_ClampYdown;

			v2f vert (appdata_t v)
			{
				v2f o;

				v.vertex.x *=  1-_Taper*(v.vertex.z+2)/4;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			fixed4 _TransparencyColor;
			//Float _TransparencyHue = 0.7;
			
			fixed4 frag (v2f IN) : COLOR
			{
				float4  tempColor = tex2D(_MainTex, IN.texcoord);
				
				

				// Calculate the average intensity of the texel's red and blue components
				float rbAverage = tempColor.r * 0.5 + tempColor.b * 0.5;

				// Calculate the difference between the green element intensity and the
				// average of red and blue intensities
				float gDelta = tempColor.g - rbAverage;

				// If the green intensity is greater than the average of red and blue
				// intensities, calculate a transparency value in the range 0.0 to 1.0
				// based on how much more intense the green element is
				tempColor.a = 1.0 - smoothstep(0.0, 0.25, gDelta);

				// Use the cube of the transparency value. That way, a fragment that
				// is partially translucent becomes even more translucent. This sharpens
				// the final result by avoiding almost but not quite opaque fragments that
				// tend to form halos at color boundaries.
				tempColor.a = tempColor.a * tempColor.a * tempColor.a;
				
				if( IN.texcoord.x < _ClampX)
					tempColor.a = 0;

				if( IN.texcoord.x > 1-_ClampX)
					tempColor.a = 0;

				if( IN.texcoord.y < _ClampYup)
					tempColor.a = 0;

				if( IN.texcoord.y > 1-_ClampYdown)
					tempColor.a = 0;

				return tempColor;

				
			}
		ENDCG
	}
}

}
