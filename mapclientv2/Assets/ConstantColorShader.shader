Shader "Custom/ConstantColorShader" {
	Properties {
		//_MainTex ("Base (RGB)", 2D) = "white" {}
		//_texture("map view", 2D) = "ttt"
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass{
			CGPROGRAM
			#pragma vertex vertex_shader
			#pragma fragment fragment_shader
			#include "UnityCG.cginc"
			
			struct v2f
			{
				float4 pos:POSITION;
				float4 color:COLOR;
			};
			
			struct output
			{
				float4 color:COLOR;
			};
			
			v2f vertex_shader(float4 position:POSITION, float4 color:COLOR)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP,position);
				o.color = color;
				return o;
			}
			
			float4 fragment_shader(v2f i):COLOR
			{
				return i.color;
			}
			
			ENDCG
		}
		
		
	} 
	//FallBack "Diffuse"
}
