#ifndef BlurCA
#define BlurCA

//#include "packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"


#ifndef SHADERGRAPH_PREVIEW
TEXTURE2D_X(_BlitTexture);
#endif 


#define ITERATIONS 64

float4 SampleTextureChannel(float2 pixelCoords, float2 direction, float pixelOffset)
{
    
    float4 result = 0;
    #ifndef SHADERGRAPH_PREVIEW
    float2 sampleCoords = pixelCoords + direction * pixelOffset;
    result += LOAD_TEXTURE2D_X_LOD(_BlitTexture, sampleCoords, 0);
    sampleCoords = pixelCoords - direction * pixelOffset;
    result += LOAD_TEXTURE2D_X_LOD(_BlitTexture, sampleCoords, 0);
    #endif 
    return result;
    
}

void RadialBlurChromaticAberration_float(float2 pixelcoords, float2 direction, float3 perChannelOffsets, out float4 result)
{
    
    result = float4(0, 1, 1, 0);
    #ifndef SHADERGRAPH_PREVIEW
    result = 0;
    
    for (int i = 0; i < ITERATIONS; i++)
    {
        
        float pixelR = SampleTextureChannel(pixelcoords, direction, perChannelOffsets.x).r;
        float pixelG = SampleTextureChannel(pixelcoords, direction, perChannelOffsets.y).g;
        float pixelB = SampleTextureChannel(pixelcoords, direction, perChannelOffsets.z).b;
        result += float4(pixelR, pixelG, pixelB, 1); 
        
        
        
    }
    
    result /= ITERATIONS;
   
    
       // for (int y = 1; y >= -1; y--)
       // {
        
        //    for (int x = -1; x <= 1; x++)
          //  {
            
            //    float2 sampleCoords = pixelcoords + float2(x, y) * pixelOffset;
            //  result += LOAD_TEXTURE2D_X_LOD(_BlitTexture, sampleCoords, 0) * 0.1111111111f;
            
   
            
          //  }
        
       // }
   
    #endif
    
}

#endif 
