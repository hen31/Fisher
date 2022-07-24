extends Label


# Declare member variables here. Examples:
# var a = 2
# var b = "text"
export var KeyName = '';
export var Prefix = '';

# Called when the node enters the scene tree for the first time.
func _ready():
		self.text = Prefix + tr(KeyName)


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
