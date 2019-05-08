Shader "Custom/CustomLight"
{
    Properties
    {
        _Color ("Tint", Color) = (0,0,0,1)
        _MainTex ("Texture", 2D) = "white" {}
        //_Glossiness ("Smoothness", Range(0,1)) = 0.5
        //_Metallic ("Metallic", Range(0,1)) = 0.0
        [HDR] _Emission ("Emission", color) = (0, 0, 0)
        _Ramp ("Toon Ramp", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Custom fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Ramp;

        struct Input
        {
            float2 uv_MainTex;
        }; 

       /* float4 LightingCustom(SurfaceOutput s, float3 lightDir, float atten){
            return 0;
        }*/
            //custom lighting function
        float4 LightingCustom(SurfaceOutput s, float3 lightDir, float atten){
            //how much does the normal point towards the light?
            float towardsLight = dot(s.Normal, lightDir);
            towardsLight = towardsLight * .5 + .5;
            float3 lightIntensity = tex2D(_Ramp, towardsLight).rgb;
            //read from toon ramp
            //combine the color 
            float4 col; 
            col.rgb = lightIntensity * s.Albedo * atten * _LightColor0.rgb; 
            col.a = s.Alpha; 
            return col; 
            //return float4(lightIntensity, 1);
           // return towardsLight;
        }

        half _Glossiness;
        half _Metallic;
        fixed4 _Color; 

        
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

       
        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex); 
            c *= _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
          //  o.Metallic = _Metallic;
           // o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
