Shader "HHHJ/Common/ShadowCaster"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
		
		//============================================ Normal ========================================
		
		Pass
		{
			Name "ShadowCaster-Front"
			Tags { "LightMode" = "ShadowCaster" }

			ZWrite On ZTest LEqual Cull Back

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"

			struct v2f {
					V2F_SHADOW_CASTER;
					UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert(appdata_base v)
			{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
					return o;
			}

			float4 frag(v2f i) : SV_Target
			{
					SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
		
		Pass
		{
			Name "ShadowCaster-Back"
			Tags { "LightMode" = "ShadowCaster" }

			ZWrite On ZTest LEqual Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"

			struct v2f {
					V2F_SHADOW_CASTER;
					UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert(appdata_base v)
			{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
					return o;
			}

			float4 frag(v2f i) : SV_Target
			{
					SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
		
		Pass
		{
			Name "ShadowCaster-TwoSide"
			Tags { "LightMode" = "ShadowCaster" }

			ZWrite On ZTest LEqual Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"

			struct v2f {
					V2F_SHADOW_CASTER;
					UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert(appdata_base v)
			{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
					return o;
			}

			float4 frag(v2f i) : SV_Target
			{
					SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
    
		//============================================ Cutout ========================================
	
		Pass {
			Name "ShadowCaster-Front-Cutout"
			Tags { "LightMode" = "ShadowCaster" }
	
			ZWrite On ZTest LEqual Cull Back
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_instancing // allow instanced shadow pass for most of the shaders
			#include "UnityCG.cginc"
			
			struct v2f {
				V2F_SHADOW_CASTER;
				float2  uv : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			uniform float4 _MainTex_ST;
			
			v2f vert( appdata_base v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			uniform sampler2D _MainTex;
			uniform fixed _Cutoff;
			uniform fixed4 _Color;
			
			float4 frag( v2f i ) : SV_Target
			{
				fixed4 texcol = tex2D( _MainTex, i.uv );
				clip( texcol.a*_Color.a - _Cutoff );
			
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
	
		}
		
		Pass {
			Name "ShadowCaster-Back-Cutout"
			Tags { "LightMode" = "ShadowCaster" }
	
			ZWrite On ZTest LEqual Cull Front
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_instancing // allow instanced shadow pass for most of the shaders
			#include "UnityCG.cginc"
			
			struct v2f {
				V2F_SHADOW_CASTER;
				float2  uv : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			uniform float4 _MainTex_ST;
			
			v2f vert( appdata_base v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			uniform sampler2D _MainTex;
			uniform fixed _Cutoff;
			uniform fixed4 _Color;
			
			float4 frag( v2f i ) : SV_Target
			{
				fixed4 texcol = tex2D( _MainTex, i.uv );
				clip( texcol.a*_Color.a - _Cutoff );
			
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
	
		}
		
		
		Pass {
			Name "ShadowCaster-TwoSide-Cutout"
			Tags { "LightMode" = "ShadowCaster" "RenderType"="AlphaTest" "Queue"="AlphaTest" }
	
			ZWrite On ZTest LEqual Cull Off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_instancing // allow instanced shadow pass for most of the shaders
			#include "UnityCG.cginc"
			
			struct v2f {
				V2F_SHADOW_CASTER;
				float2  uv : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			uniform float4 _MainTex_ST;
			
			v2f vert( appdata_base v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			uniform sampler2D _MainTex;
			uniform fixed _Cutoff;
			uniform fixed4 _Color;
			
			float4 frag( v2f i ) : SV_Target
			{
				fixed4 texcol = tex2D( _MainTex, i.uv );
				clip( texcol.a*_Color.a - _Cutoff );
			
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
	
		}
	}
}
