namespace DirectoryStatistics.Models;

public class Folder
{
	public required string Path { get; set; }
	public string Name { get; set; } = null!;
	public List<Folder> Folders { get; set; } = [];
	public List<TreeMapFile> Files { get; set; } = [];
}

public class TreeMapFile
{
	public required string Path { get; set; }
	public required string Name { get; set; }
	public required long Size { get; set; }
}
