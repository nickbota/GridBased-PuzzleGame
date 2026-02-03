using Zenject;
using UnityEngine;
using Core;
using Visual;
using GridSystem;
using UI;

public class GameInstaller : MonoInstaller
{
    [Header("MonoBehaviour References")]
    [SerializeField] private GameController _gameController;
    [SerializeField] private GridView _gridView;
    [SerializeField] private UIManager _uiManager;

    public override void InstallBindings()
    {
        if (_gameController != null)
        {
            Container.BindInterfacesAndSelfTo<GameController>()
                .FromInstance(_gameController)
                .AsSingle()
                .NonLazy();
        }

        if (_gridView != null) 
            Container.Bind<GridView>().FromInstance(_gridView).AsSingle().NonLazy();

        if (_uiManager != null)
        {
            // This covers both the UIManager class AND any interfaces it implements
            Container.BindInterfacesAndSelfTo<UIManager>()
                .FromInstance(_uiManager)
                .AsSingle()
                .NonLazy();
        }

        Container.Bind<IGridFactory>().To<GridFactory>().AsSingle();
    }
}