extends Node


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	var adMobNode = $"../AdMob"
	var gamestate = $"/root/GameState"
	adMobNode.load_banner()
	adMobNode.show_banner()
	if gamestate.NumberOfTimesPlayed % 2 == 0 && gamestate.DisplayAd:
		adMobNode.load_interstitial();
	
func _exit_tree():
	var adMobNode = $"../AdMob"	
	adMobNode.hide_banner()

func _on_AdMob_interstitial_loaded():
	var gamestate = $"/root/GameState"
	if gamestate.DisplayAd:
		var adMobNode = $"../AdMob"
		adMobNode.show_interstitial()
		gamestate.DisplayAd = false
