// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BOXOPHOBIC/LowPolyWater/HIGH Cubemap"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		[HideInInspector]_SpecColor("SpecularColor",Color)=(1,1,1,1)
		[Header(Surface Control)] _WaterColor("Water Color", Color) = (0,0.5,1,1)
		_WaterSpecular("Water Specular", Range( 0 , 10)) = 1
		_WaterGloss("Water Gloss", Range( 0 , 10)) = 3
		_SmoothNormals("Smooth Normals", Range( 0 , 1)) = 0.5
		[Header(Reflection and Depth)] _FresnelPower("Fresnel Power", Range( 0 , 5)) = 0.75
		_ReflectionBoost("Reflection Boost", Range( 1 , 5)) = 1
		_ReflectionIntensity("Reflection Intensity", Range( 0 , 1)) = 0.5
		_DepthOffset("Depth Offset", Float) = 1
		_DepthFalloff("Depth Falloff", Float) = 2
		[Header(Edge Control)] _EdgeColor("Edge Color", Color) = (1,1,1,1)
		_EdgeIntensity("Edge Intensity", Range( 0 , 1)) = 0.5
		_EdgeOffset("Edge Offset", Range( 0 , 1)) = 0.8
		_EdgeFalloff("Edge Falloff", Float) = 10
		[IntRange]_EdgeLevels("Edge Levels", Range( 1 , 10)) = 5
		[Header(Motion Control)] _WaveHeight("Wave Height", Float) = 0.2
		_WaveCycles("Wave Cycles", Float) = 1.5
		_WaveSpeed("Wave Speed", Float) = 25
		_WaveDirectionZX("Wave Direction Z-X", Range( 0 , 1)) = 0
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		GrabPass{ "_GrabScreen1" }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf BlinnPhong alpha:fade keepalpha vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float4 screenPos;
		};

		uniform float4 _WaterColor;
		uniform fixed _SmoothNormals;
		uniform fixed _ReflectionBoost;
		uniform fixed _ReflectionIntensity;
		uniform sampler2D _GrabScreen1;
		uniform float _DepthOffset;
		uniform sampler2D _CameraDepthTexture;
		uniform float _DepthFalloff;
		uniform float _FresnelPower;
		uniform float4 _EdgeColor;
		uniform fixed _EdgeOffset;
		uniform float _EdgeFalloff;
		uniform float _EdgeLevels;
		uniform fixed _EdgeIntensity;
		uniform float _WaterGloss;
		uniform float _WaterSpecular;
		uniform fixed _WaveDirectionZX;
		uniform float _WaveSpeed;
		uniform float _WaveCycles;
		uniform float _WaveHeight;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex);
			float3 appendResult159 = float3( 0 , _WaveCycles , 0 );
			float3 VERTEXANIM = ( sin( ( ( lerp( ase_worldPos.z , ase_worldPos.x , _WaveDirectionZX ) + ( _Time.x * _WaveSpeed ) ) * appendResult159 ) ) * ( _WaveHeight * 0.1 ) );
			v.vertex.xyz += VERTEXANIM;
			float3 normalWorld = UnityObjectToWorldNormal( v.normal );
			half3 NORMAL = lerp( normalWorld , float3(0,1,0) , _SmoothNormals );
			v.normal = NORMAL;
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			fixed4 WATERCOLOR = _WaterColor;
			float3 ase_worldPos = i.worldPos;
			half3 NORMAL = lerp( i.worldNormal , float3(0,1,0) , _SmoothNormals );
			float3 temp_output_327_0 = reflect( normalize( ( ase_worldPos - _WorldSpaceCameraPos ) ) , NORMAL );
			float3 reflectDir = ( ( ( min( min( max( ( ( (unity_SpecCube0_BoxMax.xyz) - ase_worldPos ) / temp_output_327_0 ) , ( ( (unity_SpecCube0_BoxMin.xyz) - ase_worldPos ) / temp_output_327_0 ) ).x , max( ( ( (unity_SpecCube0_BoxMax.xyz) - ase_worldPos ) / temp_output_327_0 ) , ( ( (unity_SpecCube0_BoxMin.xyz) - ase_worldPos ) / temp_output_327_0 ) ).y ) , max( ( ( (unity_SpecCube0_BoxMax.xyz) - ase_worldPos ) / temp_output_327_0 ) , ( ( (unity_SpecCube0_BoxMin.xyz) - ase_worldPos ) / temp_output_327_0 ) ).z ) * temp_output_327_0 ) + ase_worldPos ) - (unity_SpecCube0_ProbePosition.xyz) );
			float4 reflectionHDR = (UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, reflectDir,0));
			fixed4 REFLECTION = lerp( WATERCOLOR , float4( ( (DecodeHDR(reflectionHDR, unity_SpecCube0_HDR)) * _ReflectionBoost ) , 0.0 ) , _ReflectionIntensity );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPos108 = ase_screenPos;
			float2 componentMask234 = ase_screenPos108.xy;
			float2 componentMask117 = NORMAL.xz;
			float4 tex2DNode115 = tex2D( _GrabScreen1, ( ( componentMask234 / ase_screenPos108.w ) + ( componentMask117 * float2( 0.25,0.25 ) ) ) );
			float4 ase_screenPos161 = ase_screenPos;
			float eyeDepth162 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos161))));
			float temp_output_257_0 = abs( ( eyeDepth162 - ase_screenPos161.w ) );
			fixed DEPTH = saturate( pow( ( _DepthOffset + temp_output_257_0 ) , ( 1.0 - max( _DepthFalloff , 1.0 ) ) ) );
			fixed4 REFRACTION = lerp( WATERCOLOR , tex2DNode115 , DEPTH );
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float fresnelFinalVal129 = (0.0 + 1.0*pow( 1.0 - dot( normalize( NORMAL ), worldViewDir ) , _FresnelPower));
			float FRESNEL = ( 1.0 - abs( fresnelFinalVal129 ) );
			fixed4 EDGECOLOR = _EdgeColor;
			fixed EDGE = ( ( 1.0 - (0.05 + (floor( ( saturate( pow( ( _EdgeOffset + temp_output_257_0 ) , max( _EdgeFalloff , 0.0 ) ) ) * _EdgeLevels ) ) - 0.0) * (1.0 - 0.05) / (_EdgeLevels - 0.0)) ) * _EdgeIntensity );
			fixed4 ALBEDO = lerp( lerp( REFLECTION , REFRACTION , FRESNEL ) , EDGECOLOR , EDGE );
			o.Albedo = ALBEDO.rgb;
			float temp_output_368_0 = ( 1.0 - EDGE );
			o.Specular = ( _WaterGloss * temp_output_368_0 );
			o.Gloss = ( temp_output_368_0 * _WaterSpecular );
			fixed OPACITY = saturate( ( EDGE + ( ( 1.0 - DEPTH ) * 10.0 ) ) );
			o.Alpha = OPACITY;
		}

		ENDCG
	}
	Fallback "BOXOPHOBIC/LowPolyWater/MOBILE Cubemap"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=7201
1927;29;1906;1014;2497.021;3769.385;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;9;-1920,960;Fixed;False;Property;_SmoothNormals;Smooth Normals;3;0;0.5;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.WorldSpaceCameraPos;318;-1920,-624;Float;False;0;1;FLOAT3
Node;AmplifyShaderEditor.WorldPosInputsNode;317;-1920,-768;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.Vector3Node;104;-1920,800;Float;False;Constant;_Vector0;Vector 0;6;0;0,1,0;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldNormalVector;265;-1920,640;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;6;-1536,640;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;319;-1664,-768;Float;False;2;0;FLOAT3;0.0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.GetLocalVarNode;324;-1920,-512;Float;False;97;0;1;FLOAT3
Node;AmplifyShaderEditor.WorldPosInputsNode;320;-1280,-352;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.NormalizeNode;321;-1504,-768;Float;False;1;0;FLOAT3;0.0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.RegisterLocalVarNode;97;-1344,640;Half;False;NORMAL;-1;True;1;0;FLOAT3;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.CustomExpressionNode;323;-1280,-416;Float;False;unity_SpecCube0_BoxMin.xyz;3;0;0;1;FLOAT3
Node;AmplifyShaderEditor.WorldPosInputsNode;322;-1280,-704;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.CustomExpressionNode;325;-1280,-768;Float;False;unity_SpecCube0_BoxMax.xyz;3;0;0;1;FLOAT3
Node;AmplifyShaderEditor.ReflectOpNode;327;-1280,-544;Float;False;2;0;FLOAT3;0.0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;328;-1088,-768;Float;False;2;0;FLOAT3;0,0,0,0;False;1;FLOAT3;0.0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;326;-1088,-416;Float;False;2;0;FLOAT3;0,0,0,0;False;1;FLOAT3;0.0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleDivideOpNode;329;-896,-464;Float;False;2;0;FLOAT3;0,0,0,0;False;1;FLOAT3;0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleDivideOpNode;330;-896,-640;Float;False;2;0;FLOAT3;0.0;False;1;FLOAT3;0.0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.ScreenPosInputsNode;161;-1920,-1536;Float;False;1;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMaxOp;331;-640,-768;Float;False;2;0;FLOAT3;0.0;False;1;FLOAT3;0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.ScreenDepthNode;162;-1728,-1536;Float;False;0;1;0;FLOAT4;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.BreakToComponentsNode;332;-512,-768;Float;False;FLOAT3;1;0;FLOAT3;0.0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;163;-1536,-1184;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.AbsOpNode;257;-1376,-1184;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMinNode;333;-256,-768;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;168;-1152,-1088;Float;False;Property;_EdgeFalloff;Edge Falloff;13;0;10;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;170;-1504,-1280;Fixed;False;Property;_EdgeOffset;Edge Offset;12;0;0.8;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMinNode;335;-128,-768;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RelayNode;334;-512,-544;Float;False;1;0;FLOAT3;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMaxOp;226;-960,-1104;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;169;-1104,-1280;Float;False;2;0;FLOAT;0.0,0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;336;64,-768;Float;False;2;0;FLOAT;0.0;False;1;FLOAT3;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;348;-1152,-1408;Float;False;Property;_DepthFalloff;Depth Falloff;9;0;2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;294;-896,-1280;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;337;16,-624;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;339;224,-768;Float;False;2;0;FLOAT3;0.0;False;1;FLOAT3;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.CustomExpressionNode;338;208,-560;Float;False;unity_SpecCube0_ProbePosition.xyz;3;0;0;1;FLOAT3
Node;AmplifyShaderEditor.SaturateNode;260;-704,-1280;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ScreenPosInputsNode;108;-1920,0;Float;False;1;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;262;-768,-1088;Float;False;Property;_EdgeLevels;Edge Levels;14;1;[IntRange];5;1;10;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;118;-1920,192;Float;False;97;0;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;349;-1504,-1536;Float;False;Property;_DepthOffset;Depth Offset;8;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMaxOp;350;-896,-1408;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;340;384,-768;Float;False;2;0;FLOAT3;0.0;False;1;FLOAT3;0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.ComponentMaskNode;234;-1728,0;Float;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.ComponentMaskNode;117;-1728,192;Float;False;True;False;True;True;1;0;FLOAT3;0,0,0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.GetLocalVarNode;148;-1920,-2048;Float;False;97;0;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;261;-448,-1280;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0,0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;351;-752,-1408;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;352;-1104,-1536;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;149;-1920,-1920;Float;False;Property;_FresnelPower;Fresnel Power;4;0;0.75;0;5;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;112;-1472,0;Float;False;2;0;FLOAT2;0.0;False;1;FLOAT;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.NormalizeNode;172;-1664,-2048;Float;False;1;0;FLOAT3;0.0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.FloorOpNode;263;-304,-1280;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;310;-1472,176;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.25,0.25;False;1;FLOAT2
Node;AmplifyShaderEditor.PowerNode;353;-512,-1536;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.CustomExpressionNode;343;643,-670;Float;False;UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, reflectDir,0);4;1;True;reflectDir;FLOAT3;0,0,0;1;0;FLOAT3;0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.TFHCRemap;264;-160,-1280;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.05;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;267;-1920,1440;Fixed;False;Property;_WaveDirectionZX;Wave Direction Z-X;18;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;80;-1920,1712;Float;False;Property;_WaveSpeed;Wave Speed;17;0;25;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;223;-1920,-3488;Float;False;Property;_WaterColor;Water Color;0;0;0,0.4980392,1,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.FresnelNode;129;-1472,-2048;Float;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.5;False;1;FLOAT
Node;AmplifyShaderEditor.TimeNode;79;-1920,1520;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.CustomExpressionNode;345;848,-672;Float;False;DecodeHDR(reflectionHDR, unity_SpecCube0_HDR);3;1;True;reflectionHDR;FLOAT4;0,0,0,0;1;0;FLOAT4;0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SaturateNode;354;-256,-1536;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;268;-1920,1280;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;116;-1280,64;Float;False;2;0;FLOAT2;0.0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;373;816,-592;Fixed;False;Property;_ReflectionBoost;Reflection Boost;5;0;1;1;5;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-1664,1520;Float;False;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;357;-640,256;Float;False;355;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;365;-160,-1088;Fixed;False;Property;_EdgeIntensity;Edge Intensity;11;0;0.5;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;212;-1664,-3488;Fixed;False;WATERCOLOR;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.OneMinusNode;367;32,-1280;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.AbsOpNode;222;-1264,-2048;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;266;-1632,1360;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;216;-1024,0;Float;False;212;0;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;342;816,-512;Fixed;False;Property;_ReflectionIntensity;Reflection Intensity;6;0;0.5;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ScreenColorNode;115;-1024,80;Float;False;Global;_GrabScreen1;Grab Screen 1;-1;0;Object;-1;1;0;FLOAT2;0,0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;355;32,-1536;Fixed;False;DEPTH;-1;True;1;0;FLOAT;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;344;640,-768;Float;False;212;0;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;374;1120,-672;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.GetLocalVarNode;251;-1920,-2464;Float;False;355;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;160;-1664,1712;Float;False;Property;_WaveCycles;Wave Cycles;16;0;1.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;356;-384,0;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.AppendNode;159;-1440,1632;Float;False;FLOAT3;0;0;0;0;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleAddOpNode;92;-1472,1456;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;360;-1728,-2464;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;366;192,-1280;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;311;-1104,-2048;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;347;1408,-768;Float;False;3;0;COLOR;0.0;False;1;FLOAT3;0.0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;56;-1280,1712;Float;False;Property;_WaveHeight;Wave Height;15;0;0.2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;246;384,-1280;Fixed;False;EDGE;-1;True;1;0;FLOAT;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;308;-1920,-2880;Float;False;304;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;375;-1536,-2464;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;10.0;False;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;152;-192,0;Fixed;False;REFRACTION;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.GetLocalVarNode;153;-1920,-3072;Float;False;25;0;1;COLOR
Node;AmplifyShaderEditor.ColorNode;210;-1920,-3712;Float;False;Property;_EdgeColor;Edge Color;10;0;1,1,1,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;364;-1920,-2560;Float;False;246;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-1280,1456;Float;False;2;0;FLOAT;0.0;False;1;FLOAT3;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;1600,-768;Fixed;False;REFLECTION;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RegisterLocalVarNode;304;-896,-2048;Float;False;FRESNEL;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;154;-1920,-2976;Float;False;152;0;1;COLOR
Node;AmplifyShaderEditor.SinOpNode;53;-1088,1456;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;220;-1104,1696;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.1;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;361;-1280,-2560;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;297;-1664,-3712;Fixed;False;EDGECOLOR;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.GetLocalVarNode;300;-1408,-2928;Float;False;297;0;1;COLOR
Node;AmplifyShaderEditor.LerpOp;146;-1408,-3072;Float;False;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.GetLocalVarNode;313;-1920,-4416;Float;False;246;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;301;-1408,-2864;Float;False;246;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;258;-1920,-4336;Float;False;Property;_WaterSpecular;Water Specular;1;0;1;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;298;-1024,-3072;Float;False;3;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-896,1456;Float;False;2;0;FLOAT3;0.0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SaturateNode;362;-1152,-2560;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;368;-1728,-4416;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;131;-1920,-4496;Float;False;Property;_WaterGloss;Water Gloss;2;0;3;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;156;-1920,-4608;Float;False;155;0;1;COLOR
Node;AmplifyShaderEditor.RegisterLocalVarNode;155;-832,-3072;Fixed;False;ALBEDO;-1;True;1;0;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.GetLocalVarNode;158;-1920,-4128;Float;False;98;0;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;316;-1536,-4400;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;312;-1536,-4496;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;124;-1024,256;Fixed;False;Property;_RefractionIntensity;Refraction Intensity;7;0;0.75;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;309;-1920,-4224;Float;False;307;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;157;-1920,-4032;Float;False;97;0;1;FLOAT3
Node;AmplifyShaderEditor.LerpOp;215;-640,128;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.RegisterLocalVarNode;307;-960,-2560;Fixed;False;OPACITY;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;98;-704,1456;Float;False;VERTEXANIM;-1;True;1;0;FLOAT3;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-1152,-4608;Float;False;True;2;Float;ASEMaterialInspector;0;BlinnPhong;BOXOPHOBIC/LowPolyWater/HIGH Cubemap;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;Transparent;0.5;True;False;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;SrcAlpha;OneMinusSrcAlpha;0;SrcAlpha;OneMinusSrcAlpha;OFF;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;Relative;0;BOXOPHOBIC/LowPolyWater/MOBILE Cubemap;-1;-1;-1;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;272;-2176,-3072;Float;False;100;100;;0;// ALBEDO;0;0
Node;AmplifyShaderEditor.CommentaryNode;273;-2176,-3712;Float;False;100;100;;0;// WATER / EGDE COLORS;0;0
Node;AmplifyShaderEditor.CommentaryNode;276;-2176,-768;Float;False;100;100;;0;// REFLECTION;0;0
Node;AmplifyShaderEditor.CommentaryNode;279;-2176,1280;Float;False;100;100;;0;// WAVE ANIMATION;0;0
Node;AmplifyShaderEditor.CommentaryNode;274;-2176,-1536;Float;False;100;100;;0;// DEPTH AND EDGE CONTROL;0;0
Node;AmplifyShaderEditor.CommentaryNode;278;-2176,640;Float;False;100;100;;0;// NORMALS;0;0
Node;AmplifyShaderEditor.CommentaryNode;277;-2176,0;Float;False;100;100;;0;// REFRECTION;0;0
Node;AmplifyShaderEditor.CommentaryNode;303;-2176,-2048;Float;False;100;100;;0;// FRESNEL CONTROL;0;0
Node;AmplifyShaderEditor.CommentaryNode;306;-2176,-2560;Float;False;100;100;;0;// OPACITY;0;0
Node;AmplifyShaderEditor.CommentaryNode;280;-2176,-4608;Float;False;100;100;;0;// FINAL;0;0
WireConnection;6;0;265;0
WireConnection;6;1;104;0
WireConnection;6;2;9;0
WireConnection;319;0;317;0
WireConnection;319;1;318;0
WireConnection;321;0;319;0
WireConnection;97;0;6;0
WireConnection;327;0;321;0
WireConnection;327;1;324;0
WireConnection;328;0;325;0
WireConnection;328;1;322;0
WireConnection;326;0;323;0
WireConnection;326;1;320;0
WireConnection;329;0;326;0
WireConnection;329;1;327;0
WireConnection;330;0;328;0
WireConnection;330;1;327;0
WireConnection;331;0;330;0
WireConnection;331;1;329;0
WireConnection;162;0;161;0
WireConnection;332;0;331;0
WireConnection;163;0;162;0
WireConnection;163;1;161;4
WireConnection;257;0;163;0
WireConnection;333;0;332;0
WireConnection;333;1;332;1
WireConnection;335;0;333;0
WireConnection;335;1;332;2
WireConnection;334;0;327;0
WireConnection;226;0;168;0
WireConnection;169;0;170;0
WireConnection;169;1;257;0
WireConnection;336;0;335;0
WireConnection;336;1;334;0
WireConnection;294;0;169;0
WireConnection;294;1;226;0
WireConnection;339;0;336;0
WireConnection;339;1;337;0
WireConnection;260;0;294;0
WireConnection;350;0;348;0
WireConnection;340;0;339;0
WireConnection;340;1;338;0
WireConnection;234;0;108;0
WireConnection;117;0;118;0
WireConnection;261;0;260;0
WireConnection;261;1;262;0
WireConnection;351;0;350;0
WireConnection;352;0;349;0
WireConnection;352;1;257;0
WireConnection;112;0;234;0
WireConnection;112;1;108;4
WireConnection;172;0;148;0
WireConnection;263;0;261;0
WireConnection;310;0;117;0
WireConnection;353;0;352;0
WireConnection;353;1;351;0
WireConnection;343;0;340;0
WireConnection;264;0;263;0
WireConnection;264;2;262;0
WireConnection;129;0;172;0
WireConnection;129;3;149;0
WireConnection;345;0;343;0
WireConnection;354;0;353;0
WireConnection;116;0;112;0
WireConnection;116;1;310;0
WireConnection;82;0;79;1
WireConnection;82;1;80;0
WireConnection;212;0;223;0
WireConnection;367;0;264;0
WireConnection;222;0;129;0
WireConnection;266;0;268;3
WireConnection;266;1;268;1
WireConnection;266;2;267;0
WireConnection;115;0;116;0
WireConnection;355;0;354;0
WireConnection;374;0;345;0
WireConnection;374;1;373;0
WireConnection;356;0;216;0
WireConnection;356;1;115;0
WireConnection;356;2;357;0
WireConnection;159;1;160;0
WireConnection;92;0;266;0
WireConnection;92;1;82;0
WireConnection;360;0;251;0
WireConnection;366;0;367;0
WireConnection;366;1;365;0
WireConnection;311;0;222;0
WireConnection;347;0;344;0
WireConnection;347;1;374;0
WireConnection;347;2;342;0
WireConnection;246;0;366;0
WireConnection;375;0;360;0
WireConnection;152;0;356;0
WireConnection;84;0;92;0
WireConnection;84;1;159;0
WireConnection;25;0;347;0
WireConnection;304;0;311;0
WireConnection;53;0;84;0
WireConnection;220;0;56;0
WireConnection;361;0;364;0
WireConnection;361;1;375;0
WireConnection;297;0;210;0
WireConnection;146;0;153;0
WireConnection;146;1;154;0
WireConnection;146;2;308;0
WireConnection;298;0;146;0
WireConnection;298;1;300;0
WireConnection;298;2;301;0
WireConnection;57;0;53;0
WireConnection;57;1;220;0
WireConnection;362;0;361;0
WireConnection;368;0;313;0
WireConnection;155;0;298;0
WireConnection;316;0;368;0
WireConnection;316;1;258;0
WireConnection;312;0;131;0
WireConnection;312;1;368;0
WireConnection;215;0;216;0
WireConnection;215;1;115;0
WireConnection;215;2;124;0
WireConnection;307;0;362;0
WireConnection;98;0;57;0
WireConnection;0;0;156;0
WireConnection;0;3;312;0
WireConnection;0;4;316;0
WireConnection;0;9;309;0
WireConnection;0;11;158;0
WireConnection;0;12;157;0
ASEEND*/
//CHKSM=61F2C87C6AA76EAECAD0ADB1C5D49D1087BEC0B4