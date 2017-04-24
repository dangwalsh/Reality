Shader "Gensler/unlit_wireframe" {

	Properties
	{
		_Color("Color", Color) = (0,0,0,0)
		_EdgeColor("Edge Color", Color) = (0,1,0,1)
		_Width("Width", float) = 0.1
	}
		SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }

		Pass
	{
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Front
		AlphaTest Greater 0.5 //try removing this

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		uniform float4 _Color;
	uniform float4 _EdgeColor;
	uniform float _Width;

	struct appdata
	{
		float4 vertex : POSITION;
		float4 texcoord1 : TEXCOORD0;
		float4 color : COLOR;
	};

	struct v2f
	{
		float4 pos : POSITION;
		float4 texcoord1 : TEXCOORD0;
		float4 color : COLOR;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.texcoord1 = v.texcoord1;
		o.color = v.color;
		return o;
	}

	fixed4 frag(v2f i) : COLOR
	{
		fixed4 answer;

	float lx = step(_Width, i.texcoord1.x);
	float ly = step(_Width, i.texcoord1.y);
	float hx = step(i.texcoord1.x, 1.0 - _Width);
	float hy = step(i.texcoord1.y, 1.0 - _Width);

	answer = lerp(_EdgeColor, _Color, lx*ly*hx*hy);

	return answer;
	}
		ENDCG
	}

		Pass
	{
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Back
		AlphaTest Greater 0.5 //try removing this

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		uniform float4 _Color;
	uniform float4 _EdgeColor;
	uniform float _Width;

	struct appdata
	{
		float4 vertex : POSITION;
		float4 texcoord1 : TEXCOORD0;
		float4 color : COLOR;
	};

	struct v2f
	{
		float4 pos : POSITION;
		float4 texcoord1 : TEXCOORD0;
		float4 color : COLOR;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.texcoord1 = v.texcoord1;
		o.color = v.color;
		return o;
	}

	fixed4 frag(v2f i) : COLOR
	{
		fixed4 answer;

	float lx = step(_Width, i.texcoord1.x);
	float ly = step(_Width, i.texcoord1.y);
	float hx = step(i.texcoord1.x, 1.0 - _Width);
	float hy = step(i.texcoord1.y, 1.0 - _Width);

	answer = lerp(_EdgeColor, _Color, lx*ly*hx*hy);

	return answer;
	}
		ENDCG
	}

	}
		Fallback "Vertex Colored", 1
}
