#version 460 core

layout(location=0)in vec3 v_Position;
layout(location=1)in vec3 v_Normal;
layout(location=2)in vec2 v_TexCoords;

out vec3 f_Normal;
out vec2 f_TexCoords;

uniform mat4 u_Model;
uniform mat4 u_View;
uniform mat4 u_Projection;
uniform mat4 u_Rotation;

void main() {
	f_Normal = (u_Rotation * vec4(v_Normal, 1.0)).xyz;
	f_TexCoords = v_TexCoords;
	gl_Position = u_Projection * u_View * u_Model * vec4(v_Position, 1.0);
}
