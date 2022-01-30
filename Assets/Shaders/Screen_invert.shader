Shader "GGJ/Screen_invert"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_StepValue("Step amount value", Range(0.0, 1.0)) = 0.2
		_PowResult("Pow amount", Float) = 1.0
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
			float _StepValue;
			float _PowResult;
	
            fixed4 frag (v2f_img i) : COLOR {
                fixed4 col = tex2D(_MainTex, fixed2(1 - i.uv.x, i.uv.y));
                
				float lum = col.x * 0.299f + col.y * 0.587 + col.z * 0.144;
                col.rgb = 1 -  step(_StepValue, pow(lum, _PowResult));
                return col;
            }
            ENDCG
        }
    }
}
