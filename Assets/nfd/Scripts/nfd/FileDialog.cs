using UnityEngine;
using UnityEngine.Events;
using System;

namespace nfd
{

	public enum FileDialogType
	{
		Save,
		Open,
		Folder
	}

	[Serializable]
	public class FileDialogResultEvent : UnityEvent<NfdResult, string, string[]> { }

	public class FileDialog : MonoBehaviour
	{
		public FileDialogType fileDialogType = FileDialogType.Open;

		public string defaultPath;
		public string filterList;

		public bool allowOpenMultiple = false;

		public FileDialogResultEvent onResult;

		public void Show() {
			string outPath = null;
			string[] outPaths = null;
			NfdResult result = NfdResult.NFD_CANCEL;

			switch (fileDialogType) {
				case FileDialogType.Open:
					if (!allowOpenMultiple) {
						result = NativeFileDialog.OpenDialog(filterList, defaultPath, out outPath);
					} else {
						result = NativeFileDialog.OpenDialogMultiple(filterList, defaultPath, out outPaths);
						if (outPaths != null && outPaths.Length > 0) {
							outPath = outPaths[0];
						}
					}
					break;
				case FileDialogType.Folder:
					result = NativeFileDialog.PickFolder(defaultPath, out outPath);
					break;
				case FileDialogType.Save:
					result = NativeFileDialog.SaveDialog(filterList, defaultPath, out outPath);
					break;
			}

			if (onResult != null) {
				onResult.Invoke(result, outPath, outPaths);
			}
		}
	}
}
