using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This componet is used to create parallax scrolling on multiple objects.
It's reccomended to use in Unity 2D to parralax scrolling for infinite scrollers.

Follow these steps to set up the scene so the component works:
*1.)Set up the scene
*2.)Importing textures.
		*if doing pixel art: filter mode = point(no filter) and compression = none
		*Change Wrap Mode to Repeat
		*Then click Apply.
3.)Create a new material. 
	*Shader: Unlit/Transparent
4.)Add your texture to the material
5.) Create a Quad object: GameObject -> 3D Object ->Quad.
	*explain why
6.) Scale the Quads to the camera size and move them to the correct layers.
7.) Drag the material onto the quad object.
8.) Make sure this script has a reference to the object through the array.
*/
public class backgroundScrolling : MonoBehaviour
{
    //scroll speed added to all backgrounds 
    public float additionalScrollSpeed;
    //an array of all the background game objects
    public GameObject[] backgrounds;
    //an array that corresponds to the backgrounds array, where it gives the scroll speed for each individual background
    public float[] scrollSpeed;

    private void FixedUpdate() {
        
        //loops through array of objects, making scrolling occur for each
        for (int background = 0; background < backgrounds.Length; background++)
        {
            //gets the renderer for this item in the array
            Renderer rend = backgrounds[background].GetComponent<Renderer>();
            //calculates the scroll offset
            float offset = Time.time * (scrollSpeed[background] + additionalScrollSpeed);
            //offsets the texture of this item based on the offset calculated, this is not added to a previous offset.
            rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }
    }
}

/*
TODO: (homework)
1.)The backgrounds UV texture offset changes dramatically when speed is increased.
This is due to the fact the fact that the offset is calculated based on time.
To fix this issue, create another array which keeps track of the previous offset for each background.
Such that this line looks like:
    float offset = previousOfsset[background] + (scrollSpeed[background] + additionalScrollSpeed);
2.)Some could argue by accesing the renderer in each fixed update it may be cause the program to be slow.
So create an array that stores the renderer for all the backgrounds, and gets the renders only once at the start of the game.
*/