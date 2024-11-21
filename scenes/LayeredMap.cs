using Godot;
using System;
using System.Numerics;

public partial class LayeredMap : Node2D
{
	TileMapLayer groundLayer;
	TileMapLayer objectLayer;

	int worldHeight = 192;
	int worldWidth =  320;

	FastNoiseLite feature;
	FastNoiseLite altitude;

	Godot.Collections.Dictionary<string, Vector2I> tiles = new Godot.Collections.Dictionary<string, Vector2I>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		altitude = new();
		feature = new();
		altitude.Seed = (int)GD.Randi();
		feature.Seed = (int)GD.Randi();
		altitude.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
		feature.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
		groundLayer = GetNode<TileMapLayer>("GroundLayer");
		objectLayer = GetNode<TileMapLayer>("ObjectLayer");
		CharacterBody2D player = GetNode("../Player").GetChild<CharacterBody2D>(0);
		loadTilesIntoDictionary();
		generateMap(player.Position);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void generateMap(Godot.Vector2 pos) {
		var tilePos = groundLayer.LocalToMap(pos);
		for(int i = 0; i < worldWidth; i++) {
			for(int j = 0; j < worldHeight; j++) {

				var x = (tilePos.X - worldWidth / 2) + i;
				var y = (tilePos.Y - worldHeight / 2) + j;
				var currentAltitude = altitude.GetNoise2D(x, y);
				var currentFeature = feature.GetNoise2D(x, y);

				defineCell(currentAltitude, currentFeature, new Vector2I(x, y));
			}
		}
	}

	public void defineCell(float currentAltitude, float currentFeature, Vector2I coords) {
		if(currentAltitude < 0) {
			generateOcean(currentAltitude, coords);
		} else {
			generateLand(currentAltitude, currentFeature, coords);
		}
		
	}


	public void generateOcean(float currentAltitude, Vector2I coords) {
		if ( currentAltitude > -0.1 && currentAltitude < 0) {
			groundLayer.SetCell(coords, 0, tiles["coastSea"]);
		} else if (currentAltitude > -0.2 && currentAltitude <= -0.1) {
			groundLayer.SetCell(coords, 0, tiles["shallowSea"]);
		} else if(currentAltitude > -0.3 && currentAltitude <= -0.2) {
			groundLayer.SetCell(coords, 0, tiles["shallowOcean"]);
		} else {
			groundLayer.SetCell(coords, 0, tiles["deepOcean"]);
		}
	}

	public void generateLand(float currentAltitude, float currentFeature, Vector2I coords) {

		if (currentAltitude > 0 && currentAltitude < 0.05) {
			groundLayer.SetCell(coords, 0, tiles["sand"]);
		} else if (currentAltitude < 0.2) {
			groundLayer.SetCell(coords, 0, currentFeature > 0.3 ? tiles["lowerDirt"] : tiles["lowerGrass"]);
/* 			if(currentFeature < -0.1) {
				objectLayer.SetCell(coords, 0, tiles["forest"]);
			} */
		} else if (currentAltitude  < 0.4) {
			groundLayer.SetCell(coords, 0, currentFeature > 0.3 ? tiles["higherDirt"] : tiles["higherGrass"]);
/* 			if(currentFeature < -0.1) {
				objectLayer.SetCell(coords, 0, tiles["pineForest"]);
			} */
		} else if (currentAltitude < 0.5) {
			groundLayer.SetCell(coords, 0, tiles["lowerIce"]);
			//objectLayer.SetCell(coords, 0, tiles["mountain"]);
		} else if (currentAltitude >= 0.5) {
			groundLayer.SetCell(coords, 0, tiles["higherIce"]);
			//objectLayer.SetCell(coords, 0, tiles["peaks"]);
		}
	}

	public void loadTilesIntoDictionary() {
		tiles.Add("mountain", new(2, 2));
		tiles.Add("peaks", new(3, 2));
		tiles.Add("forest", new(0, 3));
		tiles.Add("pineForest", new(2, 3));

		tiles.Add("coastSea", new(0,0));
		tiles.Add("shallowSea",new(1,0));
		tiles.Add("shallowOcean", new(2, 0));
		tiles.Add("deepOcean", new(3, 0));

		tiles.Add("lowerDirt", new(0,1));
		tiles.Add("higherDirt", new(1,1));
		tiles.Add("lowerGrass", new(2, 1));
		tiles.Add("higherGrass", new(3, 1));
		tiles.Add("lowerIce", new(0, 2));
		tiles.Add("higherIce", new(1,2));
		tiles.Add("sand", new(1, 3));
	}
}
