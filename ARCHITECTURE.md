# GeometryAstro — Архитектура и инструкция разработчика

## 1. Обзор проекта

**GeometryAstro** — мобильная 2D аркада (Geometry Wars-подобная) на Unity.

### Технологический стек

| Технология | Назначение |
|-----------|-----------|
| **VContainer** | Dependency Injection контейнер |
| **UniRx** | Реактивные расширения (Rx для Unity) |
| **UniTask** | Асинхронный фреймворк (замена Task/Coroutine) |
| **DOTween** | Tweening-анимации |
| **Newtonsoft.Json** | JSON сериализация данных |
| **Unity IAP** | Внутриигровые покупки |
| **Unity Ads** | Реклама |

---

## 2. Структура папок

```
Assets/Scripts/
├── Editor/                     # Редакторные скрипты
└── Runtime/
    ├── Bootstrap/              # Точка входа — BootstrapScope/Flow
    │   └── Units/              # ILoadUnit реализации (FooLoadingUnit)
    ├── Configs/                # ScriptableObject конфиги
    │   ├── ActiveSkillConfig/
    │   ├── EnemyConfig/
    │   ├── GameConfig/
    │   ├── ItemConfig/
    │   ├── PlayerConfig/
    │   ├── PhaseConfig/
    │   └── SkillConfig/
    ├── Core/                   # Игровой scope — CoreScope/Flow
    ├── EventBus/               # Шина событий
    ├── Gameplay/               # Игровая логика
    │   ├── ActiveSkills/       # Оружие и способности
    │   ├── Component/          # Переиспользуемые компоненты
    │   ├── Enemy/              # Враги и волны
    │   ├── ItemSystem/         # Предметы и дроп
    │   ├── LevelSystem/        # Система уровней и навыков
    │   ├── Player/             # Игрок
    │   ├── VFX/                # Визуальные эффекты
    │   └── View/               # Игровые View-компоненты
    ├── Loading/                # Loading сцена
    ├── Menu/                   # Главное меню
    ├── Meta/                   # Meta scope (переход к меню)
    ├── Services/               # Глобальные сервисы
    ├── Settings/               # Константы, перечисления, модели кеша
    ├── ScriptableObjects/      # Базовые SO
    ├── UI/                     # UI система
    │   ├── Interfaces/
    │   ├── Pages/              # Models + Views
    │   ├── Popups/
    │   └── UIElements/
    └── Utilities/              # Утилиты (пулинг, логгинг, криптография)
```

---

## 3. Архитектура: Scopes и жизненный цикл

### 3.1 Scopes (VContainer LifetimeScope)

Проект использует иерархию DI-контейнеров:

```
BootstrapScope (Singleton — DontDestroyOnLoad)
│   Регистрирует: LoadingService, DataService, SceneService,
│   VaultService, SoundService, LocalisationService, LoadObjectsService
│
├── LoadingScope (Scoped)
│   └── MetaScope (Scoped)
│       └── MenuScope (Scoped)
│           Регистрирует: UIService, PurchasingService
│
└── CoreScope (Scoped — на каждую игровую сессию)
    Регистрирует: Player, GameplayCamera, InputHandler,
    WaveController, EnemySpawner, ActiveSkillController,
    SkillService, ModificatorContainer, TickService,
    PauseService, UIService, VFXService, ItemSpawner и др.
```

### 3.2 Жизненный цикл приложения

```
[Bootstrap] → [Loading] → [Meta] → [Menu] → [Core/Gameplay] → [Menu] → ...
```

1. **BootstrapFlow.Start()** — грузит `LocalisationService`, `DataService`, `VaultService` → переход на Loading
2. **LoadingFlow.Start()** → **MetaFlow.Start()** → переход на Menu
3. **MenuFlow.Start()** — регистрирует UI-страницы меню, ожидает нажатие "Play"
4. **CoreFlow.Start()** — инициализирует все игровые системы → запускает геймплей
5. По окончании — возврат в Menu

### 3.3 Правила Lifetime

| Lifetime | Когда использовать | Где регистрировать |
|----------|--------------------|--------------------|
| **Singleton** | Глобальные сервисы (сохранения, звук, локализация) | `BootstrapScope` |
| **Scoped** | Игровые системы, живущие одну сессию | `CoreScope` / `MenuScope` |

---

## 4. Ключевые системы

### 4.1 EventBus

**Файлы**: `Runtime/EventBus/`

Глобальная шина событий с WeakReference для предотвращения утечек памяти.

**Доступ**: `EventBusHolder.EventBus` (статический синглтон)

**Правила**:
- События — `readonly struct : IEvent` (нет аллокаций в heap)
- Подписчики реализуют `IEventReceiver<T>`
- Каждый подписчик имеет `UniqueId` (GUID)
- **Обязательно** отписываться в `Dispose()` / `OnDisable()`

**Создание события**:
```csharp
public readonly struct MyNewEvent : IEvent
{
    public readonly int Data;
    public MyNewEvent(int data) { Data = data; }
}
```

**Подписка на событие**:
```csharp
public class MyReceiver : IEventReceiver<MyNewEvent>
{
    public UniqueId Id { get; } = new UniqueId();

    public void Init()
    {
        EventBusHolder.EventBus.Register(this as IEventReceiver<MyNewEvent>);
    }

    public void OnEvent(MyNewEvent @event)
    {
        // обработка
    }

    public void Dispose()
    {
        EventBusHolder.EventBus.Unregister(this as IEventReceiver<MyNewEvent>);
    }
}
```

**Отправка события**:
```csharp
EventBusHolder.EventBus.Raise(new MyNewEvent(42));
```

---

### 4.2 Система сервисов

Все сервисы регистрируются через VContainer. Если сервис требует асинхронной загрузки, он реализует `ILoadUnit`.

```csharp
public interface ILoadUnit
{
    UniTask Load();
}
```

**Список глобальных сервисов** (Singleton в BootstrapScope):

| Сервис | Назначение |
|--------|-----------|
| `DataService` | Сохранение/загрузка данных (JSON-кеш) |
| `VaultService` | Управление валютой (монеты) |
| `SceneService` | Загрузка сцен (с поддержкой Empty scene) |
| `LoadingService` | Оркестрация загрузки ILoadUnit с логированием |
| `SoundService` | Звук и музыка |
| `LocalisationService` | XML-локализация (EN/UA/RU) |
| `LoadObjectsService` | Обёртка над Resources.Load |

**Список игровых сервисов** (Scoped в CoreScope):

| Сервис | Назначение |
|--------|-----------|
| `TickService` | Централизованный Update/FixedUpdate через UniRx |
| `PauseService` | Пауза игры (Time.timeScale) |
| `UIService` | Управление UI-страницами и попапами |
| `SkillService` | Прокачка пассивных и активных навыков |
| `VFXService` | Управление визуальными эффектами |

**Добавление нового сервиса**:

1. Создать класс в `Services/`:
```csharp
public class MyService : ILoadUnit
{
    [Inject]
    public void Construct(DataService dataService) { }

    public async UniTask Load()
    {
        // инициализация
        await UniTask.CompletedTask;
    }
}
```

2. Зарегистрировать в нужном Scope:
```csharp
// Глобальный → BootstrapScope
builder.Register<MyService>(Lifetime.Singleton);

// Игровой → CoreScope
builder.Register<MyService>(Lifetime.Scoped);
```

3. Загрузить во Flow (если `ILoadUnit`):
```csharp
await _loadingService.BeginLoading(_myService);
```

---

### 4.3 Система модификаторов

**Файлы**: `Runtime/Gameplay/LevelSystem/Containers/`

Централизованное управление всеми числовыми характеристиками игрока.

```
ModificatorContainer
  └── Dictionary<ModificatorType, Modificator>
        ├── Damage       (base=1.0, bonus=0.0)
        ├── MaxHealth     (base=100, bonus=0.0)
        ├── Armor         (base=0, bonus=0.0)
        ├── SpeedMoving   (base=..., bonus=0.0)
        ├── CriticalChance
        ├── CriticalDamageMultiplier
        ├── PickUpRadius
        ├── ReceivedExperience
        ├── ReceivingCoins
        └── ... (17 типов)
```

**Интерфейс**: `IReadableModificator`
```csharp
public interface IReadableModificator
{
    float Value { get; }  // BaseValue + BonusValue
}
```

Все системы (Player, ActiveSkills, EnemySpawner) читают `IReadableModificator.Value` каждый кадр. При апгрейде навыка SkillService вызывает `Modificator.AddBonus(value)` → изменения применяются мгновенно.

---

### 4.4 Система активных навыков (ActiveSkills)

**Файлы**: `Runtime/Gameplay/ActiveSkills/`

Архитектура:
```
ActiveSkillController (управляет всеми активными навыками)
  ├── ActiveSkillFactory → создаёт IActiveSkill по типу
  │     └── Builders/ → ActiveSkillBuilder, StandardGunBuilder, DashBuilder...
  │
  ├── IActiveSkill (базовый контракт)
  │     ├── IShootable (стреляющие навыки)
  │     ├── IReloadable (перезаряжаемые навыки)
  │     └── IEvolution (эволюция навыка)
  │
  ├── ProjectileFactory → создаёт пули (ObjectPool)
  ├── EnemyDetectors → CircleEnemyDetector, RaycastEnemyDetector...
  └── BulletModels → BaseBullet, ExplosiveBullet, DamageAreaBullet...
```

**Типы оружия**: StandardGun, AutoGun, MachineGun, RifleGun, RocketGun, EnergyGun, LaserSkill, AuraSkill, DroneSkill  
**Способности**: DashSkill, ShieldSkill, CloakingSkill

**Добавление нового оружия**:

1. Добавить enum в `Settings/Enumerators.cs`:
   - `SkillType.MyGun = N`
   - `ActiveSkillType.MyGun = M`

2. Создать модель в `ActiveSkills/ActiveSkillModels/`:
```csharp
public class MyGun : IActiveSkill, IShootable, IReloadable
{
    public ActiveSkillType SkillType => ActiveSkillType.MyGun;
    public void Init() { }
    public void Tick() { /* перезарядка + стрельба */ }
    public void Evolve() { /* эволюция */ }
    public void Dispose() { }
}
```

3. Создать Builder в `ActiveSkillSystem/Builders/`:
```csharp
public class MyGunBuilder : ActiveSkillBuilder
{
    public override IActiveSkill Build(/* параметры */)
    {
        return new MyGun(/* ... */);
    }
}
```

4. Зарегистрировать в `ActiveSkillFactory.CreateSkill()` — добавить case

5. Создать конфиг в `Configs/ActiveSkillConfig/`

---

### 4.5 Система врагов

**Файлы**: `Runtime/Gameplay/Enemy/`

```
WaveController (управляет волнами)
  └── EnemySpawner (пул врагов + фабрика)
        ├── EnemyFactory → выбирает Builder по EnemyBuilderType
        │     ├── DefaultEnemyBuilder (враг идёт к игроку)
        │     └── SawEnemyBuilder (враг летит в направлении)
        │
        └── Enemy (MonoBehaviour)
              ├── IMove (MoveToTargetComponent / MoveInDirectionComponent)
              ├── IRotation (NoRotationComponent / OnTargetRotateComponent)
              ├── IHealth (BaseHealthComponent)
              ├── AttackComponent
              ├── FreezeComponent
              └── AnimationComponent
```

**Добавление нового типа врага**:

1. Добавить `EnemyType.MyEnemy` в `Enumerators.cs`
2. Создать `EnemyData` конфиг c характеристиками
3. При необходимости — создать новый `EnemyBuilder : EnemyBuilderBase`
4. Добавить case в `EnemyFactory.GetBuilder()`
5. Настроить данные волн (WaveData)

---

### 4.6 Система предметов (Items)

**Файлы**: `Runtime/Gameplay/ItemSystem/`

```
ItemSpawner (управляет появлением предметов)
  ├── ItemFactory → создаёт ItemModel по ItemType
  ├── ObjectPool<ItemView> → переиспользование View
  └── RandomDroper → вероятности дропа
```

**Типы**: SmallXp, MediumXp, BigXp, Coin, Medicine, RocketAmmo, FrozenBomb, Bomb, Chest, Magnet

**Добавление нового предмета**:

1. Добавить `ItemType.MyItem` в `Enumerators.cs`
2. Создать класс в `ItemSystem/ItemModels/`:
```csharp
public class MyItem : ItemModel
{
    public override void ReleseItem(Vector3 position)
    {
        EventBusHolder.EventBus.Raise(new MyItemReleaseEvent());
    }
}
```
3. Добавить case в `ItemFactory`
4. Настроить конфиг (спрайт, вероятность дропа)
5. Обработать событие через `IEventReceiver`

---

### 4.7 UI система

**Файлы**: `Runtime/UI/`

Архитектура Model-View:

```
UIService
  ├── List<IUIPage> — страницы (GamePage, PausePage, LevelUpPage, SettingsPage, GameOverPage)
  └── List<IUIPopup> — попапы
```

**Контракты**:
```csharp
public interface IUIPage
{
    void Init();
    void Show();
    void Hide();
    void Dispose();
}
```

**Добавление новой страницы**:

1. Создать Model (`UI/Pages/Models/`):
```csharp
public class MyPageModel : IUIModel
{
    private readonly UIService _uiService;
    public MyPageModel(UIService uiService) { _uiService = uiService; }

    public void OnAction()
    {
        _uiService.OpenPage<GamePageView>();
    }
}
```

2. Создать View (`UI/Pages/Views/`):
```csharp
public class MyPageView : IUIPage
{
    private readonly MyPageModel _model;
    private GameObject _pageObject;

    public MyPageView(MyPageModel model) { _model = model; }

    public void Init() { _pageObject = /* найти на Canvas */; }
    public void Show() { _pageObject.SetActive(true); }
    public void Hide() { _pageObject.SetActive(false); }
    public void Dispose() { }
}
```

3. Зарегистрировать во Flow:
```csharp
// CoreFlow.RegisterUI() или MenuFlow.RegisterUI()
var pages = new List<IUIPage>
{
    new MyPageView(new MyPageModel(_uiService)),
};
_uiService.RegisterUI(pages, popups);
```

---

### 4.8 TickService

**Файл**: `Runtime/Services/TickService.cs`

Централизованная замена `Update()`/`FixedUpdate()` MonoBehaviour. Автоматически паузится.

```csharp
// Подписаться
_tickService.RegisterFixedUpdate(MyFixedTick);
_tickService.RegisterUpdate(MyTick);

// Отписаться
_tickService.UnregisterFixedUpdate(MyFixedTick);

// Таймер
_tickService.RegisterTimer(TimeSpan.FromSeconds(5), OnTimerCallback);
```

> **Правило**: Не использовать `Update()`/`FixedUpdate()` в MonoBehaviour напрямую. Регистрировать через `TickService`.

---

### 4.9 Система сохранений (DataService)

**Файл**: `Runtime/Services/DataService.cs`

Данные сохраняются в `Application.persistentDataPath` как JSON-файлы.

**Типы кеша**:

| CacheType | Класс | Содержимое |
|-----------|-------|-----------|
| `AppSettingsData` | `AppSettingsData` | Язык, громкость, настройки эффектов |
| `PurchaseData` | `PurchaseData` | Статус покупок (убрана реклама) |
| `PlayerValutData` | `PlayerVaultData` | Монеты, лучший счёт |
| `UserData` | `UserData` | Данные игрока |
| `UpgradeData` | `ModificatorUpgradeData` | Уровни прокачки модификаторов |

**Работа с кешем**:
```csharp
// Чтение
int coins = _dataService.PlayerVaultData.coins;

// Изменение + сохранение
_dataService.PlayerVaultData.coins = 100;
_dataService.SaveCache(CacheType.PlayerValutData);

// Сброс к дефолтным значениям
_dataService.ResetData(CacheType.UserData);
```

---

## 5. Паттерны проекта

| Паттерн | Где используется |
|---------|-----------------|
| **Composition Root / Scope** | `BootstrapScope`, `CoreScope`, `MetaScope`, `MenuScope`, `LoadingScope` |
| **Dependency Injection** | VContainer: `[Inject]`, `Configure()`, `builder.Register<T>()` |
| **EventBus (Observer)** | `EventBusHolder.EventBus` — глобальная шина событий |
| **Builder** | `EnemyBuilderBase`, `ActiveSkillBuilder`, `StandardGunBuilder` |
| **Abstract Factory** | `EnemyFactory`, `ActiveSkillFactory`, `ProjectileFactory` |
| **Object Pool** | `ObjectPool<T>` для пуль, врагов, предметов |
| **Model-View** | UI: `GamePageModel` + `GamePageView` |
| **Strategy** | `IMove`, `IRotation`, `IEnemyBuilder`, `IEnemyDetector` |
| **Component** | `MoveComponent`, `HealthComponent`, `AttackComponent`, `FreezeComponent` |

---

## 6. Соглашения по коду

### 6.1 Namespaces

```
TandC.GeometryAstro.Core
TandC.GeometryAstro.Bootstrap
TandC.GeometryAstro.Gameplay
TandC.GeometryAstro.Services
TandC.GeometryAstro.UI
TandC.GeometryAstro.EventBus
TandC.GeometryAstro.Data
TandC.GeometryAstro.Settings
TandC.GeometryAstro.Utilities
```

### 6.2 Именование

- Приватные поля: `_camelCase` с подчёркиванием
- Публичные свойства: `PascalCase`
- Интерфейсы: `I` префикс (`IMove`, `IHealth`, `IActiveSkill`)
- Enum значения: `PascalCase`
- Константы: `UPPER_CASE` для глобальных, `PascalCase` для приватных

### 6.3 Общие правила

1. **Async**: Только `UniTask`. Никаких `System.Threading.Tasks.Task` или `Coroutine`
2. **DI**: Всегда через VContainer. Не использовать `new` для сервисов
3. **Events**: Только через `EventBusHolder.EventBus`. События — `readonly struct`
4. **Update loop**: Через `TickService`, не напрямую в MonoBehaviour
5. **Пулинг**: `ObjectPool<T>` для часто создаваемых/уничтожаемых объектов
6. **Данные**: Конфиги — ScriptableObject. Сохранения — JSON через `DataService`
7. **Dispose**: Всегда отписываться от EventBus и TickService в `Dispose()`

---

## 7. Быстрая справка: частые операции

### Открыть UI-страницу
```csharp
_uiService.OpenPage<GameOverPageView>();
```

### Показать попап
```csharp
_uiService.ShowPopup<MyPopup>(data);
```

### Загрузить сцену
```csharp
_sceneService.LoadScene(RuntimeConstants.Scenes.Menu).Forget();
```

### Поставить игру на паузу
```csharp
EventBusHolder.EventBus.Raise(new PauseGameEvent(true));   // Пауза
EventBusHolder.EventBus.Raise(new PauseGameEvent(false));  // Снять паузу
```

### Добавить монеты
```csharp
_vaultService.Coins.Add(100);
```

### Получить модификатор
```csharp
IReadableModificator damage = _modificatorContainer.GetModificator(ModificatorType.Damage);
float currentDamage = damage.Value;
```

### Получить локализованную строку
```csharp
string text = _localisationService.GetString("key_name");
```

---

## 8. Сцены (Build Index)

| Константа | Назначение |
|-----------|-----------|
| `RuntimeConstants.Scenes.Bootstrap` | Инициализация |
| `RuntimeConstants.Scenes.Loading` | Экран загрузки |
| `RuntimeConstants.Scenes.Empty` | Пустая сцена (для перехода между скоупами) |
| `RuntimeConstants.Scenes.Meta` | Мета-переход |
| `RuntimeConstants.Scenes.Menu` | Главное меню |
| `RuntimeConstants.Scenes.Core` | Геймплей |
