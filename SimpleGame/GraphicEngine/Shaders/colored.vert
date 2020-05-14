#version 120

attribute vec3 position;
attribute vec3 in_color;
attribute vec2 tex_coords;

varying vec3 color;
varying vec2 texturePosition;

uniform mat4 transformationMatrix;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

void main(void) {
    gl_Position = projectionMatrix * viewMatrix * transformationMatrix * vec4(position, 1.0);
    color = in_color;
    texturePosition = tex_coords;
}