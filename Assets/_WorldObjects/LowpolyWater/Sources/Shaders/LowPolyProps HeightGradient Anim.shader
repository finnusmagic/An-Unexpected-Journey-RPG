// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BOXOPHOBIC/LowPolyProps/HeightGradient Anim"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_TopColor("Top Color", Color) = (0.2647059,0.2647059,0.2647059,1)
		_TopColorPos("Top Color Pos", Float) = 10
		_BottomColor("Bottom Color", Color) = (0.4852941,0.4852941,0.4852941,1)
		_BottomColorPos("Bottom Color Pos", Float) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.5
		_WindSpeed("Wind Speed", Float) = 10
		_WindAmplitude("Wind Amplitude", Float) = 100
		_WindCycles("Wind Cycles", Vector) = (0,0,0,0)
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 2.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _BottomColor;
		uniform float4 _TopColor;
		uniform float _BottomColorPos;
		uniform float _TopColorPos;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _WindSpeed;
		uniform float3 _WindCycles;
		uniform float _WindAmplitude;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex);
			float3 temp_cast_0 = (( ase_worldPos.z + ( _Time.x * _WindSpeed ) )).xxx;
			float3 temp_cast_1 = (( _WindAmplitude * 0.1 )).xxx;
			float3 temp_cast_2 = (v.color.a).xxx;
			float3 MOTION = mul( unity_ObjectToWorld , float4( ( ( sin( ( temp_cast_0 * _WindCycles ) ) * temp_cast_1 ) * temp_cast_2 ) , 0.0 ) );
			v.vertex.xyz += MOTION;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float4 COLOR = lerp( _BottomColor , _TopColor , saturate( (0.0 + (ase_worldPos.y - _BottomColorPos) * (1.0 - 0.0) / (_TopColorPos - _BottomColorPos)) ) );
			o.Albedo = COLOR.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Standard"
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=10010
1927;29;1906;1014;3148.005;5121.066;1.626316;True;False
Node;AmplifyShaderEditor.RangedFloatNode;404;-1920,-3360;Float;False;Property;_WindSpeed;Wind Speed;6;0;10;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TimeNode;403;-1920,-3552;Float;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;406;-1664,-3472;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;405;-1920,-3712;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;408;-1488,-3712;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.Vector3Node;407;-1472,-3488;Float;False;Property;_WindCycles;Wind Cycles;8;0;0,0,0;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;410;-1152,-3488;Float;False;Property;_WindAmplitude;Wind Amplitude;7;0;100;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;409;-1152,-3712;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT3;0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;411;-912,-3488;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.1;False;1;FLOAT
Node;AmplifyShaderEditor.SinOpNode;412;-976,-3712;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;397;-1920,-4096;Float;False;Property;_BottomColorPos;Bottom Color Pos;3;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;394;-1920,-4016;Float;False;Property;_TopColorPos;Top Color Pos;1;0;10;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;392;-1920,-4352;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;414;-736,-3488;Float;False;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;413;-816,-3712;Float;False;2;2;0;FLOAT3;0.0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.TFHCRemap;395;-1664,-4096;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;416;-624,-3712;Float;False;2;2;0;FLOAT3;0.0;False;1;FLOAT;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.ObjectToWorldMatrixNode;415;-448,-3712;Float;False;0;1;FLOAT4x4
Node;AmplifyShaderEditor.ColorNode;391;-1408,-4352;Float;False;Property;_BottomColor;Bottom Color;2;0;0.4852941,0.4852941,0.4852941,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;379;-1408,-4176;Float;False;Property;_TopColor;Top Color;0;0;0.2647059,0.2647059,0.2647059,1;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SaturateNode;396;-1360,-4000;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;417;-192,-3584;Float;False;2;2;0;FLOAT4x4;0.0;False;1;FLOAT3;0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.LerpOp;390;-1024,-4352;Float;False;3;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.RegisterLocalVarNode;399;-832,-4352;Float;False;COLOR;-1;True;1;0;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;380;-1920,-5008;Float;False;Property;_Metallic;Metallic;4;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;384;-1920,-4832;Float;False;98;0;1;FLOAT3
Node;AmplifyShaderEditor.GetLocalVarNode;400;-1920,-5120;Float;False;399;0;1;COLOR
Node;AmplifyShaderEditor.RegisterLocalVarNode;98;0,-3712;Float;False;MOTION;-1;True;1;0;FLOAT3;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;381;-1920,-4928;Float;False;Property;_Smoothness;Smoothness;5;0;0.5;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-1536,-5120;Float;False;True;0;Float;ASEMaterialInspector;0;Standard;BOXOPHOBIC/LowPolyProps/HeightGradient Anim;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;SrcAlpha;OneMinusSrcAlpha;0;SrcAlpha;OneMinusSrcAlpha;OFF;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;Relative;0;Standard;-1;-1;-1;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;280;-2176,-5120;Float;False;100;100;;0;// FINAL;0;0
Node;AmplifyShaderEditor.CommentaryNode;279;-2176,-3712;Float;False;100;100;;0;// MOTION;0;0
Node;AmplifyShaderEditor.CommentaryNode;398;-2176,-4352;Float;False;100;100;;0;// COLOR;0;0
WireConnection;406;0;403;1
WireConnection;406;1;404;0
WireConnection;408;0;405;3
WireConnection;408;1;406;0
WireConnection;409;0;408;0
WireConnection;409;1;407;0
WireConnection;411;0;410;0
WireConnection;412;0;409;0
WireConnection;413;0;412;0
WireConnection;413;1;411;0
WireConnection;395;0;392;2
WireConnection;395;1;397;0
WireConnection;395;2;394;0
WireConnection;416;0;413;0
WireConnection;416;1;414;4
WireConnection;396;0;395;0
WireConnection;417;0;415;0
WireConnection;417;1;416;0
WireConnection;390;0;391;0
WireConnection;390;1;379;0
WireConnection;390;2;396;0
WireConnection;399;0;390;0
WireConnection;98;0;417;0
WireConnection;0;0;400;0
WireConnection;0;3;380;0
WireConnection;0;4;381;0
WireConnection;0;11;384;0
ASEEND*/
//CHKSM=569BEFED0B02C72C33B708783C9C1CD27CA1E62E