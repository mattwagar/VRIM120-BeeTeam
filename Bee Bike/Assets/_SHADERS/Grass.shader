Shader "Roystan/Grass"
{
    Properties
    {
		[Header(Shading)]
        _TopColor("Top Color", Color) = (1,1,1,1)
		_BottomColor("Bottom Color", Color) = (1,1,1,1)
		_TranslucentGain("Translucent Gain", Range(0,1)) = 0.5
		_BendRotationRandom("Bend Rotation Random", Range(0, 1)) = 0.2 
		_BladeWidth("Blade Width", Float) = 0.05
        _BladeWidthRandom("Blade Width Random", Float) = 0.02
        _BladeHeight("Blade Height", Float) = 0.5
        _BladeHeightRandom("Blade Height Random", Float) = 0.3
		_TessellationUniform("Tessellation Uniform", Range(1, 64)) = 1 
		_WindDistortionMap("Wind Distortion Map", 2D) = "white" {}
        _WindFrequency("Wind Frequency", Vector) = (0.05, 0.05, 0, 0) 
		_WindStrength("Wind Strength", Float) = 1

    }

	CGINCLUDE
	#include "UnityCG.cginc"
	#include "Autolight.cginc" 
	#include "CustomTessellation.cginc"

	//inside CGINCLUDE block 
	float _BendRotationRandom;
	float _BladeHeight;
    float _BladeHeightRandom;	
    float _BladeWidth;
    float _BladeWidthRandom;
	sampler2D _WindDistortionMap; 
	float4 _WindDistortionMap_ST; 
	float2 _WindFrequency;
	float _WindStrength;

	struct geometryOutput 
	{ 
		float4 pos : SV_POSITION;
		float2 uv: TEXCOORD0;
	};

	/*struct vertexInput 
	{
		float4 vertex : POSITION; 
		float3 normal : NORMAL; 
		float4 tangent : TANGENT; 
	}; 

	struct vertexOutput 
	{ 
		float4 vertex : SV_POSITION; 
		float3 normal : NORMAL;
		float4 tangent : TANGENT;
	}; */


	geometryOutput VertexOutput(float3 pos, float2 uv)
	{
		geometryOutput o;
		o.pos = UnityObjectToClipPos(pos);
		o.uv = uv;
		return o;
	} 

	float rand(float3 co)
	{
		return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 53.539))) * 43758.5453);
	} 

	float3x3 AngleAxis3x3(float angle, float3 axis)
	{
		float c, s;
		sincos(angle, s, c);

		float t = 1 - c;
		float x = axis.x;
		float y = axis.y;
		float z = axis.z;

		return float3x3(
			t * x * x + c, t * x * y - s * z, t * x * z + s * y,
			t * x * y + s * z, t * y * y + c, t * y * z - s * x,
			t * x * z - s * y, t * y * z + s * x, t * z * z + c
			);
	} 


	[maxvertexcount(3)]
	void geo(point vertexOutput IN[1], inout TriangleStream<geometryOutput> triStream)
	{
		float3 pos = IN[0].vertex;

		float3 vNormal = IN[0].normal;
        float4 vTangent = IN[0].tangent;
        float3 vBinormal = cross(vNormal, vTangent) * vTangent.w;
		float2 uv = pos.xz * _WindDistortionMap_ST.xy + _WindDistortionMap_ST.zw + _WindFrequency * _Time.y;
		float2 windSample = (tex2Dlod(_WindDistortionMap, float4(uv, 0, 0)).xy * 2 - 1) * _WindStrength; 
		float3 wind = normalize(float3(windSample.x, windSample.y, 0)); 
		float3x3 windRotation = AngleAxis3x3(UNITY_PI * windSample, wind);


		float3x3 tangentToLocal = float3x3( 
           vTangent.x, vBinormal.x, vNormal.x,
	       vTangent.y, vBinormal.y, vNormal.y,
	       vTangent.z, vBinormal.z, vNormal.z
		); 
		

	    float3x3 facingRotationMatrix = AngleAxis3x3(rand(pos) * UNITY_TWO_PI, float3(0, 0, 1)); 
		float3x3 bendRotationMatrix = AngleAxis3x3(rand(pos.yyx) * _BendRotationRandom * UNITY_PI * 0.5, float3(-1, 0, 0));
	    float3x3 transformationMatrix = mul(mul(mul(tangentToLocal, windRotation), facingRotationMatrix), bendRotationMatrix);

		
		float height = (rand(pos.zyx) * 2 - 1) * _BladeHeightRandom + _BladeHeight;
        float width = (rand(pos.xzy) * 2 - 1) * _BladeWidthRandom + _BladeWidth;
		triStream.Append(VertexOutput(pos + mul(transformationMatrix, float3(width, 0, 0)), float2(0, 0)));
        triStream.Append(VertexOutput(pos + mul(transformationMatrix, float3(-width, 0, 0)),float2(1,0)));
        triStream.Append(VertexOutput(pos + mul(transformationMatrix, float3(0, 0, height)),float2(.5, 1)));
	}
	
 

	// Simple noise function, sourced from http://answers.unity.com/answers/624136/view.html
	// Extended discussion on this function can be found at the following link:
	// https://forum.unity.com/threads/am-i-over-complicating-this-random-function.454887/#post-2949326
	// Returns a number in the 0...1 range.
	/*float rand(float3 co)
	{
		return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 53.539))) * 43758.5453);
	}*/

	// Construct a rotation matrix that rotates around the provided axis, sourced from:
	// https://gist.github.com/keijiro/ee439d5e7388f3aafc5296005c8c3f33
	

	/*float3x3 AngleAxis3x3(float angle, float3 axis)
	{
		float c, s;
		sincos(angle, s, c);

		float t = 1 - c;
		float x = axis.x;
		float y = axis.y;
		float z = axis.z;

		return float3x3(
			t * x * x + c, t * x * y - s * z, t * x * z + s * y,
			t * x * y + s * z, t * y * y + c, t * y * z - s * x,
			t * x * z - s * y, t * y * z + s * x, t * z * z + c
			);
	} */

	

/*	vertexOutput vert(vertexInput v)
   {
	vertexOutput o;
	o.vertex = v.vertex;
	o.normal = v.normal;
	o.tangent = v.tangent;
	return o;
   }
*/

	/*float4 vert(float4 vertex : POSITION) : SV_POSITION
	{
		return vertex;
	}*/
	ENDCG

    SubShader
    {
		Cull Off

        Pass
        {
			Tags
			{
				"RenderType" = "Opaque"
				"LightMode" = "ForwardBase"
			}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma geometry geo
			#pragma target 4.6
			#pragma hull hull 
			#pragma domain domain 
            
			#include "Lighting.cginc"

			float4 _TopColor;
			float4 _BottomColor;
			float _TranslucentGain;

			float4 frag (geometryOutput i, fixed facing : VFACE) : SV_Target
            {	
				return lerp(_BottomColor, _TopColor, i.uv.y);
            }
            ENDCG
        }
    }
}