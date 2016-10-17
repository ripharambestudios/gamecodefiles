using UnityEngine;
using System.Collections;

public class PlayerShotgunGrowth : MonoBehaviour {

	public float expandSize = 10f;
	private float destroyTimer = 0f;
	private float maxSize = 10f;
	// Use this for initialization
	void Start () {
		StartCoroutine(Scale());
	}

	// Update is called once per frame
	void Update () {


		destroyTimer += Time.deltaTime;

		if(destroyTimer > 5f)
		{
			Destroy(this.gameObject);
		}
	}

	IEnumerator Scale()
	{
		while (destroyTimer <= 5f)
		{
			while (maxSize > transform.localScale.x)
			{
				this.transform.localScale += new Vector3(1, 1, 1) * (Time.deltaTime*5) * expandSize;
				yield return null;
			}
			yield return null;
		}

	}
}
