using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRespawnPosition", menuName = "PlayerRespawn/RespawnPosition")]
public class PlayerRespawnPositon : ScriptableObject
{
    public Vector3 respawnPositionCheckPoint;
}