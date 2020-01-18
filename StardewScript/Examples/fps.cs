//css_include ../Mods/StardewScript/Includes/references.cs

using Microsoft.Xna.Framework;
using StardewModdingAPI.Events;
using StardewScript.Framework;
using StardewValley;
using System;


public class FpsCounter : Script
{
	/// <summary>
	/// The number of frames rendered since <see cref="_lastRenderingCall"/>
	/// </summary>
	private int _numFramesRendered = 0;

	/// <summary>
	/// The rendering time in milliseconds since <see cref="_lastRenderingCall"/>
	/// </summary>
	private double _frameRenderingTime = 0;

	/// <summary>
	/// The currently displayed FPS string
	/// </summary>
	private string _fpsString = "-";

	/// <summary>
	/// The date and time when the last rendering call was invoked.
	/// </summary>
	private DateTime _lastRenderingCall = DateTime.UtcNow;

	/// <summary>
	/// The date and time when the FPS string was last updated.
	/// </summary>
	private DateTime _lastFPSUpdate = DateTime.UtcNow;

	protected override void InitializeScript()
	{
		base.InitializeScript();
	}

	protected override void OnRendered(object sender, RenderedEventArgs e)
	{
		var millisecondsSinceLastCall = DateTime.UtcNow.Subtract(_lastRenderingCall).TotalMilliseconds;
		_lastRenderingCall = DateTime.UtcNow;

		_numFramesRendered++;
		_frameRenderingTime  += millisecondsSinceLastCall;

		// Check if the last FPS update was more than a second ago. If so,
		// recalculate the FPS and update the FPS string.
		if (DateTime.UtcNow.Subtract(_lastFPSUpdate).TotalSeconds >= 1)
		{
			_lastFPSUpdate = DateTime.UtcNow;

			var averageRenderingTimePerFrame = _frameRenderingTime / _numFramesRendered;
			_fpsString = $"{(1000/averageRenderingTimePerFrame):F0}";
			

			_frameRenderingTime = 0;
			_numFramesRendered = 0;
		}

		Utility.drawTextWithColoredShadow(e.SpriteBatch, $"{_fpsString} FPS", Game1.smallFont, new Vector2(10,10), Color.White, Color.Black, 1);
	}
}