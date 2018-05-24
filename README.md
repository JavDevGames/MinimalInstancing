# MinimalInstancing

Sample project that shows how to use the Graphics.DrawInstanced() API.

## InstanceColorShader.shader
This shader has the minimum amount of code needed to allow for user defined coloring and GPU intancing support 

## DrawInstancedScript.cs
Monobehaviour that takes in values for number of meshes in a grid (width x height) and draws the grid in batches of 1023 instances at a time
*Prefab* and *Material* need to be supplied in order to draw something

## InstancedMaterial.mat
Material that uses the InstanceColorShader to draw the instances of the mesh

### InstancingScene.unity
Sample scene that draws 200 x 200 prisms in white color (as shown below)

![Instanced Prisms](http://coding.javdev.com/tests/instancing/instanced_prisms.png)
