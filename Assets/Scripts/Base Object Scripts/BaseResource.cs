using UnityEngine;
using static ResourceHelpers;

/* A resource is any raw material that needs to be taken from the world before being used by the place
 * Resources should always be found in the resource array of the worldGrid
 * Resources should never be created except by extracting it from the inital world generation
 * Resources should not be added to a chunk anytime after it was initially generated
 */
public abstract class BaseResource : MonoBehaviour
{
	protected GridControl grid;
	protected int remainingResource;

	/* Generate initializes the resource to be ready to exist in the world 
	 * No special preconditions
	 * POSTCONDITIONS: The resource is added to its proper spot in the worldGrid
	 * cont.: The resource has been fully initalized and is ready to be utilized by the player
	 */
	public virtual void Generate(GridControl grid_)
	{
		grid = grid_;
		AddToWorld(grid, this);
	}

	/* Delete will remove this from the world and return the memory to the system 
	 * No special preconditions
	 * POSTCONDITIONS: This resource is removed from the worldGrid and everything is destroyed
	 */
	public virtual void Delete()
	{
		RemoveResourceFromWorld(grid, this);
		Destroy(gameObject);
	}

	// Returns the number of remaining resource in this tile
	public virtual int GetRemaining() { return remainingResource; }

	/* Extract removes 1 from the remainingResource, then returns the output of extracting it
	 * No special preconditions
	 * POSTCONDITIONS: remainingResource will be exactly 1 less than before
	 * cont.: If remainingResource goes to 0, this resource will be deleted
	 * cont.: The returnOutput will return the proper output of extracting this resource
	 */
	public abstract void Extract(out GameObject returnOutput);
}