#version 410 core

in vec3 color;
in vec2 texturePosition;

out vec4 out_color;

uniform sampler2D ourTexture;
uniform float isTextured;

void main(void) {
	if (isTextured > 0.5)
		out_color = texture(ourTexture, texturePosition);
	else
		out_color = vec4(color, 1.0);
}