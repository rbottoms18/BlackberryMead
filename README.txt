☼ BlackberryMead ☼

Light-weight game engine built on top of the MonoGame Framework.
Created by Romato.

**REQUIRES C# VERSION .NET 7.0 OR GREATER**

Disclaimer: This engine is developed for and alongside an independent game and is not indented (yet)
as a a stand alone engine. Features may be incomplete or buggy. Features will be added
whenever they are needed in the main game, not for completeness of this library.

------------------------------------------------------------------------------------------------------

To get started, make sure that NuGet packages MonoGame.Framework.DesktopGL and MonoGame.Extended
are installed. Ensure that the project's current C# version is .NET 7.0 or greater. This can
be changed in the project's "Properties" tab on the right-click context menu under Application->
General->Target Framework. Next right click on the Solution and Add->Existing Project. Navigate to
BlackberryMead and select BlackberryMead/BlackberryMead.shproj. Then right click on the main project and 
Add->Shared Project Reference. Select BlackberryMead. Build the project to confirm there are no issues.
BlackberryMead should now be accessable from the main project.

If access to the ContentManager is desired at static-level access, change the default Game1.cs class
to derive from BlackberryMead.Framework.Game instead of Microsoft.Xna.Framework.Game.