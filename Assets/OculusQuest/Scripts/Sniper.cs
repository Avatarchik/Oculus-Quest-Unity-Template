using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
	public GameObject BulletPrefab;

	public Transform Muzzle;

	public int RemainingBullet { get; private set; } = 0;
}
