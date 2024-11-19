#version 460 core

in vec3 f_Normal;
in vec2 f_TexCoords;
out vec4 out_Color;

uniform sampler2D u_Texture;
uniform vec3 u_LightDirection;
uniform vec3 u_LightColor;
uniform vec3 u_AmbientLight;
uniform vec4 u_Color;

void main() {
	float lightIntensity = max(-dot(u_LightDirection, normalize(f_Normal)), 0);

	out_Color = u_Color * texture(u_Texture, f_TexCoords);
	out_Color.rgb *= clamp(u_LightColor * lightIntensity + u_AmbientLight, 0, 1);
}
