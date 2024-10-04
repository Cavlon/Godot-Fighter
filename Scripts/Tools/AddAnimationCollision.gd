@tool
extends EditorScript

var anim_name = "5S"
var anim_frame: float = 32

func _run():
	var anim = get_scene().get_node("Sprite").get_node("AnimationPlayer")
	var animation = anim.get_animation(anim_name)
	
	if animation.get_track_count() < 3:
		var index = animation.add_track(Animation.TYPE_METHOD)
		animation.track_set_path(index, "..")
	
	var method_dictionary = {
		"method": "DestroyHurtBoxes",
		"args": [],
	}
	
	animation.track_insert_key(2, anim_frame / 60, method_dictionary, 0)
	
	add_collision(animation)
	add_hurtboxes(animation)

func add_collision(animation):
	var collider = get_scene().get_node("CollisionArea").get_node("CollisionBox")
	
	if animation.get_track_count() < 4:
		var index = animation.add_track(Animation.TYPE_METHOD)
		animation.track_set_path(index, "..")
	
	var method_dictionary = {
		"method": "UpdateCollision",
		"args": [collider.shape.size.x, collider.shape.size.y, collider.position],
	}
	
	animation.track_insert_key(3, anim_frame / 60, method_dictionary, 0)

func add_hurtboxes(animation):
	var hurtboxes = get_scene().get_node("Hurtboxes").get_children()
	var boxcount = hurtboxes.size()
	
	for i in range(boxcount):
		if animation.get_track_count() < 5 + i:
			var index = animation.add_track(Animation.TYPE_METHOD)
			animation.track_set_path(index, "..")
		
		var method_dictionary = {
			"method": "CreateHurtBox",
			"args": [hurtboxes[i].shape.size.x, hurtboxes[i].shape.size.y, hurtboxes[i].position],
		}
		
		animation.track_insert_key(4+i, anim_frame / 60, method_dictionary, 0)
