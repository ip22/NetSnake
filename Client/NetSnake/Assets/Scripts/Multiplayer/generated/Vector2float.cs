// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.46
// 

using Colyseus.Schema;

public partial class Vector2float : Schema {
	[Type(0, "uint32")]
	public uint id = default(uint);

	[Type(1, "uint8")]
	public byte type = default(byte);

	[Type(2, "number")]
	public float x = default(float);

	[Type(3, "number")]
	public float z = default(float);
}

