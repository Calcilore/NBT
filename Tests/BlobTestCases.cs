using NBT;
using NBT.Tags;

namespace Tests;

/// <summary>
/// Single source of truth for all binary blob test cases.
/// Used by both BlobGenerator (to produce GeneratedBlobs.cs) and
/// BinaryBlobTest (to run the snapshot tests).
///
/// To add a new test case, add an entry here then run:
///   dotnet run --project BlobGenerator
/// </summary>
internal static class BlobTestCases {
    public static (string Name, INbtTag Tag)[] All => [
        ("ByteTagPositive",    new ByteTag(null, 0x56)),
        ("ByteTagNegative",    new ByteTag(null, -1)),
        ("ShortTag",           new ShortTag(null, 1000)),
        ("IntegerTag",         new IntegerTag(null, 42)),
        ("LongTag",            new LongTag(null, 0x0102030405060708L)),
        ("FloatTag",           new FloatTag(null, 1.0f)),
        ("DoubleTag",          new DoubleTag(null, 1.0)),
        ("StringTagAscii",     new StringTag(null, "hi")),
        ("BooleanTagTrue",     new BooleanTag(null, true)),
        ("BooleanTagFalse",    new BooleanTag(null, false)),
        ("ByteArrayTag",       new ArrayTag<sbyte>(null, 0, 1, -1)),
        ("IntArrayTag",        new ArrayTag<int>(null, 1, 2, 3)),
        ("LongArrayTag",       new ArrayTag<long>(null, 1L, 2L)),
        ("ListTagIntegers",    new ListTag<IntegerTag>(null, [new IntegerTag(null, 1), new IntegerTag(null, 2)])),
        ("CompoundTagEmpty",   new CompoundTag(null)),
        ("CompoundTagComplex", new CompoundTag(null,
            new StringTag("name", "Steve"),
            new IntegerTag("level", 10),
            new ListTag<IntegerTag>("scores", [new IntegerTag(null, 100), new IntegerTag(null, 200)]),
            new ArrayTag<sbyte>("flags", 1, 0, 1)
        ))
    ];
}
