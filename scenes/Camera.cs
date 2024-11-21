using Godot;
using System;

public partial class Camera : Camera2D
{

	[Export]
	int Speed {get; set;} = 400;
	Vector2 previousPosition = Vector2.Zero;
	bool moveCamera = false;
	public Vector2 minimum_zoom = new Vector2(
        (float) 0.3, (float) 0.3
    );

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		var velocity = Vector2.Zero;
		if(Input.IsActionPressed("move_right")) {
			velocity.X += 1;
		}
		if(Input.IsActionPressed("move_down")) {
			velocity.Y += 1;
		}
		if(Input.IsActionPressed("move_left")) {
			velocity.X -= 1;
		}
		if(Input.IsActionPressed("move_up")) {
			velocity.Y -= 1;
		}

		if(Input.IsActionPressed("zoom_out")) {
			var currentZoom = GetZoom();
			if(currentZoom > minimum_zoom) {
				SetZoom(
					new Vector2(
						(float) (currentZoom.X - 0.1), 
						(float) (currentZoom.Y - 0.1))
				);
			}
		}
		if(Input.IsActionPressed("zoom_in")) {
			var currentZoom = GetZoom();
			SetZoom(
				new Vector2(
					(float) (currentZoom.X + 0.1), 
					(float) (currentZoom.Y + 0.1))
			);
		}

		if(velocity.Length() > 0) {
			velocity = velocity.Normalized() * Speed;
		}
		Position += velocity * (float)delta;
	}

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton) {
			InputEventMouseButton mouseEvent = (InputEventMouseButton) @event;
			if(mouseEvent.ButtonIndex == MouseButton.Left) {
				GetViewport().SetInputAsHandled();

				if(mouseEvent.IsPressed()) {
					previousPosition = mouseEvent.Position;
					moveCamera = true;
				} else {
					moveCamera = false;
				}
			} 
		} else if(@event is InputEventMouseMotion && moveCamera) {
			GetViewport().SetInputAsHandled();
			InputEventMouseMotion mouseMotion = (InputEventMouseMotion)@event;
			Position += (previousPosition - mouseMotion.Position);
			previousPosition = mouseMotion.Position;
		} 
    }
}
