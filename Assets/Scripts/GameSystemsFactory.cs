using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystems : IUpdatable
{
    public ProjectileSystem ProjectileSystem { get; }
    public EventBus EventBus { get; }
    public WeaponFactory WeaponFactory { get; }

    // Todo:
    // ComboSystem
    // ScoreSystem ?
    // Player ?

    private readonly IUpdatable[] updatables;

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

        updatables = new IUpdatable[]
        {
            ProjectileSystem
        };
    }

    public void Update(float deltaTime)
    {
        foreach (var system in updatables)
            system.Update(deltaTime);
    }
}

public class GameSystemsFactory 
{
   private GameSystemsFactory() { }

   public static GameSystems Create(Bounds playfield)
   {
        EventBus eventBus = new EventBus();
        ProjectileSystem projectileSystem = new ProjectileSystem(eventBus, playfield);
        WeaponFactory weaponFactory = new WeaponFactory(projectileSystem);
        
        return new GameSystems(projectileSystem, eventBus, weaponFactory);
   }
}
