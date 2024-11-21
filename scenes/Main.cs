using Godot;
using System;

public partial class Main : Node2D
{
	LayeredMap worldMap;
	PackedScene worldMapScene = (PackedScene) GD.Load("res://scenes/layered_map.tscn");
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		worldMap = GetNode<LayeredMap>("WorldMap");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

		public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("restart")) {
			worldMap.QueueFree();
			worldMap = (LayeredMap) worldMapScene.Instantiate();
			//CallDeferred("AddChild", worldMap);
			AddChild(worldMap);
		}


	}
}
