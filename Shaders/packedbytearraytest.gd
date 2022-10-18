extends Node


# Called when the node enters the scene tree for the first time.
func _ready():
	
	var sizedata2 := PackedInt32Array()
	sizedata2.resize(2)
	sizedata2[0] = int(200)
	sizedata2[1] = int(693)
	#print(sizedata2[0])
	#print(sizedata2[1])
	
	
	var size_data_bytes := PackedByteArray(sizedata2.to_byte_array());
	print(size_data_bytes.size())
	var _bruh = int(0);
	print(size_data_bytes.to_int32_array())
	print(size_data_bytes)
	
	
	#pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	pass
