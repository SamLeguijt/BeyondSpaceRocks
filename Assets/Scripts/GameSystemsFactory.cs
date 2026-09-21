using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystems
{
    public ProjectileSystem ProjectileSystem { get; }
    public EventBus EventBus { get; }

    // Todo:
    // ComboSystem
    // ScoreSystem ?
    // Player ?

    public GameSystems(
        ProjectileSystem projectileSystem,
        EventBus eventBus)
    {
        if (projectileSystem == null || eventBus == null)
            throw new System.Exception(); 

        ProjectileSystem = projectileSystem;
        EventBus = eventBus;
    }
}

public class GameSystemsFactory 
{
   private GameSystemsFactory() { }

   public static GameSystems Create()
   {
        EventBus eventBus = new EventBus();
        ProjectileSystem projectileSystem = new ProjectileSystem(eventBus);

        return new GameSystems(projectileSystem, eventBus);
   }
}
