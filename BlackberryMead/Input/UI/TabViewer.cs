using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// UI Element that stores multiple states accessable through 'tabs'
    /// </summary>
    public class TabViewer : UIComponent
    {
        public override Rectangle Rect { get; protected set; }

        public override Point Origin { get; protected set; }

        /// <summary>
        /// List of components tied to each tab.
        /// </summary>
        // This is stored in a dictionary because I want each tab to have a name
        // getable when the mouse is hovered over
        [JsonInclude]
        public Dictionary<string, Window> Tabs { get; private set; }

        /// <summary>
        /// Rectangles that when clicked open the corresponding tab
        /// </summary>
        private readonly Rectangle[] tabRects;

        /// <summary>
        /// Spacing between tabs
        /// </summary>
        [JsonInclude]
        public int TabSpacing { get; protected set; }

        /// <summary>
        /// Dimensions of each of the tabRects
        /// </summary>
        [JsonInclude]
        public Size TabDimensions { get; protected set; }

        /// <summary>
        /// Whether the tabs are vertical or not.
        /// If true, tabs are alligned vertically. If false, tabs are alligned horizontally
        /// </summary>
        [JsonInclude]
        public bool VerticalTabs { get; protected set; }

        /// <summary>
        /// Vertical offset of the tabs
        /// </summary>
        [JsonInclude]
        public int TabVerticalOffset { get; protected set; }

        /// <summary>
        /// Horizontal offset of the tabs
        /// </summary>
        [JsonInclude]
        public int TabHorizontalOffset { get; protected set; }

        /// <summary>
        /// Array of strings corresponding to the names of each of the tabs.
        /// Same length as tabRects.
        /// </summary>
        private readonly string[] tabNames;

        /// <summary>
        /// Index of the tab rect currently hovered by the mouse.
        /// </summary>
        private int hoverTabIndex;

        /// <summary>
        /// Index of the tab rect currently selected to be displayed
        /// </summary>
        private int selectedTabIndex;

        /// <summary>
        /// Source Rectangle of the tab sprite
        /// </summary>
        [JsonInclude]
        public Rectangle TabSourceRect { get; protected set; }

        /// <summary>
        /// Coordiante offset of the selected tab
        /// </summary>
        [JsonInclude]
        public Point SelectedTabOffset { get; protected set; }

        /// <summary>
        /// Source rect of the selected tab sprite
        /// </summary>
        [JsonInclude]
        public Rectangle SelectedTabSourceRect { get; protected set; }

        /// <summary>
        /// The name of the action that enables selecting of a tab.
        /// </summary>
        public string SelectTabAction { get; set; } = "Select";

        public override List<string> Actions { get => new() { SelectTabAction }; }

        /// <summary>
        /// Current tab window to be updated and drawn.
        /// </summary>
        private Window currentWindow;


        /// <summary>
        /// Create a new tab viewer
        /// </summary>
        /// <inheritdoc/>
        /// <param name="Tabs">Named UIWindows to be set as tabs.</param>
        /// <param name="VerticalTabs">Tabs allign vertically if true, horizontal if false.</param>
        /// <param name="TabDimensions">Dimensions of a tab rectangle.</param>
        /// <param name="TabSpacing">Spacing between tab rectangles.</param>
        /// <param name="TabVerticalOffset">Vertical offset of the tab rectangles from the Origin</param>
        /// <param name="TabHorizontalOffset">Horizontal offset of the tab rectangles from the Origin</param>
        /// <param name="TabSourceRect">Source rect of the tab rectangle sprite</param>
        /// <param name="SelectedTabOffset">Offset of the rectangle that corresponds to the currently
        ///     selected tab</param>
        /// <param name="SelectedTabSourceRect">Source rectangle of the sprite to be drawn to the currently selected
        ///     tab rect</param>
        [JsonConstructor]
        public TabViewer(Dictionary<string, Window> Tabs, bool VerticalTabs, Size TabDimensions,
            int TabSpacing, int TabVerticalOffset, int TabHorizontalOffset, Rectangle TabSourceRect,
            Point SelectedTabOffset, Rectangle SelectedTabSourceRect, Size Dimensions,
            Alignment VerticalAlign, Alignment HorizontalAlign, int Scale, int VerticalOffset, int HorizontalOffset,
            string SelectTabAction = "Select") :
            base(Dimensions, VerticalAlign, HorizontalAlign, Scale, VerticalOffset, HorizontalOffset)
        {
            this.Tabs = Tabs;
            this.TabDimensions = TabDimensions * Scale;
            this.TabSpacing = TabSpacing * Scale;
            this.TabVerticalOffset = TabVerticalOffset * Scale;
            this.TabHorizontalOffset = TabHorizontalOffset * Scale;
            this.VerticalTabs = VerticalTabs;
            this.TabSourceRect = TabSourceRect;
            this.SelectedTabOffset = new Point(SelectedTabOffset.X * Scale, SelectedTabOffset.Y * Scale);
            this.SelectedTabSourceRect = SelectedTabSourceRect;
            this.tabRects = new Rectangle[Tabs.Count];
            this.SelectTabAction = SelectTabAction;

            tabNames = new string[Tabs.Count];
            currentWindow = Tabs.Values.First();
            selectedTabIndex = 0;

            InitializeTabRects();
        }


        /// <summary>
        /// Initializes the Tab Rectangles
        /// </summary>
        private void InitializeTabRects()
        {
            // Create the tab rects
            for (int i = 0; i < Tabs.Count; i++)
            {
                Rectangle tabRect = new(Origin.X + TabHorizontalOffset, Origin.Y + TabVerticalOffset,
                    TabDimensions.Width, TabDimensions.Height);
                if (VerticalTabs)
                    tabRect.Location += new Point(0, i * (TabSpacing + TabDimensions.Height));
                else
                    tabRect.Location += new Point(i * (TabSpacing + TabDimensions.Width), 0);

                tabRects[i] = tabRect;
                // Get the name of the tab and add it to the names array
                tabNames[i] = Tabs.Keys.ToList()[i];
            }

            // Shift the currently selected tab rect over
            tabRects[selectedTabIndex].Location += SelectedTabOffset;
        }


        public override void Realign(Rectangle boundingRect)
        {
            base.Realign(boundingRect);

            InitializeTabRects();

            // Reallign the contents of each tab
            foreach (Window tab in Tabs.Values)
            {
                tab.Realign(Rect);
            }
        }


        public override void Update(InputState input)
        {
            // Update hover
            hoverTabIndex = -1;
            for (int i = 0; i < tabRects.Length; i++)
            {
                if (tabRects[i].Contains(input.MousePosition))
                {
                    hoverTabIndex = i;
                    // If the hover tab has been selected
                    if (input[SelectTabAction])
                    {
                        SwitchTab(i);
                    }
                }
            }

            // Update the selected tab
            currentWindow.Update(input);
        }


        public override void OnClose()
        {
            base.OnClose();
            SwitchTab(0);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Draw tab rectangles
            foreach (Rectangle r in tabRects)
            {
                spriteBatch.Draw(UserInterface.SpriteSheet, r, TabSourceRect, Color.White);
            }
            // Draw the selected tab sprite
            spriteBatch.Draw(UserInterface.SpriteSheet, tabRects[selectedTabIndex], SelectedTabSourceRect, Color.White);

            //if (hoverTabIndex != -1)
            //    spriteBatch.FillRectangle(tabRects[hoverTabIndex], Color.Blue, 0);

            // Draw the current window
            currentWindow.Draw(spriteBatch);
        }


        public override Dictionary<string, UIComponent> GetChildren()
        {
            Dictionary<string, UIComponent> children = new() { };
            for (int i = 0; i < Tabs.Count; i++)
            {
                // Add the tab
                children.Add(Tabs.Keys.ElementAt(i), Tabs[Tabs.Keys.ElementAt(i)]);
                // Add the tab elements
                children = children.Concat(Tabs.Values.ElementAt(i).GetChildren()).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
            return children;
        }


        /// <summary>
        /// Switches the currently open tab.
        /// </summary>
        /// <param name="newTabIndex">Index of the new tab to open in <paramref name="Tabs"/>.</param>
        private void SwitchTab(int newTabIndex)
        {
            // Revert previously selected tab rect
            tabRects[selectedTabIndex].Location -= SelectedTabOffset;

            // Shift newly selected tab rect to the "selected" position
            tabRects[newTabIndex].Location += SelectedTabOffset;

            // Set new selected tab index
            selectedTabIndex = newTabIndex;

            // Call on close on the current window
            currentWindow.OnClose();

            // Set the current window based on the name of the window
            currentWindow = Tabs[tabNames[newTabIndex]];
        }
    }
}
