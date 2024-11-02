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
	public bool Expanded { get; set; }

	[JsonIgnore]
	public long TotalFolderSize => _totalFolderSize ??= Files.Sum(s => s.Size) + Folders.Sum(c => c.TotalFolderSize);
	private long? _totalFolderSize;

	public decimal GetPercentageOfRootFolderSize(long totalSizeBytes) => _percentageOfRootFolderSize ??= ((decimal) TotalFolderSize / totalSizeBytes) * 100;
	private decimal? _percentageOfRootFolderSize;

	[JsonIgnore]
	public string TotalFolderSizeFormatted => _totalFolderSizeFormatted ??= $"{ByteSize.LargestWholeNumberBinaryValue:N1} {ByteSize.LargestWholeNumberBinarySymbol}";
	private string? _totalFolderSizeFormatted;

	[JsonIgnore]
	private ByteSize ByteSize => _byteSize ??= ByteSize.FromBytes(TotalFolderSize);
	private ByteSize? _byteSize;
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
