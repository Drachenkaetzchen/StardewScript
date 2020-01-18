using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace StardewScript.Framework
{
    // ReSharper disable once UnusedType.Global
    public abstract partial class Script : IScript
    {
        /// <summary>
        /// The emergency callback
        /// </summary>
        private Action<string, Exception> _emergencyCallback;

        /// <summary>
        /// The filename of the script itself
        /// </summary>
        private string ScriptFilename { get; set; }

        #region Event Handlers

        #region Display Event Handlers

        private EventHandler<MenuChangedEventArgs> _menuChangedEventHandler;
        private EventHandler<RenderingEventArgs> _renderingEventHandler;
        private EventHandler<RenderedEventArgs> _renderedEventHandler;
        private EventHandler<RenderingWorldEventArgs> _renderingWorldEventHandler;
        private EventHandler<RenderedWorldEventArgs> _renderedWorldEventHandler;
        private EventHandler<RenderingActiveMenuEventArgs> _renderingActiveMenuEventHandler;
        private EventHandler<RenderedActiveMenuEventArgs> _renderedActiveMenuEventHandler;
        private EventHandler<RenderingHudEventArgs> _renderingHudEventHandler;
        private EventHandler<RenderedHudEventArgs> _renderedHudEventHandler;
        private EventHandler<WindowResizedEventArgs> _windowResizedEventHandler;

        #endregion

        #region Game Loop Event Handlers

        private EventHandler<GameLaunchedEventArgs> _gameLaunchedEventHandler;
        private EventHandler<UpdateTickingEventArgs> _updateTickingEventHandler;
        private EventHandler<UpdateTickedEventArgs> _updateTickedEventHandler;
        private EventHandler<OneSecondUpdateTickingEventArgs> _oneSecondUpdateTickingEventHandler;
        private EventHandler<OneSecondUpdateTickedEventArgs> _oneSecondUpdateTickedEventHandler;
        private EventHandler<SaveCreatingEventArgs> _saveCreatingEventHandler;
        private EventHandler<SaveCreatedEventArgs> _saveCreatedEventHandler;
        private EventHandler<SavingEventArgs> _savingEventHandler;
        private EventHandler<SavedEventArgs> _savedEventHandler;
        private EventHandler<SaveLoadedEventArgs> _saveLoadedEventHandler;
        private EventHandler<DayStartedEventArgs> _dayStartedEventHandler;
        private EventHandler<DayEndingEventArgs> _dayEndingEventHandler;
        private EventHandler<TimeChangedEventArgs> _timeChangedEventHandler;
        private EventHandler<ReturnedToTitleEventArgs> _returnedToTitleEventHandler;

        #endregion

        #region Input Event Handlers

        private EventHandler<ButtonPressedEventArgs> _buttonPressedEventHandler;
        private EventHandler<ButtonReleasedEventArgs> _buttonReleasedEventHandler;
        private EventHandler<CursorMovedEventArgs> _cursorMovedEventHandler;
        private EventHandler<MouseWheelScrolledEventArgs> _mouseWheelScrolledEventHandler;

        #endregion

        #region Multiplayer Event Handlers

        private EventHandler<PeerContextReceivedEventArgs> _peerContextReceivedEventHandler;
        private EventHandler<ModMessageReceivedEventArgs> _modMessageReceivedEventHandler;
        private EventHandler<PeerDisconnectedEventArgs> _peerDisconnectedEventHandler;

        #endregion

        #region Player Event Handlers

        private EventHandler<InventoryChangedEventArgs> _inventoryChangedEventHandler;
        private EventHandler<LevelChangedEventArgs> _levelChangedEventHandler;
        private EventHandler<WarpedEventArgs> _warpedEventHandler;

        #endregion

        #region World Event Handlers

        private EventHandler<LocationListChangedEventArgs> _locationListChangedEventHandler;
        private EventHandler<BuildingListChangedEventArgs> _buildingListChangedEventHandler;
        private EventHandler<ChestInventoryChangedEventArgs> _chestInventoryChangedEventHandler;
        private EventHandler<LargeTerrainFeatureListChangedEventArgs> _largeTerrainFeatureListChangedEventHandler;
        private EventHandler<DebrisListChangedEventArgs> _debrisListChangedEventHandler;
        private EventHandler<NpcListChangedEventArgs> _npcListChangedEventHandler;
        private EventHandler<ObjectListChangedEventArgs> _objectListChangedEventHandler;
        private EventHandler<TerrainFeatureListChangedEventArgs> _terrainFeatureListChangedEventHandler;

        #endregion

        #endregion

        #region Non-overridable Initialize/DeInitialize methods

        /// <summary>
        /// Initializes the mod. The emergency callback must be set prior to calling this.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="monitor"></param>
        public void Initialize(IModHelper helper, IMonitor monitor)
        {
            if (_emergencyCallback == null)
            {
                Monitor.Log("The emergency shutdown callback is not set, can't initialize script.", LogLevel.Error);
                return;
            }

            ModHelper = helper;
            Monitor = monitor;

            #region EventHandler Setup

            // Display Events
            _menuChangedEventHandler = DelegateEvent<MenuChangedEventArgs>(OnMenuChanged);
            _renderingEventHandler = DelegateEvent<RenderingEventArgs>(OnRendering);
            _renderedEventHandler = DelegateEvent<RenderedEventArgs>(OnRendered);
            _renderingWorldEventHandler = DelegateEvent<RenderingWorldEventArgs>(OnRenderingWorld);
            _renderedWorldEventHandler = DelegateEvent<RenderedWorldEventArgs>(OnRenderedWorld);
            _renderingActiveMenuEventHandler = DelegateEvent<RenderingActiveMenuEventArgs>(OnRenderingActiveMenu);
            _renderedActiveMenuEventHandler = DelegateEvent<RenderedActiveMenuEventArgs>(OnRenderedActiveMenu);
            _renderingHudEventHandler = DelegateEvent<RenderingHudEventArgs>(OnRenderingHud);
            _renderedHudEventHandler = DelegateEvent<RenderedHudEventArgs>(OnRenderedHud);
            _windowResizedEventHandler = DelegateEvent<WindowResizedEventArgs>(OnWindowResized);

            // Game Loop Events
            _gameLaunchedEventHandler = DelegateEvent<GameLaunchedEventArgs>(OnGameLaunched);
            _updateTickingEventHandler = DelegateEvent<UpdateTickingEventArgs>(OnUpdateTicking);
            _updateTickedEventHandler = DelegateEvent<UpdateTickedEventArgs>(OnUpdateTicked);

            _oneSecondUpdateTickingEventHandler =
                DelegateEvent<OneSecondUpdateTickingEventArgs>(OnOneSecondUpdateTicking);

            _oneSecondUpdateTickedEventHandler = DelegateEvent<OneSecondUpdateTickedEventArgs>(OnOneSecondUpdateTicked);
            _saveCreatingEventHandler = DelegateEvent<SaveCreatingEventArgs>(OnSaveCreating);
            _saveCreatedEventHandler = DelegateEvent<SaveCreatedEventArgs>(OnSaveCreated);
            _savingEventHandler = DelegateEvent<SavingEventArgs>(OnSaving);
            _savedEventHandler = DelegateEvent<SavedEventArgs>(OnSaved);
            _saveLoadedEventHandler = DelegateEvent<SaveLoadedEventArgs>(OnSaveLoaded);
            _dayStartedEventHandler = DelegateEvent<DayStartedEventArgs>(OnDayStarted);
            _dayEndingEventHandler = DelegateEvent<DayEndingEventArgs>(OnDayEnding);
            _timeChangedEventHandler = DelegateEvent<TimeChangedEventArgs>(OnTimeChanged);
            _returnedToTitleEventHandler = DelegateEvent<ReturnedToTitleEventArgs>(OnReturnedToTitle);

            // Input Events
            _buttonPressedEventHandler = DelegateEvent<ButtonPressedEventArgs>(OnButtonPressed);
            _buttonReleasedEventHandler = DelegateEvent<ButtonReleasedEventArgs>(OnButtonReleased);
            _cursorMovedEventHandler = DelegateEvent<CursorMovedEventArgs>(OnCursorMoved);
            _mouseWheelScrolledEventHandler = DelegateEvent<MouseWheelScrolledEventArgs>(OnMouseWheelScrolled);

            // Multiplayer Events
            _peerContextReceivedEventHandler = DelegateEvent<PeerContextReceivedEventArgs>(OnPeerContextReceived);
            _modMessageReceivedEventHandler = DelegateEvent<ModMessageReceivedEventArgs>(OnModMessageReceived);
            _peerDisconnectedEventHandler = DelegateEvent<PeerDisconnectedEventArgs>(OnPeerDisconnected);

            // Player Events
            _inventoryChangedEventHandler = DelegateEvent<InventoryChangedEventArgs>(OnInventoryChanged);
            _levelChangedEventHandler = DelegateEvent<LevelChangedEventArgs>(OnLevelChanged);
            _warpedEventHandler = DelegateEvent<WarpedEventArgs>(OnWarped);

            // World Events
            _locationListChangedEventHandler = DelegateEvent<LocationListChangedEventArgs>(OnLocationListChanged);
            _buildingListChangedEventHandler = DelegateEvent<BuildingListChangedEventArgs>(OnBuildingListChanged);
            _chestInventoryChangedEventHandler = DelegateEvent<ChestInventoryChangedEventArgs>(OnChestInventoryChanged);

            _largeTerrainFeatureListChangedEventHandler =
                DelegateEvent<LargeTerrainFeatureListChangedEventArgs>(OnLargeTerrainFeatureListChanged);

            _debrisListChangedEventHandler = DelegateEvent<DebrisListChangedEventArgs>(OnDebrisListChanged);
            _npcListChangedEventHandler = DelegateEvent<NpcListChangedEventArgs>(OnNpcListChanged);
            _objectListChangedEventHandler = DelegateEvent<ObjectListChangedEventArgs>(OnObjectListChanged);

            _terrainFeatureListChangedEventHandler =
                DelegateEvent<TerrainFeatureListChangedEventArgs>(OnTerrainFeatureListChanged);

            #endregion

            #region Attach Event Handlers

            // Display Events
            ModHelper.Events.Display.MenuChanged += _menuChangedEventHandler;
            ModHelper.Events.Display.Rendering += _renderingEventHandler;
            ModHelper.Events.Display.Rendered += _renderedEventHandler;
            ModHelper.Events.Display.RenderingWorld += _renderingWorldEventHandler;
            ModHelper.Events.Display.RenderedWorld += _renderedWorldEventHandler;
            ModHelper.Events.Display.RenderingActiveMenu += _renderingActiveMenuEventHandler;
            ModHelper.Events.Display.RenderedActiveMenu += _renderedActiveMenuEventHandler;
            ModHelper.Events.Display.RenderingHud += _renderingHudEventHandler;
            ModHelper.Events.Display.RenderedHud += _renderedHudEventHandler;
            ModHelper.Events.Display.WindowResized += _windowResizedEventHandler;

            // Game Loop Events
            ModHelper.Events.GameLoop.GameLaunched += _gameLaunchedEventHandler;
            ModHelper.Events.GameLoop.UpdateTicking += _updateTickingEventHandler;
            ModHelper.Events.GameLoop.UpdateTicked += _updateTickedEventHandler;
            ModHelper.Events.GameLoop.OneSecondUpdateTicking += _oneSecondUpdateTickingEventHandler;
            ModHelper.Events.GameLoop.OneSecondUpdateTicked += _oneSecondUpdateTickedEventHandler;
            ModHelper.Events.GameLoop.SaveCreating += _saveCreatingEventHandler;
            ModHelper.Events.GameLoop.SaveCreated += _saveCreatedEventHandler;
            ModHelper.Events.GameLoop.Saving += _savingEventHandler;
            ModHelper.Events.GameLoop.Saved += _savedEventHandler;
            ModHelper.Events.GameLoop.SaveLoaded += _saveLoadedEventHandler;
            ModHelper.Events.GameLoop.DayStarted += _dayStartedEventHandler;
            ModHelper.Events.GameLoop.DayEnding += _dayEndingEventHandler;
            ModHelper.Events.GameLoop.TimeChanged += _timeChangedEventHandler;
            ModHelper.Events.GameLoop.ReturnedToTitle += _returnedToTitleEventHandler;

            // Input Events
            ModHelper.Events.Input.ButtonPressed += _buttonPressedEventHandler;
            ModHelper.Events.Input.ButtonReleased += _buttonReleasedEventHandler;
            ModHelper.Events.Input.CursorMoved += _cursorMovedEventHandler;
            ModHelper.Events.Input.MouseWheelScrolled += _mouseWheelScrolledEventHandler;

            // Multiplayer Events
            ModHelper.Events.Multiplayer.PeerContextReceived += _peerContextReceivedEventHandler;
            ModHelper.Events.Multiplayer.ModMessageReceived += _modMessageReceivedEventHandler;
            ModHelper.Events.Multiplayer.PeerDisconnected += _peerDisconnectedEventHandler;

            // Player Events
            ModHelper.Events.Player.InventoryChanged += _inventoryChangedEventHandler;
            ModHelper.Events.Player.LevelChanged += _levelChangedEventHandler;
            ModHelper.Events.Player.Warped += _warpedEventHandler;

            // World Events
            ModHelper.Events.World.LocationListChanged += _locationListChangedEventHandler;
            ModHelper.Events.World.BuildingListChanged += _buildingListChangedEventHandler;
            ModHelper.Events.World.ChestInventoryChanged += _chestInventoryChangedEventHandler;
            ModHelper.Events.World.LargeTerrainFeatureListChanged += _largeTerrainFeatureListChangedEventHandler;
            ModHelper.Events.World.DebrisListChanged += _debrisListChangedEventHandler;
            ModHelper.Events.World.NpcListChanged += _npcListChangedEventHandler;
            ModHelper.Events.World.ObjectListChanged += _objectListChangedEventHandler;
            ModHelper.Events.World.TerrainFeatureListChanged += _terrainFeatureListChangedEventHandler;

            #endregion

            InitializeScript();
        }

        /// <summary>
        /// Wraps the given event in an exception handler to allow for emergency shutdown
        /// </summary>
        /// <param name="func">The actual event handler.</param>
        /// <typeparam name="TEventArgs">The event type.</typeparam>
        /// <returns>The wrapped event handler</returns>
        private EventHandler<TEventArgs> DelegateEvent<TEventArgs>(Action<object, TEventArgs> func)
        {
            return (sender, args) =>
            {
                try
                {
                    func.Invoke(sender, args);
                }
                catch (Exception e)
                {
                    InvokeEmergencyShutdownCallback(e);
                }
            };
        }

        /// <summary>
        /// Invokes the emergency shutdown callback
        /// </summary>
        /// <param name="e">The occurred exception.</param>
        private void InvokeEmergencyShutdownCallback(Exception e)
        {
            _emergencyCallback.Invoke(ScriptFilename, e);
        }

        /// <summary>
        /// Sets a callback to invoke when an exception in a script occurred.
        /// </summary>
        /// <param name="scriptFilename"></param>
        /// <param name="callback"></param>
        public void SetEmergencyShutdownCallback(string scriptFilename, Action<string, Exception> callback)
        {
            ScriptFilename = scriptFilename;
            _emergencyCallback = callback;
        }

        /// <summary>
        /// De-Initializes the script. Removes all event handlers.
        /// </summary>
        public void DeInitialize()
        {
            // Display Events
            ModHelper.Events.Display.MenuChanged -= _menuChangedEventHandler;
            ModHelper.Events.Display.Rendering -= _renderingEventHandler;
            ModHelper.Events.Display.Rendered -= _renderedEventHandler;
            ModHelper.Events.Display.RenderingWorld -= _renderingWorldEventHandler;
            ModHelper.Events.Display.RenderedWorld -= _renderedWorldEventHandler;
            ModHelper.Events.Display.RenderingActiveMenu -= _renderingActiveMenuEventHandler;
            ModHelper.Events.Display.RenderedActiveMenu -= _renderedActiveMenuEventHandler;
            ModHelper.Events.Display.RenderingHud -= _renderingHudEventHandler;
            ModHelper.Events.Display.RenderedHud -= _renderedHudEventHandler;
            ModHelper.Events.Display.WindowResized -= _windowResizedEventHandler;

            // Game Loop Events
            ModHelper.Events.GameLoop.GameLaunched -= _gameLaunchedEventHandler;
            ModHelper.Events.GameLoop.UpdateTicking -= _updateTickingEventHandler;
            ModHelper.Events.GameLoop.UpdateTicked -= _updateTickedEventHandler;
            ModHelper.Events.GameLoop.OneSecondUpdateTicking -= _oneSecondUpdateTickingEventHandler;
            ModHelper.Events.GameLoop.OneSecondUpdateTicked -= _oneSecondUpdateTickedEventHandler;
            ModHelper.Events.GameLoop.SaveCreating -= _saveCreatingEventHandler;
            ModHelper.Events.GameLoop.SaveCreated -= _saveCreatedEventHandler;
            ModHelper.Events.GameLoop.Saving -= _savingEventHandler;
            ModHelper.Events.GameLoop.Saved -= _savedEventHandler;
            ModHelper.Events.GameLoop.SaveLoaded -= _saveLoadedEventHandler;
            ModHelper.Events.GameLoop.DayStarted -= _dayStartedEventHandler;
            ModHelper.Events.GameLoop.DayEnding -= _dayEndingEventHandler;
            ModHelper.Events.GameLoop.TimeChanged -= _timeChangedEventHandler;
            ModHelper.Events.GameLoop.ReturnedToTitle -= _returnedToTitleEventHandler;

            // Input Events
            ModHelper.Events.Input.ButtonPressed -= _buttonPressedEventHandler;
            ModHelper.Events.Input.ButtonReleased -= _buttonReleasedEventHandler;
            ModHelper.Events.Input.CursorMoved -= _cursorMovedEventHandler;
            ModHelper.Events.Input.MouseWheelScrolled -= _mouseWheelScrolledEventHandler;

            // Multiplayer Events
            ModHelper.Events.Multiplayer.PeerContextReceived -= _peerContextReceivedEventHandler;
            ModHelper.Events.Multiplayer.ModMessageReceived -= _modMessageReceivedEventHandler;
            ModHelper.Events.Multiplayer.PeerDisconnected -= _peerDisconnectedEventHandler;

            // Player Events
            ModHelper.Events.Player.InventoryChanged -= _inventoryChangedEventHandler;
            ModHelper.Events.Player.LevelChanged -= _levelChangedEventHandler;
            ModHelper.Events.Player.Warped -= _warpedEventHandler;

            // World Events
            ModHelper.Events.World.LocationListChanged -= _locationListChangedEventHandler;
            ModHelper.Events.World.BuildingListChanged -= _buildingListChangedEventHandler;
            ModHelper.Events.World.ChestInventoryChanged -= _chestInventoryChangedEventHandler;
            ModHelper.Events.World.LargeTerrainFeatureListChanged -= _largeTerrainFeatureListChangedEventHandler;
            ModHelper.Events.World.DebrisListChanged -= _debrisListChangedEventHandler;
            ModHelper.Events.World.NpcListChanged -= _npcListChangedEventHandler;
            ModHelper.Events.World.ObjectListChanged -= _objectListChangedEventHandler;
            ModHelper.Events.World.TerrainFeatureListChanged -= _terrainFeatureListChangedEventHandler;
        }

        public void Dispose()
        {
        }

        #endregion

    }
}