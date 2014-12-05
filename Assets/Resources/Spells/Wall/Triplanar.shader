/*  Author: Mark Davis
 *
 *  This shader provides triplanar texturing with normal and specular support.
 *
 *  Much of this code was inspired by the first chapter in GPU Gems 3.
 *  Lighting was inspired by Unity's own examples.
 *
 *  I highly recommend experimenting with some of the textures from this site:
 *  http://www.filterforge.com/filters/category46-page1.html
 */

Shader "Voxelform/Triplanar 1/Diffuse Normal Specular" {

    Properties {
      _WallTexture ("Wall", 2D) = "white" {}
      _NormalMap ("Normal Map", 2D) = "white" {}
      _SpecularMap ("Specular Map", 2D) = "white" {}
      _NormalPower ("Normal Power", Float) = 1
      _SpecularPower ("Specular Power", Float) = 1
      _TriplanarFrequency ("Triplanar Frequency", Float) = .2

    }

    SubShader {

      Tags { "RenderType" = "Opaque" }

      CGPROGRAM
      #pragma target 3.0
      #include "UnityCG.cginc"
      #pragma surface surf SimpleLambert

      float _NormalPower;
      float _SpecularPower;
      float _TriplanarFrequency;

      float4 _Rotation;

      sampler2D _WallTexture;
      sampler2D _NormalMap;
      sampler2D _SpecularMap;

      struct CustomSurfaceOutput
      {
         half3 Albedo;
         half3 Normal;
         half3 Emission;
         half Alpha;
         half3 BumpNormal;
         half Specular;
      };

      half4 LightingSimpleLambert (CustomSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
      {
         half NdotL = dot(normalize(s.BumpNormal), normalize(lightDir));

         half3 h = normalize (lightDir + normalize(viewDir));

         half nh = max (0, dot (s.BumpNormal, h));
         half spec = smoothstep(0, 1.0, pow(nh, 32.0 * s.Specular)) * _SpecularPower;

         half4 c;
         c.rgb = (s.Albedo * _LightColor0.rgb * NdotL + _LightColor0.rgb * spec) * atten;
         c.a = s.Alpha;

         return c;
      }

      struct Input
      {
         float3 uv_Texture;
         float3 worldPos;
         float3 worldNormal;
      };

      void surf (Input IN, inout CustomSurfaceOutput o)
      {
         float3 blendingWeights = abs( normalize(IN.worldNormal));
         blendingWeights = (blendingWeights - 0.2) * 7;
         blendingWeights = max(blendingWeights, 0);
         blendingWeights /= (blendingWeights.x + blendingWeights.y + blendingWeights.z ).xxx;

         float4 blendedColor;
         float3 blendedNormal;
         float blendedSpecular;

         float2 coord1 = IN.worldPos.zy * -_TriplanarFrequency;
         float2 coord2 = IN.worldPos.zx * _TriplanarFrequency;
         float2 coord3 = float2(IN.worldPos.x, -IN.worldPos.y) * -_TriplanarFrequency;

         float4 col1 = tex2D(_WallTexture, coord1);
         float4 col2 = tex2D(_WallTexture, coord2);
         float4 col3 = tex2D(_WallTexture, coord3);

         float2 bumpFetch1 = tex2D(_NormalMap, coord1).xy - 0.5;
         float2 bumpFetch2 = tex2D(_NormalMap, coord2).xy - 0.5;
         float2 bumpFetch3 = tex2D(_NormalMap, coord3).xy - 0.5;

         float3 bump1 = float3(0, -bumpFetch1.y, -bumpFetch1.x);
         float3 bump2 = float3(bumpFetch2.y, 0, bumpFetch2.x);
         float3 bump3 = float3(bumpFetch3.x, bumpFetch3.y, 0);

         float spec1 = tex2D(_SpecularMap, coord1).x;
         float spec2 = tex2D(_SpecularMap, coord2).x;
         float spec3 = tex2D(_SpecularMap, coord3).x;

         blendedSpecular = 1.0 - (spec1 * blendingWeights.x
            + spec2 * blendingWeights.y
            + spec3 * blendingWeights.z);

         blendedColor = col1.xyzw * blendingWeights.xxxx +
         col2.xyzw * blendingWeights.yyyy +
         col3.xyzw * blendingWeights.zzzz;

         blendedNormal = bump1.xyz * blendingWeights.xxx +
         bump2.xyz * blendingWeights.yyy +
         bump3.xyz * blendingWeights.zzz;

         float4 n = float4(blendedNormal.x, blendedNormal.y, -blendedNormal.z, 1);
         float4 camVec = normalize(n);

         o.BumpNormal = normalize(IN.worldNormal + (camVec) * -_NormalPower);
         o.Specular = blendedSpecular;
         o.Albedo = blendedColor * 3;
         o.Alpha = 1.0;

      }

      ENDCG

    }

    Fallback "Diffuse"

}

