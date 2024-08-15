using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ZenjectInstaller : MonoInstaller
{
    [SerializeField] private List<Weapon> weapons = new();
    public override void InstallBindings()
    {
        Container.Bind<List<Weapon>>().FromInstance(weapons).AsTransient();
        //Container.Bind<PlayerInteractObjects>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GoldManager>().AsSingle();
    }
}
