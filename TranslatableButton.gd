extends Button


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
export var KeyName = '';

# Called when the node enters the scene tree for the first time.
func _ready():
		self.text = tr(KeyName)


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
