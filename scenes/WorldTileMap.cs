using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class WorldTileMap : TileMapLayer
{
	[Export]
	int worldHeight = 192;
	int worldWidth =  320;
	[Export]
	String seed = "";
	Vector2 initialPos;
	CharacterBody2D player; 
	FastNoiseLite altitude;
	Array chunks = Array.Empty<Vector2I>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		altitude = new();
		altitude.Seed = (int) GD.Randi();
		if(seed != "") {
			var asciiBuffer = seed.ToAsciiBuffer();
			int worldSeed = 0;
			for(int i = 0; i < asciiBuffer.Length; i++) {
				worldSeed += asciiBuffer.GetValue(i).ToString().ToInt();
			}
			altitude.Seed = worldSeed;
		}
		altitude.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
		// player = GetParent<Node2D>().GetChild(1).GetChild<CharacterBody2D>(0);
		player = GetNode("../Player").GetChild<CharacterBody2D>(0);
		initialPos = player.Position;
		generateMap(initialPos);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}



	public void generateMap(Vector2 position) {

		var tilePos = LocalToMap(position);
		var noiseMatrix = new List<List<float>>();
		for(int i = 0; i < worldWidth; i++) {
            var noiseList = new List<float>();
			for(int j = 0; j < worldHeight; j++) {
				var x = (tilePos.X - worldWidth / 2) + i;
				var y = (tilePos.Y - worldHeight / 2) + j;
				var currentAltitude = altitude.GetNoise2D(x, y);
				noiseList.Add(currentAltitude);
				var coords = new Vector2I(x, y);
				if(onBorder(j)) {
					SetCell(coords, 1, new Vector2I(0, 0));
				} else {
					if (currentAltitude < 0){
						SetCell(coords, 1, new Vector2I(0, 0));
					} 
					if (currentAltitude >= 0 && currentAltitude < 0.06){					
						SetCell(coords, 3, new Vector2I(0, 5));
					} 
					if (currentAltitude >= 0.06 && currentAltitude < 0.2) {
						SetCell(coords, 0, new Vector2I(1,1));
					} 
					if (currentAltitude >= 0.2) {
						SetCell(coords, 2, new Vector2I(4, 5));
					}
				}
			}
			noiseMatrix.Add(noiseList);
		}
	}

	public bool onBorder(int i) {
		if(i < 2 ) {
			return true;
		}
		if(i > worldWidth - 2) {
			return true;
		}
		return false;
	}
}
