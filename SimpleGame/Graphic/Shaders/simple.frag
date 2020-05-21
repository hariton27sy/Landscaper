#version 120

varying vec3 color;
varying vec2 texturePosition;

uniform sampler2D tex;
uniform float isTextured;

void main(void) {
	if (isTextured > 0.5){
		gl_FragColor.rgb = texture2D(tex, texturePosition).rgb;
		gl_FragColor.a = 1.0;
	}
	else{
		gl_FragColor.rgb = color;
		gl_FragColor.a = 1.0;
	}
}