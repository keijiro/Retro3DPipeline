Shader "Retro3D/Surface"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Tint", Color) = (0.5, 0.5, 0.5)
    }

    HLSLINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    float4 _MainTex_ST;
    half3 _Color;

    struct Attributes
    {
        float4 position : POSITION;
        float2 texcoord : TEXCOORD;
    };

    struct Varyings
    {
        float4 position : SV_POSITION;
        noperspective float2 texcoord : TEXCOORD;
        UNITY_FOG_COORDS(1)
    };

    Varyings Vertex(Attributes input)
    {
        float3 vp = UnityObjectToViewPos(input.position.xyz);
        vp = floor(vp * 64) / 64;
        Varyings output;
        output.position = UnityViewToClipPos(vp);
        output.texcoord = TRANSFORM_TEX(input.texcoord, _MainTex);
        UNITY_TRANSFER_FOG(output, output.position);
        return output;
    }

    half4 Fragment(Varyings input) : SV_Target
    {
        float2 uv = input.texcoord;
        uv = floor(uv * 256) / 256;
        half4 c = tex2D(_MainTex, uv);
        c = floor(c * 12) / 12;
        c.rgb *= _Color * 2;
        UNITY_APPLY_FOG(input.fogCoord, c);
        return c;
    }

    ENDHLSL

    SubShader
    {
        Pass
        {
            Tags { "LightMode" = "Base" }
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            ENDHLSL
        }
    }
}
