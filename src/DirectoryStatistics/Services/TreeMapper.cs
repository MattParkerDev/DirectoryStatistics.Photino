using DirectoryStatistics.Models;

namespace DirectoryStatistics.Services;

public static class TreeMapper
{
	public static List<Folder> GetSubFolders(this Folder folder)
	{
		var directoryInfo = new DirectoryInfo(folder.Path);

		List<Folder> subFolders = [];

		var subFolderInfos = directoryInfo.EnumerateDirectories().ToList();
		foreach (var subFolderInfo in subFolderInfos)
		{
			var subFolder = new Folder
			{
				Path = subFolderInfo.FullName,
				Name = subFolderInfo.Name,
			};
			subFolder.Folders = subFolder.GetSubFolders();
			subFolders.Add(subFolder);
		}
		return subFolders;
	}
}
