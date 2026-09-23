using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystems
{
    public ProjectileSystem ProjectileSystem { get; }
    public EventBus EventBus { get; }
    public WeaponFactory WeaponFactory { get; }

    // Todo:
    // ComboSystem
    // ScoreSystem ?
    // Player ?

    public GameSystems(
        ProjectileSystem projectileSystem,
        EventBus eventBus,
        WeaponFactory factory)
    {
        if (projectileSystem == null || eventBus == null)
            throw new System.Exception(); 

        ProjectileSystem = projectileSystem;
        EventBus = eventBus;
        WeaponFactory = factory;
    }
}

public class GameSystemsFactory 
{
   private GameSystemsFactory() { }

   public static GameSystems Create()
   {
        EventBus eventBus = new EventBus();
        ProjectileSystem projectileSystem = new ProjectileSystem(eventBus);
        WeaponFactory weaponFactory = new WeaponFactory(projectileSystem);
        
        return new GameSystems(projectileSystem, eventBus, weaponFactory);
   }
}
