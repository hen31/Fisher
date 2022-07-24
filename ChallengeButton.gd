extends Button


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


var share = null # our share singleton instance

# Called when the node enters the scene tree for the first time.
func _ready():
	text = tr('ChallengeBtn')
# initialize the share singleton if it exists
	if Engine.has_singleton("GodotShare"):
		share = Engine.get_singleton("GodotShare")


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass


func _on_ChallengeButton_pressed():
	if share != null:
		var gamestate = get_node("/root/GameState")
		share.shareText(tr('shareTitle'), tr('shareSubject'), tr('shareMessage') % [gamestate.LastScore, gamestate.LastSeedHex])

