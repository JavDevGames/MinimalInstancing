# MinimalInstancing

# Part01

Sample project that shows how to use the Graphics.DrawInstanced() API.

### InstanceColorShader.shader
This shader has the minimum amount of code needed to allow for user defined coloring and GPU intancing support 

### DrawInstancedScript.cs
Monobehaviour that takes in values for number of meshes in a grid (width x height) and draws the grid in batches of 1023 instances at a time
*Prefab* and *Material* need to be supplied in order to draw something

### InstancedMaterial.mat
Material that uses the InstanceColorShader to draw the instances of the mesh

### InstancingScene.unity
Sample scene that draws 200 x 200 prisms in white color (as shown below)

![Instanced Prisms](http://coding.javdev.com/tests/instancing/instanced_prisms.png)

# Part02

### InstanceColorShader.shader
No change from Part01

### DrawInstancedScript.cs
New functionality:
1. Threshold: Specifies the min. noise value to accept. This is used for randomizing when prisms show up.
2. ColorArray: Array of colors to show for the different prisms, used in the Material Property Block

#### Material Property Block
This controls the color setting on a per instance basis. Using this, we can have multiple colors on the same mesh, and keep them batched using Graphics.DrawMeshInstanced().
  Calling
  `propertyBlock.SetVectorArray("_Color", batchedColorArray);` 
  `Graphics.DrawMeshInstanced(mMeshFilter.sharedMesh, 0, meshMaterial, batchedMatrices, batchCount, propertyBlock);` 
  ensures that can pass the property block configured with an array down to the Shader so that it can extract the data per instance.

### InstancedMaterial.mat
No chance from Part01

### InstancingScene.unity
Sample scene that draws 200 x 200 prisms in white color (as shown below)

![Instanced Prisms](http://coding.javdev.com/tests/instancing/instanced_prisms_part02.png)