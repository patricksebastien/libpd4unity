using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LibPDBinding
{
	public static partial class LibPD
	{			
		[DllImport("libpdcsharp", EntryPoint="libpd_process_patch")]
		[MethodImpl(MethodImplOptions.Synchronized)]
		private static extern int libpd_process_patch(int ticks, [In] IntPtr inBuffer, [Out] IntPtr outBuffer, int dz) ;
		
		public static int ProcessPatch(int ticks, IntPtr inBuffer, IntPtr outBuffer, int dz)
		{
			return libpd_process_patch(ticks, inBuffer, outBuffer, dz);
		}
	}
}



