# Project Title

Name: Sasha Kuechenmeister

Student Number: C18404082

Class Group: 

# Description of the project
The goal for my project was to create a procedurally generated island in Unity.  
The user can change different variables to generate a completely unique island.  
This can be done through changing any of the following:  
1. create different regions (water, grass, snow, etc.)
2. noise scale
3. octaves
4. persistance
5. lacunarity
6. seed
7. height multiplier


# Instructions for use
To use this project, you need to press play on Unity. Once started, the player will spawn on an island where they can then walk around and explore. Users can use the WASD controls to move and look around.

# How it works
This section will discuss how everything works and examine some of the important scripts, models and assets used in this project.  

## Procedural Mesh
This is a procedural landmass generated using octaves of perlin noise. The island is highly customizable in the Unity Editor through some custom editor scripts and variables available to the user in the Map Generator.  
The first option in the Map Generator is having the choice to choose between 4 different modes of generating. The first two generate 2D textures onto a plane (under the mesh), the third one creates a mesh using the first two textures applied to it, and the fourth one takes the third one and applies a falloff map to it (creates the island).

## Clouds

## Day / Night

#



# List of classes/assets in the project and whether made yourself or modified or if its from a source, please give the reference

As this was my first time ever working with Unity and not having an ideal introduction to the platform, I had to make use of a tutorial to help me. Below you can see what I created myself and what I learned/modified/used from a tutorial.  

| Class/asset | Source |  
|-----------|-----------|
| Move_Cloud.cs | Self Made |  
| Spawn_Clouds.cs | Self Made |  
| Cloud_Behaviour.cs | Self Made |  
| Object_Pool.cs | Self Made |  
| Day_Night.cs | Self Made |  
| OnPlay.cs | Self Made |  
| MapDisplay.cs | Followed Tutorial [Procedural Landmass Generation](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) |  
| MapGenerator.cs | Followed Tutorial [Procedural Landmass Generation](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) |  
| MeshGenerator.cs | Followed Tutorial [Procedural Landmass Generation](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) |  
| FallOffGenerator.cs | Followed Tutorial [Procedural Landmass Generation](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) |   
| Noise.cs | Followed Tutorial [Procedural Landmass Generation](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) |    
| TextureGenerator.cs | Followed Tutorial [Procedural Landmass Generation](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) |    


# References

https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3  
https://www.red-gate.com/simple-talk/development/dotnet-development/procedural-generation-unity-c/  
https://gamedevacademy.org/complete-guide-to-procedural-level-generation-in-unity-part-1/  

# What I am most proud of in the assignment

I am most proud of the clouds generation. I was able to spawn clouds in a given area and have them float over the island. The clouds were able to grow and shrink. When they hit their maximum size, they would start to shrink until they reach their minimum size and then repeat this process.
I was also quite proud of the Day / Night Cycle even though it was quite simple. It adds another element to the scene by having two directional lights (sun and moon) rotate around the islands mesh. The sun allowed for some awe inspiring sunsets and sunrises.

# Proposal submitted earlier can go here:

## This is how to markdown text:

This is *emphasis*

This is a bulleted list

- Item
- Item

This is a numbered list

1. Item
1. Item

This is a [hyperlink](http://bryanduggan.org)

# Headings
## Headings
#### Headings
##### Headings

This is code:

```Java
public void render()
{
	ui.noFill();
	ui.stroke(255);
	ui.rect(x, y, width, height);
	ui.textAlign(PApplet.CENTER, PApplet.CENTER);
	ui.text(text, x + width * 0.5f, y + height * 0.5f);
}
```

So is this without specifying the language:

```
public void render()
{
	ui.noFill();
	ui.stroke(255);
	ui.rect(x, y, width, height);
	ui.textAlign(PApplet.CENTER, PApplet.CENTER);
	ui.text(text, x + width * 0.5f, y + height * 0.5f);
}
```

This is an image using a relative URL:

![An image](images/p8.png)

This is an image using an absolute URL:

![A different image](https://bryanduggandotorg.files.wordpress.com/2019/02/infinite-forms-00045.png?w=595&h=&zoom=2)

This is a youtube video:

[![YouTube](http://img.youtube.com/vi/J2kHSSFA4NU/0.jpg)](https://www.youtube.com/watch?v=J2kHSSFA4NU)

This is a table:

| Heading 1 | Heading 2 |
|-----------|-----------|
|Some stuff | Some more stuff in this column |
|Some stuff | Some more stuff in this column |
|Some stuff | Some more stuff in this column |
|Some stuff | Some more stuff in this column |
