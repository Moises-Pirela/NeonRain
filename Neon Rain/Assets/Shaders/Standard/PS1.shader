Shader "PS1/PS1" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _VertexDetail ("Vertex Detail", Float ) = 80
        _AlphaClip ("Alpha Clip", Range(0, 1)) = 0.25
        _BumpMap ("Normal", 2D) = "bump" {}
        _NormalIntensity ("Normal Intensity", Float ) = 1
        _ColourDepth ("Colour Depth", Float ) = 32
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma target 3.0
            // Dithering function, to use with scene UVs (screen pixel coords)
            // 2x2 Bayer matrix, based on https://en.wikipedia.org/wiki/Ordered_dithering
            float BinaryDither2x2( float value, float2 sceneUVs ) {
                float2x2 mtx = float2x2(
                    float2( 1, 3 )/5.0,
                    float2( 4, 2 )/5.0
                );
                float2 px = floor(_ScreenParams.xy * sceneUVs);
                int xSmp = fmod(px.x,2);
                int ySmp = fmod(px.y,2);
                float2 xVec = 1-saturate(abs(float2(0,1) - xSmp));
                float2 yVec = 1-saturate(abs(float2(0,1) - ySmp));
                float2 pxMult = float2( dot(mtx[0],yVec), dot(mtx[1],yVec) );
                return round(value + dot(pxMult, xVec));
            }
            uniform float4 _LightColor0;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _Color)
                UNITY_DEFINE_INSTANCED_PROP( float, _AlphaClip)
                UNITY_DEFINE_INSTANCED_PROP( float, _NormalIntensity)
                UNITY_DEFINE_INSTANCED_PROP( float, _VertexDetail)
                UNITY_DEFINE_INSTANCED_PROP( float, _ColourDepth)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 projPos : TEXCOORD5;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float _VertexDetail_var = UNITY_ACCESS_INSTANCED_PROP( Props, _VertexDetail );
                float3 node_1173 = (mul( UNITY_MATRIX_V, float4(mul(unity_ObjectToWorld, v.vertex).rgb,0) ).xyz.rgb*_VertexDetail_var);
                v.vertex.xyz += ((round(node_1173)-node_1173)/_VertexDetail_var);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float _NormalIntensity_var = UNITY_ACCESS_INSTANCED_PROP( Props, _NormalIntensity );
                float3 normalLocal = lerp(_BumpMap_var.rgb,float3(0,0,1),(1.0 - _NormalIntensity_var));
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float _AlphaClip_var = UNITY_ACCESS_INSTANCED_PROP( Props, _AlphaClip );
                clip( BinaryDither2x2(pow(_Diffuse_var.a,(_AlphaClip_var*4.0+0.0)) - 1.5, sceneUVs) );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _Color_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Color );
                float _ColourDepth_var = UNITY_ACCESS_INSTANCED_PROP( Props, _ColourDepth );
                float3 diffuseColor = floor((_Diffuse_var.rgb*_Color_var.rgb) * _ColourDepth_var) / (_ColourDepth_var - 1);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma target 3.0
            // Dithering function, to use with scene UVs (screen pixel coords)
            // 2x2 Bayer matrix, based on https://en.wikipedia.org/wiki/Ordered_dithering
            float BinaryDither2x2( float value, float2 sceneUVs ) {
                float2x2 mtx = float2x2(
                    float2( 1, 3 )/5.0,
                    float2( 4, 2 )/5.0
                );
                float2 px = floor(_ScreenParams.xy * sceneUVs);
                int xSmp = fmod(px.x,2);
                int ySmp = fmod(px.y,2);
                float2 xVec = 1-saturate(abs(float2(0,1) - xSmp));
                float2 yVec = 1-saturate(abs(float2(0,1) - ySmp));
                float2 pxMult = float2( dot(mtx[0],yVec), dot(mtx[1],yVec) );
                return round(value + dot(pxMult, xVec));
            }
            uniform float4 _LightColor0;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _Color)
                UNITY_DEFINE_INSTANCED_PROP( float, _AlphaClip)
                UNITY_DEFINE_INSTANCED_PROP( float, _NormalIntensity)
                UNITY_DEFINE_INSTANCED_PROP( float, _VertexDetail)
                UNITY_DEFINE_INSTANCED_PROP( float, _ColourDepth)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 projPos : TEXCOORD5;
                LIGHTING_COORDS(6,7)
                UNITY_FOG_COORDS(8)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float _VertexDetail_var = UNITY_ACCESS_INSTANCED_PROP( Props, _VertexDetail );
                float3 node_1173 = (mul( UNITY_MATRIX_V, float4(mul(unity_ObjectToWorld, v.vertex).rgb,0) ).xyz.rgb*_VertexDetail_var);
                v.vertex.xyz += ((round(node_1173)-node_1173)/_VertexDetail_var);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float _NormalIntensity_var = UNITY_ACCESS_INSTANCED_PROP( Props, _NormalIntensity );
                float3 normalLocal = lerp(_BumpMap_var.rgb,float3(0,0,1),(1.0 - _NormalIntensity_var));
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float _AlphaClip_var = UNITY_ACCESS_INSTANCED_PROP( Props, _AlphaClip );
                clip( BinaryDither2x2(pow(_Diffuse_var.a,(_AlphaClip_var*4.0+0.0)) - 1.5, sceneUVs) );
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _Color_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Color );
                float _ColourDepth_var = UNITY_ACCESS_INSTANCED_PROP( Props, _ColourDepth );
                float3 diffuseColor = floor((_Diffuse_var.rgb*_Color_var.rgb) * _ColourDepth_var) / (_ColourDepth_var - 1);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma target 3.0
            // Dithering function, to use with scene UVs (screen pixel coords)
            // 2x2 Bayer matrix, based on https://en.wikipedia.org/wiki/Ordered_dithering
            float BinaryDither2x2( float value, float2 sceneUVs ) {
                float2x2 mtx = float2x2(
                    float2( 1, 3 )/5.0,
                    float2( 4, 2 )/5.0
                );
                float2 px = floor(_ScreenParams.xy * sceneUVs);
                int xSmp = fmod(px.x,2);
                int ySmp = fmod(px.y,2);
                float2 xVec = 1-saturate(abs(float2(0,1) - xSmp));
                float2 yVec = 1-saturate(abs(float2(0,1) - ySmp));
                float2 pxMult = float2( dot(mtx[0],yVec), dot(mtx[1],yVec) );
                return round(value + dot(pxMult, xVec));
            }
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float, _AlphaClip)
                UNITY_DEFINE_INSTANCED_PROP( float, _VertexDetail)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float4 projPos : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                float _VertexDetail_var = UNITY_ACCESS_INSTANCED_PROP( Props, _VertexDetail );
                float3 node_1173 = (mul( UNITY_MATRIX_V, float4(mul(unity_ObjectToWorld, v.vertex).rgb,0) ).xyz.rgb*_VertexDetail_var);
                v.vertex.xyz += ((round(node_1173)-node_1173)/_VertexDetail_var);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float _AlphaClip_var = UNITY_ACCESS_INSTANCED_PROP( Props, _AlphaClip );
                clip( BinaryDither2x2(pow(_Diffuse_var.a,(_AlphaClip_var*4.0+0.0)) - 1.5, sceneUVs) );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
///    CustomEditor "ShaderForgeMaterialInspector"
}
