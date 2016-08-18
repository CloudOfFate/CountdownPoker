using UnityEngine;
using System.Collections;

public class Play  
{
	public enum Phase {One,Two,Three,Four,Five,NULL};

	public Enemy EnemyInstance;
	public Player PlayerInstance;
	public GameManager GMInstance;
	public CommonMemory CommonMemoryInstance;
	public EnemyMemory EnemyMemoryInstance;

	public Phase CurrentPhase;
	public PlayerAction CorrespondingAction;
	public bool PlayInUse;

	public Play (Enemy _Enemy)
	{
		EnemyInstance = _Enemy;
		PlayerInstance = EnemyInstance.PlayerInstance;
		GMInstance = _Enemy.PlayerInstance.GManager;
		CommonMemoryInstance = GMInstance.CommonMemoryLog;
		EnemyMemoryInstance = EnemyInstance.Memory;

		CurrentPhase = Phase.NULL;
		PlayInUse = false;
	}

	public virtual float ApplicabilityOfPlay() {return 0.0f;}

	public virtual void StartPlay() {}

	public virtual BettingDecision ExecuteBetPlay(){return BettingDecision.Fold;}

	public virtual AuctionDecision ExecuteAuctionPlay(){return AuctionDecision.NULL;}

	public virtual void EndPlay() {}
}
