# Project Title

Name: Sasha Kuechenmeister

Student Number: C18404082

Class Group: TU857/4

# Description of the project
The initial goal for my project was to create a procedurally generated island that a player could wander around and explore. I also wanted to have natures sounds such as birds singing and waves crashing to create a calming environment. After finishing my project I am happy that I was able to achieve all of these plus adding some features such as clouds floating over the island and also having a day and night cycle.  
Some of the key features of my project include:
* implementing perlin noise to procedurally generate the island terrain
* allow a person to use Unity Editor to generate new and unique islands
* feature immersive audio of birds singing and waves crashing, as well as a soothing background track
* day and night cycle
* generating clouds in the sky which are able to shrink/grow


# Instructions for use
To use this project, you need to press play on Unity. Once started, the player will spawn on an island where they can then walk around and explore. Users can use the WASD controls to move and look around.

# How it works
This section will discuss how everything works and examine some of the important scripts, models and assets used in this project.  

## Procedural Mesh
This is a procedural landmass generated using octaves of perlin noise. The island is highly customizable in the Unity Editor through some custom editor scripts and variables available to the user in the Map Generator.  
The main variables that can be changed in the Map Generator are:
1. choice to choose between 4 different modes of generating. The first two (noise and colour) generate 2D textures onto a plane (under the mesh), the third one creates a mesh using the first two textures applied to it, and the fourth one takes the third one and applies a falloff map to it (creates the island).  

  - noise map
<img src="https://user-images.githubusercontent.com/55543651/146084074-a1edb40e-4849-4096-b2f4-f6a9af8a1ed6.jpg" width="200">

  - colour map
<img src="https://user-images.githubusercontent.com/55543651/146085063-7327f1f7-225b-487d-b557-913d174ad3d6.jpg" width="200">

  - mesh map
<img src="https://user-images.githubusercontent.com/55543651/146084986-334fe9c9-865c-442b-8497-e64913d6258c.jpg" width="200">

  - falloff map
<img src="https://user-images.githubusercontent.com/55543651/146084085-484a3918-d483-4513-8282-85f1de2cd951.jpg" width="200">

<br>

2. the Noise scale which essentially zooms in and out of the noise map created.  
3. the Octaves of Noise, this decides how many octaves of noise will be stacked on top of each other.  
4. lacunarity which allows the user to increase the frequency / level of detail per octave.  
5. Persistence which controls the amplitude / level of impact of later octaves.  
6. Mesh Height Multiplier which works in tandem with the Mesh Height Curve. All mesh vertices y numbers are multiplied by the height multiplier. The height curve can then limit the multiplication of those y values based on their noise value. Essentially it allows the user to stop things like the water being bumpy
7. the Seed and Offset can be used to randomise the location of the sample noise.
8. the offset bool which allows for the generated map to be a landmass or an island (tick yes for island)
9. auto update bool allows Unity to live update the mesh with each change that the user makes
10. regions are structs that hold information about the colour map of the generated map. The user can create as many regions as they wish, assign them a colour and height between 0-1 (i.e. 0 = water, 1 = snow peak on a mountain)

```
// enum which holds the different run modes: (2D noise map, 2D colour map, 3D Mesh, 3D Falloff map)
public enum DrawMode {NoiseMap, ColourMap, Mesh, FalloffMap};
// current mode the component is in
public DrawMode drawMode;

public float noiseScale; // number that determines at what distance to have perlin noise
public int octaves; // number that determines the levels of detail
[Range(0,1)] // changes persistance into a slider
public float persistance; // number that determines how much detail is added or removed at each octave (adjusts frequency)
public float lacunarity; // number that determines how much each octave contributes to the overall shape (adjusts amplitude)

public int seed;
public Vector2 offset; // vector for offsetting the seed

public bool useFalloff; // option to use falloff map

public float meshHeightMultiplier; // how much the Y-axis of the mesh is multiplied by
public AnimationCurve meshHeightCurve; // curve that allows for customization of the height multiplication

public bool autoUpdate; // saves last changed configuration


public TerrainType[] regions; // allows for different terrain types

```

<br>

<img src="https://user-images.githubusercontent.com/55543651/146083883-7205888f-c2ff-4f29-8ac0-0a7a59bde20c.jpg" width="300">


## Clouds
Generated clouds that spawn off-screen and float over the island. The clouds try to maintain a consistent hover distance and avoid hitting the ground. When the clouds are over land, they start to slowly grow, when they are above the water, they grow quicker. Once they reach their maximum size, they start to shrink until they reach their smallest size, and then repeat the process.

<img src="https://user-images.githubusercontent.com/55543651/146085982-87062793-229c-489c-97f1-fe32ff69a661.jpg" width="300">



## Day / Night
There are two directional lights which rotate around the mesh. One is the moon, and the other is the sun. The sun is linked to the procedural skybox, resulting in some spectacular sunsets and sunrises.

<img src="https://user-images.githubusercontent.com/55543651/146086476-1b4e6ea3-b819-419b-b822-4fd392537a56.png" width="300">

## Endless Terrain
This section was really hard for me to understand and I had to heavily rely on the tutorial mentioned later. However, I found it was a very important part to my project as it allows the viewer to see islands in the distance and in theory if they were to travel across the water it would constantly generate new unique islands for the player to explore.
Also if falloff map is disabled, the player can walk across the landmass forever.


# List of classes/assets in the project and whether made yourself or modified or if its from a source, please give the reference

As this was my first time ever working with Unity and coding in C#, I had to make use of a tutorial to help me. Below you can see what I created myself and what I learned/modified/used from a tutorial.  

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
| Cloud_Generator | Self Made | 
| Game_Manager | Self Made | 
| Sun | Self Made | 
| Moon | Self Made | 
| Player | Self Made | 
| Audio | Downloaded Audio source from "Mixkit.com" | 
| Cloud Prefab | Self Made | 
| Move_Cloud.cs | Self Made | 


# References

https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3  
https://www.red-gate.com/simple-talk/development/dotnet-development/procedural-generation-unity-c/  
https://gamedevacademy.org/complete-guide-to-procedural-level-generation-in-unity-part-1/  

# What I am most proud of in the assignment

I am most proud of the clouds generation. I was able to spawn clouds in a given area and have them float over the island. The clouds were able to grow and shrink. When they hit their maximum size, they would start to shrink until they reach their minimum size and then repeat this process.
I was also quite proud of the Day / Night Cycle even though it was quite simple. It adds another element to the scene by having two directional lights (sun and moon) rotate around the islands mesh. The sun allowed for some awe inspiring sunsets and sunrises.

# Proposal submitted earlier can go here:


# Video

[![Watch the video](https://user-images.githubusercontent.com/55543651/146086958-b58890cd-10d0-4b44-ade3-af266e4cb9aa.png)](https://youtu.be/y8EwBQH6ka4)


