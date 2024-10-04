@tool
extends EditorScript

func _run():
	var anim = get_scene().get_node("Sprite").get_node("AnimationPlayer")
	var animation
	var track_index
	for key in anim.get_animation_list():
		if key != "jump" and key != "fall" and key != "5H":
			animation = anim.get_animation(key)
			track_index = animation.add_track(Animation.TYPE_METHOD)
			animation.track_set_path(track_index, "..")
			track_index = animation.add_track(Animation.TYPE_METHOD)
			animation.track_set_path(track_index, "..")
			track_index = animation.add_track(Animation.TYPE_METHOD)
			animation.track_set_path(track_index, "..")
