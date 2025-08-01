Shader "Custom/Mask"
{
    SubShader
    {
        Tags {
            "Queue" = "Transparent+3"
        }

        Pass {
            Blend Zero One
        }
    }
}
