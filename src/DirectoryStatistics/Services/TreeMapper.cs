using System.Collections.Concurrent;
using DirectoryStatistics.Models;

namespace DirectoryStatistics.Services;

public static class TreeMapper
{
	public static List<Folder> GetSubFolders(this Folder folder)
	{
		var directoryInfo = new DirectoryInfo(folder.Path);

		ConcurrentBag<Folder> subFolders = [];

		var files = GetFiles(directoryInfo);
		if (files.Count is not 0)
		{
			var pseudoFolder = new Folder
			{
				Path = folder.Path,
				Name = "<Files>",
				IsPseudoFolder = true,
				Files = files
			};
			subFolders.Add(pseudoFolder);
		}

		List<DirectoryInfo> subFolderInfos;
		try
		{
			subFolderInfos = directoryInfo.EnumerateDirectories().ToList();
		}
		catch (UnauthorizedAccessException)
		{
			return subFolders.ToList();
		}
		Parallel.ForEach(subFolderInfos, subFolderInfo =>
		{
			var subFolder = new Folder
			{
				Path = subFolderInfo.FullName,
				Name = subFolderInfo.Name,
			};
			subFolder.Folders = subFolder.GetSubFolders();
			subFolders.Add(subFolder);
		});
		return subFolders.ToList();
	}

	public static List<TreeMapFile> GetFiles(DirectoryInfo directoryInfo)
	{
		List<FileInfo> fileInfos;
		try
		{
			fileInfos = directoryInfo.EnumerateFiles().ToList();
		}
		catch (UnauthorizedAccessException)
		{
			return [];
		}

		var files = fileInfos.Select(s => new TreeMapFile
		{
			Path = s.FullName,
			Name = s.Name,
			Size = s.Length
		}).ToList();
		return files;
	}
}
