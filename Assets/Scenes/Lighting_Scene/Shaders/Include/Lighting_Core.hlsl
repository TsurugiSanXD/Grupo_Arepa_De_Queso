//Include Guard
#ifndef LIGHTING_CORE
#define LIGHTING_CORE



void GetMainLight_float(float3 positionWS, out float3 directionWS, out float3 color, out float shadowAttenuation)
{
    
    #if defined(SHADERGRAPH_PREVIEW)
    directionWS = normalize(float3(1, 1, -1));
    color = float3(1,1,1);
    shadowAttenuation = 1;
    #else 
    
    Light l = GetMainLight();
    directionWS = l.direction;
    color = l.color;
    shadowAttenuation = l.shadowAttenuation;

    #endif

}

#endif 
