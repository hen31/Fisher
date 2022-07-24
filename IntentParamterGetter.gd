extends Node2D


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
export var checkAtStart = true

# Called when the node enters the scene tree for the first time.
func _ready():
	if checkAtStart:
		get_tree().paused = false
		check_app_link()


func check_app_link():
	if Engine.has_singleton('AppLinks'):
		var applinks = Engine.get_singleton('AppLinks')
		var app_url = applinks.getUrl()
		var gamestate = get_node("/root/GameState")
		gamestate.AppUrl = app_url
		get_node("UrlNode").HasAppUrl()

func _notification(what):
	if what == NOTIFICATION_APP_RESUMED:
			check_app_link()

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
