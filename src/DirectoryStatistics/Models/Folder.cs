using System.Text.Json.Serialization;
using ByteSizeLib;

namespace DirectoryStatistics.Models;

public class Folder
{
	public required string Path { get; set; }
	public string Name { get; set; } = null!;
	public List<Folder> Folders { get; set; } = [];
	public List<TreeMapFile> Files { get; set; } = [];
	public bool IsPseudoFolder { get; set; }

	[JsonIgnore]
	public long TotalFolderSize => Files.Sum(s => s.Size) + Folders.Sum(c => c.TotalFolderSize);
	public decimal GetPercentageOfRootFolderSize(long totalSizeBytes) => ((decimal) TotalFolderSize / totalSizeBytes) * 100;

	[JsonIgnore]
	public string TotalFolderSizeFormatted => $"{ByteSize.LargestWholeNumberBinaryValue:N1} {ByteSize.LargestWholeNumberBinarySymbol}";

	[JsonIgnore]
	private ByteSize ByteSize => ByteSize.FromBytes(TotalFolderSize);
}

public class TreeMapFile
{
	public required string Path { get; set; }
	public required string Name { get; set; }
	public required long Size { get; set; }

	[JsonIgnore]
	public string SizeFormatted => $"{ByteSize.LargestWholeNumberBinaryValue:N1} {ByteSize.LargestWholeNumberBinarySymbol}";

	[JsonIgnore]
	private ByteSize ByteSize => ByteSize.FromBytes(Size);
}
