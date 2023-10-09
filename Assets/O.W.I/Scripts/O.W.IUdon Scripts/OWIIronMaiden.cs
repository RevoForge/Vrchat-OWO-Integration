
using UdonSharp;
using UnityEngine;

public class OWIIronMaiden : UdonSharpBehaviour
{
	public int sensationPriority = 1;
	public int intensity = 100;

	 public void TriggerIronMaiden()
	{
		Debug.Log($"VRC_OWO_WorldIntegration: [{{ \"priority\": { sensationPriority} , \"sensation\": \"Death\",\"frequency\": 60,\"duration\": 1,\"intensity\": {intensity},\"rampup\":0,\"rampdown\":0,\"exitdelay\":0,\"Muscles\": {{ \"allMuscles\": 100}}}},{{ \"sensation\": \"Death\",\"frequency\": 100,\"duration\": 10,\"intensity\": {intensity},\"rampup\":0.3,\"rampdown\":1,\"exitdelay\":0,\"Muscles\": {{ \"allMuscles\": 100 }}}}]");
	}

}
