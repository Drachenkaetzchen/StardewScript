using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace StardewScript.Framework
{
    // ReSharper disable once UnusedType.Global
    public abstract class Script : IScript
    {
        #region Protected Members

        /// <summary>
        /// The mod helper
        /// </summary>
        /// ReSharper disable once MemberCanBePrivate.Global
        protected IModHelper ModHelper { get; private set; }

        /// <summary>
        /// The monitor
        /// </summary>
        /// ReSharper disable once MemberCanBePrivate.Global
        protected IMonitor Monitor { get; private set; }

        #endregion
        
        private Action<string, Exception> _emergencyCallback;

        private string ScriptFilename { get; set; }

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

        #region Non-overridable Initialize/DeInitialize methods

        public void Initialize(IModHelper helper, IMonitor monitor)
        {
            if (_emergencyCallback == null)
            {
                Monitor.Log("The emergency shutdown callback is not set, can't initialize script.", LogLevel.Error);
                return;
            }

            ModHelper = helper;
            Monitor = monitor;

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

            InitializeScript();
        }

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

        private void InvokeEmergencyShutdownCallback(Exception e)
        {
            _emergencyCallback.Invoke(ScriptFilename, e);
        }

        public void SetEmergencyShutdownCallback(string scriptFilename, Action<string, Exception> callback)
        {
            ScriptFilename = scriptFilename;
            _emergencyCallback = callback;
        }

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


        #region Display Event Stubs

        /// <summary>Raised after a game menu is opened, closed, or replaced.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
        }

        /// <summary>Raised before the game draws anything to the screen in a draw tick, as soon as the sprite batch is opened. The sprite batch may be closed and reopened multiple times after this event is called, but it's only raised once per draw tick. This event isn't useful for drawing to the screen, since the game will draw over it.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnRendering(object sender, RenderingEventArgs e)
        {
        }

        /// <summary>Raised after the game draws to the sprite patch in a draw tick, just before the final sprite batch is rendered to the screen. Since the game may open/close the sprite batch multiple times in a draw tick, the sprite batch may not contain everything being drawn and some things may already be rendered to the screen. Content drawn to the sprite batch at this point will be drawn over all vanilla content (including menus, HUD, and cursor).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnRendered(object sender, RenderedEventArgs e)
        {
        }

        /// <summary>Raised before the game world is drawn to the screen. This event isn't useful for drawing to the screen, since the game will draw over it.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnRenderingWorld(object sender, RenderingWorldEventArgs e)
        {
        }

        /// <summary>Raised after the game world is drawn to the sprite patch, before it's rendered to the screen. Content drawn to the sprite batch at this point will be drawn over the world, but under any active menu, HUD elements, or cursor.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnRenderedWorld(object sender, RenderedWorldEventArgs e)
        {
        }

        /// <summary>When a menu is open (<see cref="Game1.activeClickableMenu"/> isn't null), raised before that menu is drawn to the screen. This includes the game's internal menus like the title screen. Content drawn to the sprite batch at this point will appear under the menu.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnRenderingActiveMenu(object sender, RenderingActiveMenuEventArgs e)
        {
        }

        /// <summary>When a menu is open (<see cref="Game1.activeClickableMenu"/> isn't null), raised after that menu is drawn to the sprite batch but before it's rendered to the screen. Content drawn to the sprite batch at this point will appear over the menu and menu cursor.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnRenderedActiveMenu(object sender, RenderedActiveMenuEventArgs e)
        {
        }

        /// <summary>Raised before drawing the HUD (item toolbar, clock, etc) to the screen. The vanilla HUD may be hidden at this point (e.g. because a menu is open). Content drawn to the sprite batch at this point will appear under the HUD.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnRenderingHud(object sender, RenderingHudEventArgs e)
        {
        }

        /// <summary>Raised after drawing the HUD (item toolbar, clock, etc) to the sprite batch, but before it's rendered to the screen. The vanilla HUD may be hidden at this point (e.g. because a menu is open). Content drawn to the sprite batch at this point will appear over the HUD.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnRenderedHud(object sender, RenderedHudEventArgs e)
        {
        }

        /// <summary>Raised after the game window is resized.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnWindowResized(object sender, WindowResizedEventArgs e)
        {
        }

        #endregion

        #region Game Loop Event Stubs

        /// <summary>Raised after the game is launched, right before the first update tick. This happens once per game session (unrelated to loading saves). All mods are loaded and initialized at this point, so this is a good time to set up mod integrations.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
        }

        /// <summary>Raised before the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnUpdateTicking(object sender, UpdateTickingEventArgs e)
        {
        }

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
        }

        /// <summary>Raised once per second before the game state is updated.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnOneSecondUpdateTicking(object sender, OneSecondUpdateTickingEventArgs e)
        {
        }

        /// <summary>Raised once per second after the game state is updated.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnOneSecondUpdateTicked(object sender, OneSecondUpdateTickedEventArgs e)
        {
        }

        /// <summary>Raised before the game creates a new save file.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnSaveCreating(object sender, SaveCreatingEventArgs e)
        {
        }

        /// <summary>Raised after the game finishes creating the save file.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnSaveCreated(object sender, SaveCreatedEventArgs e)
        {
        }

        /// <summary>Raised before the game begins writing data to the save file (except the initial save creation).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnSaving(object sender, SavingEventArgs e)
        {
        }

        /// <summary>Raised after the game finishes writing data to the save file (except the initial save creation).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnSaved(object sender, SavedEventArgs e)
        {
        }

        /// <summary>Raised after the player loads a save slot and the world is initialized.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
        }

        /// <summary>Raised after the game begins a new day (including when the player loads a save).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnDayStarted(object sender, DayStartedEventArgs e)
        {
        }

        /// <summary>Raised before the game ends the current day. This happens before it starts setting up the next day and before <see cref="OnSaving"/>.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnDayEnding(object sender, DayEndingEventArgs e)
        {
        }

        /// <summary>Raised after the in-game clock time changes.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnTimeChanged(object sender, TimeChangedEventArgs e)
        {
        }

        /// <summary>Raised after the game returns to the title screen.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnReturnedToTitle(object sender, ReturnedToTitleEventArgs e)
        {
        }

        #endregion

        #region Input Event Stubs

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
        }

        /// <summary>Raised after the player releases a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnButtonReleased(object sender, ButtonReleasedEventArgs e)
        {
        }

        /// <summary>Raised after the player moves the in-game cursor.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnCursorMoved(object sender, CursorMovedEventArgs e)
        {
        }

        /// <summary>Raised after the player scrolls the mouse wheel.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnMouseWheelScrolled(object sender, MouseWheelScrolledEventArgs e)
        {
        }

        #endregion

        #region Multiplayer Event Stubs

        /// <summary>Raised after the mod context for a peer is received. This happens before the game approves the connection, so the player doesn't yet exist in the game. This is the earliest point where messages can be sent to the peer via SMAPI.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnPeerContextReceived(object sender, PeerContextReceivedEventArgs e)
        {
        }

        /// <summary>Raised after a mod message is received over the network.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnModMessageReceived(object sender, ModMessageReceivedEventArgs e)
        {
        }

        /// <summary>Raised after the connection with a peer is severed.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnPeerDisconnected(object sender, PeerDisconnectedEventArgs e)
        {
        }

        #endregion

        #region Player Event Stubs

        /// <summary>Raised after items are added or removed to a player's inventory. NOTE: this event is currently only raised for the current player.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnInventoryChanged(object sender, InventoryChangedEventArgs e)
        {
        }

        /// <summary>Raised after a player skill level changes. This happens as soon as they level up, not when the game notifies the player after their character goes to bed.  NOTE: this event is currently only raised for the current player.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnLevelChanged(object sender, LevelChangedEventArgs e)
        {
        }

        /// <summary>Raised after a player warps to a new location. NOTE: this event is currently only raised for the current player.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnWarped(object sender, WarpedEventArgs e)
        {
        }

        #endregion

        #region World Event Stubs

        /// <summary>Raised after a game location is added or removed.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnLocationListChanged(object sender, LocationListChangedEventArgs e)
        {
        }

        /// <summary>Raised after buildings are added or removed in a location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnBuildingListChanged(object sender, BuildingListChangedEventArgs e)
        {
        }

        /// <summary>Raised after debris are added or removed in a location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnDebrisListChanged(object sender, DebrisListChangedEventArgs e)
        {
        }

        /// <summary>Raised after large terrain features (like bushes) are added or removed in a location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnLargeTerrainFeatureListChanged(object sender,
            LargeTerrainFeatureListChangedEventArgs e)
        {
        }

        /// <summary>Raised after NPCs are added or removed in a location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnNpcListChanged(object sender, NpcListChangedEventArgs e)
        {
        }

        /// <summary>Raised after objects are added or removed in a location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnObjectListChanged(object sender, ObjectListChangedEventArgs e)
        {
        }

        /// <summary>Raised after items are added or removed from a chest.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnChestInventoryChanged(object sender, ChestInventoryChangedEventArgs e)
        {
        }

        /// <summary>Raised after terrain features (like floors and trees) are added or removed in a location.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnTerrainFeatureListChanged(object sender, TerrainFeatureListChangedEventArgs e)
        {
        }

        #endregion


        #region Script-defined entry point

        /// <summary>
        /// Initializes the script. This is executed as soon as the script is loaded.
        /// </summary>
        protected virtual void InitializeScript()
        {
        }

        #endregion
    }
}