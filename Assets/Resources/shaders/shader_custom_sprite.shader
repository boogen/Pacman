Shader "Custom/shader_custom_sprite"
{
	Properties
	{ 
		_MainTex ("Sprite Texture", 2D) = "white" {} 
		_Color ("Color", Color) = (1,1,1,1) 
	}

	SubShader
	{
		Tags { "Queue"="Transparent+300" "IgnoreProjector"="True" "RenderType"="Transparent" } 
		Lighting Off
		Cull Off
		ZWrite Off
		Fog { Mode Off } 
		Blend SrcAlpha OneMinusSrcAlpha 
		Pass
		{ 
			SetTexture [_MainTex]
			{ 
				constantColor [_Color]
//				combine texture * constant, texture
//				combine constant, texture
				combine constant, constant * texture
			} 
		}
	}
}
