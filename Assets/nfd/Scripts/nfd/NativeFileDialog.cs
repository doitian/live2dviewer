using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace nfd
{

	public enum NfdResult
	{
		NFD_ERROR,       /* programmatic error */
		NFD_OKAY,        /* user pressed okay, or successful return */
		NFD_CANCEL       /* user pressed cancel */
	}

	// Native C functions wrappers.
	public static class NativeFileDialog
	{
		public static NfdResult OpenDialog(
			string filterList,
			string defaultPath,
			out string outPath
		) {
			var filterListPtr = StringToPtr(filterList);
			var defaultPathPtr = StringToPtr(defaultPath);
			var outPathPtr = IntPtr.Zero;

			var result = NFD_OpenDialog(filterListPtr, defaultPathPtr, out outPathPtr);
			outPath = PtrToString(outPathPtr);
			if (outPathPtr != IntPtr.Zero) {
				NFDi_Free(outPathPtr);
			}

			if (filterList != null) {
				Marshal.FreeHGlobal(filterListPtr);
			}
			if (filterList != null) {
				Marshal.FreeHGlobal(defaultPathPtr);
			}

			return result;
		}

		public static NfdResult OpenDialogMultiple(
			string filterList,
			string defaultPath,
			out string[] outPaths
		) {
			var filterListPtr = StringToPtr(filterList);
			var defaultPathPtr = StringToPtr(defaultPath);
			var outPathsPtr = IntPtr.Zero;

			var result = NFD_OpenDialogMultiple(filterListPtr, defaultPathPtr, out outPathsPtr);
			if (outPathsPtr != IntPtr.Zero) {
				outPaths = new string[NFD_PathSet_GetCount(outPathsPtr)];
				for (int i = 0; i < outPaths.Length; i++) {
					outPaths[i] = PtrToString(NFD_PathSet_GetPath(outPathsPtr, i));
				}
				NFD_PathSet_Free(outPathsPtr);
			} else {
				outPaths = null;
			}

			if (filterList != null) {
				Marshal.FreeHGlobal(filterListPtr);
			}
			if (filterList != null) {
				Marshal.FreeHGlobal(defaultPathPtr);
			}

			return result;
		}

		public static NfdResult SaveDialog(
			string filterList,
			string defaultPath,
			out string outPath
		) {
			var filterListPtr = StringToPtr(filterList);
			var defaultPathPtr = StringToPtr(defaultPath);
			var outPathPtr = IntPtr.Zero;

			var result = NFD_SaveDialog(filterListPtr, defaultPathPtr, out outPathPtr);
			outPath = PtrToString(outPathPtr);
			if (outPathPtr != IntPtr.Zero) {
				NFDi_Free(outPathPtr);
			}

			if (filterList != null) {
				Marshal.FreeHGlobal(filterListPtr);
			}
			if (filterList != null) {
				Marshal.FreeHGlobal(defaultPathPtr);
			}

			return result;
		}

		public static NfdResult PickFolder(
			string defaultPath,
			out string outPath
		) {
			var defaultPathPtr = StringToPtr(defaultPath);
			var outPathPtr = IntPtr.Zero;

			var result = NFD_PickFolder(defaultPathPtr, out outPathPtr);
			outPath = PtrToString(outPathPtr);
			if (outPathPtr != IntPtr.Zero) {
				NFDi_Free(outPathPtr);
			}

			if (defaultPath != null) {
				Marshal.FreeHGlobal(defaultPathPtr);
			}

			return result;
		}

		public static string GetError() {
			return PtrToString(NFD_GetError());
		}

		#if UNITY_IPHONE && !UNITY_EDITOR
		private const string DLL_NAME = "__Internal";
		#else
		private const string DLL_NAME = "nfd";
		#endif

		private static string PtrToString(IntPtr ptr) {
			if (ptr != IntPtr.Zero) {
				int len = 0;
				while (Marshal.ReadByte(ptr, len) != 0) {
					++len;
				}
				byte[] buffer = new byte[len];
				Marshal.Copy(ptr, buffer, 0, buffer.Length);
				var str = Encoding.UTF8.GetString(buffer);
				return str;
			}
			return null;
		}

		private static IntPtr StringToPtr(string str) {
			if (str != null) {
				var bytes = Encoding.UTF8.GetBytes(str);
				var ptr = Marshal.AllocHGlobal(bytes.Length + 1);
				Marshal.Copy(bytes, 0, ptr, bytes.Length);
				Marshal.WriteByte(ptr, bytes.Length, 0);
				return ptr;
			}
			return IntPtr.Zero;
		}

		[DllImport(DLL_NAME)]
		private static extern void NFDi_Free(IntPtr ptr);

		/* single file open dialog */
		[DllImport(DLL_NAME)]
		private static extern NfdResult NFD_OpenDialog(
			IntPtr filterList,
			IntPtr defaultPath,
			out IntPtr outPath
		);

		/* multiple file open dialog */
		[DllImport(DLL_NAME)]
		private static extern NfdResult NFD_OpenDialogMultiple(
			IntPtr filterList,
			IntPtr defaultPath,
			out IntPtr outPaths
		);

		/* save dialog */
		[DllImport(DLL_NAME)]
		private static extern NfdResult NFD_SaveDialog(
			IntPtr filterList,
			IntPtr defaultPath,
			out IntPtr outPath
		);

		/* select folder dialog */
		[DllImport(DLL_NAME)]
		private static extern NfdResult NFD_PickFolder(
			IntPtr defaultPath,
			out IntPtr outPath
		);

		/* get last error -- set when nfdresult_t returns NFD_ERROR */
		[DllImport(DLL_NAME)]
		private static extern IntPtr NFD_GetError();

		/* get the number of entries stored in pathSet */
		[DllImport(DLL_NAME)]
		private static extern int NFD_PathSet_GetCount(IntPtr pathSet);

		/* Get the UTF-8 path at offset index */
		[DllImport(DLL_NAME)]
		private static extern IntPtr NFD_PathSet_GetPath(IntPtr pathSet, int index);

		/* Free the pathSet */    
		[DllImport(DLL_NAME)]
		private static extern IntPtr NFD_PathSet_Free(IntPtr pathSet);
	}
}
