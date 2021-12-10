# Project Title

Name: Sasha Kuechenmeister

Student Number: C18404082

Class Group: 

# Description of the project



# Instructions for use

# How it works

# List of classes/assets in the project and whether made yourself or modified or if its from a source, please give the reference

| Class/asset | Source |  
|-----------|-----------|
| Move_Cloud.cs | Self Made |  
| Spawn_Clouds.cs | Self Made |  
| Cloud_Behaviour.cs | Self Made |  
| Object_Pool | Self Made |  
| Day_Night.cs | Self Made |  
| OnPlay | Self Made |  
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
