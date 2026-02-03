# Настройка уровней и камней

## Создание камней

1. Перейдите в `Assets/Data/Gems`
2. Правый клик → `Create > GridBased_Puzzle > Gem Definition`
3. Настройте параметры камня в Inspector

## Создание уровней

1. Перейдите в `Assets/Data/Levels`
2. Правый клик → `Create > GridBased_Puzzle > Level Configuration`
3. Добавьте камни в список **Gem Definitions**, перетащив их из `Assets/Data/Gems`

## Назначение уровней

1. Откройте `Assets/Scenes/SampleScene.unity`
2. Выберите `GameController` в Hierarchy
3. Перетащите конфигурации уровней в компонент управления уровнями
