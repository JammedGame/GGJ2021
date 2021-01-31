public class Pirate : Barrel
{
	public int Index;

	public override void OnRelease(PlayerController ship)
	{
		var pivot = ship.PiratePivots[Index];
		transform.parent = pivot.transform;
		transform.localPosition = default;
		transform.localEulerAngles = default;
		Destroy(rigidBody);
	}
}