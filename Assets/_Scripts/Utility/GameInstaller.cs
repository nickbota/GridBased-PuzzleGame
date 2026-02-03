using Zenject;
using UnityEngine;
using Core;
using Visual;
using GridSystem;

public class GameInstaller : MonoInstaller
{
    [Header("MonoBehaviour References")]
    [SerializeField] private GameController _gameController;
    [SerializeField] private GridView _gridView;

    public override void InstallBindings()
    {
        if (_gameController != null) Container.Bind<GameController>().FromInstance(_gameController).AsSingle().NonLazy();
        if (_gridView != null) Container.Bind<GridView>().FromInstance(_gridView).AsSingle().NonLazy();
        
        Container.Bind<IGridFactory>().To<GridFactory>().AsSingle();
    }
}