#nullable enable
using BlackberryMead.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A navigatable, interactable interface accessible by the player.
    /// </summary>
    public class UserInterface
    {
        /// <summary>
        /// An empty UIGroup.
        /// </summary>
        /// <remarks>Null-Object Pattern implimentation.</remarks>
        public static readonly Window EmptyWindow = new(new Dictionary<string, UIComponent>(),
            new List<string> { }, new Size(0, 0), new Rectangle(0, 0, 0, 0),
            Alignment.Top, Alignment.Left, 1, 0, 0);

        /// <summary>
        /// Default deserialization options for deserializing UIComponents.
        /// </summary>
        /// <remarks>
        /// Does not allow deserialization of custom <see cref="UIComponent"/>. To do so,
        /// override <see cref="UIPolyTypeResolver"/>.
        /// </remarks>
        public static JsonSerializerOptions DefaultDeserializationOptions = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            IncludeFields = true,
            Converters = { new JsonStringEnumConverter() },
            TypeInfoResolver = new UIPolyTypeResolver()
        };

        /// <summary>
        /// SpriteSheet of UserInterface graphical components.
        /// </summary>
        public static Texture2D? SpriteSheet;

        /// <summary>
        /// UIComponents that constitute this <see cref="UserInterface"/>. 
        /// </summary>
        /// <remarks>Keys are component/structure names, values are the components they represent.</remarks>
        public Dictionary<string, Window> Windows { get; set; }

        /// <summary>
        /// Whether the UI is currently visible or not.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Whether a non-default window is currently open.
        /// </summary>
        public bool IsWindowOpen { get; protected set; }

        /// <summary>
        /// Name of the Main Window of this.
        /// </summary>
        public string MainWindowName { get; private set; }

        /// <summary>
        /// Name of the default <see cref="Window"/> of this. <br/>
        /// If a window with the given name does not exist, an empty window will be used instead.
        /// </summary>
        public string DefaultWindowName
        {
            get { return defaultWindowName; }
            set
            {
                defaultWindowName = value;
                if (value != "")
                    defaultWindow = Windows.TryGetValue(defaultWindowName, out Window? window) ? window : EmptyWindow;
                else
                    defaultWindow = EmptyWindow;
            }
        }

        /// <summary>
        /// Scale at which this is drawn.
        /// </summary>
        public float RenderScale
        {
            get => renderScale;
            protected set
            {
                renderScale = value;
            }
        }
        private float renderScale;


        /// <summary>
        /// All <see cref="UIComponent"/> children contained in this.
        /// </summary>
        public Dictionary<string, UIComponent> Children { get; protected set; }

        /// <summary>
        /// List of actions subscribed to by the current <see cref="mainWindow"/>.
        /// </summary>
        public List<string> CurrentWindowActions { get { return mainWindow.Actions; } }

        /// <summary>
        /// Whether the borders of all the UIElements are drawn.
        /// </summary>
        public bool ShowBorders { get; set; }

        /// <summary>
        /// Maximum level to which the UI can be zoomed out
        /// </summary>
        public float MaxZoomOutLevel { get; init; }

        /// <summary>
        /// Event thrown when all required content is loaded.
        /// </summary>
        public event EventHandler OnContentLoaded;

        /// <summary>
        /// Camera of the UserInterface.
        /// </summary>
        protected readonly OrthographicCamera camera;

        /// <summary>
        /// Render Target this is drawn to.
        /// </summary>
        /// <remarks>A <see cref="UserInterface"/> is first drawn to a RenderTarget and then to the screen.</remarks>
        protected RenderTarget2D renderTarget;

        /// <summary>
        /// Graphics device needed to create <see cref="RenderTarget2D"/>.
        /// </summary>
        protected readonly GraphicsDevice graphicsDevice;

        /// <summary>
        /// UIGroup that is the currently displayed window the user is interacting with.
        /// </summary>
        protected Window mainWindow;

        /// <summary>
        /// Default window that opens when all others are closed.
        /// </summary>
        /// <remarks>Default value is <see cref="EmptyWindow"/>.</remarks>
        protected Window defaultWindow;

        /// <inheritdoc cref="DefaultWindowName"/>
        protected string defaultWindowName = "";

        /// <summary>
        /// Size of the screen.
        /// </summary>
        protected Size ScreenDim { get; }

        /// <summary>
        /// Dimensions of <see cref="renderTarget"/>.
        /// </summary>
        protected Size RenderDim { get; set; }

        /// <summary>
        /// Subsection of the RenderDim that gets drawn to the screen.
        /// </summary>
        protected Rectangle viewport { get; set; }

        /// <summary>
        /// Children of this that are <see cref="IRenderable"/>.
        /// </summary>
        private List<IRenderable> renderableChildren = new();

        /// <summary>
        /// ContentManager for calling <see cref="UIComponent.LoadContent(ContentManager)"/>.
        /// </summary>
        private ContentManager content;


        /// <summary>
        /// Creates a new UserInterface from a list of UIComponents.
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice for drawing</param>
        /// <param name="windows">Dictionary of UIWindows in this. Keys are window names, values UIWindows</param>
        /// <param name="ScreenDim">Size of the screen. </param>
        public UserInterface(GraphicsDevice graphicsDevice, ContentManager content, Dictionary<string, Window> windows, Size ScreenDim,
            float maxZoomOutLevel)
        {
            Windows = windows;
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            mainWindow = EmptyWindow;
            MainWindowName = "";
            defaultWindow = EmptyWindow;
            MaxZoomOutLevel = maxZoomOutLevel;
            this.ScreenDim = ScreenDim;
            this.RenderDim = ScreenDim / MaxZoomOutLevel;
            viewport = new Rectangle((RenderDim - ScreenDim) / 2, ScreenDim);

            // Initialize with default render scale
            RenderScale = 1f;

            // Create new camera
            camera = new(graphicsDevice);
            camera.LookAt(new Vector2((int)(RenderDim.Width / 2), (int)(RenderDim.Height / 2)));

            // Create rendertarget
            renderTarget = new RenderTarget2D(graphicsDevice, RenderDim.Width,
                RenderDim.Height);

            // Align components
            RealignComponents();

            // Set Children
            Children = GetChildren();

            // Get list of IRenderables
            renderableChildren = Children.Values.OfType<IRenderable>().ToList();
            Initialize(graphicsDevice);

            if (ShowBorders)
            {
                foreach (UIComponent child in Children.Values)
                    child.ShowBorder = true;
            }

            // Set children content
            foreach (UIComponent component in Children.Values)
            {
                component.LoadContent(content);
            }
            OnContentLoadedEvent(new EventArgs());
        }


        /// <summary>
        /// Updates this and all its components. <br/> <see cref="Render(SpriteBatch)"/> must be called independently after 
        /// this and before <see cref="Draw(SpriteBatch)"/> for changes to be displayed. 
        /// </summary>
        /// <remarks>
        /// To Update and Render in one call, use <see cref="UpdateAndRender(InputState, SpriteBatch)"/>.
        /// </remarks>
        public virtual void Update(InputState input)
        {
            if (!IsVisible)
                return;

            // Create a copy of the gameInput and modify the mouse position to locate it inside the UI
            InputState uiInput = input;
            uiInput.MousePosition = ScreenToUI(input.MousePosition);

            // Update components
            foreach (UIComponent component in mainWindow.Components.Values)
            {
                if (component.IsVisible)
                    component.Update(uiInput);
            }
        }


        /// <summary>
        /// Updates and Renders this.
        /// </summary>
        /// <param name="input">Input required for <see cref="Update(InputState)"/>.</param>
        /// <param name="spriteBatch">SpriteBatch required for <see cref="Render(SpriteBatch)"/>.</param>
        public void UpdateAndRender(InputState input, SpriteBatch spriteBatch)
        {
            Update(input);
            Render(spriteBatch);
        }


        /// <summary>
        /// Gets the Child element with the given name from in this.
        /// </summary>
        /// <typeparam name="T">Type of the child component where <typeparamref name="T"/> : <see cref="UIComponent"/>.</typeparam> 
        /// <param name="name">Name of the child component.</param>
        /// <param name="child">The child component of type <typeparamref name="T"/>.</param>
        /// <returns>True if a component of the given type with the given name exists inside this, false if not.</returns>
        public bool GetChild<T>(string name, out T? child) where T : UIComponent
        {
            if (Children.TryGetValue(name, out var element))
            {
                if (element is T t)
                {
                    child = t;
                    return true;
                }
            }
            child = default;
            return false;
        }


        /// <summary>
        /// Draws all the components of this to the RenderTarget.
        /// </summary>
        /// <remarks>
        /// Call this before calling <see cref="UserInterface.Draw(SpriteBatch)"/>.
        /// </remarks>
        /// <param name="spriteBatch">SpriteBatch used to draw this.</param>
        public void Render(SpriteBatch spriteBatch)
        {
            // Pre-render all renderable children.
            foreach (IRenderable renderable in renderableChildren)
            {
                renderable.Render(spriteBatch, graphicsDevice);
            }

            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Color.Transparent);

            BeginSpriteBatch(spriteBatch, SamplerState.PointClamp, null);

            foreach (UIComponent component in Windows.Values)
            {
                if (component.IsVisible)
                    component.Draw(spriteBatch);
            }

            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
        }


        /// <summary>
        /// Draws this to the screen.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch used to draw this.</param>
        /// <remarks>
        /// <paramref name="spriteBatch"/> must not have had 
        /// <see cref="SpriteBatch.Begin(SpriteSortMode, BlendState, 
        /// SamplerState, DepthStencilState, RasterizerState, Effect, Matrix?)"/>
        /// called.
        /// </remarks>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible)
                return;

            BeginSpriteBatch(spriteBatch, null, camera.GetViewMatrix());

            spriteBatch.Draw(renderTarget, Vector2.Zero, Color.White);

            if (ShowBorders)
                Shapes.DrawBorder(viewport, Color.Red, 3, spriteBatch);

            spriteBatch.End();

        }


        /// <summary>
        /// Closes the UserInterface and makes it no longer visible.
        /// </summary>
        public void Close()
        {
            IsVisible = false;
            // Preform OnClose call
            foreach (Window window in Windows.Values)
            {
                window.OnClose();
            }
        }


        /// <summary>
        /// Rescale this to a new scale.
        /// </summary>
        /// <param name="renderScale">Scale to be rescaled to.</param>
        public void Rescale(float renderScale)
        {
            this.RenderScale = (float)Math.Round(renderScale, 2);

            // Resize the viewport
            Size newViewportSize = ScreenDim / RenderScale;
            viewport = new Rectangle((RenderDim - newViewportSize) / 2, newViewportSize);

            // Realign all components based on the size of the render target
            RealignComponents();

            camera.Zoom = RenderScale;
        }


        /// <summary>
        /// Closes the currently open window and opens the specified window.
        /// If the window with the given name does not exist, nothing will be opened.
        /// </summary>
        /// <param name="windowName">Window to be opened.</param>
        public void OpenWindow(string windowName)
        {
            // Close the current window
            CloseWindow();

            // If there exists a window with the given name, open it
            if (Windows.TryGetValue(windowName, out var window))
            {
                // Hide the default window.
                mainWindow.IsVisible = false;
                // Set new main window.
                mainWindow = window;
                window.IsVisible = true;
                MainWindowName = windowName;
                IsWindowOpen = true;
            }
        }


        /// <summary>
        /// Closes the currently open MainWindow.
        /// </summary>
        /// <remarks>Does not open a new window.</remarks>
        public void CloseWindow()
        {
            mainWindow.IsVisible = false;
            mainWindow.OnClose();
            mainWindow = defaultWindow;
            MainWindowName = DefaultWindowName;
            mainWindow.IsVisible = true;
            IsWindowOpen = false;
        }


        /// <summary>
        /// Converts a coordinate on the screen into a position in the UserInterface
        /// </summary>
        /// <param name="screenCoordinate">Position in screen-space.</param>
        /// <returns></returns>
        protected Point ScreenToUI(Point screenCoordinate)
        {
            return camera.ScreenToWorld(screenCoordinate.ToVector2()).ToPoint();
        }


        /// <summary>
        /// Converts a coordinate in the UserInterface space into a position on the screen.
        /// </summary>
        /// <param name="uiCoordinate">Position in UI-space.</param>
        /// <returns></returns>
        protected Point UIToScreen(Point uiCoordinate)
        {
            return camera.WorldToScreen(uiCoordinate.ToVector2()).ToPoint();
        }


        /// <summary>
        /// Realigns all components in this to the current zoom of the <see cref="renderTarget"/>.
        /// </summary>
        protected void RealignComponents()
        {
            foreach (Window window in Windows.Values)
            {
                // If any of the windows are viewport matching, change their dimensions to be the size
                // of the viewport and then realign.
                if (window.MatchViewport)
                    window.Dimensions = new Size(viewport.Size);
                window.Realign(viewport);
            }
        }


        /// <summary>
        /// Returns a dictionary of all the children <see cref="UIComponent"/> contained in this.
        /// </summary>
        /// <returns>A dictionary of child <see cref="UIComponent"/> of this.</returns>
        protected Dictionary<string, UIComponent> GetChildren()
        {
            Dictionary<string, UIComponent> children = new();
            foreach ((string name, Window window) in Windows)
            {
                children.Add(name, window);
                children = children.Concat(window.GetChildren()).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
            return children;
        }


        /// <summary>
        /// Initializes this.
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice used for initialization.</param>
        /// <remarks>Must be called after instantiation and before <see cref="Render(SpriteBatch)"/> or <br/><see cref="Draw(SpriteBatch)"/>.</remarks>
        public void Initialize(GraphicsDevice graphicsDevice)
        {
            foreach (IRenderable renderable in renderableChildren)
            {
                renderable.Initialize(graphicsDevice);
            }
        }


        /// <summary>
        /// Invokes <see cref="SpriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Matrix?)"/>
        /// on <paramref name="spriteBatch"/> using a <see cref="SamplerState"/> and a view <see cref="Matrix"/>.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to begin.</param>
        /// <param name="samplerState">SamplerState to use for the call.</param>
        /// <param name="viewMatrix">Matrix to change view.</param>
        private void BeginSpriteBatch(SpriteBatch spriteBatch, SamplerState? samplerState, Matrix? viewMatrix)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                samplerState,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null, transformMatrix: viewMatrix);
        }


        /// <summary>
        /// Raises the Event for required Content finishing loading.
        /// </summary>
        /// <param name="e"></param>
        private void OnContentLoadedEvent(EventArgs e)
        {
            OnContentLoaded?.Invoke(this, e);
        }
    }
}
