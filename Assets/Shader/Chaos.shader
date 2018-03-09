Shader "Custom/Chaos" 
{
	Properties 
	{
	    _Color ("Main Color", Color) = (1,1,1,1)
	    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_Player1_Rad ("_Player1_Rad", Float) = 1.0
		_Player2_Rad("_Player2_Rad", Float) = 1.0
		_LightMaxRadius("LightMaxRadius", Float) = 0.5
		_Player1_Pos ("_Player1_Pos", Vector) = (0,0,0,1)
		_Player2_Pos ("_Player2_Pos", Vector) = (0,0,0,1)
	}

	SubShader 
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert alpha:blend

		sampler2D _MainTex;
		fixed4	_Color;
		float	_Player1_Rad;
		float	_Player2_Rad;
		float	_LightMaxRadius;
		float4	_Player1_Pos;
		float4	_Player2_Pos;

		struct Input 
		{
			float2 uv_MainTex;
			float2 location;
		};

		float powerForPos(float4 pos, float2 nearVertex, float radius);

		void vert(inout appdata_full vertexData, out Input outData) 
		{
			float4 pos = UnityObjectToClipPos(vertexData.vertex);
			float4 posWorld = mul(unity_ObjectToWorld, vertexData.vertex);
			outData.uv_MainTex = vertexData.texcoord;
			outData.location = posWorld.xy;
		}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 baseColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			float alpha = (1.0 - (baseColor.a + powerForPos(_Player1_Pos, IN.location, _Player1_Rad) + powerForPos(_Player2_Pos, IN.location, _Player2_Rad)));

			o.Albedo = baseColor.rgb;
			o.Alpha = alpha;
		}

		// return 0 if (pos - nearVertex) > _FogRadius
		float powerForPos(float4 pos, float2 nearVertex, float radius) 
		{
			float atten = clamp(radius - length(pos.xy - nearVertex.xy), 0.0, radius);

			return (1.0 / _LightMaxRadius) *  atten / radius;
		}

		ENDCG
	}

	Fallback "Legacy Shaders/Transparent/VertexLit"
}