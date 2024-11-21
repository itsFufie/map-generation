using Godot;
using System;

public partial class PlayerCamera : Camera2D
{
	// Called when the node enters the scene tree for the first time.

	Vector2 minimum_zoom = new Vector2(0.1f, 0.1f);
	public override void _Ready()
	{
		SetZoom(minimum_zoom);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if(Input.IsActionPressed("zoom_out")) {
			var currentZoom = GetZoom();	
			var currentX  = Math.Round(currentZoom.X, 1);
			if(currentX > minimum_zoom.X) {
				SetZoom(
					new Vector2(
						(float) (currentZoom.X - 0.1f), 
						(float) (currentZoom.Y - 0.1f))
				);
			} 
		}
		if(Input.IsActionPressed("zoom_in")) {
			var currentZoom = GetZoom();
			SetZoom(
				new Vector2(
					(float) (currentZoom.X + 0.1f), 
					(float) (currentZoom.Y + 0.1f))
			);
		}
	}
}
