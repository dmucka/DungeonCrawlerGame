sampler2D colorSampler : register(S0);

float4 overlayColor : register(C0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(colorSampler, uv.xy);

    if (color.a == 0)
        return color;
  
    float4 overlaidColor = float4((color.rgb + overlayColor.rgb) * overlayColor.a, color.a);

    return overlaidColor;
}