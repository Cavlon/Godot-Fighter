@tool
extends EditorScript

var anim_name = "5S"
var anim_frame: float = 12
var destroy = false

var damage = 30
var type = 0
var hitlevel = 1
var hitstun = 20
var blockstun = 10
var xlaunch = 600
var ylaunch = 0
var decay = 5

func _run():
	var anim = get_scene().get_node("Sprite").get_node("AnimationPlayer")
	var animation = anim.get_animation(anim_name)
	
	var index = animation.add_track(Animation.TYPE_METHOD)
	animation.track_set_path(index, "..")
	
	if destroy:
		var method_dictionary = {
			"method": "DestroyHitBoxes",
			"args": [],
		}
		
		animation.track_insert_key(2, anim_frame / 60, method_dictionary, 0)
	
	add_hitboxes(animation)

func add_hitboxes(animation):
	var hitboxes = get_scene().get_node("Hitboxes").get_children()
	var boxcount = hitboxes.size()
	
	for i in range(boxcount):
		var index = animation.add_track(Animation.TYPE_METHOD)
		animation.track_set_path(index, "..")
		
		var method_dictionary = {
			"method": "CreateHitBox",
			"args": [hitboxes[i].shape.size.x, hitboxes[i].shape.size.y, damage, type, hitboxes[i].position, hitlevel, hitstun, blockstun, xlaunch, ylaunch, decay],
		}
		
		animation.track_insert_key(index, anim_frame / 60, method_dictionary, 0)
