#version 400 core

in vec3 position;
in vec3 in_color;
in vec2 tex_coords;

out vec3 color;
out vec2 texturePosition;

uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

void main(void) {
    gl_Position = projectionMatrix * viewMatrix * transformationMatrix * vec4(position, 1.0);
    color = in_color;
    texturePosition = tex_coords;
}