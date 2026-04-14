using NBT;
using NBT.Tags;

namespace Tests;

/// <summary>
/// Snapshot tests that verify each NBT tag type serialises to a fixed binary blob
/// and deserialises back to an equal object.
///
/// Test cases are defined once in <see cref="BlobTestCases"/>.
/// Expected blobs live in <see cref="GeneratedBlobs"/>, produced by BlobGenerator.
/// To update after an intentional format change:
///   dotnet run --project BlobGenerator
/// </summary>
[TestFixture]
public class BinaryBlobTest {

    private static IEnumerable<TestCaseData> BlobCases() {
        foreach ((string name, INbtTag tag) in BlobTestCases.All) {
            yield return new TestCaseData(name, tag).SetName(name);
        }
    }

    [Test, TestCaseSource(nameof(BlobCases))]
    public void Serialise_MatchesBlob(string name, INbtTag tag) {
        Assert.That(GeneratedBlobs.All.TryGetValue(name, out byte[]? expected), Is.True,
            $"No blob found for '{name}'. Run: dotnet run --project BlobGenerator");

        byte[] actual = tag.Serialise();
        Assert.That(actual, Is.EqualTo(expected),
            $"Serialised bytes do not match expected blob.\n" +
            $"Expected ({expected!.Length} bytes): {BytesToHex(expected)}\n" +
            $"Actual   ({actual.Length} bytes):   {BytesToHex(actual)}");
    }

    [Test, TestCaseSource(nameof(BlobCases))]
    public void Deserialise_MatchesOriginal(string name, INbtTag original) {
        Assert.That(GeneratedBlobs.All.TryGetValue(name, out byte[]? blob), Is.True,
            $"No blob found for '{name}'. Run: dotnet run --project BlobGenerator");

        INbtTag deserialized = NbtReader.ReadNbt(blob!);

        // BooleanTag has no distinct wire format — it deserialises as ByteTag
        INbtTag expected = original is BooleanTag
            ? new ByteTag(original.GetName(), ((ByteTag)original).Value)
            : original;

        Assert.That(deserialized, Is.EqualTo(expected),
            $"Deserialised tag does not equal original for '{name}'.\n" +
            $"Expected type: {expected.GetType().Name}\n" +
            $"Actual type:   {deserialized.GetType().Name}");
    }

    private static string BytesToHex(byte[] bytes) =>
        string.Join(" ", bytes.Select(b => b.ToString("X2")));
}
