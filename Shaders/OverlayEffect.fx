sampler2D colorSampler : register(S0);

float4 overlayColor : register(C0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(colorSampler, uv.xy);

    if (color.a == 0 || color.r < 0.07f && color.g < 0.07f && color.b < 0.07f || color.r == 1.0f && color.g == 1.0f && color.b == 1.0f)
        return color;
  
    float4 overlaidColor = float4((color.rgb + overlayColor.rgb) * overlayColor.a, color.a);

    return overlaidColor;
}