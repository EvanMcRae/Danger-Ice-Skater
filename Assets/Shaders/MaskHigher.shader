Shader "Custom/MaskHigher"
{
    SubShader
    {
        Tags {
            "Queue" = "Transparent+1"
        }

        Pass {
            Blend Zero One
        }
    }
}
